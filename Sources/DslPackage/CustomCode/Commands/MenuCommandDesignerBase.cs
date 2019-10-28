using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using TXSoftware.DataObjectsNetEntityModel.Common;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal abstract class MenuCommandBase
    {
        public abstract int CommandId { get; }

        public abstract void QueryStatus(MenuCommand menuCommand);

        public abstract void ExecCommand(MenuCommand menuCommand);

        public abstract void SetOwner(object owner);
    }

    internal abstract class MenuCommandBase<TOwner> : MenuCommandBase
    {
        public TOwner Owner { get; set; }

        protected MenuCommandBase()
        {
        }

        public override void SetOwner(object owner)
        {
            this.Owner = (TOwner) owner;
        }
    }

    internal abstract class MenuCommandExplorerBase : MenuCommandBase<DONetEntityModelDesignerExplorer>
    {
    }

    internal abstract class MenuCommandDesignerBase : MenuCommandBase<DONetEntityModelDesignerCommandSet>
    {
        protected internal CurrentModelSelection GetCurrentSelectedPersistentType()
        {
            CurrentModelSelection result = new CurrentModelSelection();

            result.DocData = Owner.GetModelingDocData();
            result.DiagramDocView = Owner.GetDiagramDocView();

            result.CurrentSelection = Owner.CurrentSelection;

            result.IsPersistentTypeSelected = Owner.CurrentSelection.OfType<PersistentShape>().Count() == 1;
            result.IsCompartmentSelected = Owner.CurrentSelection.OfType<Compartment>().Count() == 1;

            if (result.IsPersistentTypeSelected || result.IsCompartmentSelected)
            {
                if (result.IsPersistentTypeSelected)
                {
                    result.CurrentPersistentType =
                        Owner.CurrentSelection.OfType<PersistentShape>().Select(shape => shape.ModelElement).OfType
                            <PersistentType>().Single();
                }
                else
                {
                    Compartment compartment = Owner.CurrentSelection.OfType<Compartment>().Single();
                    result.CompartmentName = compartment.Name;
                    result.CurrentPersistentType = (PersistentType)compartment.ParentShape.ModelElement;
                }
            }

            return result;
        }
    }

    internal struct CurrentModelSelection
    {
        internal ICollection CurrentSelection;
        internal bool IsPersistentTypeSelected { get; set; }
        internal bool IsCompartmentSelected { get; set; }
        internal string CompartmentName { get; set; }
        internal PersistentType CurrentPersistentType { get; set; }
        internal ModelingDocData DocData { get; set; }
        internal DiagramDocView DiagramDocView { get; set; }

        internal IEnumerable<T> GetFromSelection<T>()
        {
            return GetFromSelection<T>(true);
        }

        internal IEnumerable<T> GetFromSelection<T>(bool autoGetElementFromShape)
        {
            var shapeElements = this.CurrentSelection.OfType<ShapeElement>();
            if (shapeElements.Count() > 0 && autoGetElementFromShape)
            {
                return shapeElements.Select(shape => shape.ModelElement).OfType<T>();
            }

            return this.CurrentSelection.OfType<T>();
        }

        internal void MakeActionWithinTransaction(string transactionActionName, Action action)
        {
            DocData.Store.MakeActionWithinTransaction(transactionActionName, action);
        }
    }
}