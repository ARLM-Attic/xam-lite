using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XAMLite;
using System.Windows.Media;
using Color = Microsoft.Xna.Framework.Color;
using System.Windows.Input;

namespace XAMLiteDemo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        XAMLiteLabel _label;
        XAMLiteImage catImage;
        XAMLiteImage dogImage;
        XAMLiteGrid grid;
        XAMLiteLabel label2;
        XAMLiteGrid gridWithRadioButtons;

        XAMLiteImageWithRollover dogImage2;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "XAMLite Demo (XNA)";

            // Default screen size.
            _graphics.PreferredBackBufferWidth = 600;
            _graphics.PreferredBackBufferHeight = 400;

            // Enable this for random screen to (to test we're not hard-coding anything
            // base on screen size). Note: we're not attempting to support real-time
            // user resizing of the window.
            bool useRandomWindowSize = false;
            if (useRandomWindowSize)
            {
                var dice = new Random();
                _graphics.PreferredBackBufferWidth = dice.Next(600, 800);
                _graphics.PreferredBackBufferHeight = dice.Next(400, 500);
            }

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // grid Example
            grid = new XAMLiteGrid(this);
            grid.Name = "grid";
            grid.Width = 400;
            grid.Height = 250;
            grid.HorizontalAlignment = HorizontalAlignment.Right;
            grid.VerticalAlignment = VerticalAlignment.Bottom;
            grid.Background = Brushes.White;
            grid.Margin = new Thickness(5, 10, -50, 5);
            grid.Opacity = 0.5;
            grid.MouseDown += new MouseButtonEventHandler(grid_MouseDown);
            //Components.Add(grid);

            // Rectangle example #1
            /*XAMLiteRectangle rectangle1 = new XAMLiteRectangle(this);
            rectangle1.Width = 300;
            rectangle1.Height = 100;
            rectangle1.Fill = Brushes.LightGray;
            rectangle1.Stroke = Brushes.Green;
            rectangle1.StrokeThickness = 1;
            rectangle1.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle1.VerticalAlignment = VerticalAlignment.Top;
            rectangle1.Margin = new Thickness(0, 25, 0, 0);
            Components.Add(rectangle1);*/

            // Rectangle example #2.
            XAMLiteRectangle rectangle2 = new XAMLiteRectangle(this);
            rectangle2.Margin = new Thickness(10, 10, 10, 10);
            rectangle2.Stroke = Brushes.Pink;
            Components.Add(rectangle2);

            gridWithRadioButtons = new XAMLiteGrid(this);
            gridWithRadioButtons.Width = 300;
            gridWithRadioButtons.Height = 200;
            gridWithRadioButtons.Margin = new Thickness(5, 0, 0, 0);
            gridWithRadioButtons.HorizontalAlignment = HorizontalAlignment.Left;
            gridWithRadioButtons.VerticalAlignment = VerticalAlignment.Center;
            gridWithRadioButtons.Background = Brushes.BlueViolet;
            Components.Add(gridWithRadioButtons);

            XAMLiteImage background = new XAMLiteImage(this);
            background.SourceName = @"Images/OptionsDialogBackground";
            background.HorizontalAlignment = HorizontalAlignment.Stretch;
            background.VerticalAlignment = VerticalAlignment.Stretch;
            gridWithRadioButtons.Children.Add(background);

            XAMLiteImageWithRollover OKButton = new XAMLiteImageWithRollover(this);
            OKButton.SourceName = @"Images/BtnOkay";
            OKButton.RolloverSourceName = @"Images/BtnOkay-Over";
            OKButton.HorizontalAlignment = HorizontalAlignment.Center;
            OKButton.VerticalAlignment = VerticalAlignment.Bottom;
            OKButton.Margin = new Thickness(0, 0, 0, 10);
            OKButton.MouseDown += new MouseButtonEventHandler(OKButton_MouseDown);
            gridWithRadioButtons.Children.Add(OKButton);

            XAMLiteRadioButton r1a = new XAMLiteRadioButton(this);
            r1a.Name = "RadioButton1a";
            r1a.GroupName = "Set1";
            r1a.Content = "RB1a";
            r1a.MouseDown += new MouseButtonEventHandler(r1a_MouseDown);
            r1a.HorizontalAlignment = HorizontalAlignment.Left;
            r1a.VerticalAlignment = VerticalAlignment.Top;
            r1a.Margin = new Thickness(5, 15, 0, 0);
            r1a.IsEnabled = false;
            gridWithRadioButtons.Children.Add(r1a);

            XAMLiteRadioButton r1b = new XAMLiteRadioButton(this);
            r1b.Name = "RadioButton1b";
            r1b.GroupName = "Set1";
            r1b.Content = "RB1b";
            r1b.IsEnabled = false;
            r1b.IsChecked = true;
            r1b.MouseDown += new MouseButtonEventHandler(r1b_MouseDown);
            r1b.HorizontalAlignment = HorizontalAlignment.Left;
            r1b.VerticalAlignment = VerticalAlignment.Top;
            r1b.Margin = new Thickness(5, 35, 0, 0);
            gridWithRadioButtons.Children.Add(r1b);

            XAMLiteRadioButton r2a = new XAMLiteRadioButton(this);
            r2a.Name = "RadioButton2a";
            r2a.GroupName = "Set2";
            r2a.IsChecked = true;
            r2a.Content = "RB2a";
            r2a.RadioButtonSourceName = "Icons/RadioButton";
            r2a.RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";
            r2a.MouseDown += new MouseButtonEventHandler(r2a_MouseDown);
            r2a.IsEnabled = false;
            r2a.HorizontalAlignment = HorizontalAlignment.Right;
            r2a.VerticalAlignment = VerticalAlignment.Top;
            r2a.Margin = new Thickness(0, 35, 35, 0);
            gridWithRadioButtons.Children.Add(r2a);

            XAMLiteRadioButton r2b = new XAMLiteRadioButton(this);
            r2b.Name = "RadioButton2b";
            r2b.GroupName = "Set2";;
            r2b.Content = "RB2b";
            r2b.RadioButtonSourceName = "Icons/RadioButton";
            r2b.IsChecked = true;
            r2b.IsEnabled = false;
            r2b.RadioButtonSelectedSourceName = "Icons/RadioButtonSelected";
            r2b.MouseDown += new MouseButtonEventHandler(r2b_MouseDown);
            r2b.HorizontalAlignment = HorizontalAlignment.Right;
            r2b.VerticalAlignment = VerticalAlignment.Top;
            r2b.Margin = new Thickness(0, 15, 35, 0);
            gridWithRadioButtons.Children.Add(r2b);

            label2 = new XAMLiteLabel(this);
            label2.Content = "";
            label2.HorizontalAlignment = HorizontalAlignment.Right;
            label2.VerticalAlignment = VerticalAlignment.Center;
            label2.Margin = new Thickness(0, 0, 10, 0);
            gridWithRadioButtons.Children.Add(label2);
            // Clickable-image example. LOADED TO GRID
            /*dogImage = new XAMLiteImage(this);
            dogImage.Name = "DogImage";
            dogImage.Width = 100;
            dogImage.Height = 100;
            dogImage.Margin = new Thickness(5, 5, 5, 5);
            dogImage.VerticalAlignment = VerticalAlignment.Stretch;
            dogImage.HorizontalAlignment = HorizontalAlignment.Stretch;
            dogImage.SourceName = @"Textures/Dog";
            dogImage.MouseDown += new MouseButtonEventHandler(dogImage_MouseDown);
            dogImage.MouseEnter += new MouseEventHandler(dogImage_MouseEnter);
            dogImage.MouseLeave += new MouseEventHandler(dogImage_MouseLeave);
            grid.Children.Add(dogImage);*/

            // XAMLite label example. LOADED TO GRID
            _label = new XAMLiteLabel(this);
            _label.Name = "Right-Bottom";
            _label.Content = "Hello, world!";
            _label.Foreground = Brushes.Yellow;
            _label.Margin = new Thickness(0, 0, 0, 0);
            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(_label);


            // XAMLite label example. LOADED TO GRID
            XAMLiteLabel labelBottomLeft = new XAMLiteLabel(this);
            labelBottomLeft.Name = "Bottom-Left Label";
            labelBottomLeft.Content = "Bottom-Left Label";
            labelBottomLeft.Margin = new Thickness(10, 0, 0, 10);
            labelBottomLeft.Foreground = Brushes.White;
            labelBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            labelBottomLeft.VerticalAlignment = VerticalAlignment.Bottom;
            grid.Children.Add(labelBottomLeft);

            // Clickable-image example.
            catImage = new XAMLiteImage(this);
            catImage.Width = 100;
            catImage.Height = 100;
            catImage.VerticalAlignment = VerticalAlignment.Bottom;
            catImage.HorizontalAlignment = HorizontalAlignment.Right;
            catImage.SourceName = @"Textures/Cat";
            catImage.MouseDown += new MouseButtonEventHandler(catImage_MouseDown);
            catImage.MouseEnter += new MouseEventHandler(catImage_MouseEnter);
            catImage.MouseLeave += new MouseEventHandler(catImage_MouseLeave);
            Components.Add(catImage);

            // Rollover example
            dogImage2 = new XAMLiteImageWithRollover(this);
            dogImage2.SourceName = @"Textures/Dog";
            dogImage2.RolloverSourceName = @"Textures/Dog-Bright";
            dogImage2.VerticalAlignment = VerticalAlignment.Top;
            dogImage2.HorizontalAlignment = HorizontalAlignment.Right;
            dogImage2.MouseEnter += new MouseEventHandler(dogImage2_MouseEnter);
            dogImage2.MouseLeave += new MouseEventHandler(dogImage2_MouseLeave);
            Components.Add(dogImage2);

            /*XAMLiteTextBlock textBlock = new XAMLiteTextBlock(this);
            textBlock.Background = Brushes.Orange;
            textBlock.Foreground = Brushes.Green;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            // we may want enum with possible choices??
            // then: textBlock.FontFamily = FontFamily.Times;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.Run = "Something to talk about!!!";
            textBlock.Width = 100;
            textBlock.Height = 100;
            Components.Add(textBlock);*/

            // faking the Run class for WPF programmers LOADED TO GRID
            /*XAMLiteTextBlock textBlock2 = new XAMLiteTextBlock(this, new Run("This constructor uses a mock Run class for WPF developers>>>>>>>>>>>>> >>>>>>>>>>>>> MMMMMMMMMMM. MMMMMMMM."));
            textBlock2.Background = Brushes.Bisque;
            textBlock2.Foreground = Brushes.Red;
            textBlock2.Padding = new Thickness(10, 5, 10, 5);
            textBlock2.Margin = new Thickness(0, 0, 5, 0);
            textBlock2.TextWrapping = TextWrapping.Wrap;
            textBlock2.FontFamily = new FontFamily("Courier10");
            textBlock2.Width = 250;
            textBlock2.Rotate90 = true;
            textBlock2.HorizontalAlignment = HorizontalAlignment.Right;
            textBlock2.VerticalAlignment = VerticalAlignment.Bottom;
            grid.Children.Add(textBlock2);*/

            XAMLiteMenu menu1 = new XAMLiteMenu(this);
            menu1.Width = 75;
            menu1.Margin = new Thickness(10, 5, 0, 0);
            menu1.HorizontalAlignment = HorizontalAlignment.Left;
            menu1.VerticalAlignment = VerticalAlignment.Top;
            menu1.Background = Brushes.Black;
            Components.Add(menu1);

            XAMLiteMenuItem menuitem1 = new XAMLiteMenuItem(this);
            menuitem1.Header = "Head of Menu";
            menuitem1.Background = Brushes.Black;
            menuitem1.Foreground = Brushes.White;
            menu1.Items.Add(menuitem1);

            XAMLiteMenuItem menuitem2 = new XAMLiteMenuItem(this);
            menuitem2.Header = "Menu Item 2";
            menuitem2.Background = Brushes.Black;
            menuitem2.Foreground = Brushes.White;
            menu1.Items.Add(menuitem2);

            XAMLiteMenuItem menuitem3 = new XAMLiteMenuItem(this);
            menuitem3.Header = "Menu Item 3 blah blah";
            menuitem3.Background = Brushes.Black;
            menuitem3.Foreground = Brushes.White;
            menu1.Items.Add(menuitem3);

            XAMLiteMenu menu2 = new XAMLiteMenu(this);
            menu2.Width = 75;
            menu2.Margin = new Thickness(125, 5, 0, 0);
            menu2.HorizontalAlignment = HorizontalAlignment.Left;
            menu2.VerticalAlignment = VerticalAlignment.Top;
            menu2.Background = Brushes.Black;
            Components.Add(menu2);

            XAMLiteMenuItem menuitem2_1 = new XAMLiteMenuItem(this);
            menuitem2_1.Header = "Head of Menu";
            menuitem2_1.Background = Brushes.Black;
            menuitem2_1.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_1);

            XAMLiteMenuItem menuitem2_2 = new XAMLiteMenuItem(this);
            menuitem2_2.Header = "Menu Item 2";
            menuitem2_2.Background = Brushes.Black;
            menuitem2_2.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_2);

            XAMLiteMenuItem menuitem2_3 = new XAMLiteMenuItem(this);
            menuitem2_3.Header = "Menu Item 3 blah blah";
            menuitem2_3.Background = Brushes.Black;
            menuitem2_3.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_3);

            XAMLiteMenuItem submenuitem2_3_1 = new XAMLiteMenuItem(this);
            submenuitem2_3_1.Header = "SubMenu Item 1";
            submenuitem2_3_1.Background = Brushes.Black;
            submenuitem2_3_1.Foreground = Brushes.White;
            menuitem2_3.Items.Add(submenuitem2_3_1);

            XAMLiteMenuItem submenuitem2_3_2 = new XAMLiteMenuItem(this);
            submenuitem2_3_2.Header = "SubMenu Item 2 plus more words";
            submenuitem2_3_2.Background = Brushes.Black;
            submenuitem2_3_2.Foreground = Brushes.White;
            menuitem2_3.Items.Add(submenuitem2_3_2);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();
        }

        void OKButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gridWithRadioButtons.Visible = Visibility.Hidden;
        }

        void r2b_MouseDown(object sender, MouseButtonEventArgs e)
        {
            label2.Content = "You've selected RB2b";
        }

        void r2a_MouseDown(object sender, MouseButtonEventArgs e)
        {
            label2.Content = "You've selected RB2a";
        }

        void r1a_MouseDown(object sender, MouseButtonEventArgs e)
        {
            label2.Content = "You've selected RB1a";
        }

        void r1b_MouseDown(object sender, MouseButtonEventArgs e)
        {
            label2.Content = "You've selected RB1b";
        }

        void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int add = 5;
            grid.Margin = new Thickness(grid.Margin.Left, grid.Margin.Top, grid.Margin.Right + add, grid.Margin.Bottom + add);
        }

        /// <summary>
        /// Here's another mouse-leave event handler method for leaving the rollover dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage2_MouseLeave(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseLeave event was raised!";
        }

        /// <summary>
        /// Here's another mouse-enter event handler method for entering the rollover dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage2_MouseEnter(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseEnter event was raised!";
        }

        /// <summary>
        /// Here's the mouse-leave event handler method for leaving the dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void catImage_MouseLeave(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseLeave event was raised!";
        }

        /// <summary>
        /// Here's the mouse-enter event handler method for entering on the dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void catImage_MouseEnter(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseEnter event was raised!";
        }

        /// <summary>
        /// Here's the mouse-down event handler method for clicking on the cat image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void catImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _label.Content = "You clicked the cat!";
        }

        /// <summary>
        /// Here's the mouse-leave event handler method for leaving the dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage_MouseLeave(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseLeave event was raised!";
            dogImage.SourceName = @"Textures/Dog";
        }

        /// <summary>
        /// Here's the mouse-enter event handler method for entering on the dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dogImage_MouseEnter(object sender, MouseEventArgs e)
        {
            _label.Content = "MouseEnter event was raised!";
            dogImage.SourceName = @"Textures/Dog-Bright";
        }

        /// <summary>
        /// Here's the mouse-down event handler method for clicking on the dog image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dogImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _label.Content = "You clicked the dog!";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Call LoadContent() on all game components. (This inludes any XAMLite game components.)
            base.LoadContent();

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            // Draw all game components. (This includes our XAMLite drawable game components).
            base.Draw(gameTime);

        }
    }
}
