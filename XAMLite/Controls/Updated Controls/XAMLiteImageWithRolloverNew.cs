using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XAMLite
{
    /// <summary>
    /// Emulates a xaml image with rollover
    /// </summary>
    public class XAMLiteImageWithRolloverNew : XAMLiteImageNew
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
        /// Texture for when the mouse hovers on the control.
        /// </summary>
        private Texture2D _rolloverTexture;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteImageWithRolloverNew(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Constructor that includes a preloaded Texture2D for the
        /// normal state of the rollover image.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="texture"> </param>
        public XAMLiteImageWithRolloverNew(Game game, Texture2D texture)
            : base(game, texture)
        {
        }

        /// <summary>
        /// Loads the content for the control.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            _rolloverTexture = Game.Content.Load<Texture2D>(RolloverSourceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visibility != System.Windows.Visibility.Visible) // || GridIsHidden)
            {
                return;
            }

            SpriteBatch.Begin();
            if (RenderTransform == null)
            {
                SpriteBatch.Draw(MouseEntered ? _rolloverTexture : Texture, Panel, (IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity));
            }
            else
            {
                SpriteBatch.Draw(MouseEntered ? _rolloverTexture : Texture, Panel, null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1);
            }

            SpriteBatch.End();
        }
    }
}
