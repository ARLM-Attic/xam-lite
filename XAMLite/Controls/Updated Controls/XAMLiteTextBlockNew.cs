using System;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace XAMLite
{
    /// <summary>
    /// This is the wrappable text chunk, which doesn't support user input.
    /// Here are some projects that might have good references for this control:
    /// http://simplegui.codeplex.com/
    /// http://xnagui.codeplex.com/
    /// http://xnatoolgui.codeplex.com/
    /// http://neoforce.codeplex.com/
    /// </summary>
    public class XAMLiteTextBlockNew : XAMLiteGridNew
    {
        // for WPF developers, in case they do not load Run in the Constructor.
        // They could then type: textblock.Run
        // I do not see that a Run is still included in WPF.
        public string Run
        {
            get { return Text; }
            set { Text = value; }
        }

        private string _text;

        /// <summary>
        /// Sets the content of the label within the text box.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }

            set
            {
                _text = value;

                if (_textLabel != null)
                {
                    _textLabel.Content = value;
                }
            }
        }

        /// <summary>
        /// Specifies whether text wraps when it reaches the edge of the containing box.
        /// </summary>
        public TextWrapping TextWrapping { get; set; }

        /// <summary>
        /// Specifies the alignment of the text.
        /// </summary>
        public TextAlignment TextAlignment { get; set; }

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        private FontFamily _fontFamily;

        ///// <summary>
        ///// True when the font family has changed.
        ///// </summary>
        //private bool _fontFamilyChanged;

        /// <summary>
        /// The font family the text belongs to.
        /// </summary>
        public FontFamily FontFamily
        {
            get
            {
                return _fontFamily;
            }

            set
            {
                _fontFamily = value;
                //_fontFamilyChanged = true;
            }
        }

        /// <summary>
        /// Character spacing for the font
        /// </summary>
        public int Spacing { get; set; }

        /// <summary>
        /// Amount of space between the text and the edge of the background 
        /// that contains it.
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// The color of the content, whether text or some other object.
        /// </summary>
        public Brush Foreground { get; set; }

        /// <summary>
        /// The text contained in the text box.
        /// </summary>
        private XAMLiteLabelNew _textLabel;

        /// <summary>
        /// Background fo the text block.
        /// </summary>
        private XAMLiteRectangleNew _background;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteTextBlockNew(Game game)
            : base(game)
        {
            Text = "TextBlock";
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            FontFamily = new FontFamily("Verdana12"); 
            Foreground = Brushes.Black;
            Padding = new Thickness(5, 2, 5, 5);
            Spacing = 2;
            Width = 50;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="run"> </param>
        public XAMLiteTextBlockNew(Game game, Run run)
            : base(game)
        {
            Text = run.TextBlock; 
            FontFamily = new FontFamily("Verdana12");
            TextAlignment = TextAlignment.Left;
            TextWrapping = TextWrapping.Wrap;
            Foreground = Brushes.Black;
            Padding = new Thickness(5, 2, 5, 5);
            Spacing = 2;
            Width = 50;
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _textLabel = new XAMLiteLabelNew(Game)
            {
                Content = Text,
                HorizontalAlignment = TextAlignment == TextAlignment.Left
                    || TextAlignment == TextAlignment.Justify ?
                    HorizontalAlignment.Left : TextAlignment == TextAlignment.Center ?
                    HorizontalAlignment.Center : HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontFamily = FontFamily,
                Spacing = Spacing,
                Foreground = Foreground,
                Padding = Padding,
                Width = Width
            };

            if (Text != string.Empty || _textLabel.Width == 0)
            {
                // Add to the game so that it has a measureable size.
                Game.Components.Add(_textLabel);
                UpdateForTextWrapping();
            }

            UpdateWidthAndHeight();

            if (Game.Components.Contains(_textLabel))
            {
                Game.Components.Remove(_textLabel);
            }

            _background = new XAMLiteRectangleNew(Game)
                {
                    Fill = Background,
                    Width = Width,
                    Height = Height,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };
            Children.Add(_background);

            Children.Add(_textLabel);
        }

        /// <summary>
        /// Updates the Width and Height of the control.
        /// </summary>
        private void UpdateWidthAndHeight()
        {
            //var w = (int)_textLabel.MeasureString().X + (int)_textLabel.Padding.Left + (int)_textLabel.Padding.Right;
            //Console.WriteLine("w: " + w);
            //Console.WriteLine("Width: " + Width);
            //Width = Width < w ? w : Width;

            //Console.WriteLine("Final Width: " + Width);

            var h = (int)_textLabel.MeasureString().Y + (int)_textLabel.Padding.Top + (int)_textLabel.Padding.Bottom;
            Height = Height < h ? h : Height;
        }

        /// <summary>
        /// Word wraps the text when TextWrapping = Wrap.
        /// </summary>
        private void UpdateForTextWrapping()
        {
            // When applicable, wrap the text and create the label.
            if (TextWrapping == TextWrapping.Wrap)
            {
                Text = WordWrapper.Wrap(Text, Width, (int)_textLabel.MeasureString().X, _textLabel.Padding);
            }
        }
    }
}
