// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XAMLiteButton.cs" company="ForgeFX">
//   
// </copyright>
// <summary>
//   Defines the XAMLiteButton type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace XAMLite
{
    using System.Diagnostics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Button class with rollover texture.
    /// </summary>
    public class XAMLiteButton : XAMLiteControl
    {
        /// <summary>
        /// The 2-D image.
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { get; set; }

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
        /// Initializes a new instance of the <see cref="XAMLiteButton"/> class. 

        /// </summary>
        /// <param name="game">
        /// Reference to the Game
        /// </param>
        public XAMLiteButton(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Updates the Button.
        /// </summary>
        /// <param name="gameTime">Reference to the GameTime.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (MarginChanged)
            {
                MarginChanged = false;
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// Draws the Button.
        /// </summary>
        /// <param name="gameTime">The GameTime reference.</param>
        public override void Draw(GameTime gameTime)
        {
            if (Visible == System.Windows.Visibility.Visible)
            {
                SpriteBatch.Begin();

                SpriteBatch.Draw(MouseEntered ? rolloverTexture : texture, Panel, (Color.White * (float)Opacity));

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// Loads the button content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            Debug.Assert((SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension.");
            this.texture = Game.Content.Load<Texture2D>(SourceName);

            if (this.Width == 0)
            {
                this.Width = this.texture.Width;
            }
            if (this.Height == 0)
            {
                this.Height = this.texture.Height;
            }


            this.rolloverTexture = Game.Content.Load<Texture2D>(this.RolloverSourceName);

            // Sets the size and location of the image.
            Panel = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }
    }
}
