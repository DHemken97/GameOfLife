using System;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
namespace GameOfLifeSimulator
{
    public static class Simulator
    {

        public static Cell[][] Board;
        public static Random r = new Random();
        private static int ChangeChance;
        public static void Randomize(int chance, int size)
        {
            Board = new Cell[size][];

            for(var x=0; x<size; x++)
            {
                Board[x] = new Cell[size];

                for (var y = 0; y< size; y++)
                {
                    Board[x][y] = new Cell { IsAlive = r.NextBool(chance),Generations = 2, Chance = ChangeChance};
                }
            }
            BindCells();
        }

        private static void BindCells()
        {
            cells = Board.SelectMany(r => r).ToArray();
            var size = Board.GetLength(0);
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    var n = new[]
                    {
                        TryGet(x - 1, y - 1),
                        TryGet(x, y - 1),
                        TryGet(x + 1, y - 1),
                        TryGet(x - 1, y),
                        TryGet(x + 1, y),
                        TryGet(x - 1, y + 1),
                        TryGet(x, y + 1),
                        TryGet(x + 1, y + 1)

                    };
                   Board[x][y].Neighbors = n.Where(c => c != null).ToArray();
                }
            }
        }

        private static Cell TryGet(int x, int y)
        {
            var size = Board.GetLength(0);
            if (x < 0 || x >= size) return null;
            if (y < 0 || y >= size) return null;
            return Board[x][y];
        }
        public static Bitmap RenderBoard()
        {
            var image = new Bitmap(Board.GetLength(0), Board.GetLength(0));
            for (var x = 0; x < image.Width; x++)
            {

                for (var y = 0; y < image.Height; y++)
                {
                    var cell = Board[x][y];
                  image.SetPixel(x,y, cell.GetColor());
                }
            }

            return image;
        }

        public static bool NextBool(this Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }

        private static Cell[] cells;
        public static void Step()
        {
             Task.WhenAll(StepRow(cells));
            //Task.WhenAll(Board.Select(StepRow));

        }

        private static async Task StepRow(Cell[] row)
        {
             
            await Task.Run(() =>
            {
                row.ToList().ForEach(c => c.Tick());
            });
        }

   

        public static void ChangeSettings(bool showNew, 
            bool showOld, 
            int changeChance, 
            int oldAge, 
            int threshold,
            bool twoLayer,
            int deathAge)
        {
            ChangeChance = changeChance;
            var size = Board.GetLength(0);
            for (var x = 0; x < size; x++)
            {

                for (var y = 0; y < size; y++)
                {
                    Board[x][y].Chance = changeChance;
                    Board[x][y].ShowNew = showNew;
                    Board[x][y].ShowOld = showOld;
                    Board[x][y].OldGeneration = oldAge;
                    Board[x][y].Threshold = threshold;
                    Board[x][y].TwoLayer = twoLayer;
                    Board[x][y].DeathAge = deathAge;
                    
                }
            }

        }
    }
}
