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
        /// 
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { 
            get; 
            set; 
        }

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
        /// 
        /// </summary>
        protected override void LoadContent ()
        {
            base.LoadContent();

            Debug.Assert( ( SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension." );
            this.texture = Game.Content.Load<Texture2D>( SourceName );
            this.Width = this.texture.Width;
            this.Height = this.texture.Height;
            _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (timeForUpdate)
            {
                timeForUpdate = false;
                _panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw ( GameTime gameTime )
        {
            //
            if (Visible == System.Windows.Visibility.Visible)
            {
                this.spriteBatch.Begin();

                // Opacity
                float opacity = (float)this.Opacity;

                //
                this.spriteBatch.Draw(this.texture, _panel, (Color.White * opacity));

                //
                this.spriteBatch.End();
            }
        }
    }

}