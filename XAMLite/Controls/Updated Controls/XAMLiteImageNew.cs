using System.Diagnostics;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace XAMLite
{
    using System;

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
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteImageNew(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            Debug.Assert((SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension.");
            Texture = Game.Content.Load<Texture2D>(SourceName);

            if (Width == 0)
            {
                Width = Texture.Width;
            }

            if (Height == 0)
            {
                Height = Texture.Height;
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


            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (RenderTransform == null)
                {
                    SpriteBatch.Draw(Texture, Panel, Color.White * (float)Opacity);
                }
                else
                {
                    SpriteBatch.Draw(Texture, Panel, null, Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
                }

                SpriteBatch.End();
            }
        }
    }
}