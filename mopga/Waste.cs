using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mopga
{
    class Waste : GameItem
    {
        readonly Random r = new Random();

        public Vector2 position;

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
