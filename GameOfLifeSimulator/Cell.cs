using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLifeSimulator
{
    public class Cell
    {
        public bool DrawUpdate => HasUpdate();

        private bool HasUpdate()
        {
            if (ShowOld && Generations > OldGeneration && OldGeneration > 0) return true;
            if (IsAlive && Generations > DeathAge && DeathAge > 0) return true;
            if (ShowNew && Generations <= 3) return true;
            return false;
        }

        private Color LastColor;
        public KeyValuePair<int, int> Location;
        public Cell[] Neighbors;
        public bool IsAlive;
        public bool IsNew => Generations <= 2;
        public bool ShowNew { get; set; }
        public bool ShowOld { get; set; }
        public int Threshold { get; set; }
        public int DeathAge { get; set; }
        public int LastUpdate;
        public bool TwoLayer;
        public int OldGeneration;
        public int Chance;
        private Random r = new Random();
        private int LastChange;
        public int Generations => Simulator.Tick - LastChange;
        public void Tick()
        {
            if (LastUpdate == Simulator.Tick)
                return;


            LastUpdate = Simulator.Tick;

                var living = Neighbors.Count(cell => cell.IsAlive);

                if (TwoLayer)
                    living = Neighbors.SelectMany(n => n.Neighbors).Count(n => n.IsAlive);


                if (living == Threshold && !IsAlive)
                {
                    IsAlive = r.NextBool(Chance);
                    LastChange = LastUpdate;
                }
                else if (IsAlive && (living < Threshold - 1 || living > Threshold + 1))
                {
                    IsAlive = false;
                    LastChange = LastUpdate;
                }


            if (DeathAge > 0 && Generations > DeathAge && IsAlive)
            {
                LastChange = LastUpdate;
                IsAlive = false;
            }

        }

        public Color GetColor()
        {
            if (IsAlive)
                return ShowNew&&IsNew ? Color.Green : Color.White;
            if (ShowNew && IsNew)
                return Color.Red;
            
                return ShowOld&&Generations>OldGeneration ? Color.Blue : Color.Black;


        }
    }
}
