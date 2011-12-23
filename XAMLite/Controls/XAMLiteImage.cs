using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    public class XAMLiteImage : XAMLiteControl
    {
        /// <summary>
        /// The 2-D image.
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="assetName"></param>
        public XAMLiteImage ( Game game  )
            : base( game )
        {
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Loads the content.
        /// </summary>
        protected override void LoadContent ()
        {
            base.LoadContent();

            Debug.Assert( ( SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension." );
            this.texture = Game.Content.Load<Texture2D>( SourceName );

            if (this.Width == 0)
            {
                this.Width = this.texture.Width;
            }
            if (this.Height == 0)
            {
                this.Height = this.texture.Height;
            }

            // Sets the size and location of the image.
            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        /// <summary>
        /// Updates the XAMLiteImage.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (marginChanged)
            {
                marginChanged = false;
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }
            
        }

        /// <summary>
        /// Draws the XAMLiteImage to the screen according to its size and opacity.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw ( GameTime gameTime )
        {
            //
            if (Visible == System.Windows.Visibility.Visible)
            {
                this.spriteBatch.Begin();

                this.spriteBatch.Draw(this.texture, _panel, (Color.White * (float)Opacity));

                this.spriteBatch.End();
            }
        }
    }
}