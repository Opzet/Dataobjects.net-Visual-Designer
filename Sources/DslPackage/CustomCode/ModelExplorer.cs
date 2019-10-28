using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using TXSoftware.DataObjectsNetEntityModel.Common;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    internal partial class DONetEntityModelDesignerExplorer
    {
        public override void AddCommandHandlers(IMenuCommandService menuCommandService)
        {
            base.AddCommandHandlers(menuCommandService);

            // Add external type (from model explorer node)
            RegisterCommand<MenuCommandAddExternalType>(menuCommandService);
        }

        protected override IElementVisitor CreateElementVisitor()
        {
            this.ObjectModelBrowser.DoubleClick += ObjectModelBrowserOnDoubleClick;

            return base.CreateElementVisitor();
        }

        private void ObjectModelBrowserOnDoubleClick(object sender, EventArgs eventArgs)
        {
            // Get the node selected in the model explorer tree
            ModelElementTreeNode node = (this.ObjectModelBrowser.SelectedNode as ModelElementTreeNode);

            if (node != null)
            {
                // Get the corresponding model element
                ModelElement element = node.ModelElement;

                if (element != null)
                {
                    // Get the corresponding shape
                    // If the model element is in a compartment the result will be null
                    ShapeElement shape = DiagramUtil.GetModelElementFirstShape(element);

                    if (shape == null)
                    {
                        // If the element is in a compartment, try to get the parent model element to select that
                        ModelElement parentElement = GetCompartmentElementFirstParent(element);

                        if (parentElement != null)
                        {
                            // Get the corresponding shape
                            shape = DiagramUtil.GetModelElementFirstShape(parentElement);
                        }
                    }

                    // Select the shape
                    if (shape != null)
                    {
                        SelectShape(shape, this.ModelingDocData);
                    }
                }
            }
        }

        private static void SelectShape(ShapeElement shapeElement, DocData docData)
        {
            // Validation

            if (shapeElement == null)
            {
                throw new ArgumentNullException("shapeElement");
            }

            if (docData == null)
            {
                throw new ArgumentNullException("docData");
            }

            ModelingDocView docView = docData.DocViews[0];
            if (docView != null)
            {

                docView.SelectObjects(1, new object[] {shapeElement}, 0);
            }
        }

        private static ModelElement GetCompartmentElementFirstParent(ModelElement modelElement)
        {
            // Get the domain class associated with model element.
            DomainClassInfo domainClass = modelElement.GetDomainClass();

            if (domainClass != null)
            {
                // A element is only considered to be in a compartment if it participates in only 1 embedding relationship
                // This might be wrong for some models

                if (domainClass.AllEmbeddedByDomainRoles.Count == 1)
                {
                    // Get a collection of all the links to this model element
                    // Since this is in a compartment there will only be one
                    ReadOnlyCollection<ElementLink> links = DomainRoleInfo.GetAllElementLinks(modelElement);
                    if (links.Count == 1)
                    {
                        // Get the model element participating in the link that isn't the current one
                        // That will be the parent
                        // Probably there is a better way to achieve the same result
                        foreach (ModelElement linkedElement in links[0].LinkedElements)
                        {
                            if (!modelElement.Equals(linkedElement))
                            {
                                return linkedElement;
                            }
                        }
                    }
                }

            }

            return null;
        }



        private void RegisterCommand<T>(IMenuCommandService menuCommandService) where T : MenuCommandExplorerBase, new()
        {
            var menuCommand = CommandsUtil.CreateCommand<T>(this);
            menuCommandService.AddCommand(menuCommand);
        }

        internal ExplorerTreeNode GetSelectedNode()
        {
            return (this.ObjectModelBrowser.SelectedNode as ExplorerTreeNode);
        }

        internal EntityModel GetEntityModel()
        {
            var explorerTreeNode = GetSelectedNode();
            ModelElement representedElement = FindRootRepresentedElement(explorerTreeNode);
            return representedElement.Store.ElementDirectory.FindElements<EntityModel>().Single();
        }

        internal ExplorerTreeNode FindTreeNodeByText(string nodeText)
        {
            return RecursiveFindNode(this.ObjectModelBrowser.Nodes, nodeText);
        }

        internal void SelectTreeNodeByText(string nodeText)
        {
            var treeNode = FindTreeNodeByText(nodeText);
            if (treeNode != null) this.ObjectModelBrowser.SelectedNode = treeNode;
        }

        private ExplorerTreeNode RecursiveFindNode(TreeNodeCollection nodes, string nodeText)
        {
            ExplorerTreeNode result = null;

            foreach (TreeNode treeNode in nodes)
            {
                if (Util.StringEqual(treeNode.Text, nodeText, true))
                {
                    result = (ExplorerTreeNode) treeNode;
                    break;
                }

                if (treeNode.Nodes.Count > 0)
                {
                    result = RecursiveFindNode(treeNode.Nodes, nodeText);
                    if (result != null)
                    {
                        break;
                    }
                }
            }

            return result;
        }

        private ModelElement FindRootRepresentedElement(ExplorerTreeNode node)
        {
            ModelElement result = node.RepresentedElement;
            ExplorerTreeNode parentNode = node.Parent as ExplorerTreeNode;
            while (parentNode != null)
            {
                result = parentNode.RepresentedElement;
                parentNode = parentNode.Parent as ExplorerTreeNode;
            }

            return result;
        }

        #region Code to suppress "Delete" for Validators

        /// <summary>
        /// Override to stop the "Delete" command appearing for
        /// Validators.
        /// </summary>
        protected override void ProcessOnStatusDeleteCommand(MenuCommand command)
        {
            // Check the selected items to see if they contain
            // Validators.
            //            if (this.SelectedElement.GetType() == typeof(PropertyValidators))
            //            {
            // Disable the menu command
            //                command.Enabled = false;
            //                command.Visible = false;
            //            }
            //            else

            if (this.SelectedElement.GetType() == typeof(DomainType))
            {
                DomainType domainType = this.SelectedElement as DomainType;

                bool enabled = !domainType.IsBuildIn;
                command.Enabled = enabled;
                command.Visible = enabled;
            }
            else
            {
                // Otherwise, delegate to the base method.
                base.ProcessOnStatusDeleteCommand(command);
            }
        }

        #endregion
    }
}