using System;
using System.Runtime;

namespace Game{
    public class Pair<T1, T2>{
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Pair(T1 item1, T2 item2){
            Item1 = item1;
            Item2 = item2;
        }
    }



    public abstract class StorageSystem{
        protected int capacity;
        public int currentCapacity {get; protected set;}
        protected int weight;
        protected int value;

        public int leftCapacity => capacity - currentCapacity;

        protected Dictionary<ItemType, Pair<List<Item>, int>> storage;

        public StorageSystem(int capacity){
            this.capacity = capacity;
            this.currentCapacity = 0;
            this.weight = 0;
            this.value = 0;
            this.storage = new Dictionary<ItemType, Pair<List<Item>, int>>();
        }

        
        public bool AddItem(Item item){
            if (currentCapacity < capacity){
                if (storage.ContainsKey(item.itemType)){
                    if (item.maxAmount == 1){
                        storage[item.itemType].Item1.Add(item);
                        currentCapacity += item.amount;
                    } else{
                        int newAmount = storage[item.itemType].Item1[0].amount + item.amount;
                        int inStack = storage[item.itemType].Item1[0].maxAmount;
                        if ((newAmount/inStack) > (storage[item.itemType].Item2/inStack)){
                            currentCapacity += 1;
                        }
                        // storage[item.itemType].Item2 = newAmount;
                    }
                } else {
                    storage[item.itemType] = new Pair<List<Item>, int>(new List<Item>(), 0);
                    storage[item.itemType].Item1.Add(item);
                    currentCapacity += 1;
                }

                storage[item.itemType].Item2 += item.amount;
                return true;
            }
            return false;
        }

        public bool AddItem(Material material){
            if (currentCapacity < capacity){
                if (storage.ContainsKey(material.itemType)){
                    int newAmount = storage[material.itemType].Item1[0].amount + material.amount;
                    int inStack = storage[material.itemType].Item1[0].maxAmount;
                    if ((newAmount/inStack) > (storage[material.itemType].Item2/inStack)){
                        currentCapacity += 1;
                    }
                    storage[material.itemType].Item2 = newAmount;
                } else {
                    storage[material.itemType] = new Pair<List<Item>, int>(new List<Item>(), 0);
                    storage[material.itemType].Item1.Add(material);
                    storage[material.itemType].Item2 = material.amount;
                    storage[material.itemType].Item1[0].amount = 0;
                    currentCapacity += 1;
                }

                return true;
            }
            return false;
        }

        public bool AddItem(Tool tool){
            if (currentCapacity < capacity){
                if (storage.ContainsKey(tool.itemType)){
                    storage[tool.itemType].Item1.Add(tool);
                } else {
                    storage[tool.itemType] = new Pair<List<Item>, int>(new List<Item>(), tool.amount);
                    storage[tool.itemType].Item1.Add(tool);
                }
                currentCapacity += tool.amount;
                return true;
            }
            return false;
        }

        public int CountItem(ItemType itemType){
            if (storage.ContainsKey(itemType)){
                return storage[itemType].Item2;
            }
            return 0;
        }

        public Item RemoveItem(ItemType itemType, int amount = 1){
            if (storage.ContainsKey(itemType)){
                if (amount > storage[itemType].Item2)
                    amount = storage[itemType].Item2;
                if (amount > storage[itemType].Item1[0].maxAmount)
                    amount = storage[itemType].Item1[0].maxAmount;

                storage[itemType].Item2 -= amount;
                Item item = storage[itemType].Item1[0];
                if (item.maxAmount == 1) {
                    storage[itemType].Item1.RemoveAt(0);
                } else {
                    item.amount = amount;
                }

                if(storage[itemType].Item2 == 0){
                    storage[itemType].Item2 = 0;
                    storage.Remove(itemType);
                    currentCapacity--;
                } else if (amount == item.maxAmount) {
                    currentCapacity--;
                }

                return item;
            }
            return null!;
        }

        public Tool RemoveTool(ItemType itemType){
            if (storage.ContainsKey(itemType)){
                Tool tool = (Tool)storage[itemType].Item1[0];
                storage.Remove(itemType);
                return tool;
            }
            return null!;
        }

    }
}
