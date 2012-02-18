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
    /// Tool Tip Demo
    /// NOTE: Currently not drawing correctly.  The logic seems to believe the
    /// XAMLiteControl target is actually slightly up and to the right of its 
    /// current position it is being drawn too.  This is not the case in 
    /// ProMiner.  In ProMiner, everything is drawing where it is expected.
    /// </summary>
    public class Game3 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        XAMLiteImage catImage;
        XAMLiteImage dogImage;
        XAMLiteToolTip toolTip_01;

        public Game3()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "XAMLite Demo (XNA) - Tool Tip Display";
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
            // Cat Image
            catImage = new XAMLiteImage(this);
            catImage.SourceName = @"Textures/Cat";
            catImage.VerticalAlignment = VerticalAlignment.Top;
            catImage.HorizontalAlignment = HorizontalAlignment.Center;
            catImage.MouseEnter += new MouseEventHandler(catImage_MouseEnter);
            catImage.MouseLeave += new MouseEventHandler(catImage_MouseLeave);
            Components.Add(catImage);

            // Dog Image
            dogImage = new XAMLiteImage(this);
            dogImage.SourceName = @"Textures/Dog";
            dogImage.MouseEnter += new MouseEventHandler(dogImage_MouseEnter);
            dogImage.MouseLeave += new MouseEventHandler(dogImage_MouseLeave);
            dogImage.VerticalAlignment = VerticalAlignment.Bottom;
            dogImage.HorizontalAlignment = HorizontalAlignment.Center;
            Components.Add(dogImage);

            toolTip_01 = new XAMLiteToolTip(this);
            toolTip_01.Content = "";
            toolTip_01.TextWrapping = TextWrapping.Wrap;
            toolTip_01.Width = 200;
            toolTip_01.Placement = PlacementMode.MousePoint;
            toolTip_01.FontFamily = new System.Windows.Media.FontFamily("Verdana12");
            toolTip_01.Background = System.Windows.Media.Brushes.Black;
            toolTip_01.Foreground = System.Windows.Media.Brushes.White;
            toolTip_01.Padding = new System.Windows.Thickness(10, 5, 5, 10);
            Components.Add(toolTip_01);

            // Initialize all game components. (This includes calling Initialize() on all XAMLite controls, 
            // since they are game components).
            base.Initialize();
        }

        void dogImage_MouseLeave(object sender, MouseEventArgs e)
        {
            toolTip_01.Content = "";
            toolTip_01.IsEnabled = false;
            toolTip_01.IsOpen = false;
        }
        
        void dogImage_MouseEnter(object sender, MouseEventArgs e)
        {
            toolTip_01.Content = "This is the Dog Image.";
            toolTip_01.Placement = PlacementMode.Top;
            toolTip_01.PlacementTarget = dogImage;
            toolTip_01.IsEnabled = true;
            toolTip_01.IsOpen = true;
        }

        void catImage_MouseLeave(object sender, MouseEventArgs e)
        {
            toolTip_01.Content = "";
            toolTip_01.IsEnabled = false;
            toolTip_01.IsOpen = false;
        }

        void catImage_MouseEnter(object sender, MouseEventArgs e)
        {
            toolTip_01.Content = "This is the Cat Image.";
            toolTip_01.Placement = PlacementMode.Bottom;
            toolTip_01.PlacementTarget = catImage;
            toolTip_01.IsEnabled = true;
            toolTip_01.IsOpen = true;
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
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            // Draw all game components. (This includes our XAMLite drawable game components).
            base.Draw(gameTime);

        }
    }
}
