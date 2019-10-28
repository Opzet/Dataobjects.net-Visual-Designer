using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace TXSoftware.DataObjectsNetEntityModel.Common.Modeling
{
    public static class DynamicTypeDescriptor
    {
        #region IAttributeBuilder

        public interface IAttributeBuilder
        {
            ModelElement Element { get; }

            TResult TestElement<TResult>(Func<ModelElement, TResult> func, TResult defaultValue);

            void ReplaceElementPropertyAtribute<T>(Guid ownerDomainPropertyInfoId,
                                                   Func<ElementPropertyDescriptor, T> instantiateAttribute) where T : Attribute;
            void RemoveElementPropertyAttributes(Guid ownerDomainPropertyInfoId, Func<ElementPropertyDescriptor, Attribute, bool> predicate);

            void ReplaceRolePlayerPropertyAtribute<T>(Guid ownerDomainPropertyInfoId,
                                                      Func<RolePlayerPropertyDescriptor, T> instantiateAttribute) where T : Attribute;
            void RemoveRolePlayereAttributes(Guid ownerDomainPropertyInfoId, Func<RolePlayerPropertyDescriptor, Attribute, bool> predicate);
            
            PropertyDescriptorCollection Build();
        }

        public enum ProcessPropertyType
        {
            Element,
            RolePlayer
        }

        #region class AttributeBuilderNop

        internal sealed class AttributeBuilderNop : IAttributeBuilder
        {
            private PropertyDescriptorCollection propertyDescriptors;
            public AttributeBuilderNop(PropertyDescriptorCollection propertyDescriptors, ModelElement appliedTo)
            {
                this.propertyDescriptors = propertyDescriptors;
                this.Element = appliedTo;
            }

            public ModelElement Element { get; private set; }

            public TResult TestElement<TResult>(Func<ModelElement, TResult> func, TResult defaultValue)
            {
                return defaultValue;
            }

            public void ReplaceElementPropertyAtribute<T>(Guid ownerDomainPropertyInfoId, Func<ElementPropertyDescriptor, T> instantiateAttribute) where T : Attribute
            {
                
            }

            public void RemoveElementPropertyAttributes(Guid ownerDomainPropertyInfoId, Func<ElementPropertyDescriptor, Attribute, bool> predicate)
            {
                
            }

            public void ReplaceRolePlayerPropertyAtribute<T>(Guid ownerDomainPropertyInfoId, Func<RolePlayerPropertyDescriptor, T> instantiateAttribute) where T : Attribute
            {
                
            }

            public void RemoveRolePlayereAttributes(Guid ownerDomainPropertyInfoId, Func<RolePlayerPropertyDescriptor, Attribute, bool> predicate)
            {
                
            }

            public PropertyDescriptorCollection Build()
            {
                return propertyDescriptors;
            }
        }

        #endregion class AttributeBuilderNop

        internal sealed class AttributeBuilderImplementation : IAttributeBuilder
        {
            #region class PropertyDescriptorItem

            internal class PropertyDescriptorItem
            {
                internal List<Attribute> Attributes { get; private set; }
                //internal ElementPropertyDescriptor ElementPropertyDescriptorToReplace { get; set; }
                //internal RolePlayerPropertyDescriptor RolePlayerPropertyDescriptorToReplace { get; set; }
                internal PropertyDescriptor PropertyDescriptorToReplace { get; set; }

                internal PropertyDescriptorItem()
                {
                    this.Attributes = new List<Attribute>();
                }
            }

            #endregion class PropertyDescriptorItem

            private ElementTypeDescriptor descriptor;
            private readonly Attribute[] outerAttributes;
            private readonly PropertyDescriptorCollection propertyDescriptors;
            //private readonly ModelElement modelElement;
            //private IEnumerable<ElementPropertyDescriptor> elementPropertyDescriptors;
            //private IEnumerable<RolePlayerPropertyDescriptor> rolePlayerPropertyDescriptors;
            private readonly Dictionary<ProcessPropertyType, List<PropertyDescriptor>> processingPropertyDescriptors =
                new Dictionary<ProcessPropertyType, List<PropertyDescriptor>>();

            private readonly Dictionary<Guid, PropertyDescriptorItem> updatedAttributes = new Dictionary<Guid, PropertyDescriptorItem>();
            
            public AttributeBuilderImplementation(ElementTypeDescriptor descriptor, Attribute[] attributes, 
                                                  PropertyDescriptorCollection propertyDescriptors, ModelElement appliedTo)
            {
                this.Element = appliedTo;
                this.descriptor = descriptor;
                this.outerAttributes = attributes;

                this.propertyDescriptors = propertyDescriptors;// descriptor.GetProperties(attributes);
                //this.modelElement = descriptor.ModelElement;

                if (this.Element != null)
                {
                    List<PropertyDescriptor> elementPropertyDescriptors =
                        propertyDescriptors.OfType<ElementPropertyDescriptor>().Cast<PropertyDescriptor>().ToList();
                    processingPropertyDescriptors.Add(ProcessPropertyType.Element, elementPropertyDescriptors);

                    List<PropertyDescriptor> rolePlayerPropertyDescriptors =
                        propertyDescriptors.OfType<RolePlayerPropertyDescriptor>().Cast<PropertyDescriptor>().ToList();
                    processingPropertyDescriptors.Add(ProcessPropertyType.RolePlayer, rolePlayerPropertyDescriptors);
                }
            }

            public ModelElement Element { get; private set; }

            public TResult TestElement<TResult>(Func<ModelElement, TResult> func, TResult defaultValue)
            {
                if (Element != null)
                {
                    return func(Element);
                }

                return defaultValue;
            }

            internal PropertyDescriptorItem GetDescriptorItem(ProcessPropertyType propertyType, Guid domainPropertyInfoId)
            {
                PropertyDescriptorItem propertyDescriptorItem;
                if (!updatedAttributes.ContainsKey(domainPropertyInfoId))
                {
                    propertyDescriptorItem = Create(propertyType, domainPropertyInfoId);
                    updatedAttributes.Add(domainPropertyInfoId, propertyDescriptorItem);
                }
                else
                {
                    propertyDescriptorItem = updatedAttributes[domainPropertyInfoId];
                }

                return propertyDescriptorItem;
            }

            internal PropertyDescriptorItem Create(ProcessPropertyType propertyType, Guid forDomainPropertyInfoId)
            {
                PropertyDescriptorItem result = new PropertyDescriptorItem();

                PropertyDescriptor propertyDescriptorToReplace;
                if (propertyType == ProcessPropertyType.Element)
                {
                    var elementPropertyDescriptors = this.processingPropertyDescriptors[ProcessPropertyType.Element].Cast<ElementPropertyDescriptor>();
                    propertyDescriptorToReplace = elementPropertyDescriptors.SingleOrDefault(
                        item => item.DomainPropertyInfo.Id == forDomainPropertyInfoId);
                }
                else
                {
                    var playerPropertyDescriptors = this.processingPropertyDescriptors[ProcessPropertyType.RolePlayer].Cast<RolePlayerPropertyDescriptor>();
                    propertyDescriptorToReplace = playerPropertyDescriptors.SingleOrDefault(
                        item => item.DomainRoleInfo.Id == forDomainPropertyInfoId);
                }

                result.PropertyDescriptorToReplace = propertyDescriptorToReplace;

                if (result.PropertyDescriptorToReplace == null)
                {
                    throw new ArgumentOutOfRangeException(
                        string.Format("Could not find domain property with id '{0}' on element '{1}'",
                                      forDomainPropertyInfoId, this.Element.ToString()));
                }

                if (outerAttributes != null)
                {
                    result.Attributes.AddRange(outerAttributes);
                }

                if (result.PropertyDescriptorToReplace.Attributes != null)
                {
                    result.Attributes.AddRange(result.PropertyDescriptorToReplace.Attributes.OfType<Attribute>());
                }

                return result;
            }

            public void ReplaceElementPropertyAtribute<T>(Guid ownerDomainPropertyInfoId, Func<ElementPropertyDescriptor, T> instantiateAttribute) 
                where T : Attribute
            {
                InternalReplacePropertyAttribute(ProcessPropertyType.Element, ownerDomainPropertyInfoId, instantiateAttribute);

//                PropertyDescriptorItem propertyDescriptorItem = GetDescriptorItem(ProcessPropertyType.Element, ownerDomainPropertyInfoId);
//                Attribute newAttr = instantiateAttribute(propertyDescriptorItem.PropertyDescriptorToReplace as ElementPropertyDescriptor);
//                if (newAttr == null)
//                {
//                    throw new ArgumentNullException("Cannot replace with null attribute!");
//                }
//
//                propertyDescriptorItem.Attributes.RemoveAll(attribute => attribute is T);
//                propertyDescriptorItem.Attributes.Add(newAttr);
            }

            public void ReplaceRolePlayerPropertyAtribute<T>(Guid ownerDomainPropertyInfoId, Func<RolePlayerPropertyDescriptor, T> instantiateAttribute) 
                where T : Attribute
            {
                InternalReplacePropertyAttribute(ProcessPropertyType.RolePlayer, ownerDomainPropertyInfoId, instantiateAttribute);
            }

            private void InternalReplacePropertyAttribute<TAttribute, TPropertyDescriptor>(ProcessPropertyType propertyType, Guid ownerDomainPropertyInfoId,
                                                                                           Func<TPropertyDescriptor, TAttribute> instantiateAttribute)
                where TAttribute : Attribute
                where TPropertyDescriptor : PropertyDescriptor
            {
                PropertyDescriptorItem propertyDescriptorItem = GetDescriptorItem(propertyType, ownerDomainPropertyInfoId);
                Attribute newAttr = instantiateAttribute(propertyDescriptorItem.PropertyDescriptorToReplace as TPropertyDescriptor);
                if (newAttr == null)
                {
                    throw new ArgumentNullException("Cannot replace with null attribute!");
                }

                propertyDescriptorItem.Attributes.RemoveAll(attribute => attribute is TAttribute);
                propertyDescriptorItem.Attributes.Add(newAttr);
            }

            public void RemoveElementPropertyAttributes(Guid ownerDomainPropertyInfoId, Func<ElementPropertyDescriptor, Attribute, bool> predicate)
            {
                InternalRemoveElementPropertyAttributes(ProcessPropertyType.Element, ownerDomainPropertyInfoId, predicate);

//                PropertyDescriptorItem propertyDescriptorItem = GetDescriptorItem(ProcessPropertyType.Element, ownerDomainPropertyInfoId);
//                propertyDescriptorItem.Attributes.RemoveAll(
//                    attribute =>
//                    predicate(propertyDescriptorItem.PropertyDescriptorToReplace as ElementPropertyDescriptor, attribute));
            }

            public void RemoveRolePlayereAttributes(Guid ownerDomainPropertyInfoId, Func<RolePlayerPropertyDescriptor, Attribute, bool> predicate)
            {
                InternalRemoveElementPropertyAttributes(ProcessPropertyType.RolePlayer, ownerDomainPropertyInfoId, predicate);
            }

            private void InternalRemoveElementPropertyAttributes<TPropertyDescriptor>(ProcessPropertyType propertyType, Guid ownerDomainPropertyInfoId,
                                                                                           Func<TPropertyDescriptor, Attribute, bool> predicate)
                where TPropertyDescriptor : PropertyDescriptor
            {
                PropertyDescriptorItem propertyDescriptorItem = GetDescriptorItem(propertyType, ownerDomainPropertyInfoId);
                propertyDescriptorItem.Attributes.RemoveAll(
                    attribute =>
                    predicate(propertyDescriptorItem.PropertyDescriptorToReplace as TPropertyDescriptor, attribute));
            }

            public PropertyDescriptorCollection Build()
            {
                foreach (var propertyDescriptorItem in updatedAttributes.Values)
                {
                    var newAttributes = propertyDescriptorItem.Attributes.ToArray();
                    var propertyDescriptorToReplace = propertyDescriptorItem.PropertyDescriptorToReplace;
                    if (propertyDescriptorToReplace != null)
                    {
                        PropertyDescriptor newDescriptor;
                        if (propertyDescriptorToReplace is ElementPropertyDescriptor)
                        {
                            ElementPropertyDescriptor elementPropertyDescriptor = propertyDescriptorToReplace as ElementPropertyDescriptor;
                            newDescriptor = new ElementPropertyDescriptor(Element, elementPropertyDescriptor.DomainPropertyInfo, newAttributes);
                        }
                        else
                        {
                            RolePlayerPropertyDescriptor rolePlayerPropertyDescriptor = propertyDescriptorToReplace as RolePlayerPropertyDescriptor;
                            newDescriptor = new RolePlayerPropertyDescriptor(Element, rolePlayerPropertyDescriptor.DomainRoleInfo, newAttributes);
                        }

                        propertyDescriptors.Remove(propertyDescriptorToReplace);
                        propertyDescriptors.Add(newDescriptor);
                    }
                }

                return propertyDescriptors;
            }
        }

        #endregion IAttributeBuilder

        #region methods

        public static IAttributeBuilder CreateBuilder(this ElementTypeDescriptor descriptor,
                                                      Attribute[] attributes,
                                                      PropertyDescriptorCollection propertyDescriptors,
                                                      Func<ModelElement, bool> appliedToElement)
        {
            return appliedToElement(descriptor.ModelElement)
                       ? (IAttributeBuilder)
                         new AttributeBuilderImplementation(descriptor, attributes, propertyDescriptors,
                                                            descriptor.ModelElement)
                       : new AttributeBuilderNop(propertyDescriptors, descriptor.ModelElement);
        }

        #endregion methods
    }
}