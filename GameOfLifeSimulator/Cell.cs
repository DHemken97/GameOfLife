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
            int lifeQuotient = Neighbors.Count(cell => cell.IsAlive);
            Generations = Simulator.Tick - LastChange;
            return (Generations == 1) || (IsAlive && Generations == DeathAge+1 && DeathAge > 0) || (!IsAlive && lifeQuotient >= Threshold_Lower_Spawn && lifeQuotient <= Threshold_Upper_Spawn ) || (IsAlive && lifeQuotient > Threshold_Lower && lifeQuotient < Threshold_Upper) || (ShowNew && Generations == 3) || (ShowOld && Generations == OldGeneration+1 && OldGeneration > 0);
        }

        public KeyValuePair<int, int> Location;
        public Cell[] Neighbors;
        public bool IsAlive;
        public bool IsNew => Generations <= 2;
        public bool ShowNew { get; set; } 
        public bool ShowOld { get; set; }
        public int Threshold_Upper { get; set; }
        public int Threshold_Lower { get; set; }
        public int Threshold_Lower_Spawn { get; set; }
        public int Threshold_Upper_Spawn { get; set; }
        public int DeathAge { get; set; }
        public int LastUpdate;
        public bool TwoLayer;
        public int OldGeneration;
        public int Chance;
        private Random r = new Random();
        private int LastChange;
        public int Generations;
        public void Tick()
        {
            if (LastUpdate == Simulator.Tick)
                return;


            LastUpdate = Simulator.Tick;

                var living = Neighbors.Count(cell => cell.IsAlive);

                if (TwoLayer)
                    living = Neighbors.SelectMany(n => n.Neighbors).Count(n => n.IsAlive);


                if (living == Threshold_Upper && !IsAlive)
                {
                    IsAlive = r.NextBool(Chance);
                    LastChange = LastUpdate;
                }
                else if (IsAlive && (living < Threshold_Lower  || living > Threshold_Upper))
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
            
            return ShowNew&&IsNew ? IsAlive ? Color.Green : Color.Red : IsAlive ? Color.White : ShowOld && Generations > OldGeneration ? Color.Blue : Color.Black;

        }
    }
}
