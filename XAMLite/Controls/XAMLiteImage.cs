using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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
        Texture2D texture;

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

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw ( GameTime gameTime )
        {
            
            //
            this.spriteBatch.Begin();
            
            // Opacity
            float opacity = (float)this.Opacity;

            //
            Rectangle rect = new Rectangle(  (int)this.Position.X, (int)this.Position.Y, this.Width, this.Height );
            this.spriteBatch.Draw(this.texture, rect, (Color.White * opacity));
            
            //
            this.spriteBatch.End();

        }

    }

}