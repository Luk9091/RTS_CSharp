using System;
using System.Data.SqlTypes;

namespace Game
{
    public class Merchant: Building{
        private Dictionary<ItemType, Item> buyingOrder = new Dictionary<ItemType, Item>{
            {ItemType.WOOD,     new Material(ItemType.WOOD, 5, 5, 1, 10)},
            {ItemType.STONE,    new Material(ItemType.STONE, 3, 10, 1, 10)},
            {ItemType.CLAY,     new Material(ItemType.CLAY, 2, 2, 1, 20)},
            {ItemType.BRICK,    new Material(ItemType.BRICK, 8, 5, 1, 20)},
            {ItemType.PICKAXE,  new     Tool(ItemType.PICKAXE, 10, 5, Quality.COMMON, 10)},
            {ItemType.AXE,      new     Tool(ItemType.AXE, 10, 5, Quality.COMMON, 10)},
            {ItemType.HAMMER,   new     Tool(ItemType.HAMMER, 10, 5, Quality.COMMON, 10)},
            {ItemType.SHOVEL,   new     Tool(ItemType.SHOVEL, 10, 5, Quality.COMMON, 10)},
        };


        private enum Action {NONE, BUY, SELL};
        private Action action;
        protected Item workOn;
        protected int money;
        protected int price;
        public Merchant(int storageLimit, Dictionary<ItemType, int> dependentToBuild, int residentsLimit, int decorationLimit): base(storageLimit, dependentToBuild, residentsLimit, decorationLimit){
            this.Name = "Merchant";
            workOn = null!;
        }

        private double sellingMultiplier = 0.8;
        private double buyingMultiplier = 1.2;

        private int Sell(){
            Random rng = new Random();

            if (rng.Next(0, 100) < 30){
                return 0;
            } else {
                action = Action.NONE;
                state = BuildingStates.STOPPED;
                return price;
            }
        }

        private Item Buy(){
            Random rng = new Random();
            if (rng.Next(0, 100) < 30){
                return null!;
            } else {
                action = Action.NONE;
                state = BuildingStates.STOPPED;
                return workOn;
            }
        }

        private bool SellOrder(City city){
            foreach(KeyValuePair<ItemType, int> item in city.sellingOrder){
                if (city.CountItem(item.Key) == 0){
                    continue;
                }
                workOn = city.RemoveItem(item.Key, item.Value);
                city.sellingOrder[item.Key] -= workOn.amount;
                if (city.sellingOrder[item.Key] == 0){
                    city.sellingOrder.Remove(item.Key);
                }
                action = Action.SELL;
                state = BuildingStates.WORKING;

                double boostPrice = 1;
                if (workOn is QualityItem qualityItem){
                    switch(qualityItem.quality){
                        case Quality.POOR:
                            boostPrice = 0.75;
                            break;
                        case Quality.COMMON:
                            boostPrice = 1;
                            break;
                        case Quality.GOOD:
                            boostPrice = 1.5;
                            break;
                    }
                }
                price = (int)(workOn.value * boostPrice * sellingMultiplier) * workOn.amount;
                return true;
            }
            return false;
        }

        private bool BuyOrder(City city){
            int amount = 0;
            foreach(KeyValuePair<ItemType, int> item in city.buyingOrder){
                if (buyingOrder.ContainsKey(item.Key)){
                    workOn = buyingOrder[item.Key];
                    if (item.Value > workOn.maxAmount){
                        amount = workOn.maxAmount;
                    } else {
                        amount = item.Value;
                    }


                    // break;

                    price = (int)(workOn.value * buyingMultiplier);

                    while(amount > 0){
                        if (city.money >= price * amount){
                            city.buyingOrder[workOn.itemType] -= amount;
                            if (city.buyingOrder[workOn.itemType] == 0)
                                city.buyingOrder.Remove(workOn.itemType);
                            city.money -= price * amount;
                            action = Action.BUY;
                            state = BuildingStates.WORKING;
                            return true;
                        }
                        amount--;
                    }
                }
            }
            return false;
        }



        public override void Update(City city){
            switch(state){
                case BuildingStates.UNDER_CONSTRUCTION:
                    UnderConstruction(city);
                    break;
                case BuildingStates.WORKING:
                    switch(action){
                        case Action.BUY:
                            city.AddItem(Buy());
                            break;
                        case Action.SELL:
                            city.money += Sell();
                            break;
                    }
                    break;
                case BuildingStates.STOPPED:
                    if (residents.Count() == 0){
                        if (city.citizens.Count() == 0){
                            return;
                        }
                        residents.Add(city.citizens.First());
                    }                    
                    if (SellOrder(city)) return;
                    if (BuyOrder(city)) return;
                    break;
            }
        }
    }
}
