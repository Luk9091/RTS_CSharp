using System;

namespace Game
{
    public interface ISiegeable{
        public bool Defense();
        public (List<Item>, int) Loot();
    }
}
