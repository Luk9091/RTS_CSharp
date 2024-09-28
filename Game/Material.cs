using System;

namespace Game
{
    public class Material: Item{
        public Material(ItemType type, int value, int weight, int amount, int maxAmount): base(type, value, weight){
            this.amount = amount;
            this.maxAmount = maxAmount;
        }


        public static Material operator +(Material material, int amount){
            material.amount += amount;
            return material;
        }
    }
}
