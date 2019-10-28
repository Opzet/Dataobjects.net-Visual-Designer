using Microsoft.VisualStudio.TextTemplating.VSHost;

namespace TXSoftware.DataObjectsNetEntityModel.Dsl
{
    // This is a workaround for a known bug that the setup project doesn't
    // register the text templates directory automatically.
    // See http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=1725215&SiteID=1
    // for more information.

    [ProvideIncludeFolder(".tt", 1, "TextTemplates")]
    internal sealed partial class DONetEntityModelDesignerPackage : DONetEntityModelDesignerPackageBase
    {
        
    }
}