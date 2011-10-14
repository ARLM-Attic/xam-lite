using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/system.windows.controls.control.aspx"/>
    public class XAMLiteControl : DrawableGameComponent
    {
        public MouseState ms;
        public Microsoft.Xna.Framework.Point mouseLoc;

        protected bool _mouseDown;
        protected bool _mouseEnter;
        protected bool _mouseLeave;

        public event MouseButtonEventHandler MouseDown;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// 
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Thickness Margin { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public RotateTransform RotateTransform { get; set; }
        public bool Rotate90 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // X
                int x = 0;
                switch (HorizontalAlignment)
                {

                    case HorizontalAlignment.Center:
                        x = (this.viewport.Width / 2) - (this.Width / 2);
                        break;

                    case HorizontalAlignment.Left:
                        x = (int)this.Margin.Left;
                        break;

                    case HorizontalAlignment.Right:
                        x = this.viewport.Width - (int)this.Margin.Right - this.Width;
                        break;

                    case HorizontalAlignment.Stretch:
                        this.Width = this.viewport.Width;
                        break;

                    default:
                        break;

                }

                // Y
                int y = 0;
                switch (VerticalAlignment)
                {

                    case VerticalAlignment.Bottom:
                        y = this.viewport.Height - this.Height - (int)this.Margin.Bottom;
                        break;

                    case VerticalAlignment.Center:
                        y = (this.viewport.Height / 2) - (this.Height / 2);
                        break;

                    case VerticalAlignment.Stretch:
                        this.Height = this.viewport.Height;
                        break;

                    case VerticalAlignment.Top:
                        y = (int)this.Margin.Top;
                        break;

                    default:
                        break;

                }

                //
                return new Vector2(x, y);

            }
        }

        /// <summary>
        /// Gets or sets the opacity factor applied to the entire System.Windows.UIElement
        /// when it is rendered in the user interface (UI). Default opacity is 1.0. 
        /// Expected values are between 0.0 and 1.0.
        /// </summary>
        public double Opacity { get; set; }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color. The default 
        /// color is black.
        /// </summary>
        //public Brush Foreground { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected SpriteBatch spriteBatch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        GraphicsDevice device;

        /// <summary>
        /// 
        /// </summary>
        public Viewport viewport;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteControl(Game game)
            : base(game)
        {
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.Margin = new Thickness(0, 0, 0, 0);
            this.Opacity = 1.0;
            //this.Foreground = Brushes.Black;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {

            //
            this.device = Game.GraphicsDevice;
            this.viewport = device.Viewport;

            //
            base.Initialize();

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            if (!_mouseDown && ms.LeftButton == ButtonState.Pressed)
                _mouseDown = true;
            else
                _mouseDown = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
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