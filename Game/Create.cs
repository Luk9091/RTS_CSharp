using System;
using System.ComponentModel;
/*
namespace Game
{
    public class Create: Item, IStackable{
        protected StorageSystem inventory;
        // public override Quality quality { get {
        //     int temp = 0;
        //     foreach (Item item in inventory.inventory[name]){
        //         temp = temp + (int)(item.quality);
        //     }
        //     temp = temp / inventory.storageCount;

        //     return Quality.POOR + temp;
        // }}

        public int inStack { get; private set; }
        public int stackLimit { get; private set; }
        public int weightPerOne { get {
            return inventory.weight/inStack + createWeight;
        }}

        public int valuePerOne { get {
            return inventory.value/inStack;
        }}

        private int createWeight;
        public override int weight { get {
            return inventory.weight + createWeight;
        }}


        public Create(string name, int weight, int storageLimit): base(name, 0, 100, Quality.COMMON, 0){
            this.inventory = new StorageSystem(storageLimit);
            this.createWeight = weight;
        }

        // public List<Item> CheckInventory(){
        //     List<Item> inside = new List<Item>();
        //     foreach (List<Item> items in inventory.inventory.Values){
        //         inside.AddRange(items);
        //     }
        //     return inside;
        // }

        public int AddToStack(int amount){
            int overflow = 0;
            if(inStack + amount > stackLimit){
                overflow = stackLimit - (inStack - amount);
                amount = inStack - overflow;
            }
            inStack += amount;
            createWeight = weightPerOne * inStack;
            value = valuePerOne * inStack;
            return overflow;
        }

        public int RemoveFromStack(int amount){
            if(inStack - amount < 0){
                return 0;
            }
            inStack -= amount;
            createWeight = weightPerOne * inStack;
            value = valuePerOne * inStack;
            return inStack;
        }

        public override bool Equals(object? obj){
            if (obj == null || !(obj is Create)){
                return false;
            }
            return this.name == ((Create)obj).name;
        }
        public override int GetHashCode(){
            return this.name.GetHashCode();
        }

        public static bool operator ==(Create a, Create b){
            bool status =  a.name == b.name;
            status = status & (a.quality == b.quality);
            return status;
        }

        public static bool operator !=(Create a, Create b){
            return !(a == b);
        }
    }
}
// */
