using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
namespace GameOfLifeSimulator
{
    public static class Simulator
    {

        public static int Tick;
        public static int LastUpdate;
        public static Cell[][] Board;
        public static Random r = new Random();
        private static int ChangeChance;
        private static int Size;
        private static Bitmap bufferImage;
        public static void Randomize(int chance, int size)
        {
            Size = size;
            Board = new Cell[size][];
            bufferImage = new Bitmap(size, size);
            for(var x=0; x<size; x++)
            {
                Board[x] = new Cell[size];

                for (var y = 0; y< size; y++)
                {
                    Board[x][y] = new Cell { IsAlive = r.NextBool(chance), Chance = ChangeChance};
                }
            }
            BindCells();
        }

        private static void BindCells()
        {
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
                   Board[x][y].Location = new KeyValuePair<int, int>(x, y);
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
            var changed = ChangedCells.ToList();
            foreach (var cell in changed)
            {
                var location = cell.Location;
                bufferImage.SetPixel(location.Key,location.Value, cell.GetColor());
            }
            return bufferImage;
        }

        public static void RenderWholeBoard()
        {
            for (var x = 0; x < bufferImage.Width; x++)
            {

                for (var y = 0; y < bufferImage.Height; y++)
                {
                    var cell = Board[x][y];
                    bufferImage.SetPixel(x,y,cell.GetColor());
                }
            }
        }
        public static bool NextBool(this Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }

        private static Cell[] ChangedCells => Board.SelectMany(row => row.Where(cell => 
            cell.DrawUpdate
        )).ToArray();
        public static void Step()
        {
            Tick++;
             Task.WhenAll(StepRow(ChangedCells));
            //Task.WhenAll(Board.Select(StepRow));
        }

        private static async Task StepRow(Cell[] row)
        {
            LastUpdate = row.Length;    
            await Task.Run(() =>
            {
                row.ToList().ForEach(c => c.Neighbors.ToList().ForEach(n => n.Tick()));
            });
        }

   

        public static void ChangeSettings(bool showNew, 
            bool showOld, 
            int changeChance, 
            int oldAge, 
            int threshold_lower,
            int threshold_upper,
            int threshold_lower_spawn,
            int threshold_upper_spawn,
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
                    Board[x][y].Threshold_Lower = threshold_lower;
                    Board[x][y].Threshold_Upper = threshold_upper;
                    Board[x][y].Threshold_Lower_Spawn = threshold_lower_spawn;
                    Board[x][y].Threshold_Upper_Spawn = threshold_upper_spawn;
                    Board[x][y].TwoLayer = twoLayer;
                    Board[x][y].DeathAge = deathAge;
                    
                }
            }
            RenderWholeBoard();

        }

        public static void StepAll()
        {
            Board.SelectMany(row => row.Select(cell => cell)).ToList().ForEach(c => c.Tick());
            RenderWholeBoard();
        }
    }
}
