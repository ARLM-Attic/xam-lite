using System.Diagnostics;
using System.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Used by the XAMLiteImage class for transforming an image.
    /// </summary>
    public enum RenderTransform
    {
        FlipHorizontal,

        FlipVertical,

        Normal,

        RotateClockwise90,

        RotateCounterClockwise90,

        Rotate180,
    }

    /// <summary>
    /// Emulates the code behind for the WPF image class.
    /// 
    /// Note: Currently under development.  Continue to use normal
    /// XAMLiteImage class until this class replaces it.
    /// </summary>
    public class XAMLiteImageNew : XAMLiteBaseControl
    {
        /// <summary>
        /// The 2-D image.
        /// </summary>
        protected internal Texture2D Texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Applies a render transform to the button.
        /// </summary>
        public RenderTransform RenderTransform;

        /// <summary>
        /// 
        /// </summary>
        private RenderTransform _previousRenderTransform;


        /// <summary>
        /// True when a background has been set for the image. This is primitive
        /// and only the color over the top of the image.
        /// </summary>
        protected internal bool IsColorized;

        /// <summary>
        /// True when the image is the edge of a default button.
        /// Notifies that the opacity should change.
        /// </summary>
        protected internal bool IsEdge;

        /// <summary>
        /// True when the image is the top edge of a default button.
        /// Notifies that the opacity should change.
        /// </summary>
        protected internal bool IsTopEdge;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteImageNew(Game game)
            : base(game)
        {
            RenderTransform = RenderTransform.Normal;
            _previousRenderTransform = RenderTransform;
        }

        /// <summary>
        /// Constructor that includes a loaded texture.  This can be used when 
        /// complex controls are being created so that, if the Texture2D was 
        /// already loaded from disk, it doesn't need to be loaded again.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture"> </param>
        public XAMLiteImageNew(Game game, Texture2D texture)
            : base(game)
        {
            Texture = texture;
            RenderTransform = RenderTransform.Normal;
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            if (Texture == null)
            {
                Debug.Assert((SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension.");
                Texture = Game.Content.Load<Texture2D>(SourceName);
            }

            if (Width == 0)
            {
                Width = Texture.Width;
            }

            if (Height == 0)
            {
                Height = Texture.Height;
            }

            if (Background != null)
            {
                IsColorized = true;
            }

            UpdatePanel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdatePanel();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UpdatePanel()
        {
            if (RenderTransform == _previousRenderTransform)
            {
                return;
            }

            switch (RenderTransform)
            {
                case RenderTransform.RotateClockwise90:
                case RenderTransform.RotateCounterClockwise90:
                    var h = Height;
                    Height = Width;
                    Width = h;
                    break;
            }

            _previousRenderTransform = RenderTransform;
        }

        /// <summary>
        /// Draws the XAMLiteImage to the screen according to its size and opacity.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();

            // NOTE:  When an image is rotated, the image currently cannot have an Alignment = Stretch
            switch (RenderTransform)
            {
                case RenderTransform.Normal:
                    SpriteBatch.Draw(Texture, Panel, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity);
                    break;
                case RenderTransform.FlipHorizontal:
                    SpriteBatch.Draw(Texture, Panel, null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                    break;
                case RenderTransform.FlipVertical:
                    SpriteBatch.Draw(Texture, Panel, null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0);
                    break;
                case RenderTransform.RotateClockwise90:
                    SpriteBatch.Draw(Texture, new Vector2(Panel.X, Panel.Y), null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, MathHelper.PiOver2, new Vector2(0, Panel.Width), 1, SpriteEffects.None, 1);
                    break;
                case RenderTransform.RotateCounterClockwise90:
                    SpriteBatch.Draw(Texture, new Vector2(Panel.X, Panel.Y), null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, -MathHelper.PiOver2, new Vector2(Panel.Height, 0), 1, SpriteEffects.None, 1);
                    break;
                case RenderTransform.Rotate180:
                    SpriteBatch.Draw(Texture, new Vector2(Panel.X, Panel.Y), null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, MathHelper.Pi, new Vector2(Panel.Width, Panel.Height), 1, SpriteEffects.None, 1);
                    break;
            }

            SpriteBatch.End();
        }
    }
}