using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XAMLite
{

    /// <summary>
    /// 
    /// </summary>
    public class XAMLiteLabelWithShadow : XAMLiteLabel
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="initialText"></param>
        public XAMLiteLabelWithShadow ( Game game, string initialText )
            : base( game, initialText )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw ( GameTime gameTime )
        {
            
            // Begin.
            this.spriteBatch.Begin();

            // Draw shadow text.
            var shadowPos = Position + new Vector2( 1, 1 );
            this.spriteBatch.DrawString(this.spriteFont, this.Text, shadowPos, Color.Black);

            // Draw text.
            this.spriteBatch.DrawString(this.spriteFont, this.Text, this.Position, this.Color);

            // End.
            this.spriteBatch.End();

        }

    }

}