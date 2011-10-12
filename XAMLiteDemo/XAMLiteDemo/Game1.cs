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
        XAMLiteImageWithRollover dogImage2;

        public Game1()
        {

            //
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
            bool useRandomWindowSize = true;
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

            // XAMLite label example.
            _label = new XAMLiteLabel(this);
            _label.Content = "Hello, world!";
            _label.Foreground = Brushes.Yellow;
            _label.HorizontalAlignment = HorizontalAlignment.Center;
            _label.VerticalAlignment = VerticalAlignment.Center;
            Components.Add(_label);

            // Rectangle example #1
            XAMLiteRectangle rectangle1 = new XAMLiteRectangle(this);
            rectangle1.Width = 300;
            rectangle1.Height = 100;
            rectangle1.Fill = Brushes.LightGray;
            rectangle1.Stroke = Brushes.Green;
            rectangle1.StrokeThickness = 1;
            rectangle1.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle1.VerticalAlignment = VerticalAlignment.Top;
            rectangle1.Margin = new Thickness(0, 25, 0, 0);
            Components.Add(rectangle1);

            // Rectangle example #2.
            XAMLiteRectangle rectangle2 = new XAMLiteRectangle(this);
            rectangle2.Margin = new Thickness(10, 10, 10, 10);
            rectangle2.Stroke = Brushes.Pink;
            Components.Add(rectangle2);

            // Clickable-image example.
            dogImage = new XAMLiteImage(this);
            dogImage.Width = 100;
            dogImage.Height = 100;
            dogImage.VerticalAlignment = VerticalAlignment.Bottom;
            dogImage.HorizontalAlignment = HorizontalAlignment.Left;
            dogImage.SourceName = @"Textures/Dog";
            dogImage.MouseDown += new MouseButtonEventHandler(dogImage_MouseDown);
            dogImage.MouseEnter += new MouseEventHandler(dogImage_MouseEnter);
            dogImage.MouseLeave += new MouseEventHandler(dogImage_MouseLeave);
            Components.Add(dogImage);

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
            dogImage2.SourceName = dogImage.SourceName;
            dogImage2.RolloverSourceName = @"Textures/Dog-Bright";
            dogImage2.VerticalAlignment = VerticalAlignment.Top;
            dogImage2.HorizontalAlignment = HorizontalAlignment.Right;
            dogImage2.MouseEnter += new MouseEventHandler(dogImage2_MouseEnter);
            dogImage2.MouseLeave += new MouseEventHandler(dogImage2_MouseLeave);
            Components.Add(dogImage2);

            XAMLiteTextBlock textBlock = new XAMLiteTextBlock(this);
            textBlock.Background = Brushes.Orange;
            textBlock.Foreground = Brushes.Green;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            //textBlock.TextAlignment = TextAlignment.Center;
            // we may want enum with possible choices??
            // then: textBlock.FontFamily = FontFamily.Times;
            textBlock.FontFamily = new FontFamily("Arial");
            textBlock.Run = "Something to talk about!!!";
            textBlock.Width = 100;
            textBlock.Height = 100;
            Components.Add(textBlock);


            // faking the Run class for WPF programmers
            XAMLiteTextBlock textBlock2 = new XAMLiteTextBlock(this, new Run("This constructor uses a mock Run class for WPF developers>>>>>>>>>>>>> >>>>>>>>>>>>> MMMMMMMMMMM. MMMMMMMM."));
            textBlock2.Background = Brushes.Bisque;
            textBlock2.Foreground = Brushes.Red;
            //textBlock2.Padding = new Thickness(0, 0, 0, 0);
            textBlock2.TextWrapping = TextWrapping.Wrap;
            //textBlock2.TextAlignment = TextAlignment.Center;
            textBlock2.FontFamily = new FontFamily("Courier10");
            textBlock2.Width = 250;
            textBlock2.Height = 150;
            textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
            textBlock2.VerticalAlignment = VerticalAlignment.Bottom;
            Components.Add(textBlock2);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();
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
