using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace mopga
{
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
            dustmanSpeed = 100f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            dustmanTexture = Content.Load<Texture2D>("ball");

            wastesTextures.Add(Content.Load<Texture2D>("metal1"));
            wastesTextures.Add(Content.Load<Texture2D>("paper1"));
            wastesTextures.Add(Content.Load<Texture2D>("plastic1"));
            wastesTextures.Add(Content.Load<Texture2D>("glass1"));
            wastesTextures.Add(Content.Load<Texture2D>("organic1"));

            font = Content.Load<SpriteFont>("Font");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

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

            for (int i = 0; i < wastes.Count; i++)
            {
                if ((wastes[i].position.X + 32 < dustmanPosition.X + 50)
                    && (wastes[i].position.X + 32 > dustmanPosition.X - 50)
                    && (wastes[i].position.Y + 32 < dustmanPosition.Y + 50)
                    && (wastes[i].position.Y + 32 > dustmanPosition.Y - 50)) {
                    wastes.RemoveAt(i);
                    score++;
                }
            }

            var x = r.Next(10000);

            if (x < ((score * 3) + 30))
            {
                wastes.Add(new Waste());                   
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            spriteBatch.Begin();

            spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);

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
                spriteBatch.Draw(wastesTextures[wastes[i].type], wastes[i].position, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}