using System;
using System.Drawing;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components
{
    internal sealed class IconItemPaintHelper
    {
        public Image DefaultImage { get; set; }

        public void DrawItem(DrawItemEventArgs e, object item)
        {
            if (e.Index == -1)
            {
                return;
            }

            if (!(item is IconListEntry))
            {
                if (item is Tuple<String, Int32>)
                {
                    var vp = (Tuple<String, Int32>)item;
                    item = new IconListEntry { Text = vp.Item1, Value = vp.Item2 };
                }
                else
                {
                    return;
                }
            }

            Rectangle fullRect = new Rectangle(e.Bounds.X + 1, e.Bounds.Y,
                e.Bounds.Width - 1, e.Bounds.Height);
            Brush textBrush = SystemBrushes.ControlText;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, fullRect);
                textBrush = SystemBrushes.HighlightText;
            }
            else if ((e.State & DrawItemState.Disabled) == DrawItemState.Disabled)
            {
                e.Graphics.FillRectangle(SystemBrushes.Control, fullRect);
                textBrush = SystemBrushes.GrayText;
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
            }

            Point imagePoint = new Point(e.Bounds.X + 2, e.Bounds.Y);
            IconListEntry entry = (IconListEntry)item;
            Bitmap bmp = (Bitmap)(entry.Icon ?? DefaultImage);

            if (bmp != null)
            {
                bmp.MakeTransparent(Color.Magenta);
                e.Graphics.DrawImage(bmp, imagePoint);
            }

            e.Graphics.DrawString(entry.Text, ComboBox.DefaultFont, textBrush,
                new Point(e.Bounds.X + 20, e.Bounds.Y + 1));
        }
    }
}