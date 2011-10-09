using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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

        public Game1()
        {

            //
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

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
            rectangle1.Fill = Color.LightGray;
            rectangle1.Stroke = Color.Green;
            rectangle1.StrokeThickness = 1;
            rectangle1.HorizontalAlignment = HorizontalAlignment.Center;
            rectangle1.VerticalAlignment = VerticalAlignment.Top;
            rectangle1.Margin = new Thickness(0, 25, 0, 0);
            Components.Add(rectangle1);

            // Rectangle example #2.
            XAMLiteRectangle rectangle2 = new XAMLiteRectangle(this);
            rectangle2.Margin = new Thickness(10, 10, 10, 10);
            rectangle2.Stroke = Color.Pink;
            Components.Add(rectangle2);

            // Clickable-image example.
            XAMLiteImage dogImage = new XAMLiteImage(this);
            dogImage.Width = 100;
            dogImage.Height = 100;
            dogImage.SourceName = @"Textures/Dog";
            //dogImage.MouseDown += new MouseButtonEventHandler(dogImage_MouseDown);
            Components.Add(dogImage);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();

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
