﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

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
        static Texture2D wallTexture;
        static Texture2D[,] wallTextures;

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
            Classes.Map.GenerateHouse(15);
            
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
            wallTextures = Content.Load<Texture2D>("walls-common").SplitTileset(new Point(16,16));
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
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                Classes.Map.GenerateHouse(15);
            }
            // TODO: Add your update logic here
            Classes.Player.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState:SamplerState.PointClamp);
            foreach (Classes.Tile t in Classes.Map.tileMap)
            {
                Color c = Color.Magenta;
                switch (t.type)
                {
                    case Classes.TileType.none:
                        c = Color.LightGreen;
                        break;
                    case Classes.TileType.floor:
                        if (Keyboard.GetState().IsKeyDown(Keys.Space) && t.parentRoom != null)
                        {
                            c = new Color(t.parentRoom.roomPos.X/(t.parentRoom.roomPos.Y*1f), t.parentRoom.roomPos.Y /(t.parentRoom.roomPos.X * 1f), 0);
                        }
                        else c = Color.LightGray;
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

                switch (t.type)
                {
                    case Classes.TileType.wall:
                        Texture2D tex = wallTexture;
                        switch (t.GetAccordingTexture())
                        {
                            case Classes.WallTextureType.horizontal:
                                tex = wallTextures[1, 0];
                                break;
                            case Classes.WallTextureType.vertical:
                                tex = wallTextures[0, 1];
                                break;
                            case Classes.WallTextureType.rightbottomcorner:
                                tex = wallTextures[0, 0];
                                break;
                            case Classes.WallTextureType.leftbottomcorner:
                                tex = wallTextures[1, 1];
                                break;
                            case Classes.WallTextureType.righttopcorner:
                                tex = wallTextures[0, 2];
                                break;
                            case Classes.WallTextureType.lefttopcorner:
                                tex = wallTextures[1, 2];
                                break;
                            case Classes.WallTextureType.allbutupjoint:
                                tex = wallTextures[3, 0];
                                break;
                            case Classes.WallTextureType.allbutrightjoint:
                                tex = wallTextures[2, 1];
                                break;
                            case Classes.WallTextureType.allbutbottomjoint:
                                tex = wallTextures[2, 0];
                                break;
                            case Classes.WallTextureType.allbutleftjoint:
                                tex = wallTextures[2, 2];
                                break;
                            case Classes.WallTextureType.unknown:
                                tex = blankTexture;
                                break;
                            default:
                                break;
                        }
                        spriteBatch.Draw(tex, new Rectangle(new Point(t.pos.X * 32, t.pos.Y * 32) - Classes.Player.pos, new Point(32)), Color.White);
                        break;
                    default:
                        spriteBatch.Draw(blankTexture, new Rectangle(new Point(t.pos.X * 32, t.pos.Y * 32) - Classes.Player.pos, new Point(32)), c);
                        break;
                }
                
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
