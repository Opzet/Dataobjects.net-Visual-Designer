using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace TXSoftware.DataObjectsNetEntityModel.Common.UIEditors.Components
{
/*
    /// <summary>Bevel border style.</summary>
    public enum BevelStyle
    {
        /// <summary>Lowered border.</summary>
        Lowered,
        /// <summary>Raised border.</summary>
        Raised,
        /// <summary>No border.</summary>
        Flat
    }
*/

    /// <summary>
    /// A bevel control.
    /// </summary>
    public class BevelControl : Control
    {
        private const Border3DSide DefaultShape = Border3DSide.All;
        //private const BevelStyle DefaultStyle = BevelStyle.Lowered;
        private const Border3DStyle DefaultStyle = Border3DStyle.SunkenOuter;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private Container components;

        private Border3DSide sides;

        //private BevelStyle style;
        private Border3DStyle style;
        private SolidBrush backColorBrush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));

        /// <summary>
        /// Initializes a new instance of the <see cref="BevelControl"/> class.
        /// </summary>
        public BevelControl()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            style = DefaultStyle;
            sides = DefaultShape;
        }

        /// <summary>
        /// Gets or sets the shape of the bevel.
        /// </summary>
        [DefaultValue(DefaultShape)]
        [Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
        public Border3DSide Sides
        {
            get { return sides; }
            set
            {
                sides = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the style of the bevel.
        /// </summary>
        [DefaultValue(DefaultStyle)]
        public Border3DStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                Invalidate();
            }
        }

        public new Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                if (value != base.BackColor)
                {
                    base.BackColor = value;
                    backColorBrush = new SolidBrush(value);
                }
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>Paints the rule.</summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Create a local version of the graphics object for the Bevel.
            Graphics graphics = e.Graphics;
            Rectangle rect = ClientRectangle;

            ControlPaint.DrawBorder3D(graphics, rect.Left, rect.Top, rect.Width, rect.Height, this.Style, this.Sides);
            //DrawBorder(graphics, rectangle, this.Sides);

            rect.Inflate(-2, -2);

            graphics.FillRectangle(backColorBrush, rect);

            // Calling the base class OnPaint
            base.OnPaint(e);
        }

/*
        private void DrawBorder(Graphics graphics, Rectangle rectangle, Border3DSide side)
        {
            switch (side)
            {
                case Border3DSide.All:
                    DrawBorder(graphics, rectangle, Border3DSide.Top);
                    DrawBorder(graphics, rectangle, Border3DSide.Bottom);
                    DrawBorder(graphics, rectangle, Border3DSide.Left);
                    DrawBorder(graphics, rectangle, Border3DSide.Right);
                    break;
                case Border3DSide.Left:
                    ControlPaint.DrawBorder3D(graphics, rectangle.Left, rectangle.Top, 2, rectangle.Height, this.Style);
                    break;
                case Border3DSide.Top:
                    ControlPaint.DrawBorder3D(graphics, rectangle.Left, rectangle.Top, rectangle.Width, 2, this.Style);
                    break;
                case Border3DSide.Bottom:
                    ControlPaint.DrawBorder3D(graphics, rectangle.Left, rectangle.Bottom - 2, rectangle.Width, 2,
                                              this.Style);
                    break;
                case Border3DSide.Middle:
                    break;
                case Border3DSide.Right:
                    ControlPaint.DrawBorder3D(graphics, rectangle.Right - 2, rectangle.Top, 2, rectangle.Height,
                                              this.Style);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
*/

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}