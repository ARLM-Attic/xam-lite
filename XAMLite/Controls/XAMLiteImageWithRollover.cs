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

            this.rolloverTexture = Game.Content.Load<Texture2D>(RolloverSourceName);
            this.Width = this.texture.Width;
            this.Height = this.texture.Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == System.Windows.Visibility.Visible)
            {
                this.SpriteBatch.Begin();

                if (MouseEntered)
                    this.SpriteBatch.Draw(this.rolloverTexture, Panel, (Color.White * (float)Opacity));
                else
                    this.SpriteBatch.Draw(this.texture, Panel, (Color.White * (float)Opacity));

                this.SpriteBatch.End();
            }
        }
    }
}
