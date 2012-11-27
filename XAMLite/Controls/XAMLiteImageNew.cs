using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private Texture2D _texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { get; set; }

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
            _texture = Game.Content.Load<Texture2D>(SourceName);

            if (Width == 0)
            {
                Width = _texture.Width;
            }

            if (Height == 0)
            {
                Height = _texture.Height;
            }

            base.LoadContent();
            Console.WriteLine(Margin);
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

                SpriteBatch.Draw(_texture, Panel, Color.White * (float)Opacity);

                SpriteBatch.End();
            }
        }
    }
}