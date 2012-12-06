using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    public class XAMLiteRectangleNew : XAMLiteBaseControl
    {
        /// <summary>
        /// 
        /// </summary>
        private Color _fill;

        /// <summary>
        /// The background or inner brush color of the rectangle, without a border.
        /// </summary>
        public Brush Fill
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _fill = new Color(color.R, color.G, color.B, color.A);
            }
        }

        /// <summary>
        /// Used for XNA drawing.
        /// </summary>
        private Color _strokeColor;

        /// <summary>
        /// Used for returning the current Brush color.
        /// </summary>
        private Brush _strokeBrush;

        /// <summary>
        /// The border brush color of the Rectangle.
        /// </summary>
        public Brush Stroke
        {
            get
            {
                return _strokeBrush;
            }

            set
            {
                if (value != null)
                {
                    _strokeBrush = value;
                    var solidBrush = (SolidColorBrush)value;
                    var color = solidBrush.Color;
                    _strokeColor = new Color(color.R, color.G, color.B, color.A);
                }
            }
        }

        /// <summary>
        /// The border width of the rectangle.
        /// </summary>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteRectangleNew(Game game)
            : base(game)
        {
            _fill = Color.Transparent;
            _strokeColor = Color.Transparent;
            StrokeThickness = 0;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (Width == 0)
            {
                Width = Viewport.Width - (int)Margin.Left - (int)Margin.Right;
            }

            if (Height == 0)
            {
                Height = Viewport.Height - (int)Margin.Top - (int)Margin.Bottom;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visibility == Visibility.Visible)
            {
                SpriteBatch.Begin();
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                SpriteBatch.Draw(Pixel, Panel, _fill * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, (int)Math.Round(StrokeThickness));
                SpriteBatch.Draw(Pixel, Panel, _strokeColor * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y + Height - (int)Math.Round(StrokeThickness), Width, (int)Math.Round(StrokeThickness));
                SpriteBatch.Draw(Pixel, Panel, _strokeColor * (float)Opacity);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, (int)StrokeThickness, Height);
                SpriteBatch.Draw(Pixel, Panel, _strokeColor * (float)Opacity);
                Panel = new Rectangle((int)Position.X + Width - (int)Math.Round(StrokeThickness), (int)Position.Y, (int)Math.Round(StrokeThickness), Height);
                SpriteBatch.Draw(Pixel, Panel, _strokeColor * (float)Opacity);
                SpriteBatch.End();
            }
        }
    }
}
