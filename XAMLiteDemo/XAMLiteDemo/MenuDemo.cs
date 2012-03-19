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
    public class MenuDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public MenuDemo()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "XAMLite Menu Demo (XNA)";

            // Default screen size.
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            XAMLiteMenu menu1 = new XAMLiteMenu(this);
            menu1.Margin = new Thickness(10, 5, 10, 5);
            menu1.HorizontalAlignment = HorizontalAlignment.Left;
            menu1.VerticalAlignment = VerticalAlignment.Top;

            XAMLiteMenuItem menuitem1 = new XAMLiteMenuItem(this);
            menuitem1.Header = "Head of Menu";
            menuitem1.Padding = new Thickness(10, 3, 10, 3);
            menuitem1.FontFamily = new FontFamily("Verdana16");
            menuitem1.Background = Brushes.Black;
            menuitem1.Foreground = Brushes.White;
            menu1.Items.Add(menuitem1);

            XAMLiteMenuItem menuitem2 = new XAMLiteMenuItem(this);
            menuitem2.Header = "Menu Item 2";
            menuitem2.Padding = new Thickness(0, 3, 10, 3);
            menuitem2.FontFamily = new FontFamily("Verdana16");
            menuitem2.Background = Brushes.Black;
            menuitem2.Foreground = Brushes.White;
            menu1.Items.Add(menuitem2);

            XAMLiteMenuItem menuitem3 = new XAMLiteMenuItem(this);
            menuitem3.Header = "Menu Item 3 with a bunch of text";
            menuitem3.FontFamily = new FontFamily("Verdana16");
            menuitem3.Padding = new Thickness(0, 3, 10, 3);
            menuitem3.Background = Brushes.Black;
            menuitem3.Foreground = Brushes.White;
            menu1.Items.Add(menuitem3);

            XAMLiteMenuItem submenuitem3_1 = new XAMLiteMenuItem(this);
            submenuitem3_1.Header = "SubMenu Item 1";
            submenuitem3_1.FontFamily = new FontFamily("Verdana16");
            submenuitem3_1.Padding = new Thickness(0, 3, 10, 3);
            submenuitem3_1.Background = Brushes.Black;
            submenuitem3_1.Foreground = Brushes.White;
            menuitem3.Items.Add(submenuitem3_1);

            XAMLiteMenuItem submenuitem3_2 = new XAMLiteMenuItem(this);
            submenuitem3_2.Header = "SubMenu Item 2 plus more words";
            submenuitem3_2.FontFamily = new FontFamily("Verdana16");
            submenuitem3_2.Padding = new Thickness(0, 3, 10, 3);
            submenuitem3_2.Background = Brushes.Black;
            submenuitem3_2.Foreground = Brushes.White;
            menuitem3.Items.Add(submenuitem3_2);

            Components.Add(menu1);

            XAMLiteMenu menu2 = new XAMLiteMenu(this);
            menu2.Width = 75;
            menu2.Margin = new Thickness(145, 5, 0, 0);
            menu2.HorizontalAlignment = HorizontalAlignment.Left;
            menu2.VerticalAlignment = VerticalAlignment.Top;

            XAMLiteMenuItem menuitem2_1 = new XAMLiteMenuItem(this);
            menuitem2_1.Header = "Head of Menu";
            menuitem2_1.FontFamily = new FontFamily("Verdana15");
            menuitem2_1.Padding = new Thickness(10, 3, 10, 3);
            menuitem2_1.Background = Brushes.DarkBlue;
            menuitem2_1.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_1);

            XAMLiteMenuItem menuitem2_2 = new XAMLiteMenuItem(this);
            menuitem2_2.Header = "Menu Item 2";
            menuitem2_2.FontFamily = new FontFamily("Verdana15");
            menuitem2_2.Padding = new Thickness(0, 3, 10, 3);
            menuitem2_2.Background = Brushes.DarkBlue;
            menuitem2_2.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_2);

            XAMLiteMenuItem menuitem2_3 = new XAMLiteMenuItem(this);
            menuitem2_3.Header = "Menu Item 3 blah blah";
            menuitem2_3.FontFamily = new FontFamily("Verdana15");
            menuitem2_3.Padding = new Thickness(0, 3, 10, 3);
            menuitem2_3.Background = Brushes.DarkBlue;
            menuitem2_3.Foreground = Brushes.White;
            menu2.Items.Add(menuitem2_3);

            XAMLiteMenuItem submenuitem2_3_1 = new XAMLiteMenuItem(this);
            submenuitem2_3_1.Header = "SubMenu Item 1";
            submenuitem2_3_1.FontFamily = new FontFamily("Verdana15");
            submenuitem2_3_1.Padding = new Thickness(0, 3, 10, 3);
            submenuitem2_3_1.Background = Brushes.DarkBlue;
            submenuitem2_3_1.Foreground = Brushes.White;
            menuitem2_3.Items.Add(submenuitem2_3_1);

            XAMLiteMenuItem submenuitem2_3_2 = new XAMLiteMenuItem(this);
            submenuitem2_3_2.Header = "SubMenu Item 2 plus more words";
            submenuitem2_3_2.FontFamily = new FontFamily("Verdana15");
            submenuitem2_3_2.Padding = new Thickness(0, 3, 10, 3);
            submenuitem2_3_2.Background = Brushes.DarkBlue;
            submenuitem2_3_2.Foreground = Brushes.White;
            menuitem2_3.Items.Add(submenuitem2_3_2);

            Components.Add(menu2);

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
