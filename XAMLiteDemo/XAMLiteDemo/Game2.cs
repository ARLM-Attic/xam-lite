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
    public class Game2 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        XAMLiteImage catImage;
        XAMLiteImage dogImage;
        XAMLiteGrid catsAndDogsGrid;


        public Game2()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "XAMLite Demo (XNA) - Grid Children Visibility";

            // Default screen size.
            _graphics.PreferredBackBufferWidth = 715;
            _graphics.PreferredBackBufferHeight = 365;

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
            catsAndDogsGrid = new XAMLiteGrid(this);
            catsAndDogsGrid.Name = "catsAndDogsGrid";
            catsAndDogsGrid.Width = 101;
            catsAndDogsGrid.Height = 181;
            catsAndDogsGrid.HorizontalAlignment = HorizontalAlignment.Left;
            catsAndDogsGrid.VerticalAlignment = VerticalAlignment.Top;
            catsAndDogsGrid.Background = Brushes.LightBlue;
            catsAndDogsGrid.Margin = new Thickness(22, 21, 0, 0);
            Components.Add(catsAndDogsGrid);

            // Clickable-image example.
            catImage = new XAMLiteImage(this);
            catImage.Width = 100;
            catImage.Height = 100;
            catImage.VerticalAlignment = VerticalAlignment.Bottom;
            catImage.HorizontalAlignment = HorizontalAlignment.Right;
            catImage.SourceName = @"Textures/Cat";
            Components.Add(catImage);

            // Rollover example
            dogImage = new XAMLiteImage(this);
            dogImage.SourceName = @"Textures/Dog";
            dogImage.VerticalAlignment = VerticalAlignment.Top;
            dogImage.HorizontalAlignment = HorizontalAlignment.Right;
            Components.Add(dogImage);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();

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
