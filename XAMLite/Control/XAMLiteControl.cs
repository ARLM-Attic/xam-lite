namespace XAMLite
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Color = Microsoft.Xna.Framework.Color;

    /// <summary>
    /// Describes the placement of where a Popup control such as a ToolTip 
    /// appears on the screen.
    /// Mouse:  Top left of tool tip should touch the bottom left of the mouse pointer.
    /// MousePoint:  Top left of tool tip should touch the tip of the mouse pointer.
    /// </summary>
    public enum PlacementMode
    {
        Absolute,
        Bottom,
        Center,
        Right,
        Left,
        Top,
        Mouse,
        MousePoint
    }

    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/system.windows.controls.control.aspx"/>
    public class XAMLiteControl : DrawableGameComponent
    {
        /// <summary>
        /// The state of the mouse, whether pressed, released, etc.
        /// </summary>
        protected MouseState Ms;

        /// <summary>
        /// The position of the mouse on the screen.
        /// </summary>
        protected Microsoft.Xna.Framework.Point MouseLoc;

        /// <summary>
        /// True when the mouse has been pressed while over a control.
        /// </summary>
        protected bool MousePressed;

        //protected bool _mouseUp;

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
        /// Arial 10 pt font
        /// </summary>
        protected static SpriteFont ArialSpriteFont { get; private set; }

        /// <summary>
        /// Courier 10 pt font
        /// </summary>
        protected static SpriteFont Courier10SpriteFont { get; private set; }

        /// <summary>
        /// Courier 20pt font.
        /// </summary>
        protected static SpriteFont Courier20SpriteFont { get; private set; }

        /// <summary>
        /// Kootenay 9 pt font.
        /// </summary>
        //protected static SpriteFont Kootenay9SpriteFont { get; private set; }

        /// <summary>
        /// Kootenay 20 pt font.
        /// </summary>
        //protected static SpriteFont Kootenay14SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 10 pt font.
        /// </summary>
        protected static SpriteFont Verdana10SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 10 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana10BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 pt font.
        /// </summary>
        protected static SpriteFont Verdana11SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 11 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana11BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 pt font.
        /// </summary>
        protected static SpriteFont Verdana12SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 12 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana12BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 pt font.
        /// </summary>
        protected static SpriteFont Verdana13SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 13 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana13BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 pt font.
        /// </summary>
        protected static SpriteFont Verdana14SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 14 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana14BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 15 pt font.
        /// </summary>
        protected static SpriteFont Verdana15SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 pt font.
        /// </summary>
        protected static SpriteFont Verdana16SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 16 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana16BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 pt font.
        /// </summary>
        protected static SpriteFont Verdana20SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 20 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana20BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 24 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana24BoldSpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 pt font.
        /// </summary>
        protected static SpriteFont Verdana60SpriteFont { get; private set; }

        /// <summary>
        /// Verdana 60 pt Bold font.
        /// </summary>
        protected static SpriteFont Verdana60BoldSpriteFont { get; private set; }

        /// <summary>
        /// Mouse position.
        /// </summary>
        protected Rectangle MsRect;

        /// <summary>
        /// rectangle containing the control for collision and drawing
        /// </summary>
        protected Rectangle Panel;

        /// <summary>
        /// Prevents each control from perpetually updating each item in its Update method until necessary.
        /// </summary>
        protected bool MarginChanged;

        /// <summary>
        /// Fills the space of a control with a color.
        /// </summary>
        protected static Texture2D Pixel;

        /// <summary>
        /// Font texture.
        /// </summary>
        protected SpriteFont SpriteFont;

        /// <summary>
        /// The name of the control.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Text contained in the control.
        /// </summary>
        public virtual string Text { get; set; }

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
        /// Notifies an individual control that the Visiblity should be updated.
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
                /*if(value <= 1 && value >= 0 ) {
                    if (value > 1)
                    {
                        value = 1;
                    }
                    else if (value < 0)
                    {
                        value = 0;
                    }*/

                _opacity = value;
                OpacityChanged = true;
            }
        }

        /// <summary>
        /// Notifies an individual control that the Opacity should be updated.
        /// </summary>
        protected bool OpacityChanged;

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
        /// The margin of the control.
        /// </summary>
        /// 
        private Thickness _margin;

        /// <summary>
        /// The margins on all sides of the control.
        /// </summary>
        public Thickness Margin
        {
            get
            {
                return _margin;
            }

            set
            {
                _margin = value;

                MarginChanged = true;
            }
        }

        /// <summary>
        /// True when the control has been rotated.
        /// </summary>
        public bool Rotate90 { get; set; }

        /// <summary>
        /// The position of the control on the screen.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // X
                var x = 0;
                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.Center:
                        x = (Viewport.Width - Width) / 2;
                        break;

                    case HorizontalAlignment.Left:
                        x = (int)Margin.Left;
                        break;

                    case HorizontalAlignment.Right:
                        x = Viewport.Width - (int)Margin.Right - Width;
                        break;

                    case HorizontalAlignment.Stretch:
                        Width = Viewport.Width;
                        break;
                }

                // Y
                var y = 0;
                switch (VerticalAlignment)
                {
                    case VerticalAlignment.Bottom:
                        y = Viewport.Height - Height - (int)Margin.Bottom;
                        break;

                    case VerticalAlignment.Center:
                        y = (Viewport.Height / 2) - (Height / 2);
                        break;

                    case VerticalAlignment.Stretch:
                        Height = Viewport.Height;
                        break;

                    case VerticalAlignment.Top:
                        y = (int)Margin.Top;
                        break;
                }

                //
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Any interactive control becomes inactive when this is false.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets a brush that describes the foreground color. The default 
        /// color is black.
        /// </summary>
        //public Brush Foreground { get; set; }

        /// <summary>
        /// A single spritebatch this is shared across all instances of the XAMLiteControl class 
        /// (and derived classes).
        /// </summary>
        protected static SpriteBatch SpriteBatch;



        /// <summary>
        /// The screen width and height.
        /// </summary>
        protected Viewport Viewport;

        /// <summary>
        /// List of every radio button in the UI.
        /// </summary>
        protected static List<XAMLiteRadioButton> AllRadioButtons;

        /// <summary>
        /// Determines whether the control is selected.
        /// </summary>
        protected bool Selected;

        /// <summary>
        /// List of menu titles.
        /// </summary>
        protected static List<string> AllMenuTitles;

        /// <summary>
        /// List of sub menu titles.
        /// </summary>
        protected static List<string> AllSubMenuTitles;

        /// <summary>
        /// Allows menus to automatically open on mouse over if any menu was previously selected.
        /// </summary>
        protected static bool MenuShouldAutoOpen;

        /// <summary>
        /// If zero, this will set _menuSelected to false, thus requiring a mouse down event to open a menu
        /// rather than a simple mouse over event.
        /// </summary>
        protected static int MenuVisibilityCount;

        /// <summary>
        /// Stores true or false depending on whether a sub menu is open.  If any are true, the open parent menu
        /// will not close.
        /// </summary>
        protected static Dictionary<string, bool> OpenSubMenuDictionary;

        /// <summary>
        /// Texture placed next to menu items that have a sub menu.
        /// </summary>
        protected static Texture2D Arrow;

        /// <summary>
        /// The rectangle that contains the arrow texture.
        /// </summary>
        protected static Rectangle ArrowRect;

        /// <summary>
        /// The check mark that may be placed in front of a menu item when it is selected.
        /// </summary>
        protected static Texture2D CheckMark;

        /// <summary>
        /// The rectangle that contains the check mark.
        /// </summary>
        protected static Rectangle CheckMarkRect;

        // used for labels so that the fonts, spacing, etc., will change (especially at startup) 
        // prior to being drawn to screen to prevent a noticeable size change. 
        protected bool FirstUpdate;

        /// <summary>
        /// Set to true when all menus should be closed, ie., a button click on a menu item that is
        /// not contained in the _allSubMenuTitles list.
        /// </summary>
        protected bool CloseAllMenus;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="game"></param>
        public XAMLiteControl(Game game)
            : base(game)
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Margin = new Thickness(0, 0, 0, 0);
            Opacity = 1.0;
            Visible = new Visibility();
            Visible = Visibility.Visible;
            IsEnabled = true;
            AllRadioButtons = new List<XAMLiteRadioButton>();
            AllMenuTitles = new List<string>();
            AllSubMenuTitles = new List<string>();
            OpenSubMenuDictionary = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Initializes the graphics device and viewport.
        /// </summary>
        public override void Initialize()
        {
            Viewport = Game.GraphicsDevice.Viewport;

            base.Initialize();
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
                Pixel = new Texture2D(GraphicsDevice, 1, 1);
                Pixel.SetData(new[] { Color.White });

                // for sub menu items
                Arrow = Game.Content.Load<Texture2D>("Images/arrow");
                ArrowRect = new Rectangle(0, 0, Arrow.Width, Arrow.Height);

                // for menu check marks
                CheckMark = Game.Content.Load<Texture2D>("Icons/MenuCheckMark");
                CheckMarkRect = new Rectangle(0, 0, CheckMark.Width, CheckMark.Height);

                ArialSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
                Courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
                Courier20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier20");
                Verdana10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10");
                Verdana10BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10Bold");
                Verdana11SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11");
                Verdana11BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11Bold");
                Verdana12SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12");
                Verdana12BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12Bold");
                Verdana13SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13");
                Verdana13BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13Bold");
                Verdana14SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14");
                Verdana14BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14Bold");
                Verdana15SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana15");
                Verdana16SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16");
                Verdana16BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16Bold");
                Verdana20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20");
                Verdana20BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20Bold");
                Verdana24BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana24Bold");
                Verdana60SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60");
                Verdana60BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60Bold");
            }

            //Default this controls font
            SpriteFont = Courier10SpriteFont;
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~XAMLiteControl()
        {
            Dispose();
        }

        /// <summary>
        /// Updates the control.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            MsRect = new Rectangle(Ms.X, Ms.Y, 1, 1);

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
                    MousePressed = true;
                    OnMouseDown();
                }
                else if (MousePressed && Ms.LeftButton == ButtonState.Released && MouseEntered)
                {
                    MousePressed = false;
                    OnMouseUp();
                }
            }
        }

        /// <summary>
        /// Updates the font family.
        /// </summary>
        /// <param name="fontFamily"></param>
        protected void UpdateFontFamily(FontFamily fontFamily)
        {
            switch (fontFamily.ToString())
            {
                case "Arial":
                    SpriteFont = ArialSpriteFont;
                    break;
                case "Courier20":
                    SpriteFont = Courier20SpriteFont;
                    break;
                case "Verdana10":
                    SpriteFont = Verdana10SpriteFont;
                    break;
                case "Verdana10Bold":
                    SpriteFont = Verdana10BoldSpriteFont;
                    break;
                case "Verdana11":
                    SpriteFont = Verdana11SpriteFont;
                    break;
                case "Verdana11Bold":
                    SpriteFont = Verdana11BoldSpriteFont;
                    break;
                case "Verdana12":
                    SpriteFont = Verdana12SpriteFont;
                    break;
                case "Verdana12Bold":
                    SpriteFont = Verdana12BoldSpriteFont;
                    break;
                case "Verdana13":
                    SpriteFont = Verdana13SpriteFont;
                    break;
                case "Verdana13Bold":
                    SpriteFont = Verdana13BoldSpriteFont;
                    break;
                case "Verdana14":
                    SpriteFont = Verdana14SpriteFont;
                    break;
                case "Verdana14Bold":
                    SpriteFont = Verdana14BoldSpriteFont;
                    break;
                case "Verdana15":
                    SpriteFont = Verdana15SpriteFont;
                    break;
                case "Verdana16":
                    SpriteFont = Verdana16SpriteFont;
                    break;
                case "Verdana16Bold":
                    SpriteFont = Verdana16BoldSpriteFont;
                    break;
                case "Verdana20":
                    SpriteFont = Verdana20SpriteFont;
                    break;
                case "Verdana20Bold":
                    SpriteFont = Verdana20BoldSpriteFont;
                    break;
                case "Verdana24Bold":
                    SpriteFont = Verdana24BoldSpriteFont;
                    break;
                case "Verdana60":
                    SpriteFont = Verdana60SpriteFont;
                    break;
                case "Verdana60Bold":
                    SpriteFont = Verdana60BoldSpriteFont;
                    break;
                default:
                    SpriteFont = Courier10SpriteFont;
                    break;
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
        /// Recalculate the width and height of the control.
        /// </summary>
        /// <param name="text"></param>
        protected void RecalculateWidthAndHeight(string text)
        {
            Width = (int)SpriteFont.MeasureString(text).X;
            Height = (int)SpriteFont.MeasureString(text).Y;
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

        /// <summary>
        /// HACK: When a tutorial is selected, all Menu Title Headers are erased, so currently 
        /// they are being manually added again.
        /// </summary>
        protected void ResetMenuItems()
        {
            AllMenuTitles.Add("Session");
            //_allMenuTitles.Add("Ambient Level");
            AllMenuTitles.Add("Tutorials");
            //_allMenuTitles.Add("Time of Day");
            //_allMenuTitles.Add("Truck");
            AllMenuTitles.Add("Developer [F1]");

            AllSubMenuTitles.Add("Adjust Dust Visibility");
            AllSubMenuTitles.Add("Ambient Level");
            AllSubMenuTitles.Add("Time of Day");
            AllSubMenuTitles.Add("Truck");
        }
    }
}