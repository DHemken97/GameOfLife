using System.Linq;

namespace GameOfLifeSimulator
{
    public class Cell
    {
        public Cell[] neighbors;
        public bool IsAlive;
        public int Generation = 0;
        public void Tick()
        {
            var living = neighbors.Count(cell => cell.IsAlive);
            if (living == 3)
                IsAlive = true;
            else if (living < 2 || living > 3)
                IsAlive = false;


            Generation++;
        }
    }
}
