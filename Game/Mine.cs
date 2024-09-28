using System;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace Game
{
    public class Mine: StorageSystem, IBuildingState{
        private Material productMaterial;
        public  int  progress {get; private set;}
        public ItemType neededTool {get; private set;}
        public List<Citizen> workers;
        public  int workersLimit {get; private set;}
        public BuildingStates state { get; private set;}
        public Mine(Material productMaterial, ItemType neededTool): base(10){
            this.productMaterial = productMaterial;
            this.progress = 0;
            this.workersLimit = 1;
            this.workers = new List<Citizen>();
            this.neededTool = neededTool;
            this.state = BuildingStates.STOPPED;
        }

        public bool AddWorker(Citizen citizen, string name){
            if (citizen is null) return false;
            if (workers.Count < workersLimit){
                workers.Add(citizen);
                this.state = BuildingStates.WORKING;
                Console.WriteLine($"Add worker to {name}");
                return true;
            }
            return false;
        }

        public Citizen RemoveWorker(string name){
            Citizen worker = null!;
            if (workers.Count > 0){
                worker = workers[0];
                workers.RemoveAt(0);
                Console.WriteLine($"Remove worker from mine {name}");
            }
            if (workers.Count == 0)
                this.state = BuildingStates.STOPPED;

            return worker;
        }

        private void Mining(){
            foreach(Citizen work in workers)
                progress += work.Update(neededTool);
            
            if (progress >= 100){
                Material toReturn = productMaterial;
                toReturn.amount = progress/100;
                progress -= 100 * (progress/100);
                AddItem(toReturn);
                if (leftCapacity <= 0){
                    state = BuildingStates.OVERFLOW;
                }
            }
        }

        public void Update(City city = null!){
            switch (state){
                case BuildingStates.WORKING:
                    Mining();
                    break;
                case BuildingStates.STOPPED:
                    break;
                case BuildingStates.OVERFLOW:
                    if (leftCapacity > 0 && workers.Count > 0)
                        state = BuildingStates.WORKING;
                    else if (workers.Count == 0)
                        state = BuildingStates.STOPPED;
                    break;
            }
        }

        public Item DropItem(){
            if (currentCapacity > 0){
                return RemoveItem(productMaterial.itemType, storage[productMaterial.itemType].Item2);
            }
            return null!;
        }

        public override string ToString(){
            return $"Mining {productMaterial.itemType} with {workers.Count}/{workersLimit} workers, progress: {progress,3}%, state: {state}";
        }
        
    }
}
