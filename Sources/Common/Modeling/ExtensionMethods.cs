using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
//using PropertyChangedEventArgs = System.ComponentModel.PropertyChangedEventArgs;

namespace TXSoftware.DataObjectsNetEntityModel.Common.Modeling
{
    public static class ExtensionMethods
    {
        #region modeling extension methods

        public static bool IsUndoRedoOrRollback(this ModelElement modelElement)
        {
            return (modelElement.Store.InUndoRedoOrRollback || modelElement.Store.InSerializationTransaction);
        }

        public static void MakeActionWithinTransaction(this Store store, string transactionActionName, Action action)
        {
            if (store != null && store.TransactionManager.CurrentTransaction == null)
            {
                using (Transaction transaction = store.TransactionManager.BeginTransaction(transactionActionName))
                {
                    action();
                    transaction.Commit();
                }
            }
            else
            {
                action();
            }
        }

        public static Guid CurrentTransactionId(this Store store)
        {
            return store.TransactionManager.CurrentTransaction == null
                       ? Guid.Empty
                       : store.TransactionManager.CurrentTransaction.Id;
        }

        public static void RegisterActionOnTransactionEvent<TData>(this Store store, TransactionEvent events, TData data,
                                                                   Action<TransactionEventArgs, TData> action)
        {
            RegisterActionOnTransactionEvent(store, events, CurrentTransactionId(store), data, action);
        }

        public static void RegisterActionOnTransactionEvent<TData>(this Store store, TransactionEvent events, Guid transactionId,
                                                                   TData data, Action<TransactionEventArgs, TData> action)
        {
            if (Util.IsFlagSet(TransactionEvent.Beginning, events))
            {
                store.EventManagerDirectory.TransactionBeginning.Add(transactionId,
                                                                     new EventHandler<TransactionBeginningEventArgs>((sender, args) => action(args, data)));
            }
            if (Util.IsFlagSet(TransactionEvent.Committed, events))
            {
                store.EventManagerDirectory.TransactionCommitted.Add(transactionId,
                                                                     new EventHandler<TransactionCommitEventArgs>((sender, args) => action(args, data)));
            }
            if (Util.IsFlagSet(TransactionEvent.RolledBack, events))
            {
                store.EventManagerDirectory.TransactionRolledBack.Add(transactionId,
                                                                      new EventHandler<TransactionRollbackEventArgs>((sender, args) => action(args, data)));
            }
        }

        public static void UpdatePresentationElement<T>(this ModelElement modelElement, Action<T> updateAction) where T : PresentationElement
        {
            if (PresentationViewsSubject.GetPresentation(modelElement).Count == 1)
            {
                PresentationElement presentationElement = PresentationViewsSubject.GetPresentation(modelElement)[0];
                T pe = presentationElement as T;
                if (pe != null)
                {
                    updateAction(pe);
                }
            }
        }

        public static void NotifyChangesToDesigner(this ModelElement modelElement, PropertyChangedEventArgs args)
        {
            if (!modelElement.IsDeleting && !modelElement.IsDeleted && (!modelElement.Store.InSerializationTransaction))
            {
                using (Transaction transaction = modelElement.Store.TransactionManager.BeginTransaction("Notify changes"))
                {
                    DomainClassInfo domainClassInfo = modelElement.GetDomainClass();
                    DomainPropertyInfo domainProperty = domainClassInfo.FindDomainProperty(args.PropertyName, true);
                    domainProperty.NotifyValueChange(modelElement);
                    transaction.Commit();
                }
            }
        }

        #endregion modeling extension methods
    }
}