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
        protected MouseState ms;
        protected Microsoft.Xna.Framework.Point mouseLoc;

        protected bool mouseDown;

        //protected bool _mouseUp;

        protected bool mouseEnter;

        protected bool mouseLeave;

        protected bool keyDown;

        public event MouseButtonEventHandler MouseDown;

        public event MouseButtonEventHandler MouseUp;

        public event MouseEventHandler MouseEnter;

        public event MouseEventHandler MouseLeave;

        public event KeyEventHandler KeyDown;

        public event KeyEventHandler KeyUp;

        // the set of possible fonts that are preloaded in LoadContent()
        protected SpriteFont ArialSpriteFont { get; set; }

        protected SpriteFont Courier10SpriteFont { get; set; }

        protected SpriteFont Courier20SpriteFont { get; set; }

        protected SpriteFont Kootenay9SpriteFont { get; set; }

        protected SpriteFont Kootenay14SpriteFont { get; set; }

        protected SpriteFont Verdana10SpriteFont { get; set; }

        protected SpriteFont Verdana10BoldSpriteFont { get; set; }

        protected SpriteFont Verdana11SpriteFont { get; set; }

        protected SpriteFont Verdana11BoldSpriteFont { get; set; }

        protected SpriteFont Verdana12SpriteFont { get; set; }

        protected SpriteFont Verdana12BoldSpriteFont { get; set; }

        protected SpriteFont Verdana13SpriteFont { get; set; }

        protected SpriteFont Verdana13BoldSpriteFont { get; set; }

        protected SpriteFont Verdana14SpriteFont { get; set; }

        protected SpriteFont Verdana14BoldSpriteFont { get; set; }

        protected SpriteFont Verdana15SpriteFont { get; set; }

        protected SpriteFont Verdana16SpriteFont { get; set; }

        protected SpriteFont Verdana16BoldSpriteFont { get; set; }

        protected SpriteFont Verdana20SpriteFont { get; set; }

        protected SpriteFont Verdana20BoldSpriteFont { get; set; }

        protected SpriteFont Verdana24BoldSpriteFont { get; set; }

        protected SpriteFont Verdana60SpriteFont { get; set; }

        protected SpriteFont Verdana60BoldSpriteFont { get; set; }

        protected Rectangle MsRect; // mouse position
        protected Rectangle Panel; // rectangle containing the control for collision and drawing

        /// <summary>
        /// Prevents each control from perpetually updating each item in its Update method until necessary.
        /// </summary>
        protected bool MarginChanged;

        /// <summary>
        /// Fills the space of a control with a color.
        /// </summary>
        protected Texture2D Pixel;

        /// <summary>
        /// 
        /// </summary>
        protected SpriteFont SpriteFont;

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private Visibility _visible;

        /// <summary>
        /// System.Windows.Visibility.  Maintains the visibility of a control.
        /// </summary>
        new public Visibility Visible
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
        /// 
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// 
        private Thickness _margin;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public bool Rotate90 { get; set; }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        protected SpriteBatch SpriteBatch { get; set; }

        /// <summary>
        /// 
        /// </summary>
        private GraphicsDevice _device;

        /// <summary>
        /// 
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
        protected static bool MenuSelected;

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

        protected Texture2D Arrow;
        protected Rectangle ArrowRect;

        protected Texture2D CheckMark;
        protected Rectangle CheckMarkRect;

        // used for labels so that the fonts, spacing, etc., will change (especially at startup) 
        // prior to being drawn to screen to prevent a noticeable size change. 
        protected bool FirstUpdate;

        /// <summary>
        /// Set to true when all menus should be closed, ie., a button click on a menu item that is
        /// not contained in the _allSubMenuTitles list.
        /// </summary>
        protected bool CloseAllMenus;

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public override void Initialize()
        {
            _device = Game.GraphicsDevice;
            Viewport = _device.Viewport;

            base.Initialize();
        }

        /// <summary>
        /// A single spritebatch this is shared across all instances of the XAMLiteControl class 
        /// (and derived classes).
        /// </summary>
        private static SpriteBatch staticSpriteBatch;

        /// <summary>
        /// 
        /// </summary>
        protected override void LoadContent()
        {
            // If the sprite batch that is shared across all XAMLite controls
            // hasn't yet been created, create it.
            if (staticSpriteBatch == null)
            {
                staticSpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }

            // Grab a reference to our single shared spritebatch.
            SpriteBatch = staticSpriteBatch;

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
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            MsRect = new Rectangle(ms.X, ms.Y, 1, 1);
            if (IsEnabled && Visible == Visibility.Visible)
            {
                if (Panel.Contains(MsRect))
                {
                    if (!mouseEnter)
                    {
                        mouseEnter = true;
                        OnMouseEnter();
                    }
                }
                else
                {
                    if (mouseEnter)
                    {
                        mouseEnter = false;
                        OnMouseLeave();
                    }
                }

                if (!mouseDown && ms.LeftButton == ButtonState.Pressed && mouseEnter)
                {
                    mouseDown = true;
                    OnMouseDown();
                }
                else if (mouseDown && ms.LeftButton == ButtonState.Released && mouseEnter)
                {
                    mouseDown = false;
                    OnMouseUp();
                }
            }
        }

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

        public virtual void OnKeyDown()
        {
            if (KeyDown != null)
            {
                var e = EventArgs.Empty as KeyEventArgs;
                KeyDown(this, e);
            }
        }

        /// <summary>
        /// 
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
        public virtual void OnMouseUp()
        {
            if (MouseUp != null)
            {
                var e = EventArgs.Empty as MouseButtonEventArgs;
                MouseUp(this, e);
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

        /// <summary>
        /// 
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