using System;

namespace mopga
{
    class GameItem
    {
        public int type;

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
