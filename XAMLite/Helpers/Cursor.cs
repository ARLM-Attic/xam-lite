// -----------------------------------------------------------------------
// <copyright file="Cursor.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace XAMLite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Cursor : XAMLiteImageNew
    {
        /// <summary>
        /// 
        /// </summary>
        private Texture2D _arrowTexture;

        /// <summary>
        /// 
        /// </summary>
        private Texture2D _handTexture;

        private Texture2D _iBeamTexture;

        private Vector2 _cursorTypePositionAdjustment;

        private Cursors _cursors;

        /// <summary>
        /// The image that represents the cursor. The default image is Arrow,
        /// and this currently will not draw and instead makes the standard
        /// XNA mouse arrow to be visible.
        /// </summary>
        public Cursors Cursors
        {
            get
            {
                return _cursors;
            }

            set
            {
                _cursors = value;

                switch (_cursors)
                {
                    case Cursors.Arrow:
                        // currently, when this is the mouse texture, it remains
                        // hidden and the normal game mouse is displayed.
                        Game.IsMouseVisible = true;
                        Texture = _arrowTexture;
                        Width = _arrowTexture.Width / 8;
                        Height = _arrowTexture.Height / 8;
                        Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                        _cursorTypePositionAdjustment.X = (Width / 2) - 5;
                        _cursorTypePositionAdjustment.Y = 2;
                        break;
                    case Cursors.Hand:
                        Game.IsMouseVisible = false;
                        Texture = _handTexture;
                        Width = 25;
                        Height = 25;
                        Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                        _cursorTypePositionAdjustment.X = (Width / 2) - 4;
                        _cursorTypePositionAdjustment.Y = 2;
                        break;
                    case Cursors.IBeam:
                        Game.IsMouseVisible = false;
                        Texture = _iBeamTexture;
                        Width = _iBeamTexture.Width;
                        Height = _iBeamTexture.Height;
                        Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                        _cursorTypePositionAdjustment = Vector2.Zero;
                        break;
                    default:
                        Game.IsMouseVisible = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public Cursor(Game game)
            : base(game)
        {
            DrawOrder = 1000;
            _cursorTypePositionAdjustment = new Vector2();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {    
            _arrowTexture = Game.Content.Load<Texture2D>(@"Cursors/ArrowCursor");
            _iBeamTexture = Game.Content.Load<Texture2D>("Cursors/IBeam");
            _handTexture = Game.Content.Load<Texture2D>(@"Cursors/Hand");

            Cursors = Cursors.Arrow;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (Cursors == Cursors.Arrow || Cursors == Cursors.None)
            {
                return;
            }

            Panel.X = Ms.X - (int)_cursorTypePositionAdjustment.X;
            Panel.Y = Ms.Y - (int)_cursorTypePositionAdjustment.Y;

            base.Update(gameTime);

            if (Game.IsMouseVisible && Cursors != Cursors.Arrow)
            {
                Game.IsMouseVisible = false;
            }

            if (!Game.IsMouseVisible && Cursors == Cursors.Arrow)
            {
                Game.IsMouseVisible = true;
            }
        }

        /// <summary>
        /// Draws the non standard mouse, such as the IBeam or the Hand.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            if (Visibility != Visibility.Visible || Cursors == Cursors.None || Cursors == Cursors.Arrow || (Panel.X == 0 && Panel.Y == 0))
            {
                return;
            }

            SpriteBatch.Begin();

            if (RenderTransform == null)
            {
                SpriteBatch.Draw(Texture, Panel, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity);
            }
            else
            {
                SpriteBatch.Draw(Texture, Panel, null, IsColorized ? !IsEdge ? BackgroundColor : !IsTopEdge ? BackgroundColor * 0.75f : BackgroundColor * 0.5f : Color.White * (float)Opacity, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }

            SpriteBatch.End();      
        }
    }
}
