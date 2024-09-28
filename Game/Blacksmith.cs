
using System;

namespace Game{
    public class Blacksmith: Building{
        private static Dictionary<ItemType, Pair<Dictionary<ItemType, int>, Item>> recepis = new Dictionary<ItemType, Pair<Dictionary<ItemType, int>, Item>>{
            {ItemType.AXE, new Pair<Dictionary<ItemType, int>, Item>(new Dictionary<ItemType, int>{{ItemType.WOOD, 1}, {ItemType.STONE, 1}}, new Tool(ItemType.AXE, 10, 5, Quality.COMMON, 10))},
            {ItemType.HAMMER, new Pair<Dictionary<ItemType, int>, Item>(new Dictionary<ItemType, int>{{ItemType.WOOD, 1}, {ItemType.STONE, 1}}, new Tool(ItemType.HAMMER, 10, 5, Quality.COMMON, 10))},
            {ItemType.PICKAXE, new Pair<Dictionary<ItemType, int>, Item>(new Dictionary<ItemType, int>{{ItemType.WOOD, 1}, {ItemType.STONE, 1}}, new Tool(ItemType.PICKAXE, 10, 5, Quality.COMMON, 10))},
        };


        
        protected ItemType workOn;
        public Blacksmith(int storageLimit, Dictionary<ItemType, int> dependance, int residentLimit, int decorationLimit): base(storageLimit, dependance, residentLimit, decorationLimit){
            this.Name = "Blacksmith";
            this.state = BuildingStates.UNDER_CONSTRUCTION;
            workOn = ItemType.NONE;
        }
        private Quality SelectQuality(){
            Random rng = new Random();
            int chance = rng.Next(0, 100);

            if (chance < 10){
                return Quality.POOR;
            } else if (chance < 30){
                return Quality.COMMON;
            } else{
                return Quality.GOOD;
            }

        }

        private Item Crafting(){
            foreach (Citizen worker in residents){
                progress += worker.Update(ItemType.HAMMER);
            }

            if (progress >= 100){
                Item item = recepis[workOn].Item2;
                if (item is QualityItem qualityItem){
                    qualityItem.UpdateQuality(SelectQuality());
                }

                progress = 0;
                return item;
            }
            return null!;
        }

        public override void Update(City city){
            switch(state){
                case BuildingStates.UNDER_CONSTRUCTION:
                    UnderConstruction(city);
                    break;
                case BuildingStates.WORKING:
                    Item item = Crafting();

                    if (item is not null){
                        city.AddItem(item);
                        Console.WriteLine($"{Name} crafted {item.itemType}");
                        this.state = BuildingStates.STOPPED;
                    }
                    break;

                case BuildingStates.STOPPED:
                    if (city.productionOrder.Keys.Count() > 0){
                        if (residents.Count() == 0){
                            if (city.citizens.Count() == 0){
                                return;
                            }
                            residents.Add(city.citizens.First());
                        }

                        foreach(Citizen worker in residents){
                            if (city.CountItem(ItemType.HAMMER) == 0) break;
                            
                            worker.AddItem(city.RemoveItem(ItemType.HAMMER));
                        }

                        foreach (KeyValuePair<ItemType, int> order in city.productionOrder){
                            if (recepis.ContainsKey(order.Key)){
                                state = BuildingStates.WORKING;
                                workOn = city.productionOrder.Keys.First();
                                // dependentToWork = recepis[workOn].need;
                                Console.WriteLine($"{Name} start working on {workOn}");
                                city.productionOrder[workOn]--;

                                if (city.productionOrder[workOn] <= 0){
                                    city.productionOrder.Remove(workOn);
                                }
                                break;
                            }
                        }
                    }
                    break;
            }
        }
    }
}