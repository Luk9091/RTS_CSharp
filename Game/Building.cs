using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Game{
    public class Building: IBuildingState{
        protected const int PROGRESS_POINT = 1;
        public string Name {get; protected set;}
        public Dictionary<ItemType, int> dependentToWork;
        public BuildingStates state { get; protected set;}
        public int progress { get; protected set; }

        // public List<Decoration> decorations;
        public List<Citizen> residents;
        public int residentsLimit { get; protected set; }
        public int decorationLimit { get; protected set; }
        public int storageLimit { get; protected set; }

        public int storageResidentSpace { get { return residentsLimit - residents.Count; } }


        public Building(int storageLimit, Dictionary<ItemType, int> dependentToBuild, int residentsLimit, int decorationLimit){
            this.Name = "Building";
            this.dependentToWork = dependentToBuild;
            this.state = BuildingStates.UNDER_CONSTRUCTION;
            // this.decorations = new List<Decoration>();
            this.residents = new List<Citizen>();
            this.residentsLimit = residentsLimit;
            this.decorationLimit = decorationLimit;
            this.storageLimit = storageLimit;
        }

        public bool AddResident(Citizen resident){
            if (state == BuildingStates.UNDER_CONSTRUCTION){
                return false;
            }

            if(storageResidentSpace > 0){
                residents.Add(resident);
                return true;
            }
            return false;
        }
        
        protected bool UnderConstruction(City city){
            Dictionary<ItemType, int> copyDependentToWork = new Dictionary<ItemType, int>(dependentToWork);
            foreach (KeyValuePair<ItemType, int> material in copyDependentToWork){
                if (city.CountItem(material.Key) >= material.Value){
                    Item item  = city.RemoveItem(material.Key, material.Value);
                    dependentToWork[material.Key] -= item.amount;
                    if (dependentToWork[material.Key] == 0){
                        dependentToWork.Remove(material.Key);
                    }
                    Console.WriteLine($"{Name} take {material.Key}: {item.amount}/{material.Value}");
                }
            }

            if (dependentToWork.Count == 0){
                if (progress < 100){
                    progress += PROGRESS_POINT;
                } else {
                    state = BuildingStates.STOPPED;
                    progress = 0;
                    Console.WriteLine($"{Name} is ready");
                    return true;
                }
            }
            
            return false;
        }

        public virtual void Update(City city){
            switch(state){
                case BuildingStates.UNDER_CONSTRUCTION:
                        UnderConstruction(city);
                    break;

                case BuildingStates.WORKING:
                break;
                case BuildingStates.STOPPED:
                    if (
                        dependentToWork.Count > 0 &&
                        residents.Count > 0
                    )
                    state = BuildingStates.WORKING;
                    break;
            }
        }

        public override string ToString(){
            string str = $"{Name}: {state} {progress, 3}%";
            
            if (dependentToWork.Count > 0){
                ItemType[] keys = dependentToWork.Keys.ToArray();
                int[] values = dependentToWork.Values.ToArray();
                for (int i= 0; i < dependentToWork.Count; i++){
                    str += $"\n\t{keys[i]}: {values[i]}";
                }
            }

            return str;
        }

        public void SetProgress(int progress){
            this.progress = progress;
        }
    }
}
