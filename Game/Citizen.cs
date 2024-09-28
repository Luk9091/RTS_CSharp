using System;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;

namespace Game
{
    public enum HAPPINESS_t{
        BAD,
        NEUTRAL,
        GOOD,
        LOVED,
    }
    public class Citizen: StorageSystem{ //: ISiegeable{
        protected Tool useableTool;
        public string Name {get; private set;}
        public int health { get; private set; }
        public HAPPINESS_t happiness { get; private set; }
        public int age { get; private set; }
        private int ageLimit { get; }
        public int money { get; private set; }
        public bool ableToWork { get; private set; }
        public Building home { get; private set; }
        public string workIn = "";

        public Citizen(string name, int health, HAPPINESS_t happiness, int ageLimit): base(5){
            this.Name = name;
            this.health = health;
            this.happiness = happiness;
            this.age = 0;
            this.ageLimit = ageLimit;
            this.money = 0;
            this.ableToWork = false;
            this.home = null!;
            this.useableTool = null!;
        }

        public bool AddAge(){
            if (age >= ageLimit){
                return false;
            } 
            this.age++;

            if (age == 14) ableToWork = true;
            return true;
        }

        public bool SetAge(int age){
            if (age >= ageLimit){
                return false;
            } 
            this.age = age;

            if (age >= 14) ableToWork = true;
            return true;
        }

        public int Update(ItemType toolToWork){
            if (!this.ableToWork) return 0;
            int workPoint = 50;
            int itemPoint = 0;
            if (useableTool is not null){
                itemPoint = useableTool.Use();
                if (useableTool.durability == 0){
                    useableTool = RemoveTool(toolToWork);
                    useableTool = null!;
                    Console.WriteLine($"I destroyed my {toolToWork}");
            }
            }

            return workPoint + itemPoint;
        }

        public override string ToString(){
            string str = $"{this.Name, 10}: health: {this.health, 3}, age: {age, 2}, money: {money, 3}, abel to work: {this.ableToWork, 5}";
            if (this.ableToWork){
                str = str + $" in {this.workIn}";
            }
            return str;
        }
    }
}
