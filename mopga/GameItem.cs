using Microsoft.Xna.Framework;
using System;

namespace mopga
{
    abstract class GameItem
    {
        public int type;
        public Vector2 position;

        public GameItem()
        {
            this.type = GetRandomType();
        }

        public int GetRandomType()
        {
            Random r = new Random();

            int randomType = r.Next(Enum.GetNames(typeof(WasteTypes)).Length);

            return randomType;
        }
    }
}
