using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;

namespace Game
{
    public interface IState{
        public int progress {get;}
        public void Update(City city);
        public abstract string ToString();
    }

    public interface IBuildingState: IState{
        public BuildingStates state {get;}
    }

    public interface ICitizenState: IState{
        public CitizenStates state {get;}
    }

    public enum BuildingStates{
        UNDER_CONSTRUCTION,
        WORKING,
        STOPPED,
        OVERFLOW,
        DESTROYED,
    }

    public enum CitizenStates{
        IDLE,
        WORKING,
        SLEEPING,
        HUNGRY,

        GOTO_FOOD,
        GOTO_BED,
        GOTO_WORK,
    }

    /*
    public abstract class CitizenState{
        protected CitizenStates currentState = CitizenStates.IDLE;
        protected int progress;
        public abstract CitizenState Update();
        public override string ToString(){
            return 
        }
    }
    */
}
