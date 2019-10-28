using System.Drawing;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components
{
    public sealed class IconListEntry
    {
        #region properties

        public Image Icon { get; set; }

        public string Text { get; set; }

        public object Value { get; set; }

        #endregion properties

        #region constructors 

        public IconListEntry()
        {

        }

        public IconListEntry(string text, object value) : this(text, value, null)
        {
        }

        public IconListEntry(string text, Image icon) : this(text, null, icon)
        {
        }

        public IconListEntry(string text, object value, Image icon)
        {
            Text = text;
            Value = value;
            Icon = icon;
        }

        #endregion constructors

        #region methods 

        public override string ToString()
        {
            return Text;
        }

        #endregion methods
    }
}