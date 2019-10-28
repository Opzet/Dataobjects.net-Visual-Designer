using System.Drawing;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components
{
    public sealed class IconComboBox : ComboBox
    {
        #region fields

        private readonly IconItemPaintHelper helper = new IconItemPaintHelper();

        #endregion fields

        #region properties

        public Image DefaultImage
        {
            get { return helper.DefaultImage; }
            set { helper.DefaultImage = value; }
        }

        #endregion properties
        
        #region constructors

        public IconComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawVariable;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer, true);
        }

        #endregion constructors

        #region methods

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            e.ItemHeight = base.ItemHeight;
        }


        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            helper.DrawItem(e, e.Index > -1 ? Items[e.Index] : null);
        }

        #endregion methods



    }
}