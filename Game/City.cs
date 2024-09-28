using System;
using System.Data.SqlTypes;
using System.IO;
using System.Security.Cryptography;

namespace Game
{
    public class City: StorageSystem{
        public List<Building> buildings;
        public List<Citizen> citizens;
        public Dictionary<string, Mine> mines;
        public int money { get;  set; }

        public Dictionary<ItemType, int> productionOrder;
        public Dictionary<ItemType, int> sellingOrder;
        public Dictionary<ItemType, int> buyingOrder;
        public int citizenLimit { get; private set; }


        public City(): base(100){
            buildings = new List<Building>();
            citizens = new List<Citizen>();
            sellingOrder = new Dictionary<ItemType, int>();
            buyingOrder = new Dictionary<ItemType, int>();


            for (int i = 0; i < 5; i++)
                AddCitizen();

            Dictionary<ItemType, int> dependency = new Dictionary<ItemType, int>();
            Building building = new Building(10, dependency, 4, 5);
            for (int i = 0; i < 2; i++)
                AddBuilding(building);
            
            // Building shop = new Merchant(10, dependency, 2, 5);
            // shop.SetProgress(99);
            // AddBuilding(shop);

            mines = new Dictionary<string, Mine>();
            mines.Add("cave",   new Mine(new Material(ItemType.STONE, 5, 10, 1, 10), ItemType.PICKAXE));
            mines.Add("forest", new Mine(new Material(ItemType.WOOD,  5, 10, 1, 10), ItemType.AXE));

            this.money = 100;

            productionOrder = new Dictionary<ItemType, int>{
                {ItemType.AXE, 1},
                {ItemType.PICKAXE, 1},
            };
        }



        public void DisplayCitizen(){
            int index = 0;
            Console.WriteLine("In city:");
            foreach (Citizen citizen in citizens){
                Console.WriteLine($"{index, 3}: {citizen}");
                index++;
            }
        }
        public void DisplayBuilding(){
            int index = 0;
            foreach (Building building in buildings){
                Console.WriteLine($"{index, 3}: {building}");
                index++;
            }
        }

        public void DisplayMine(){
            int index = 0;
            foreach (KeyValuePair<string, Mine> mine in mines){
                Console.WriteLine($"{index, 3}: {mine.Key}, {mine.Value}");
                index++;
            }
        }

        public void DisplayItem(){
            int index = 0;
            foreach(KeyValuePair<ItemType, Pair<List<Item>, int>> material in storage){
                UI.DisplayShortList($"{material.Key}: {material.Value.Item2}");
                index++;
            }
        }

        public void DisplayProductionOrder(){
            int index = 0;
            foreach(KeyValuePair<ItemType, int> order in productionOrder){
                UI.DisplayShortList($"p: {order.Key}: {order.Value}");
                index++;
            }
        }

        public void DisplaySellingOrder(){
            int index = 0;
            foreach(KeyValuePair<ItemType, int> order in sellingOrder){
                UI.DisplayShortList($"s: {order.Key}: {order.Value}");
                index++;
            }
        }

        public void DisplayBuyingOrder(){
            int index = 0;
            foreach(KeyValuePair<ItemType, int> order in buyingOrder){
                UI.DisplayShortList($"b: {order.Key}: {order.Value}");
                index++;
            }
        }

        private void AddCitizen(){
            string[] names = File.ReadAllLines("NameList.txt");
            Random rng = new Random();
            string name = names[rng.Next(0, names.Length)];
            Citizen citizen = new Citizen(name, 100, HAPPINESS_t.NEUTRAL, 50);
            citizen.SetAge(rng.Next(15, 25));

            citizens.Add(citizen);
        }

        private void AddBuilding(Building building){
            capacity += building.storageLimit;
            citizenLimit += building.residentsLimit;
            buildings.Add(building);
        }

        public void Build(string[] args){
            if (args.Length <= 1){
                Console.WriteLine("Not enough arguments");
                return;
            }
            args = args[1..];
            switch (UI.str2cmd(args[0])){
                case COMMANDS.BUILDING:
                case COMMANDS.HOUSE:
                    Dictionary<ItemType, int> dependency = new Dictionary<ItemType, int>{
                        {ItemType.WOOD, 10},
                        {ItemType.STONE, 5},
                    };
                    Building house = new Building(10, dependency, 4, 5);
                    AddBuilding(house);
                    Console.WriteLine("Build new house");
                    break;
                
                case COMMANDS.BLACKSMITH:
                    dependency = new Dictionary<ItemType, int>{
                        {ItemType.WOOD, 20},
                        {ItemType.STONE, 10},
                        {ItemType.CLAY, 5},
                    };
                    Building blacksmith = new Blacksmith(10, dependency, 4, 5);
                    AddBuilding(blacksmith);
                    Console.WriteLine("Build new blacksmith");
                    break;

                case COMMANDS.SHOP:
                    dependency = new Dictionary<ItemType, int>{
                        {ItemType.WOOD, 20},
                        {ItemType.STONE, 20},
                        {ItemType.BRICK, 20},
                    };
                    Building shop = new Merchant(10, dependency, 2, 5);
                    AddBuilding(shop);
                    Console.WriteLine("Build new shop");
                    break;

                case COMMANDS.MINE:
                    if (args.Length == 3){
                        switch (UI.str2cmd(args[1])){
                            case COMMANDS.FOREST:
                                mines.Add(args[2], new Mine(new Material(ItemType.WOOD, 5, 5, 1, 10), ItemType.AXE));
                                break;
                            case COMMANDS.CAVE:
                                mines.Add(args[2], new Mine(new Material(ItemType.STONE, 3, 10, 1, 10), ItemType.PICKAXE));
                                break;
                            case COMMANDS.QUARRY:
                                mines.Add(args[2], new Mine(new Material(ItemType.CLAY, 2, 2, 1, 20), ItemType.SHOVEL));
                                break;
                            default:
                                Console.WriteLine("Invalid mine name");
                                return;
                        }
                        Console.WriteLine($"Build new mine: {args[2]}");
                    } else if (args.Length == 2){
                        Console.WriteLine("Enter mine name");

                    }else {
                        Console.WriteLine("Which mine?");
                        Console.WriteLine("Mines: Forest");
                        Console.WriteLine("Mines: Cave");
                        Console.WriteLine("Mines: Quarry");
                    }
                break;

                default:
                    break;
            }
        }
        
        public int BuildingUnderConstructionCount(){
            int count = 0;
            foreach(Building building in buildings){
                if (building.state is BuildingStates.UNDER_CONSTRUCTION)
                    count++;
            }
            return count;
        }
        public int BuildingCount(){
            return buildings.Count;
        }

        public void Mine(string[] cmd){
            if (cmd.Length < 2){
                Console.WriteLine("Not enough arguments");
                return;
            }
            string mineName = cmd[1];
            if (!mines.ContainsKey(mineName)){
                Console.WriteLine("Invalid mine name");
                return;
            }
            if (cmd.Length <= 2){
                foreach(Citizen citizen in citizens){
                    if (citizen.ableToWork == true){
                        if(mines[mineName].AddWorker(citizen, mineName)){
                            if (CountItem(mines[mineName].neededTool) > 0){
                                citizen.AddItem(RemoveTool(mines[mineName].neededTool));
                            }
                            citizens.Remove(citizen);
                        } else {
                            Console.WriteLine("Mine is full");
                        }
                        return;
                    }
                }
            }
            switch (UI.str2cmd(cmd[2])){
                case COMMANDS.REMOVE:
                    if (mines[mineName].workers.Count <= 0){
                        Console.WriteLine("Mine is empty");
                        return;
                    }
                    citizens.Add(mines[mineName].RemoveWorker(mineName));
                    break;
                case COMMANDS.ADD:
                    foreach(Citizen citizen in citizens){
                        if (citizen.ableToWork == true){
                            if(mines[mineName].AddWorker(citizen, mineName))
                                citizens.Remove(citizen);
                                if (CountItem(mines[mineName].neededTool) > 0){
                                    AddItem(citizen.RemoveItem(mines[mineName].neededTool));
                                }
                            break;
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Unexpected command to mine");
                    break;
            }
        }


        public void Produce(string[] cmd){
            if (cmd.Length < 2){
                Console.WriteLine("Not enough arguments");
                return;
            }
            int count = 1;
            ItemType tool = UI.str2tool(cmd[1]);
            if (cmd.Length > 1)
                count = int.Parse(cmd[2]);
            
            if (tool == ItemType.NONE){
                Console.WriteLine("Invalid tool");
                return;
            }

            if (productionOrder.ContainsKey(tool)){
                productionOrder[tool] += count;
            } else {
                productionOrder.Add(tool, count);
            }
        }


        public void Sell(string[] args){
            if (args.Length < 2){
                Console.WriteLine("Not enough arguments");
                return;
            }
            ItemType item = UI.str2tool(args[1]);
            if (item == ItemType.NONE){
                Console.WriteLine("Invalid item");
                return;
            }

            int amount = 1;
            if (args.Length > 2){
                try{
                    amount = int.Parse(args[2]);
                }
                catch (FormatException){
                    Console.WriteLine("Invalid amount");
                    return;
                }
            }

            // if (CountItem(item) < amount){
            //     Console.WriteLine("Doesn't have item");
            //     return;
            // }

            if (sellingOrder.ContainsKey(item)){
                sellingOrder[item] += amount;
            } else {
                sellingOrder.Add(item, amount);
            }
        }

        public void Buy(string[] args){
            if (args.Length < 2){
                Console.WriteLine("Not enough arguments");
                return;
            }
            ItemType item = UI.str2tool(args[1]);
            if (item == ItemType.NONE){
                Console.WriteLine("Invalid item");
                return;
            }

            int amount = 1;
            if (args.Length > 2){
                try{
                    amount = int.Parse(args[2]);
                }
                catch (FormatException){
                    Console.WriteLine("Invalid amount");
                    return;
                }
            }

            if (buyingOrder.ContainsKey(item)){
                buyingOrder[item] += amount;
            } else {
                buyingOrder.Add(item, amount);
            }
        }



        public void Update(){
            foreach(Mine mine in mines.Values){
                mine.Update();
                if (citizens.Count > 0 && mine.currentCapacity > 0){
                    AddItem(mine.DropItem());
                }
            }
            
            foreach(Building building in buildings){
                building.Update(this);
            }

        }
        
    }
}
