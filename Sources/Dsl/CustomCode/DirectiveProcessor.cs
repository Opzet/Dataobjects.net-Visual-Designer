using System.Collections.Generic;
using TXSoftware.DataObjectsNetEntityModel.Common.Modeling;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    public partial class DONetEntityModelDesignerDirectiveProcessor
    {
        public override string[] GetReferencesForProcessingRun()
        {
            string[] baseReferences = base.GetReferencesForProcessingRun();
            List<string> references = new List<string>(baseReferences);

            references.Add(typeof(IElementEventsHandler).Assembly.Location);

            return references.ToArray();
        }       
    }
}