using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using System.Collections.Generic;

namespace XAMLite
{
    /// <summary>
    /// 
    /// </summary>
    /// <see cref="http://msdn.microsoft.com/en-us/library/system.windows.controls.control.aspx"/>
    public class XAMLiteControl : DrawableGameComponent
    {
        protected MouseState ms;
        protected Microsoft.Xna.Framework.Point mouseLoc;

        protected bool _mouseDown;
        //protected bool _mouseUp;
        protected bool _mouseEnter;
        protected bool _mouseLeave;
        protected bool _keyDown;

        public event MouseButtonEventHandler MouseDown;
        public event MouseButtonEventHandler MouseUp;
        public event MouseEventHandler MouseEnter;
        public event MouseEventHandler MouseLeave;
        public event KeyEventHandler KeyDown;
        public event KeyEventHandler KeyUp;

        // the set of possible fonts that are preloaded in LoadContent()
        protected SpriteFont arialSpriteFont { get; set; }
        protected SpriteFont courier10SpriteFont { get; set; }
        protected SpriteFont courier20SpriteFont { get; set; }
        protected SpriteFont kootenay9SpriteFont { get; set; }
        protected SpriteFont kootenay14SpriteFont { get; set; }
        protected SpriteFont verdana10SpriteFont { get; set; }
        protected SpriteFont verdana10BoldSpriteFont { get; set; }
        protected SpriteFont verdana11SpriteFont { get; set; }
        protected SpriteFont verdana11BoldSpriteFont { get; set; }
        protected SpriteFont verdana12SpriteFont { get; set; }
        protected SpriteFont verdana12BoldSpriteFont { get; set; }
        protected SpriteFont verdana13SpriteFont { get; set; }
        protected SpriteFont verdana13BoldSpriteFont { get; set; }
        protected SpriteFont verdana14SpriteFont { get; set; }
        protected SpriteFont verdana14BoldSpriteFont { get; set; }
        protected SpriteFont verdana15SpriteFont { get; set; }
        protected SpriteFont verdana16SpriteFont { get; set; }
        protected SpriteFont verdana16BoldSpriteFont { get; set; }
        protected SpriteFont verdana20SpriteFont { get; set; }
        protected SpriteFont verdana20BoldSpriteFont { get; set; }
        protected SpriteFont verdana60SpriteFont { get; set; }
        protected SpriteFont verdana60BoldSpriteFont { get; set; }

        protected Rectangle _msRect; // mouse position
        protected Rectangle _panel; // rectangle containing the control for collision and drawing

        /// <summary>
        /// Prevents each control from perpetually updating each item in its Update method until necessary.
        /// </summary>
        protected bool marginChanged;

        /// <summary>
        /// Fills the space of a control with a color.
        /// </summary>
        protected Texture2D _pixel;

        /// <summary>
        /// 
        /// </summary>
        protected SpriteFont spriteFont;

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
        protected Visibility _visible;

        /// <summary>
        /// 
        /// </summary>
        new public Visibility Visible { get { return _visible; } set { _visible = value; _visibilityChanged = true; } }

        protected bool _visibilityChanged;

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

                marginChanged = true;
            }
        }

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
                        x = (this.viewport.Width - this.Width) / 2;
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
        /// Any interactive control becomes inactive when this is false.
        /// </summary>
        public bool IsEnabled { get; set; }

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
        protected Viewport viewport;

        /// <summary>
        /// List of every radio button in the UI.
        /// </summary>
        protected static List<XAMLiteRadioButton> _allRadioButtons;

        /// <summary>
        /// Determines whether the control is selected.
        /// </summary>
        protected bool _selected;

        /// <summary>
        /// List of menu titles.
        /// </summary>
        protected static List<string> _allMenuTitles;

        /// <summary>
        /// List of sub menu titles.
        /// </summary>
        protected static List<string> _allSubMenuTitles;

        /// <summary>
        /// Allows menus to automatically open on mouse over if any menu was previously selected.
        /// </summary>
        protected static bool _menuSelected;

        /// <summary>
        /// If zero, this will set _menuSelected to false, thus requiring a mouse down event to open a menu
        /// rather than a simple mouse over event.
        /// </summary>
        protected static int _menuVisibilityCount;

        /// <summary>
        ///Stores true or false depending on whether a sub menu is open.  If any are true, the open parent menu
        ///will not close.
        /// </summary>
        protected static Dictionary<string, bool> _subMenuOpen;

        protected Texture2D arrow;
        protected Rectangle arrowRect;

        protected Texture2D checkMark;
        protected Rectangle checkMarkRect;

        // used for labels so that the fonts, spacing, etc., will change (especially at startup) 
        // prior to being drawn to screen to prevent a noticeable size change. 
        protected bool firstUpdate;

        /// <summary>
        /// Set to true when all menus should be closed, ie., a button click on a menu item that is
        /// not contained in the _allSubMenuTitles list.
        /// </summary>
        protected bool closeAllMenus;

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
            this.Visible = new Visibility();
            this.Visible = Visibility.Visible;
            this.IsEnabled = true;
            _allRadioButtons = new List<XAMLiteRadioButton>();
            _allMenuTitles = new List<string>();
            _allSubMenuTitles = new List<string>();
            _subMenuOpen = new Dictionary<string, bool>();
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

            // for Background Color
            _pixel = new Texture2D(this.GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });

            // for sub menu items
            arrow = Game.Content.Load<Texture2D>("Images/arrow");
            arrowRect = new Rectangle(0, 0, arrow.Width, arrow.Height);

            // for menu check marks
            checkMark = Game.Content.Load<Texture2D>("Icons/MenuCheckMark");
            checkMarkRect = new Rectangle(0, 0, checkMark.Width, checkMark.Height);

            this.arialSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Arial");
            this.courier10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier10");
            this.courier20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Courier20");
            this.verdana10SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10");
            this.verdana10BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana10Bold");
            this.verdana11SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11");
            this.verdana11BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana11Bold");
            this.verdana12SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12");
            this.verdana12BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana12Bold");
            this.verdana13SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13");
            this.verdana13BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana13Bold");
            this.verdana14SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14");
            this.verdana14BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana14Bold");
            this.verdana15SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana15");
            this.verdana16SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16");
            this.verdana16BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana16Bold");
            this.verdana20SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20");
            this.verdana20BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana20Bold");
            this.verdana60SpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60");
            this.verdana60BoldSpriteFont = Game.Content.Load<SpriteFont>("Fonts/Verdana60Bold");
            this.spriteFont = courier10SpriteFont;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
            _msRect = new Rectangle(ms.X, ms.Y, 1, 1);
            if (IsEnabled && this.Visible == Visibility.Visible)
            {
                //ms = Microsoft.Xna.Framework.Input.Mouse.GetState();
                //_msRect = new Rectangle(ms.X, ms.Y, 1, 1);

                if (_panel.Contains(_msRect))
                {
                    if (!_mouseEnter)
                    {
                        _mouseEnter = true;
                        OnMouseEnter();
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

                if (!_mouseDown && ms.LeftButton == ButtonState.Pressed && _panel.Contains(_msRect))
                {
                    _mouseDown = true;
                    OnMouseDown();

                    //_mouseUp = false;
                }

                else if (_mouseDown && ms.LeftButton == ButtonState.Released && _panel.Contains(_msRect))
                {
                    _mouseDown = false;
                    OnMouseUp();
                }
            }
        }

        protected void UpdateFontFamily(FontFamily _fontFamily)
        {
            switch (_fontFamily.ToString())
            {
                case "Arial":
                    this.spriteFont = arialSpriteFont;
                    break;
                case "Courier20":
                    this.spriteFont = courier20SpriteFont;
                    break;
                case "Verdana10":
                    this.spriteFont = verdana10SpriteFont;
                    break;
                case "Verdana10Bold":
                    this.spriteFont = verdana10BoldSpriteFont;
                    break;
                case "Verdana11":
                    this.spriteFont = verdana11SpriteFont;
                    break;
                case "Verdana11Bold":
                    this.spriteFont = verdana11BoldSpriteFont;
                    break;
                case "Verdana12":
                    this.spriteFont = verdana12SpriteFont;
                    break;
                case "Verdana12Bold":
                    this.spriteFont = verdana12BoldSpriteFont;
                    break;
                case "Verdana13":
                    this.spriteFont = verdana13SpriteFont;
                    break;
                case "Verdana13Bold":
                    this.spriteFont = verdana13BoldSpriteFont;
                    break;
                case "Verdana14":
                    this.spriteFont = verdana14SpriteFont;
                    break;
                case "Verdana14Bold":
                    this.spriteFont = verdana14BoldSpriteFont;
                    break;
                case "Verdana16":
                    this.spriteFont = verdana16SpriteFont;
                    break;
                case "Verdana16Bold":
                    this.spriteFont = verdana16BoldSpriteFont;
                    break;
                case "Verdana20":
                    this.spriteFont = verdana20SpriteFont;
                    break;
                case "Verdana20Bold":
                    this.spriteFont = verdana20BoldSpriteFont;
                    break;
                case "Verdana60":
                    this.spriteFont = verdana60SpriteFont;
                    break;
                case "Verdana60Bold":
                    this.spriteFont = verdana60BoldSpriteFont;
                    break;
                default:
                    this.spriteFont = courier10SpriteFont;
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public virtual void OnKeyDown()
        {
            if (KeyDown != null)
            {
                var e = EventArgs.Empty as KeyEventArgs;
                KeyDown(this, e);
            }
        }
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
            this.Width = (int)this.spriteFont.MeasureString(text).X;
            this.Height = (int)this.spriteFont.MeasureString(text).Y;
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
            _allMenuTitles.Add("Session");
            //_allMenuTitles.Add("Ambient Level");
            _allMenuTitles.Add("Tutorials");
            //_allMenuTitles.Add("Time of Day");
            //_allMenuTitles.Add("Truck");
            _allMenuTitles.Add("Dev");

            _allSubMenuTitles.Add("Adjust Dust Visibility");
            _allSubMenuTitles.Add("Ambient Level");
            _allSubMenuTitles.Add("Time of Day");
            _allSubMenuTitles.Add("Truck");
        }
    }
}