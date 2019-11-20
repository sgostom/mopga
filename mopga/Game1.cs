﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mopga
{
    public class Game1 : Game
    {
        public static int gameWidth = 1200;
        public static int gameHeight = 800;
        public static int gameOffset = 50;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameStates gameState = GameStates.Menu;

        readonly Random r = new Random();

        Dustman dustman = new Dustman();
        readonly List<Texture2D> dustmanTextures = new List<Texture2D>();
        Vector2 dustmanPosition;

        Texture2D heartTexture;
        int lifeCount = 3;

        readonly List<Waste> wastes = new List<Waste>();
        readonly List<Texture2D> wastesTextures = new List<Texture2D>();

        SpriteFont font;    
        int score = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            dustmanPosition = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2
                );

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            heartTexture = Content.Load<Texture2D>("heart");

            dustmanTextures.Add(Content.Load<Texture2D>("dustmanTextures/metalD"));
            dustmanTextures.Add(Content.Load<Texture2D>("dustmanTextures/paperD"));
            dustmanTextures.Add(Content.Load<Texture2D>("dustmanTextures/plasticD"));
            dustmanTextures.Add(Content.Load<Texture2D>("dustmanTextures/glassD"));
            dustmanTextures.Add(Content.Load<Texture2D>("dustmanTextures/organicD"));
            
            wastesTextures.Add(Content.Load<Texture2D>("wasteTextures/metal1"));
            wastesTextures.Add(Content.Load<Texture2D>("wasteTextures/paper1"));
            wastesTextures.Add(Content.Load<Texture2D>("wasteTextures/plastic1"));
            wastesTextures.Add(Content.Load<Texture2D>("wasteTextures/glass1"));
            wastesTextures.Add(Content.Load<Texture2D>("wasteTextures/organic1"));

            font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();

            switch (gameState)
            {
                case GameStates.Menu:

                    if (kstate.IsKeyDown(Keys.D1))
                        gameState = GameStates.Play;
                    
                    if (kstate.IsKeyDown(Keys.D2))
                        gameState = GameStates.Tutorial;

                    if (kstate.IsKeyDown(Keys.D3))
                        Exit();

                    break;                
                
                case GameStates.Tutorial:

                    if (kstate.IsKeyDown(Keys.Escape))
                        gameState = GameStates.Menu;

                    break;

                case GameStates.Play:

                    if (kstate.IsKeyDown(Keys.Up))
                        dustmanPosition.Y -= dustman.dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (kstate.IsKeyDown(Keys.Down))
                        dustmanPosition.Y += dustman.dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (kstate.IsKeyDown(Keys.Left))
                        dustmanPosition.X -= dustman.dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (kstate.IsKeyDown(Keys.Right))
                        dustmanPosition.X += dustman.dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (kstate.IsKeyDown(Keys.D1))
                        dustman.type = (int)WasteTypes.Metal;

                    if (kstate.IsKeyDown(Keys.D2))
                        dustman.type = (int)WasteTypes.Paper;

                    if (kstate.IsKeyDown(Keys.D3))
                        dustman.type = (int)WasteTypes.Plastic;

                    if (kstate.IsKeyDown(Keys.D4))
                        dustman.type = (int)WasteTypes.Glass;

                    if (kstate.IsKeyDown(Keys.D5))
                        dustman.type = (int)WasteTypes.Organic;


                    dustmanPosition.X = Math.Min(Math.Max(dustmanTextures[dustman.type].Width / 2, dustmanPosition.X),
                        graphics.PreferredBackBufferWidth - dustmanTextures[dustman.type].Width / 2);
                    dustmanPosition.Y = Math.Min(Math.Max(dustmanTextures[dustman.type].Height / 2, dustmanPosition.Y),
                        graphics.PreferredBackBufferHeight - dustmanTextures[dustman.type].Height / 2);

                    var x = r.Next(10000);

                    if (x < ((score * 3) + 50))
                    {
                        wastes.Add(new Waste());
                    }

                    for (int i = 0; i < wastes.Count; i++)
                    {
                        if ((wastes[i].position.X + 50 < dustmanPosition.X + 50)
                            && (wastes[i].position.X + 50 > dustmanPosition.X - 50)
                            && (wastes[i].position.Y + 50 < dustmanPosition.Y + 50)
                            && (wastes[i].position.Y + 50 > dustmanPosition.Y - 50))
                        {
                            var currentWasteType = wastes[i].type;
                            wastes.RemoveAt(i);

                            if (currentWasteType == dustman.type)
                            {
                                score++;
                            }
                            else
                            {
                                lifeCount--;
                            }
                        }
                    }

                    if (lifeCount <= 0)
                    {
                        gameState = GameStates.GameLost;
                    }

                    break;

                case GameStates.GameLost:

                    if (kstate.IsKeyDown(Keys.Enter))
                        gameState = GameStates.Play;
                    
                    if (kstate.IsKeyDown(Keys.Escape))
                        gameState = GameStates.Menu;

                    ResetData();

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            spriteBatch.Begin();

            switch (gameState)

            {
                case GameStates.Menu:
                    spriteBatch.DrawString(font, "1. Play", new Vector2(500, 300), Color.White);
                    spriteBatch.DrawString(font, "2. Tutorial", new Vector2(500, 350), Color.White);
                    spriteBatch.DrawString(font, "3. Exit", new Vector2(500, 400), Color.White);
                    break;

                case GameStates.Play:

                spriteBatch.DrawString(font, "Score: " + score, new Vector2(gameWidth - 300, 15), Color.White);

                for (int i = 0; i < lifeCount; i++)
                {
                    spriteBatch.Draw(heartTexture, new Vector2((gameWidth - 10 - 40 * (i + 1)), 10), Color.White);
                }

                spriteBatch.Draw(
                    dustmanTextures[dustman.type],
                    dustmanPosition,
                    null,
                    Color.White,
                    0f,
                    new Vector2(dustmanTextures[dustman.type].Width / 2, dustmanTextures[dustman.type].Height / 2),
                    Vector2.One,
                    SpriteEffects.None,
                    0f
                    );

                for (int i = 0; i < wastes.Count; i++)
                {
                    spriteBatch.Draw(wastesTextures[wastes[i].type], wastes[i].position, Color.White);
                }
                    break;

                case GameStates.GameLost:
                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(gameWidth - 300, 15), Color.White);
                    spriteBatch.DrawString(font, "Press enter to try again...", new Vector2(500, 388), Color.White);
                    break;

                case GameStates.Tutorial:
                    spriteBatch.DrawString(font, "Tutorial", new Vector2(500, 388), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetData()
        {
            lifeCount = 3;
            score = 0;
            wastes.Clear();
        }
    }
}