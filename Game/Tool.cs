using System;

namespace Game
{
    public class Tool: Item, QualityItem{
        public int durability { get; protected set; }
        public Quality quality {get; protected set;}
        public int workPoints { get; protected set; }

        public Tool(ItemType type, int value, int weight, Quality quality, int workPoints): base(type, value, weight){
            this.workPoints = workPoints;
            this.durability = 10;
            this.quality = quality;
            this.amount = 1;
            this.maxAmount = 1;

            this.itemType = (ItemType)type;
        }

        public void UpdateQuality(Quality newQuality){
            quality = newQuality;
        }
        
        public int Use(){
            if (durability > 0){
                durability--;
                return 1;
            }
            return 0;
        }
    }
}
