using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Html
{
    /// <summary>
    /// Represents a word inside an inline box
    /// </summary>
    /// <remarks>
    /// Because of performance, words of text are the most atomic 
    /// element in the project. It should be characters, but come on,
    /// imagine the performance when drawing char by char on the device.
    /// 
    /// It may change for future versions of the library
    /// </remarks>
    internal class CssBoxWord
        : CssRectangle
    {
        #region Fields


        private string _word;
        private PointF _lastMeasureOffset;
        private CssBox _ownerBox;
        private int _spacesAfter;
        private bool _breakAfter;
        private int _spacesBefore;
        private bool _breakBefore;
        private Image _image;


        #endregion

        #region Ctor

        internal CssBoxWord(CssBox owner)
        {
            _ownerBox = owner;
            _word = string.Empty;
        }

        /// <summary>
        /// Creates a new BoxWord which represents an image
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="image"></param>
        public CssBoxWord(CssBox owner, Image image)
            : this(owner)
        {
            Image = image;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the width of the word including white-spaces
        /// </summary>
        public float FullWidth
        {
            //get { return OwnerBox.ActualWordSpacing * (SpacesBefore + SpacesAfter) + Width; }
            get { return Width; }
        }

        /// <summary>
        /// Gets the image this words represents (if one)
        /// </summary>
        public Image Image
        {
            get { return _image; }
            set 
            { 
                _image = value;

                if (value != null)
                {
                    CssLength w = new CssLength(OwnerBox.Width);
                    CssLength h = new CssLength(OwnerBox.Height);
                    if (w.Number > 0 && w.Unit == CssLength.CssUnit.Pixels)
                    {
                        Width = w.Number;
                    }
                    else
                    {
                        Width = value.Width;
                    }

                    if (h.Number > 0 && h.Unit == CssLength.CssUnit.Pixels)
                    {

                        Height = h.Number;
                    }
                    else
                    {
                        Height = value.Height;
                    }

                    Height += OwnerBox.ActualBorderBottomWidth + OwnerBox.ActualBorderTopWidth + OwnerBox.ActualPaddingTop + OwnerBox.ActualPaddingBottom;
                    
                }
            }
        }

        /// <summary>
        /// Gets if the word represents an image.
        /// </summary>
        public bool IsImage
        {
            get { return Image != null; }
        }

        /// <summary>
        /// Gets a bool indicating if this word is composed only by spaces.
        /// Spaces include tabs and line breaks
        /// </summary>
        public bool IsSpaces
        {
            get { return string.IsNullOrEmpty(Text.Trim()); }
        }

        /// <summary>
        /// Gets if the word is composed by only a line break
        /// </summary>
        public bool IsLineBreak
        {
            get { return Text == "\n"; }
        }

        /// <summary>
        /// Gets if the word is composed by only a tab
        /// </summary>
        public bool IsTab
        {
            get { return Text == "\t"; }
        }

        /// <summary>
        /// Gets the Box where this word belongs.
        /// </summary>
        public CssBox OwnerBox
        {
            get { return _ownerBox; }
        }

        /// <summary>
        /// Gets the text of the word
        /// </summary>
        public string Text
        {
            get { return _word; }
        }

        /// <summary>
        /// Gets or sets an offset to be considered in measurements
        /// </summary>
        internal PointF LastMeasureOffset
        {
            get { return _lastMeasureOffset; }
            set { _lastMeasureOffset = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Removes line breaks and tabs on the text of the word,
        /// replacing them with white spaces
        /// </summary>
        internal void ReplaceLineBreaksAndTabs()
        {
            _word = _word.Replace('\n', ' ');
            _word = _word.Replace('\t', ' ');
        }

        internal void ProcessHtmlSpecialCharacters()
        {
            _word = _word.Replace("&lt;", "<").Replace("&gt;", ">");
        }

        /// <summary>
        /// Appends the specified char to the word's text
        /// </summary>
        /// <param name="c"></param>
        internal void AppendChar(char c)
        {
            _word += c;
        }

        /// <summary>
        /// Represents this word for debugging purposes
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            
            return string.Format("{0} ({1} char{2})", Text.Replace(' ', '-').Replace("\n", "\\n"), Text.Length, Text.Length != 1 ? "s" : string.Empty);
        }

        #endregion
    }
}
