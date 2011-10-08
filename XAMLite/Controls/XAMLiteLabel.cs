using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/ms611056.aspx"/>
    public class XAMLiteLabel : XAMLiteControl
    {

        /// <summary>
        /// 
        /// </summary>
        protected SpriteFont spriteFont;

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
                if (this.spriteFont != null)
                {
                    RecalculateWidthAndHeight(value);
                }
                base.Text = value;

            }
        }

        /// <summary>
        /// This just duplicates the Text property but is here since XAML developer will expect to be able
        /// to set the Content property of a label.
        /// </summary>
        public string Content {
            get
            {
                return this.Text;
            }

            set
            {
                this.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Color Color { get; set; }

        public XAMLiteLabel(Game game)
            : base(game)
        {

            //
            this.Text = string.Empty;

            //
            this.Color = Color.White;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteLabel ( Game game, string text )
            : base( game )
        {
            
            //
            this.Text = text;

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
            RecalculateWidthAndHeight( this.Text );
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        private void RecalculateWidthAndHeight(string text)
        {
            this.Width = (int)this.spriteFont.MeasureString(text).X;
            this.Height = (int)this.spriteFont.MeasureString(text).Y;
        }
    }

}