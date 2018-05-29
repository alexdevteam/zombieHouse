﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZombieProyect_Desktop
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        static Texture2D blankTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Classes.Map.InitializeMap(new Point(50,50));
            Classes.Map.MakeStartingRoom();
            Classes.Map.MakeAdjacentRoomFromWall(Classes.Map.rooms[0].containedWalls[1]);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            blankTexture = Content.Load<Texture2D>("blank");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach (Classes.Tile t in Classes.Map.tileMap)
            {
                Color c = Color.Magenta;
                switch (t.type)
                {
                    case Classes.TileType.none:
                        c = Color.LightGreen;
                        break;
                    case Classes.TileType.floor:
                        c = Color.LightGray;
                        break;
                    case Classes.TileType.wall:
                        c = Color.Gray;
                        break;
                    case Classes.TileType.door:
                        c = Color.MonoGameOrange;
                        break;
                    case Classes.TileType.blockeddoor:
                        c = Color.Red;
                        break;
                    default:
                        break;
                }
                Color cr = Color.DarkGray;
                if (t.parentRoom != null)
                {
                    cr = Color.Red;
                }

                if(Keyboard.GetState().IsKeyDown(Keys.Space)) //DrawRoom mode
                    spriteBatch.Draw(blankTexture, new Rectangle(new Point(t.pos.X * 32, t.pos.Y * 32) - Mouse.GetState().Position, new Point(32)), cr);
                else
                    spriteBatch.Draw(blankTexture, new Rectangle(new Point(t.pos.X * 32, t.pos.Y * 32) - Mouse.GetState().Position, new Point(32)), c);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
