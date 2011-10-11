using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;


namespace XAMLite
{
    public class XAMLiteImageWithRollover : XAMLiteImage
    {

        /// <summary>
        /// This is the image file path, minus the file extension for the Rollover image.
        /// </summary>
        public string RolloverSourceName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        Texture2D rolloverTexture;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="assetName"></param>
        public XAMLiteImageWithRollover ( Game game  )
            : base( game )
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            this.texture = Game.Content.Load<Texture2D>(SourceName);
            this.rolloverTexture = Game.Content.Load<Texture2D>(RolloverSourceName);
            this.Width = this.texture.Width;
            this.Height = this.texture.Height;
            rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            //
            this.spriteBatch.Begin();

            // Opacity
            float opacity = (float)this.Opacity;

            //
            if(_mouseEnter)
                this.spriteBatch.Draw(this.rolloverTexture, rect, (Color.White * opacity));
            else
                this.spriteBatch.Draw(this.texture, rect, (Color.White * opacity));

            //
            this.spriteBatch.End();
        }
    }
}
