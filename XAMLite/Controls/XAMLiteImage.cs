using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    public class XAMLiteImage : XAMLiteControl
    {
        public event MouseButtonEventHandler MouseDown;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// 
        /// </summary>
        protected Texture2D texture;

        /// <summary>
        /// For collision detection
        /// </summary>
        protected Rectangle rect;

        /// <summary>
        /// This is the image file path, minus the file extension.
        /// </summary>
        public string SourceName { 
            get; 
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="assetName"></param>
        public XAMLiteImage ( Game game  )
            : base( game )
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent ()
        {
            base.LoadContent();

            Debug.Assert( ( SourceName != null), "Must set SourceName property. This is the image file path, minus the file extension." );
            this.texture = Game.Content.Load<Texture2D>( SourceName );
            this.Width = this.texture.Width;
            this.Height = this.texture.Height;
            rect = new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Rectangle msRect = new Rectangle(ms.X, ms.Y, 1, 1);
            if (rect.Contains(msRect))
            {
                if (!_mouseEnter)
                {
                    _mouseEnter = true;
                    OnMouseEnter();
                }
                if (_mouseDown)
                {
                    _mouseDown = false;
                    OnMouseDown();
                }
            }
            else
            {
                if (_mouseEnter)
                {
                    _mouseEnter = false;
                    OnMouseLeave();
                }
            }
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
            this.spriteBatch.Draw(this.texture, rect, (Color.White * opacity));
            
            //
            this.spriteBatch.End();

        }

        /// <summary>
        /// 
        /// </summary>
         public virtual void OnMouseDown()
         {
             if (MouseDown != null)
             {
                 var e = EventArgs.Empty as MouseButtonEventArgs;
                 MouseDown(this, e);
             }        
         }

         /// <summary>
         /// 
         /// </summary>
         public virtual void OnMouseEnter()
         {
             if (MouseEnter != null)
             {
                 var e = EventArgs.Empty as MouseEventArgs;
                 MouseEnter(this, e);
             }
         }

         /// <summary>
         /// 
         /// </summary>
         public virtual void OnMouseLeave()
         {
             if (MouseLeave != null)
             {
                 var e = EventArgs.Empty as MouseEventArgs;
                 MouseLeave(this, e);
             }
         }

    }

}