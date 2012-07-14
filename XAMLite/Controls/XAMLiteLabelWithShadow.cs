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
            if (Visible == System.Windows.Visibility.Visible)
            {
                // Begin.
                SpriteBatch.Begin();

                // Draw shadow text.
                var shadowPos = Position + new Vector2(1, 1);
                SpriteBatch.DrawString(this.SpriteFont, this.Text, shadowPos, (Color.Black * (float)Opacity));

                // Draw text.
                SpriteBatch.DrawString(this.SpriteFont, this.Text, this.Position, (this._foregroundColor * (float)Opacity));

                // End.
                SpriteBatch.End();
            }
        }

    }

}