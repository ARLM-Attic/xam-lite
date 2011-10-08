using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace XAMLite
{

    /// <summary>
    /// 
    /// </summary>
    public class ffxImage : ffxControl
    {

        /// <summary>
        /// 
        /// </summary>
        private string assetName;

        /// <summary>
        /// 
        /// </summary>
        Texture2D texture;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="assetName"></param>
        public ffxImage ( Game game, string assetName )
            : base( game )
        {
            this.assetName = assetName;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent ()
        {
            base.LoadContent();

            this.texture = Game.Content.Load<Texture2D>( this.assetName );
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