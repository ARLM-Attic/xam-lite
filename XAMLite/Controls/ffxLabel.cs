using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ForgefxnaLib
{

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ms611056.aspx"/>
    public class ffxLabel : ffxControl
    {

        /// <summary>
        /// 
        /// </summary>
        protected SpriteFont spriteFont;

        /// <summary>
        /// 
        /// </summary>
        private string initialText;

        /// <summary>
        /// 
        /// </summary>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this.Width = (int)this.spriteFont.MeasureString( value ).X;
                this.Height = (int)this.spriteFont.MeasureString( value ).Y;
                base.Text = value;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public ffxLabel ( Game game, string initialText )
            : base( game )
        {
            
            //
            this.initialText = initialText;
            
            //
            this.Color = Color.White;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="content"></param>
        /// <param name="fontName"></param>
        protected override void LoadContent ()
        {
            this.spriteFont = Game.Content.Load<SpriteFont>( "Fonts/Courier10" );
            this.Text = this.initialText;
            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw ( GameTime gameTime )
        {
            spriteBatch.Begin();
            spriteBatch.DrawString( this.spriteFont, Text, Position, this.Color );
            spriteBatch.End();
        }

    }

}