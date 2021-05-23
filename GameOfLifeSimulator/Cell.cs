using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace GameOfLifeSimulator
{
    public class Cell
    {
        public Cell[] Neighbors;
        public bool IsAlive;
        public bool IsNew => Generations <= 2;
        public bool ShowNew { get; set; }
        public bool ShowOld { get; set; }
        public int Threshold { get; set; }
        public int DeathAge { get; set; }

        public bool TwoLayer;
        public int OldGeneration;
        public int Chance;
        private Random r = new Random();

        public int Generations = 0;
        public void Tick()
        {
            var nearbyChanges = Neighbors.Any(n => n.Neighbors.Any(nn => nn.IsNew));

            if (nearbyChanges)
            {


                var living = Neighbors.Count(cell => cell.IsAlive);

                if (TwoLayer)
                    living = Neighbors.SelectMany(n => n.Neighbors).Count(n => n.IsAlive);


                if (living == Threshold && !IsAlive)
                {
                    IsAlive = r.NextBool(Chance);
                    Generations = 0;
                }
                else if (IsAlive && (living < Threshold - 1 || living > Threshold + 1))
                {
                    IsAlive = false;
                    Generations = 0;
                }
            }

            Generations++;
            if (DeathAge > 0 && Generations > DeathAge && IsAlive)
            {
                Generations = 0;
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
