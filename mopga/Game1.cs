using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mopga
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static int gameWidth = 800;
        public static int gameHeight = 600;
        public static int gameOffset = 30;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        readonly Random r = new Random();
        Texture2D dustmanTexture;
        Vector2 dustmanPosition;
        float dustmanSpeed;
        SpriteFont font;
        int score = 0;
        readonly List<GameItem> wastes = new List<GameItem>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = gameWidth;
            graphics.PreferredBackBufferHeight = gameHeight;
            graphics.ApplyChanges();
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
            dustmanPosition = new Vector2(
                graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2
                );
            dustmanSpeed = 100f;

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
            dustmanTexture = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("Font");

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

            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                dustmanPosition.Y -= dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                dustmanPosition.Y += dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                dustmanPosition.X -= dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                dustmanPosition.X += dustmanSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            dustmanPosition.X = Math.Min(Math.Max(dustmanTexture.Width / 2, dustmanPosition.X), graphics.PreferredBackBufferWidth - dustmanTexture.Width / 2);
            dustmanPosition.Y = Math.Min(Math.Max(dustmanTexture.Height / 2, dustmanPosition.Y), graphics.PreferredBackBufferHeight - dustmanTexture.Height / 2);

            // TODO: Add your update logic here

            for (int i = 0; i < wastes.Count; i++)
            {
                if (wastes[i].position.X < dustmanPosition.X + 30) {
                    if (wastes[i].position.X > dustmanPosition.X - 30) {
                        if (wastes[i].position.Y < dustmanPosition.Y + 30) {
                            if (wastes[i].position.Y > dustmanPosition.Y - 30) {
                                wastes.RemoveAt(i);
                                score++;
                            }
                        }
                    }
                }
            }

            var x = r.Next(10000);

            if (x < ((score * 3) + 30))
            {
                wastes.Add(new GameItem());                   
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);

            // TODO: Add your drawing code here


            spriteBatch.Draw(
                dustmanTexture,
                dustmanPosition,
                null,
                Color.White,
                0f,
                new Vector2(dustmanTexture.Width / 2, dustmanTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
                );

            for (int i = 0; i < wastes.Count; i++)
            {
                spriteBatch.Draw(dustmanTexture, wastes[i].position, Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public enum WasteTypes
    {
        Metal = 0,
        Paper = 1,
        Plastic = 2,
        Glass = 3,
        Organic = 4,
    }

    public class GameItem
    {
        readonly Random r = new Random();

        public int type;
        public Vector2 position;

        public GameItem()
        {
            this.type = GetRandomType();
            this.position = GetRandomPosition();
        }

        public int GetRandomType()
        {
            Random r = new Random();

            int randomType = r.Next(Enum.GetNames(typeof(WasteTypes)).Length);

            return randomType;
        }

        public Vector2 GetRandomPosition()
        {
            int x = r.Next(Game1.gameOffset, Game1.gameWidth - Game1.gameOffset);
            int y = r.Next(Game1.gameOffset, Game1.gameHeight - Game1.gameOffset);

            return new Vector2(x, y);
        }
    }
}