using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Emulates the code behind for a xaml image.
    /// </summary>
    public class XAMLiteImage : XAMLiteControl
    {
        /// <summary>
        /// The 2-D image.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteImage(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

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

            // Sets the size and location of the image.
            Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// Updates the XAMLiteImage.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// Draws the XAMLiteImage to the screen according to its size and opacity.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            //
            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                SpriteBatch.Draw(Texture, Panel, Color.White * (float)Opacity);

                SpriteBatch.End();
            }
        }
    }
}