using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class AssociationConnector : IElementEventsHandler
    {
        void IElementEventsHandler.HandleEvent(ElementEventArgs args)
        {
            var persistentTypeHasAssociations = this.ModelElement as PersistentTypeHasAssociations;
            if (persistentTypeHasAssociations != null)
            {
                persistentTypeHasAssociations.HandleEvent(args);
            }
        }
    }
}