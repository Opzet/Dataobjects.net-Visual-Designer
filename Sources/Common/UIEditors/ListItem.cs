using System.Collections.Generic;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors
{
    public sealed class ListItem
    {
        public string Text { get; set; }

        public object Value { get; set; }

        public ListItem()
        {
        }

        public ListItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class ListItemCollection : List<ListItem>
    {
        public void SortByText()
        {
            this.Sort((item1, item2) => string.Compare(item1.Text, item2.Text));
        }

        public void SortByValue()
        {
            this.Sort((item1, item2) => Comparer<object>.Default.Compare(item1.Value, item2.Value));
        }
    }
}