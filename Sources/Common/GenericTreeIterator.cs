using System;
using System.Collections.Generic;
using System.Linq;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    public sealed class GenericTreeIterator<TItem> where TItem: class
    {
        #region class IterationResult

        internal class IterationResult
        {
            public bool Cancel { get; private set; }
            public bool CancelBreaksChildLoop { get; private set; }

            public IterationResult()
            {
                this.Cancel = false;
                this.CancelBreaksChildLoop = false;
            }

            public IterationResult(bool cancel, bool cancelBreaksChildLoop)
            {
                Cancel = cancel;
                CancelBreaksChildLoop = cancelBreaksChildLoop;
            }

            public IterationResult(bool cancel)
            {
                Cancel = cancel;
            }

            public bool RequestCancelingIteration()
            {
                return this.Cancel && !this.CancelBreaksChildLoop;
            }

            public bool RequestBreaksChildLoop()
            {
                return this.Cancel && this.CancelBreaksChildLoop;
            }
        }

        #endregion class IterationResult

        private readonly Func<TItem, IEnumerable<TItem>> getChildsFunc;
        
        private readonly Func<IEnumerable<TItem>> getRootChildsSpecialFunc;

        public GenericTreeIterator(Func<TItem, IEnumerable<TItem>> getChilds) : this(getChilds, null) {}

        public GenericTreeIterator(Func<TItem, IEnumerable<TItem>> getChilds, Func<IEnumerable<TItem>> getRootChildsSpecialFunc)
        {
            if (getChilds == null)
            {
                throw new ArgumentNullException("getChilds");
            }
            this.getChildsFunc = getChilds;
            this.getRootChildsSpecialFunc = getRootChildsSpecialFunc;
        }

        public void IterateTree<TData>(bool twoStagePass, TItem rootItem,
            Action<GenericTreeIterationArgs<TItem, TData>> iterateAction)
        {
            InternalIterateTree(twoStagePass, 0, rootItem, iterateAction);
        }

        public void IterateTree(bool twoStagePass, TItem rootItem, Action<GenericTreeIterationArgs<TItem>> iterateAction)
        {
            InternalIterateTree<object>(twoStagePass, 0, rootItem, iterateAction);
        }

        // Returns true if iteration was cancelled
        private IterationResult InternalIterateTree<TData>(bool twoStagePass, int level, TItem parent,
            Action<GenericTreeIterationArgs<TItem, TData>> iterateAction)
        {
            return InternalIterateTree<TData>(twoStagePass, level, parent, default(TData), iterateAction);
        }

        private IterationResult InternalIterateTree<TData>(bool twoStagePass, int level, TItem parent, TData data,
            Action<GenericTreeIterationArgs<TItem, TData>> iterateAction)
        {
            IEnumerable<TItem> childItems;
            if (level == 0 && getRootChildsSpecialFunc != null)
            {
                childItems = getRootChildsSpecialFunc();
            }
            else
            {
                childItems = getChildsFunc(parent);
            }
            
            foreach (TItem child in childItems)
            {
                var treeIterator = new GenericTreeIterationArgs<TItem, TData>(level, parent, child);
                treeIterator.Data = data;
                iterateAction(treeIterator);
                if (treeIterator.Cancel)
                {
                    return new IterationResult(treeIterator.Cancel, treeIterator.CancelBreaksChildLoop);
                }

                if (getChildsFunc(child).Count() > 0)
                {
                    var iterationResult = InternalIterateTree(twoStagePass, level + 1, child, treeIterator.Data, iterateAction);
                    if (iterationResult.RequestCancelingIteration())
                    {
                        return iterationResult;
                    }
                }

                if (twoStagePass)
                {
                    treeIterator.CurrentStage = GenericTreeIterationStage.Leave;
                    iterateAction(treeIterator);
                    if (treeIterator.Cancel)
                    {
                        return new IterationResult(treeIterator.Cancel, treeIterator.CancelBreaksChildLoop);
                    }
                }
            }

            return new IterationResult();
        }
    }

    public enum GenericTreeIterationStage
    {
        Enter,
        Leave
    }

    public class GenericTreeIterationArgs<TItem>
    {
        public int Level { get; internal set; }
        public TItem Parent { get; internal set; }
        public TItem Current { get; internal set; }
        public bool Cancel { get; set; }
        public bool CancelBreaksChildLoop { get; set; }
        public GenericTreeIterationStage CurrentStage { get; internal set; }

        public GenericTreeIterationArgs(int level, TItem parent, TItem current)
        {
            Level = level;
            Parent = parent;
            Current = current;
            this.CurrentStage = GenericTreeIterationStage.Enter;
        }
    }

    public class GenericTreeIterationArgs<TItem, TData> : GenericTreeIterationArgs<TItem>
    {
        public TData Data { get; set; }

        public GenericTreeIterationArgs(int level, TItem parent, TItem current) : base(level, parent, current)
        {
        }
    }
}