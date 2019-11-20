using Microsoft.Xna.Framework;
using System;

namespace mopga
{
    class Waste : GameItem
    {
        readonly Random r = new Random();

        public Waste()
        {
            this.position = GetRandomPosition();
        }

        public Vector2 GetRandomPosition()
        {
            int x = r.Next(Game1.gameOffset, Game1.gameWidth - Game1.gameOffset);
            int y = r.Next(Game1.gameOffset, Game1.gameHeight - Game1.gameOffset);

            return new Vector2(x, y);
        }
    }
}
