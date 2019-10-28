using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode
{
    internal static class DragDropHelper
    {
        /// <summary>
        /// Called by the Action when the user releases the mouse.
        /// If we are still on the same compartment but in a different list item,
        /// move the starting item to the position of the current one.
        /// </summary>
        /// <param name="dragFrom"></param>
        /// <param name="e"></param>
        internal static void DoMouseUpForShape<TElement>(ModelElement dragFrom, DiagramMouseEventArgs e) where TElement : ModelElement
        {
            // Original or "from" item:
            TElement dragFromElement = dragFrom as TElement;
            // Current or "to" item:
            TElement dragToElement = e.HitDiagramItem.RepresentedElements.OfType<TElement>().FirstOrDefault();
            if (dragFromElement != null && dragToElement != null)
            {
                // Find the common parent model element, and the relationship links:
                ElementLink parentToLink = GetEmbeddingLink(dragToElement);
                ElementLink parentFromLink = GetEmbeddingLink(dragFromElement);
                if (parentToLink != parentFromLink && parentFromLink != null && parentToLink != null)
                {
                    // Get the static relationship and role (= end of relationship):
                    DomainRelationshipInfo relationshipFrom = parentFromLink.GetDomainRelationship();
                    DomainRoleInfo parentFromRole = relationshipFrom.DomainRoles[0];
                    // Get the node in which the element is embedded, usually the element displayed in the shape:
                    ModelElement parentFrom = parentFromLink.LinkedElements[0];

                    // Same again for the target:
                    DomainRelationshipInfo relationshipTo = parentToLink.GetDomainRelationship();
                    DomainRoleInfo parentToRole = relationshipTo.DomainRoles[0];
                    ModelElement parentTo = parentToLink.LinkedElements[0];

                    // Mouse went down and up in same parent and same compartment:
                    if (parentTo == parentFrom && relationshipTo == relationshipFrom)
                    {
                        // Find index of target position:
                        int newIndex = 0;
                        var elementLinks = parentToRole.GetElementLinks(parentTo);
                        foreach (ElementLink link in elementLinks)
                        {
                            if (link == parentToLink) { break; }
                            newIndex++;
                        }

                        if (newIndex < elementLinks.Count)
                        {
                            using (Transaction t = parentFrom.Store.TransactionManager.BeginTransaction("Move list item"))
                            {
                                parentFromLink.MoveToIndex(parentFromRole, newIndex);
                                t.Commit();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get the embedding link to this element.
        /// Assumes there is no inheritance between embedding relationships.
        /// (If there is, you need to make sure you've got the relationship
        /// that is represented in the shape compartment.)
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private static ElementLink GetEmbeddingLink<TElement>(TElement child) where TElement : ModelElement
        {
            foreach (DomainRoleInfo role in child.GetDomainClass().AllEmbeddedByDomainRoles)
            {
                foreach (ElementLink link in role.OppositeDomainRole.GetElementLinks(child))
                {
                    // Just the assume the first embedding link is the only one.
                    // Not a valid assumption if one relationship is derived from another.
                    return link;
                }
            }
            return null;
        }

    }
}