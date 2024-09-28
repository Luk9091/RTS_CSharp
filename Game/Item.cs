using System;

namespace Game
{
    public enum ItemType{
        NONE,
        PICKAXE,
        AXE,
        // HOE,
        HAMMER,
        SHOVEL,

        STONE,
        WOOD,
        CLAY,
        BRICK,
        // IRON,
    }
    public abstract class Item{
        public ItemType itemType { get; protected set; }
        public int value { get; protected set; }
        public int weight { get; protected set; }
        
        public int amount { get; set; }
        public int maxAmount { get; protected set; }

        public Item(ItemType itemType, int value, int weight){
            this.itemType = itemType;
            this.value = value;
            this.weight = weight;
        }

        public Item(int value, int weight){
            this.value = value;
            this.weight = weight;
        }
    }
}
