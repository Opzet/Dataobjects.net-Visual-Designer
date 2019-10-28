using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Xml;
using TXSoftware.DataObjectsNetEntityModel.Common.UIEditors;

namespace TXSoftware.DataObjectsNetEntityModel.Common
{
    [TypeConverter(typeof(PropertyConstraintsTypeConverter))]
    public class OrmPropertyConstraints: ISerializableObject
    {
        [DisplayName(PropertyEmailConstraint.DISPLAY_NAME)]
        [Description(PropertyEmailConstraint.DESCRIPTION)]
        public PropertyEmailConstraint EmailConstraint { get; set; }

        [DisplayName(PropertyFutureConstraint.DISPLAY_NAME)]
        [Description(PropertyFutureConstraint.DESCRIPTION)]
        public PropertyFutureConstraint FutureConstraint { get; set; }

        [DisplayName(PropertyLengthConstraint.DISPLAY_NAME)]
        [Description(PropertyLengthConstraint.DESCRIPTION)]
        public PropertyLengthConstraint LengthConstraint { get; set; }

        [DisplayName(PropertyNotEmptyConstraint.DISPLAY_NAME)]
        [Description(PropertyNotEmptyConstraint.DESCRIPTION)]
        public PropertyNotEmptyConstraint NotEmptyConstraint { get; set; }

        [DisplayName(PropertyNotNullConstraint.DISPLAY_NAME)]
        [Description(PropertyNotNullConstraint.DESCRIPTION)]
        public PropertyNotNullConstraint NotNullConstraint { get; set; }

        [DisplayName(PropertyNotNullOrEmptyConstraint.DISPLAY_NAME)]
        [Description(PropertyNotNullOrEmptyConstraint.DESCRIPTION)]
        public PropertyNotNullOrEmptyConstraint NotNullOrEmptyConstraint { get; set; }

        [DisplayName(PropertyPastConstraint.DISPLAY_NAME)]
        [Description(PropertyPastConstraint.DESCRIPTION)]
        public PropertyPastConstraint PastConstraint { get; set; }

        [DisplayName(PropertyRangeConstraint.DISPLAY_NAME)]
        [Description(PropertyRangeConstraint.DESCRIPTION)]
        public PropertyRangeConstraint RangeConstraint { get; set; }

        [DisplayName(PropertyRegexConstraint.DISPLAY_NAME)]
        [Description(PropertyRegexConstraint.DESCRIPTION)]
        public PropertyRegexConstraint RegexConstraint { get; set; }

//        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
//        public PropertyConstrainTypes ConstrainTypes { get; set; }

        [Browsable(false)]
        public PropertyConstraint[] AllConstraints
        {
            get
            {
                //return constraints.ToArray();
                return new PropertyConstraint[]
                       {
                           EmailConstraint, FutureConstraint, LengthConstraint,
                           NotEmptyConstraint, NotNullConstraint, NotNullOrEmptyConstraint,
                           PastConstraint, RangeConstraint, RegexConstraint
                       };
            }
        }

        public OrmPropertyConstraints()
        {
            //this.ConstrainTypes = PropertyConstrainTypes.None;

            this.EmailConstraint = new PropertyEmailConstraint();
            this.FutureConstraint = new PropertyFutureConstraint();
            this.LengthConstraint = new PropertyLengthConstraint();
            this.NotEmptyConstraint = new PropertyNotEmptyConstraint();
            this.NotNullConstraint = new PropertyNotNullConstraint();
            this.NotNullOrEmptyConstraint = new PropertyNotNullOrEmptyConstraint();
            this.PastConstraint = new PropertyPastConstraint();
            this.RangeConstraint = new PropertyRangeConstraint();
            this.RegexConstraint = new PropertyRegexConstraint();
        }

        public override string ToString()
        {
            StringBuilder sb = this.AllConstraints.Aggregate(new StringBuilder(),
                (builder, constraint) =>
                {
                    if (constraint.Used)
                    {
                        return builder.AppendFormat("{0},", constraint.DisplayName);
                    }

                    return builder;
                });

            if (sb.Length > 0 && sb[sb.Length - 1] == ',')
            {
                return sb.Remove(sb.Length - 1, 1).ToString();
            }

            if (sb.Length == 0)
            {
                sb.Append("(None)");
            }

            return sb.ToString();
        }

        public void DeserializeFromXml(XmlReader reader)
        {
            XmlProxy xmlRoot = XmlProxySerializer.Instance.Deserialize(reader);
            DeserializeFromXml(xmlRoot);
        }

        public void DeserializeFromXml(XmlProxy xmlRoot)
        {
            List<PropertyConstraint> list = new List<PropertyConstraint>();
            foreach (XmlProxy xmlChild in xmlRoot.Childs)
            {
                PropertyConstrainType propertyConstrainType = PropertyConstraint.DeserializeType(xmlChild);
                PropertyConstraint propertyConstraint = PropertyConstraint.CreateInstance(propertyConstrainType);
                propertyConstraint.DeserializeFromXml(xmlChild);
                list.Add(propertyConstraint);
            }
            foreach (PropertyConstraint constraint in list)
            {
                UpdateConstraint(constraint);
            }
        }

        public void SerializeToXml(XmlWriter writer)
        {
            SerializeToXml(writer, "constraints");
        }

        public void SerializeToXml(XmlWriter writer, string elementName)
        {
            XmlProxy xmlRoot = new XmlProxy("constraints");
            SerializeToXml(xmlRoot);
            XmlProxySerializer.Instance.Serialize(xmlRoot, writer);
        }

        public void SerializeToXml(XmlProxy xmlRoot)
        {
            foreach (PropertyConstraint constraint in AllConstraints)
            {
                XmlProxy xmlConstraint = xmlRoot.AddChild("constraint");
                xmlConstraint.AddAttribute("type", constraint.ConstrainType);
                constraint.SerializeToXml(xmlConstraint);
            }
        }

        public IOrmAttribute[] GetAllOrmAttributes()
        {
            return AllConstraints.ToArray();
        }

        internal void UpdateConstraint(PropertyConstraint constraint)
        {
            switch (constraint.ConstrainType)
            {
                case PropertyConstrainType.Email:
                {
                    this.EmailConstraint = (PropertyEmailConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.Future:
                {
                    this.FutureConstraint = (PropertyFutureConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.Length:
                {
                    this.LengthConstraint = (PropertyLengthConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.NotEmpty:
                {
                    this.NotEmptyConstraint = (PropertyNotEmptyConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.NotNull:
                {
                    this.NotNullConstraint = (PropertyNotNullConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.NotNullOrEmpty:
                {
                    this.NotNullOrEmptyConstraint = (PropertyNotNullOrEmptyConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.Past:
                {
                    this.PastConstraint = (PropertyPastConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.Range:
                {
                    this.RangeConstraint = (PropertyRangeConstraint) constraint;
                    break;
                }
                case PropertyConstrainType.Regex:
                {
                    this.RegexConstraint = (PropertyRegexConstraint) constraint;
                    break;
                }
            }
        }

        public void FilterAttributeGroups(List<OrmAttributeGroup> attributeGroupsToFilter)
        {
            foreach (IOrmAttribute ormAttribute in GetAllOrmAttributes())
            {
                var filteredGroups = ormAttribute.GetAttributeGroups(AttributeGroupsListMode.Filtered);
                //attributeGroupsToFilter.RemoveAll(item => !filteredGroups.Contains(item));
                attributeGroupsToFilter.RemoveAll(item => !filteredGroups.Any(item.EqualsTo));
            }
        }

        public void FillAttributeGroupItems(Dictionary<string, Defaultable> result, OrmAttributeGroup @group)
        {
            foreach (IOrmAttribute ormAttribute in GetAllOrmAttributes())
            {
                Dictionary<string, Defaultable> attributeGroupItems = ormAttribute.GetAttributeGroupItems(group);
                foreach (var attributeGroupItem in attributeGroupItems)
                {
                    result.Add(attributeGroupItem.Key, attributeGroupItem.Value);
                }
            }
        }

        public PropertyConstraint this[PropertyConstrainType constrainType]
        {
            get { return this.AllConstraints.Single(constraint => constraint.ConstrainType == constrainType); }
        }

        public OrmPropertyConstraints Merge(OrmPropertyConstraints otherConstraints, MergeConflictAction mergeConflictAction)
        {
            OrmPropertyConstraints merged = new OrmPropertyConstraints();
            foreach (var constraint in this.AllConstraints)
            {
                PropertyConstrainType constrainType = constraint.ConstrainType;
                PropertyConstraint otherConstraint = otherConstraints[constrainType];
                PropertyConstraint mergedConstraint = (PropertyConstraint) constraint.MergeChanges(otherConstraint, mergeConflictAction);
                merged.UpdateConstraint(mergedConstraint);
            }
            return merged;
        }
    }

    #region class PropertyConstraintsTypeConverter

    public class PropertyConstraintsTypeConverter : ExpandableObjectConverter
    {
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        {
            PropertyConstraint email = (PropertyConstraint)propertyValues["EmailConstraint"];
            PropertyConstraint future = (PropertyConstraint)propertyValues["FutureConstraint"];
            PropertyConstraint length = (PropertyConstraint)propertyValues["LengthConstraint"];
            PropertyConstraint notEmpty = (PropertyConstraint)propertyValues["NotEmptyConstraint"];
            PropertyConstraint notNull = (PropertyConstraint)propertyValues["NotNullConstraint"];
            PropertyConstraint notNullOrEmpty = (PropertyConstraint)propertyValues["NotNullOrEmptyConstraint"];
            PropertyConstraint past = (PropertyConstraint)propertyValues["PastConstraint"];
            PropertyConstraint range = (PropertyConstraint)propertyValues["RangeConstraint"];
            PropertyConstraint regex = (PropertyConstraint)propertyValues["RegexConstraint"];
            //PropertyConstrainTypes constrainTypes = (PropertyConstrainTypes) propertyValues["ConstrainTypes"];

            OrmPropertyConstraints result = new OrmPropertyConstraints();
            result.UpdateConstraint(email);
            result.UpdateConstraint(future);
            result.UpdateConstraint(length);
            result.UpdateConstraint(notEmpty);
            result.UpdateConstraint(notNull);
            result.UpdateConstraint(notNullOrEmpty);
            result.UpdateConstraint(past);
            result.UpdateConstraint(range);
            result.UpdateConstraint(regex);
            //result.ConstrainTypes = constrainTypes;
            return result;
        }
    }

    #endregion class PropertyConstraintsTypeConverter

}