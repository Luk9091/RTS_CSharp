using System;
using System.Numerics;

namespace Game
{
    public interface IStackable{
        public int inStack { get; }
        public int stackLimit { get; }
        public int weightPerOne { get; }
        public int valuePerOne { get; }
        public int AddToStack(int amount);
        public int RemoveFromStack(int amount);
    }
}
