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

        XAMLiteImage catImage2;
        XAMLiteImage dogImage2;

        int _mouseDownCount = 0;

        public Game2 ()
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
        protected override void Initialize ()
        {

            // Here's the reference XAML we're mimicing.
            //<Grid Name="catsAndDogsGrid" Width="101" Height="181" VerticalAlignment="Top" HorizontalAlignment="Left" Margin="22,21,0,0"  Background="LightBlue">
            //    <Image Name="catImage" Width="75" Height="75" VerticalAlignment="Top" HorizontalAlignment="Left" Margin="13,12,0,0"  Source="/WpfReferenceDemo;component/Content/Cat.png" />
            //    <Image Name="dogImage" Width="75"  Height="75" VerticalAlignment="Top" HorizontalAlignment="Left" Margin="13,97,0,0"  Source="/WpfReferenceDemo;component/Content/Dog.png" />
            //</Grid>


            // Parent Grid
            catsAndDogsGrid = new XAMLiteGrid(this);
            catsAndDogsGrid.Name = "catsAndDogsGrid";
            catsAndDogsGrid.Width = 101;
            catsAndDogsGrid.Height = 181;
            catsAndDogsGrid.HorizontalAlignment = HorizontalAlignment.Left;
            catsAndDogsGrid.VerticalAlignment = VerticalAlignment.Top;
            catsAndDogsGrid.Background = Brushes.LightBlue;
            catsAndDogsGrid.Margin = new Thickness(22, 21, 0, 0);
            Components.Add(catsAndDogsGrid);

            // Child Cat Image
            catImage = new XAMLiteImage(this);
            catImage.SourceName = @"Textures/Cat";
            catImage.Width = 75;
            catImage.Height = 75;
            catImage.VerticalAlignment = VerticalAlignment.Top;
            catImage.HorizontalAlignment = HorizontalAlignment.Left;
            catImage.Margin = new Thickness(13, 12, 0, 0);
            catImage.Visible = Visibility.Visible;
            catsAndDogsGrid.Children.Add(catImage);

            // Child Dog Image
            dogImage = new XAMLiteImage(this);
            dogImage.SourceName = @"Textures/Dog";
            dogImage.Width = 75;
            dogImage.Height = 75;
            dogImage.VerticalAlignment = VerticalAlignment.Top;
            dogImage.HorizontalAlignment = HorizontalAlignment.Left;
            dogImage.Margin = new Thickness(13, 97, 0, 0);
            dogImage.Visible = Visibility.Hidden;
            catsAndDogsGrid.Children.Add(dogImage);

            // Clickable image.
            var clickableImage = new XAMLiteImage(this);
            clickableImage.Width = 100;
            clickableImage.Height = 100;
            clickableImage.VerticalAlignment = VerticalAlignment.Bottom;
            clickableImage.HorizontalAlignment = HorizontalAlignment.Right;
            clickableImage.SourceName = @"Textures/Button";
            clickableImage.MouseDown += new MouseButtonEventHandler(clickableImage_MouseDown);
            Components.Add(clickableImage);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();
        }

        void clickableImage_MouseDown (object sender, MouseButtonEventArgs e)
        {
            _mouseDownCount++;
            Window.Title = "MouseDown Count = " + _mouseDownCount;

            catsAndDogsGrid.ToggleVisibility();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent ()
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
        protected override void UnloadContent ()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update (GameTime gameTime)
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
        protected override void Draw (GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            // Draw all game components. (This includes our XAMLite drawable game components).
            base.Draw(gameTime);

        }
    }
}
