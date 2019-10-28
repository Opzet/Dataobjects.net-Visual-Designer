using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl.CustomCode.Upgrade
{
    public interface IModelVersionUpgrader
    {
        string TargetVersion { get; }

        /// <summary>
        /// Reads the properties from attributes.
        /// </summary>
        /// <typeparam name="TModelElement">The type of the model element.</typeparam>
        /// <param name="serializationContext">The serialization context.</param>
        /// <param name="element">The element.</param>
        /// <param name="reader">The reader.</param>
        /// <returns>Returns true if some changes was made, otherwise return false.</returns>
        bool ReadPropertiesFromAttributes<TModelElement>(SerializationContext serializationContext,
            TModelElement element, XmlReader reader) where TModelElement : ModelElement;
    }
}