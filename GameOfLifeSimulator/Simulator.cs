using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace GameOfLifeSimulator
{
    public static class Simulator
    {

        public static Cell[][] Board;
        public static Random r = new Random();
        public static void Randomize(int chance)
        {
            var size = 99;
            Board = new Cell[size][];

            for(var x=0; x<size; x++)
            {
                board[x] = new cell[size];

                for (var y = 0; y< size; y++)
                {
                    Board[x][y] = new Cell { IsAlive = r.NextBool(chance) };
                }
            }
        }
        
        public BitMap RenderBoard()
        {

        }

        public static bool NextBool(this Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }

    }
}
