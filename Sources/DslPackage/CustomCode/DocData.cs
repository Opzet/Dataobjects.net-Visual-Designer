using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.Win32;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade;
using ElementEventArgs = Microsoft.VisualStudio.Modeling.ElementEventArgs;
using TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class DONetEntityModelDesignerDocData
    {
        protected override void OnDocumentLoading(EventArgs e)
        {
            ModelUpgrader.Instance.CleanUp();
            base.OnDocumentLoading(e);
        }

        protected override void OnDocumentLoaded()
        {
            base.OnDocumentLoaded();

            EventManagerDirectory events = this.Store.EventManagerDirectory;
            events.ElementPropertyChanged.Add(new EventHandler<ElementPropertyChangedEventArgs>(OnElementPropertyChanged));
            events.ElementAdded.Add(new EventHandler<ElementAddedEventArgs>(OnElementAdded));
            events.RolePlayerChanged.Add(new EventHandler<RolePlayerChangedEventArgs>(OnRolePlayerChanged));

            //AddBuildInDomainTypes();

            CheckEntityModel();

            VersionUpgradeManager.CheckForUpdate();
        }

        private void CheckEntityModel()
        {
            // check model upgraders for changes
            if (ModelUpgrader.Instance.UpgradersMakeChanges)
            {
                this.Store.MakeActionWithinTransaction("Model upgrades make changes in deserialization stage.",
                    () =>{});
            }

            EntityModel entityModel = this.Store.GetEntityModel();
            if (!entityModel.HasBuildInDomainTypes)
            {
                this.Store.MakeActionWithinTransaction("Populating buildin domain types...", 
                    entityModel.GenerateBuildInDomainTypes);
            }
        }

//        private void AddBuildInDomainTypes()
//        {
//            EntityModel entityModel = this.Store.GetEntityModel();
//            this.Store.MakeActionWithinTransaction("Populating buildin domain types...",
//                delegate
//                {
//                    foreach (Type type in SystemPrimitiveTypesConverter.Types)
//                    {
//                        var buildInDomainType = new DomainType(entityModel.Partition);
//                        buildInDomainType.Name = type.Name;
//                        buildInDomainType.Namespace = type.Namespace;
//                        buildInDomainType.IsBuildIn = true;
//
//                        entityModel.DomainTypes.Add(buildInDomainType);
//                    }
//                });
//        }

        private void OnElementPropertyChanged(object sender, ElementPropertyChangedEventArgs e)
        {
            IElementEventsHandler eventsHandler;
            if (TryGetElementEventsHandler(e, out eventsHandler))
            {
                eventsHandler.HandleEvent(new Common.Modeling.ElementEventArgs(e));
            }
        }

        private void OnElementAdded(object sender, ElementAddedEventArgs e)
        {
            IElementEventsHandler eventsHandler;
            if (TryGetElementEventsHandler(e, out eventsHandler))
            {
                eventsHandler.HandleEvent(new Common.Modeling.ElementEventArgs(e));
                eventsHandler.HandleEvent(new Common.Modeling.ElementEventArgs(ElementEventType.InitializePresentationElement));
            }
        }

        private void OnRolePlayerChanged(object sender, RolePlayerChangedEventArgs e)
        {
        }

        private bool TryGetElementEventsHandler(ElementEventArgs args, out IElementEventsHandler eventsHandler)
        {
            eventsHandler = null;

            if (args.ModelElement != null)
            {
                eventsHandler = args.ModelElement as IElementEventsHandler;
            }

            return eventsHandler != null;
        }
    }
}