using System;

namespace Game
{
    public enum Quality{
        POOR = 0,
        COMMON = 1,
        GOOD = 2,
    }

    public interface QualityItem{
        public Quality quality {get;}
        public void UpdateQuality(Quality newQuality);
    }
}
