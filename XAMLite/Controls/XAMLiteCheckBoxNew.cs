using System.Diagnostics;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    using System.Windows.Media;
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteCheckBox class until this class replaces it.
    /// </summary>
    public class XAMLiteCheckBoxNew : XAMLiteBaseContent
    {
        /// <summary>
        /// True when the checkbox is checked.
        /// </summary>
        public bool IsChecked { get; set; }

        /// <summary>
        /// Container for the checkbox texture asset.
        /// </summary>
        private Rectangle _checkBox;

        /// <summary>
        /// Texture asset for the checkbox when it is unchecked.
        /// </summary>
        private Texture2D _texture;

        /// <summary>
        /// This is the image file path, minus the file extension for an 
        /// unchecked checkbox.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Texture asset for the checkbox when it is hovered over but 
        /// unchecked.
        /// </summary>
        private Texture2D _hoverTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over but is unchecked.
        /// </summary>
        public string HoverSourceName { get; set; }

        /// <summary>
        /// Texture asset for a checked checkbox.
        /// </summary>
        private Texture2D _checkedTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for when the 
        /// checkbox is checked and not hovered over.
        /// </summary>
        public string CheckedSourceName { get; set; }

        /// <summary>
        /// Texture asset for a hovered and checked checkbox.
        /// </summary>
        private Texture2D _checkedHoverTexture;

        /// <summary>
        /// This is the image file path, minus the file extension for when the
        /// checkbox is hovered over and checked.
        /// </summary>
        public string HoverCheckedSourceName { get; set; }

        /// <summary>
        /// The content portion of the CheckBox.
        /// </summary>
        private XAMLiteLabelNew label;

        /// <summary>
        /// True once the assets for the XAMLiteControl have been positioned.
        /// </summary>
        private bool _isPositioned;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteCheckBoxNew(Game game)
            : base(game)
        {
            IsChecked = false;

            SourceName = "Icons/RadioButton";
            CheckedSourceName = "Icons/RadioButtonSelected";

            Spacing = 2;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            MouseDown += XAMLiteCheckBoxMouseDown;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            Debug.Assert((SourceName != null), "Must set CheckBoxSourceName property. This is the image file path, minus the file extension.");
            _texture = Game.Content.Load<Texture2D>(SourceName);

            Debug.Assert((CheckedSourceName != null), "Must set CheckBoxSelectedSourceName property. This is the image file path, minus the file extension.");
            _checkedTexture = Game.Content.Load<Texture2D>(CheckedSourceName);

            if (HoverSourceName != null)
            {
                _hoverTexture = Game.Content.Load<Texture2D>(HoverSourceName);
            }

            if (HoverCheckedSourceName != null)
            {
                _checkedHoverTexture = Game.Content.Load<Texture2D>(HoverCheckedSourceName);
            }

            label = new XAMLiteLabelNew(Game)
                {
                    Content = Content,
                    Foreground = Foreground,
                    HorizontalAlignment = HorizontalAlignment,
                    VerticalAlignment = VerticalAlignment,
                    AttachedToOtherControl = true,
                    Padding = new Thickness(5, 0, 0, 0),
                    FontFamily = FontFamily,
                    Spacing = Spacing
                };

            Game.Components.Add(label);

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!_isPositioned)
            {
                SetPosition();
                _isPositioned = true;
            }
        }

        /// <summary>
        /// In this case there is no call to the base class to Draw(), 
        /// otherwise the Content gets drawn twice.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (IsEnabled)
                {
                    if (_checkedHoverTexture != null && _hoverTexture != null)
                    {
                        SpriteBatch.Draw(MouseEntered ? IsChecked ? _checkedHoverTexture : _hoverTexture :
                        IsChecked ? _checkedTexture : _texture,
                        _checkBox,
                        (Color.White * (float)Opacity));
                    }
                    else
                    {
                        SpriteBatch.Draw(
                        IsChecked ? _checkedTexture : _texture,
                        _checkBox,
                        (Color.White * (float)Opacity));
                    }
                }
                else
                {
                    var opacity = (float)Opacity - 0.5f;
                    if (opacity < 0f)
                    {
                        opacity = 0f;
                    }

                    SpriteBatch.Draw(_texture, _checkBox, (Color.White * opacity));
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Sets the position of the control along with all of its assets.
        /// </summary>
        protected void SetPosition()
        {
            if (_checkedTexture != null)
            {
                Width = _checkedTexture.Width + 5 + (int)SpriteFont.MeasureString(label.Content.ToString()).X;
                _checkBox = new Rectangle(
                    (int)Position.X, (int)Position.Y, _checkedTexture.Width, _checkedTexture.Height);
                label.Margin = new Thickness(Position.X + _checkBox.Width, Position.Y + ((float)_checkedTexture.Height / 2) - (SpriteFont.MeasureString(Content.ToString()).Y / 2), Margin.Right, Margin.Bottom);
                //label.Margin = new Thickness(Position.X + _checkBox.Width, Position.Y, Margin.Right, Margin.Bottom);
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XAMLiteCheckBoxMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEnabled)
            {
                IsChecked = !IsChecked;
            }
        }

        /// <summary>
        /// Dispose of the XAMLiteLabel that is used for the Content portion of
        /// the control.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (label != null)
            {
                label.Dispose();
            }
        }
    }
}
