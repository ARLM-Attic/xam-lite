using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using Point = Microsoft.Xna.Framework.Point;

namespace XAMLite
{
    /// <summary>
    /// The base class for all XAMLite objects.
    /// </summary>
    public class XAMLiteBaseControl : DrawableGameComponent
    {
        /// <summary>
        /// The name of the control.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The index will be -1 when not associated with a XAMLiteGrid.
        /// Otherwise, it will have some value from 0 or greater.
        /// </summary>
        internal int Index = -1;

        /// <summary>
        /// Allows the developer to set a parent of a particular XAMLite class.
        /// In XAMLite, mainly used to build complex components that use a grid
        /// when this grid may be inside another grid.
        /// </summary>
        public XAMLiteBaseControl Parent { get; set; }

        /// <summary>
        /// Any interactive control becomes inactive when this is false.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the element can receive focus.
        /// </summary>
        public bool Focusable { get; set; }

        /// <summary>
        /// Determines whether this element has logical focus.
        /// </summary>
        private bool _isFocused;

        /// <summary>
        /// Gets or sets a value that determines whether this element has logical focus.
        /// </summary>
        public bool IsFocused
        {
            get
            {
                var focus = false;
                
                if (Focusable)
                {
                    focus = _isFocused;
                }

                return focus;
            }

            set
            {
                if (Focusable)
                {
                    _isFocused = value;
                }
            }
        }

        /// <summary>
        /// True when the control is a part of another control.  For example, 
        /// a XAMLiteLabel associated with the XAMLiteCheckBox class.
        /// </summary>
        internal bool IsAttachedToGrid;

        /// <summary>
        /// If the control IsAttachedToGrid and the GridIsHidden, then the 
        /// control will not draw.
        /// </summary>
        //internal bool GridIsHidden;

        /// <summary>
        /// Width of the control.
        /// </summary>
        public virtual int Width { get; set; }

        /// <summary>
        /// Height of the control.
        /// </summary>
        public virtual int Height { get; set; }

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

                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// If the control IsAttachedToGrid, then the position of the control 
        /// must be mapped according to the grid that contains it rather than 
        /// the Viewport, thus the distinction between Panel and Window.
        /// </summary>
        internal Rectangle Window;
 
        /// <summary>
        /// The screen width and height.
        /// </summary>
        protected Viewport Viewport;

        /// <summary>
        /// The horizontal alignment of the control.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// The vertical alignment of the control.
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

        /// <summary>
        /// The position of the control on the screen.
        /// </summary>
        protected Vector2 Position
        {
            get
            {
                return GetPosition();
            }
        }

        /// <summary>
        /// Returns the position of the control.
        /// </summary>
        /// <returns></returns>
        private Vector2 GetPosition()
        {
            var panel = new Rectangle();

            if (IsAttachedToGrid)
            {
                panel = Window;
            }
            else
            {
                panel.X = Viewport.X;
                panel.Y = Viewport.Y;
                panel.Width = Viewport.Width;
                panel.Height = Viewport.Height;
            }

            // X
            var x = 0f;
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    x = panel.X + ((float)(panel.Width - Width) / 2) + (float)Margin.Left - (float)Margin.Right;
                    break;

                case HorizontalAlignment.Left:
                    x = panel.X + (float)Margin.Left;
                    break;

                case HorizontalAlignment.Right:
                    x = panel.X + panel.Width - (float)Margin.Right - Width;
                    break;

                case HorizontalAlignment.Stretch:
                    x = panel.X + (float)Margin.Left;

                    if (!IsAttachedToGrid)
                    {
                        Width = panel.Width;
                    }

                    break;
            }

            // Y
            var y = 0f;
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    y = panel.Y + panel.Height - Height - (float)Margin.Bottom;
                    break;

                case VerticalAlignment.Center:
                    y = panel.Y + ((float)(panel.Height - Height) / 2) + (float)Margin.Top - (float)Margin.Bottom;
                    break;

                case VerticalAlignment.Stretch:
                    y = panel.Y + (int)Margin.Top;
                    if (!IsAttachedToGrid)
                    {
                        Height = panel.Y + panel.Height;
                    }

                    break;

                case VerticalAlignment.Top:
                    y = panel.Y + (int)Margin.Top;
                    break;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// Returns the top left Vector2 of the control.
        /// </summary>
        protected Vector2 TopLeftCorner
        {
            get { return Position; }
        }

        /// <summary>
        /// Returns the top right Vector2 of the control.
        /// </summary>
        protected Vector2 TopRightCorner
        {
            get { return new Vector2(Position.X + Width, Position.Y); }
        }

        /// <summary>
        /// Returns the bottom left Vector2 of the control.
        /// </summary>
        protected Vector2 BottomLeftCorner
        {
            get { return new Vector2(Position.X, Position.Y + Height); }
        }

        /// <summary>
        /// Returns the bottom right Vector2 of the control.
        /// </summary>
        protected Vector2 BottomRightCorner
        {
            get { return new Vector2(Position.X + Width, Position.Y + Height); }
        }

        /// <summary>
        /// Returns the center Vector2 of the control.
        /// </summary>
        protected Vector2 Center
        {
            get { return new Vector2(Position.X + ((float)Width / 2), Position.Y + ((float)Height / 2)); }
        }

        /// <summary>
        /// True when the control is Visible.
        /// </summary>
        private Visibility visibility;

        /// <summary>
        /// Maintains the visibility of a control.
        /// </summary>
        public virtual Visibility Visibility
        {
            get
            {
                return visibility;
            }

            set
            {
                visibility = value;
                Visible = visibility == Visibility.Visible;

                VisibilityChanged = true;
            }
        }

        /// <summary>
        /// Notifies an individual control that the Visibility should be updated.
        /// </summary>
        protected bool VisibilityChanged;

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
        /// A single sprite batch this is shared across all instances of the XAMLiteControl class 
        /// (and derived classes).
        /// </summary>
        protected static SpriteBatch SpriteBatch;

        /// <summary>
        /// Rectangle containing the control for collision and drawing
        /// </summary>
        protected internal Rectangle Panel;

        /// <summary>
        /// Fills the space of a control with a color.
        /// </summary>
        protected static Texture2D Pixel;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        protected Color BackgroundColor;

        /// <summary>
        /// 
        /// </summary>
        private Brush _background;

        /// <summary>
        /// Background color of the Grid.
        /// </summary>
        public virtual Brush Background
        {
            get
            {
                return _background;
            }

            set
            {
                _background = value;

                if (_background != null)
                {
                    var solidBrush = (SolidColorBrush)value;
                    var color = solidBrush.Color;
                    BackgroundColor = new Color(color.R, color.G, color.B, color.A);
                }

                _transparent = value == Brushes.Transparent;
            }
        }

        /// <summary>
        /// true when the background color is transparent.
        /// </summary>
        private bool _transparent;

        ///// <summary>
        ///// The cursor that has the potential to change as it mouses over, 
        ///// clicks, etc. on a control.
        ///// </summary>
        public static Cursor Cursor;

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
        protected static Rectangle MsRect;

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
        /// 
        /// </summary>
        protected internal bool PositionChanged;

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
            base.LoadContent();

            Visibility = Visibility.Visible;

            // If the sprite batch that is shared across all XAMLite controls
            // hasn't yet been created, create it.
            if (SpriteBatch == null)
            {
                SpriteBatch = new SpriteBatch(Game.GraphicsDevice);

                // for Background Color
                Pixel = new Texture2D(Game.GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });
            }

            // Create a single cursor when the game mouse is visible for 
            // the application.
            if (Cursor == null)
            {
                Cursor = new Cursor(Game);
                Cursor.Initialize();
                Game.Components.Add(Cursor);
            }

            // Sets the size and location of the image.
            Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);

            if (PositionChanged)
            {
                Panel = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                PositionChanged = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        //public override void Draw(GameTime gameTime)
        //{
        //    base.Draw(gameTime);

        //    if (Visibility == Visibility.Visible)
        //    {
        //        SpriteBatch.Begin();

        //        if (!_transparent)
        //        {
        //            //if (this is XAMLiteLabelNew)
        //            //{
        //            //    SpriteBatch.Draw(
        //            //        Pixel,
        //            //        new Rectangle(Panel.X, Panel.Y, Panel.Width, Panel.Height - (int)(Height * 0.3)),
        //            //        _backgroundColor * (float)Opacity);
        //            //}
        //            //else
        //            {
        //                SpriteBatch.Draw(
        //                    Pixel,
        //                    Panel,
        //                    _backgroundColor * (float)Opacity);
        //            }
        //        }

        //        SpriteBatch.End();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        protected virtual void HandleInput(GameTime gameTime)
        {
            Ms = Mouse.GetState();
            MsRect.X = Ms.X;
            MsRect.Y = Ms.Y;

            // if deselected after the control had focus, remove the focus
            if (IsFocused && Ms.LeftButton == ButtonState.Pressed && !MousePressed)
            {
                // focus removed.
                IsFocused = false;
            }

            // record the mouse down vector2
            if (!MousePressed && Ms.LeftButton == ButtonState.Pressed && !MousePressPositionRecorded)
            {
                MousePressPositionRecorded = true;

                MousePressPosition.X = MsRect.X;
                MousePressPosition.Y = MsRect.Y;
            }

            if (IsEnabled && Visibility == Visibility.Visible)
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
            switch (Visibility)
            {
                case Visibility.Visible:
                    Visibility = Visibility.Hidden;
                    break;

                case Visibility.Hidden:
                    Visibility = Visibility.Visible;
                    break;

                case Visibility.Collapsed:
                    Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
