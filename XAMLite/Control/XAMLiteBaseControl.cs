using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Point = Microsoft.Xna.Framework.Point;

namespace XAMLite
{
    using System.Windows.Media;
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// The base class for all XAMLite objects.
    /// </summary>
    public class XAMLiteBaseControl : DrawableGameComponent
    {
        /// <summary>
        /// Any interactive control becomes inactive when this is false.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// A single sprite batch this is shared across all instances of the XAMLiteControl class 
        /// (and derived classes).
        /// </summary>
        protected static SpriteBatch SpriteBatch;

        /// <summary>
        /// Rectangle containing the control for collision and drawing
        /// </summary>
        protected Rectangle Panel;

        /// <summary>
        /// Fills the space of a control with a color.
        /// </summary>
        protected static Texture2D Pixel;

        /// <summary>
        /// The name of the control.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// True when the control is Visible.
        /// </summary>
        private Visibility _visible;

        /// <summary>
        /// System.Windows.Visibility.  Maintains the visibility of a control.
        /// </summary>
        public new Visibility Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
                VisibilityChanged = true;
            }
        }

        /// <summary>
        /// Notifies an individual control that the Visibility should be updated.
        /// </summary>
        protected bool VisibilityChanged;

        /// <summary>
        /// The horizontal alignment of the control.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// The vertical alignment of the control.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// Width of the control.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Height of the control.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Maintains the public double Opacity.
        /// </summary>
        private double _opacity;

        /// <summary>
        /// Gets or sets the opacity factor applied to the entire System.Windows.UIElement
        /// when it is rendered in the user interface (UI). Default opacity is 1.0. 
        /// Expected values are between 0.0 and 1.0.
        /// </summary>
        public double Opacity
        {
            get
            {
                return _opacity;
            }

            set
            {
                _opacity = value;
                OpacityChanged = true;
            }
        }

        /// <summary>
        /// Notifies an individual control that the Opacity should be updated.
        /// </summary>
        protected bool OpacityChanged;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        private Color _backgroundColor;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        public Brush Background
        {
            set
            {
                var solidBrush = (SolidColorBrush)value;
                var color = solidBrush.Color;
                _backgroundColor = new Color(color.R, color.G, color.B, color.A);

                _transparent = value == Brushes.Transparent;

                System.Console.WriteLine("Setting background: " + _backgroundColor.ToString() + " Width: " + Width + " " + _transparent);
            }
        }

        /// <summary>
        /// true when the background color is transparent.
        /// </summary>
        private bool _transparent;

        /// <summary>
        /// The margin of the control.
        /// </summary>
        private Thickness _margin;

        /// <summary>
        /// The margins on all sides of the control.
        /// </summary>
        public virtual Thickness Margin
        {
            get
            {
                return _margin;
            }

            set
            {
                _margin = value;
            }
        }

        /// <summary>
         /// The position of the control on the screen.
         /// </summary>
        protected Vector2 Position
        {
            get
            {
                // X
                var x = 0f;
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        x = ((float)(Viewport.Width - Width) / 2) + (float)Margin.Left - (float)Margin.Right;
                        break;

                    case HorizontalAlignment.Left:
                        x = (float)Margin.Left;
                        break;

                    case HorizontalAlignment.Right:
                        x = Viewport.Width - (float)Margin.Right - Width;
                        break;

                    case HorizontalAlignment.Stretch:
                        Width = Viewport.Width;
                        break;
                }

                // Y
                var y = 0f;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        y = Viewport.Height - Height - (float)Margin.Bottom;
                        break;

                    case VerticalAlignment.Center:
                        y = ((float)(Viewport.Height - Height) / 2) + (float)Margin.Top - (float)Margin.Bottom;
                        break;

                    case VerticalAlignment.Stretch:
                        Height = Viewport.Height;
                        break;

                    case VerticalAlignment.Top:
                        y = (int)Margin.Top;
                        break;
                }

                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// The screen width and height.
        /// </summary>
        protected Viewport Viewport;

        /// <summary>
        /// The state of the mouse, whether pressed, released, etc.
        /// </summary>
        protected static MouseState Ms;

        /// <summary>
        /// The position of the mouse on the screen.
        /// </summary>
        protected static Point MouseLoc;

        /// <summary>
        /// Mouse position.
        /// </summary>
        protected Rectangle MsRect;

        /// <summary>
        /// True when the mouse has been pressed while over a control.
        /// </summary>
        protected bool MousePressed;

        /// <summary>
        /// True when a control has been entered.
        /// </summary>
        protected bool MouseEntered;

        /// <summary>
        /// True when a control has been left.
        /// </summary>
        protected bool MouseLeft;

        /// <summary>
        /// True when a key has been pressed.
        /// </summary>
        protected bool KeyPressed;

        /// <summary>
        /// Event fired when MousePressed becomes true.
        /// </summary>
        public event MouseButtonEventHandler MouseDown;

        /// <summary>
        /// Event fired when Mouse is released.
        /// </summary>
        public event MouseButtonEventHandler MouseUp;

        /// <summary>
        /// Event fired when a control is entered.
        /// </summary>
        public event MouseEventHandler MouseEnter;

        /// <summary>
        /// Event fired when a control is left.
        /// </summary>
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// Event fired when a key is pressed.
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Event fired when a key is released.
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// True when the mouse click position has been recorded.  Reset to
        /// false once the left mouse button is released.
        /// </summary>
        protected static bool MousePressPositionRecorded;

        /// <summary>
        /// Whenever a mouse click occurs, its position is recorded.  It is
        /// then used to determine if mouse dragging occurred.
        /// </summary>
        protected static Vector2 MousePressPosition;

        /// <summary>
        /// True when all of the static lists that contain menu headers, sub
        /// menu headers, radio buttons, etc. get instantiated.
        /// </summary>
        protected static bool StaticVariablesCreated;

        /// <summary>
        /// List of every radio button in the UI.
        /// </summary>
        protected static List<XAMLiteRadioButton> AllRadioButtons;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteBaseControl(Game game)
            : base(game)
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Opacity = 1.0;
            Visible = new Visibility();
            Visible = Visibility.Visible;
            IsEnabled = true;
            _margin = new Thickness(0, 0, 0, 0);
            Viewport = Game.GraphicsDevice.Viewport;

            if (!StaticVariablesCreated)
            {
                MousePressPosition = new Vector2();
                AllRadioButtons = new List<XAMLiteRadioButton>();
                
                StaticVariablesCreated = true;
            }
        }

        /// <summary>
        /// Loads the content of the control.
        /// </summary>
        protected override void LoadContent()
        {
            // If the sprite batch that is shared across all XAMLite controls
            // hasn't yet been created, create it.
            // Also load all the static fonts and sprite textures the first time we come in here.
            if (SpriteBatch == null)
            {
                SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

                // for Background Color
                Pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });
            }

            // Sets the size and location of the image.
            Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            base.LoadContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (Visible == Visibility.Visible)
            {
                SpriteBatch.Begin();

                if (!_transparent)
                {
                    if (this is XAMLiteLabelNew)
                    {
                        SpriteBatch.Draw(
                            Pixel,
                            new Rectangle(Panel.X, Panel.Y, Panel.Width, Panel.Height - (int)(Height * 0.3)),
                            _backgroundColor * (float)Opacity);
                    }
                    else
                    {
                        SpriteBatch.Draw(
                            Pixel,
                            Panel,
                            _backgroundColor * (float)Opacity);
                    }
                }

                SpriteBatch.End();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandleInput(GameTime gameTime)
        {
            Ms = Mouse.GetState();
            MsRect.X = Ms.X;
            MsRect.Y = Ms.Y;

            // record the mouse down vector2
            if (!MousePressed && Ms.LeftButton == ButtonState.Pressed && !MousePressPositionRecorded)
            {
                MousePressPositionRecorded = true;

                MousePressPosition.X = MsRect.X;
                MousePressPosition.Y = MsRect.Y;
            }

            if (IsEnabled && Visible == Visibility.Visible)
            {
                if (Panel.Contains(MsRect))
                {
                    if (!MouseEntered)
                    {
                        MouseEntered = true;
                        OnMouseEnter();
                    }
                }
                else
                {
                    if (MouseEntered)
                    {
                        MouseEntered = false;
                        OnMouseLeave();
                    }
                }

                if (!MousePressed && Ms.LeftButton == ButtonState.Pressed && MouseEntered)
                {
                    if (Math.Abs(MousePressPosition.X - Ms.X) < 0.01 && Math.Abs(MousePressPosition.Y - Ms.Y) < 0.01)
                    {
                        MousePressed = true;
                        OnMouseDown();
                    }
                }
                else if (MousePressed && Ms.LeftButton == ButtonState.Released)
                {
                    MousePressed = false;

                    if (MouseEntered)
                    {
                        OnMouseUp();
                    }
                }

                if (Ms.LeftButton == ButtonState.Released && MousePressPositionRecorded)
                {
                    MousePressPositionRecorded = false;
                }
            }
        }

        /// <summary>
        /// Fires off the key down event.
        /// </summary>
        public virtual void OnKeyDown()
        {
            if (KeyDown != null)
            {
                var e = EventArgs.Empty as KeyEventArgs;
                KeyDown(this, e);
            }
        }

        /// <summary>
        /// Fires off the key up event.
        /// </summary>
        public virtual void OnKeyUp()
        {
            if (KeyUp != null)
            {
                var e = EventArgs.Empty as KeyEventArgs;
                KeyUp(this, e);
            }
        }

        /// <summary>
        /// Fires off the MouseDown event.
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
        /// Fires off the Mouse Up event.
        /// </summary>
        public virtual void OnMouseUp()
        {
            if (MouseUp != null)
            {
                var e = EventArgs.Empty as MouseButtonEventArgs;
                MouseUp(this, e);
            }
        }

        /// <summary>
        /// Fires off the Mouse Enter event.
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
        /// Fires off the Mouse Leave event.
        /// </summary>
        public virtual void OnMouseLeave()
        {
            if (MouseLeave != null)
            {
                var e = EventArgs.Empty as MouseEventArgs;
                MouseLeave(this, e);
            }
        }

        /// <summary>
        /// Toggles between Visibility.Visible and Visibility.Hidden.
        /// I'm cheating a bit here since this isn't strictly part of the standard WPF API. -AK
        /// </summary>
        public void ToggleVisibility()
        {
            switch (Visible)
            {
                case Visibility.Visible:
                    Visible = Visibility.Hidden;
                    break;

                case Visibility.Hidden:
                    Visible = Visibility.Visible;
                    break;

                case Visibility.Collapsed:
                    Visible = Visibility.Visible;
                    break;
            }
        }
    }
}
