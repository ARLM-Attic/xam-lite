using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    /// <summary>
    /// Emulates the code behind for a xaml image.
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
        public ScaleTransform RenderTransform;

        /// <summary>
        /// True when a background has been set for the image. This is primitive
        /// and only the color over the top of the image.
        /// </summary>
        protected bool IsColorized;

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
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
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

            base.LoadContent();
        }

        /// <summary>
        /// Draws the XAMLiteImage to the screen according to its size and opacity.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visibility == Visibility.Hidden)
            {
                return;
            }

            SpriteBatch.Begin();

            if (RenderTransform == null)
            {
                SpriteBatch.Draw(Texture, Panel, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity);
            }
            else
            {
                SpriteBatch.Draw(Texture, Panel, null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            SpriteBatch.End();              
        }
    }
}