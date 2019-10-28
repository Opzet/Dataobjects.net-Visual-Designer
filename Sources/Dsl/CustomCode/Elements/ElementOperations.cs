using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public class CustomDesignSurfaceElementOperations: DesignSurfaceElementOperations
    {
        #region constructors

        public CustomDesignSurfaceElementOperations(IServiceProvider serviceProvider, EntityDiagram diagram): base(serviceProvider, diagram)
        {}

        #endregion constructors

        public override void Copy(IDataObject data, ICollection<ModelElement> elements, ClosureType closureType, PointF sourcePosition)
        {
            int i = 5;
            base.Copy(data, elements, closureType, sourcePosition);
        }

        public override void MergeElementGroup(ModelElement targetElement, ElementGroup elementGroup)
        {
            int i = 5;
            base.MergeElementGroup(targetElement, elementGroup);
        }

        protected override void OnElementsReconstituted(MergeElementGroupEventArgs e)
        {
            int i = 5;
            base.OnElementsReconstituted(e);
        }

        protected override ElementGroup CreateElementGroup(ICollection<ModelElement> elements, ClosureType closureType)
        {
            int i = 5;
            return base.CreateElementGroup(elements, closureType);
        }

        protected override void AddCustomFormat(IDataObject data, ICollection<ModelElement> elements, ClosureType closureType, PointF sourcePosition)
        {
            int i = 5;
            base.AddCustomFormat(data, elements, closureType, sourcePosition);
        }

        public override void MergeElementGroupPrototype(ModelElement targetElement, ElementGroupPrototype elementGroupPrototype)
        {
            int i = 5;
            base.MergeElementGroupPrototype(targetElement, elementGroupPrototype);
        }

        protected override void PropagateElementGroupContextToTransaction(ModelElement targetElement, ElementGroup elementGroup, Transaction transaction)
        {
            int i = 5;
            base.PropagateElementGroupContextToTransaction(targetElement, elementGroup, transaction);
        }

        protected override void OnMerging(MergeElementGroupEventArgs e)
        {
            int i = 5;
            base.OnMerging(e);
        }

        public override void Merge(ModelElement targetElement, IDataObject data)
        {
            int i = 5;
            base.Merge(targetElement, data);
        }

        protected override void OnMerged(MergeElementGroupEventArgs e)
        {
            int i = 5;
            base.OnMerged(e);
        }
    }
}