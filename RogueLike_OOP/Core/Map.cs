using RogueLike.Components;
using RogueLike.Components.Position;
using RogueLike.Utils;


namespace RogueLike.Core
{
    public class Map
    {
        public int Depth { get; set; }
        public int Width { get; set; }
        public GameObject[,] Field { get; set; }
        public int Seed { get; set; }

        public Map(int depth = 5, int width = 5, int seed = -1)
        {
            Seed = seed != -1 ? seed : (int)DateTime.Now.Ticks;
            var (field_, depth_, width_) = GenerateMap(depth, width, Seed);
            Depth = depth_;
            Width = width_;
            Field = field_;
        }
        public override string ToString()
        {
            string stringMap = "";
            for (int y = 0; y < Depth; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    stringMap += Field[y, x];
                }
                stringMap += "\n";
            }
            return String.Format(stringMap);
        }
        private (GameObject[,] field, int depth, int width) GenerateMap(int depth, int width, int seed)
        {
            Random rand = new Random(Seed);

            int[,] directionGrid = new int[depth, width];

            int[] directions = new int[] { 1, 2, 4, 8 };

            int N = directions[0];
            int S = directions[1];
            int E = directions[2];
            int W = directions[3];

            Dictionary<int, int> DX = new Dictionary<int, int>
            {
                { E, 1 },
                { W, -1 },
                { N, 0 },
                { S, 0 }
            };

            Dictionary<int, int> DY = new Dictionary<int, int>
            {
                { E, 0 },
                { W, 0 },
                { N, -1 },
                { S, 1 }
            };

            Dictionary<int, int> OPPOSITE = new Dictionary<int, int>
            {
                { E, W },
                { W, E },
                { N, S },
                { S, N }
            };

            // Generate directions map
            void CarvePassagesFrom(int cx, int cy)
            {
                var shuffledDirections = Utils.Utils.Shuffle(directions, rand);
                foreach (var direction in shuffledDirections)
                {
                    int nx = cx + DX[direction];
                    int ny = cy + DY[direction];

                    if (ny >= 0 && ny < depth && nx >= 0 && nx < width && directionGrid[ny, nx] == 0)
                    {
                        directionGrid[cy, cx] |= direction;
                        directionGrid[ny, nx] |= OPPOSITE[direction];
                        CarvePassagesFrom(nx, ny);
                    }
                }
            }

            (GameObject[,] field, int depth, int width) GetObjectMap()
            {
                int new_depth = depth + 1;
                int new_width = width * 2 + 1;

                GameObject[,] map = new GameObject[new_depth, new_width];

                // Insert top walls
                map[0, 0] = new EmptyCell(new Position2D(0, 0));
                for (int x = 1; x < new_width; x++)
                {
                    map[0, x] = new HorizontalWall(new Position2D(0, x));
                }
                map[0, new_width - 1] = new EmptyCell(new Position2D(0, new_width - 1));

                // Declare indexes
                int y = 1;
                int y_direction = y - 1;

                for (; y < new_depth; y++, y_direction = y - 1)
                {
                    map[y, 0] = new VerticalWall(new Position2D(y, 0));

                    // Declare indexes
                    int x = 1;
                    int x_direction = x / 2;
                    for (; x < new_width - 1; x += 2, x_direction = x / 2)
                    {
                        map[y, x] = (directionGrid[y_direction, x_direction] & S) != 0 ?
                                                    new EmptyCell(new Position2D(y, x)) :
                                                    new HorizontalWall(new Position2D(y, x));
                        if ((directionGrid[y_direction, x_direction] & E) != 0)
                        {
                            map[y, x + 1] = ((directionGrid[y_direction, x_direction] | directionGrid[y_direction, x_direction + 1]) & S) != 0 ?
                                                    new EmptyCell(new Position2D(y, x + 1)) :
                                                    new HorizontalWall(new Position2D(y, x + 1));
                        }
                        else
                        {
                            map[y, x + 1] = new VerticalWall(new Position2D(y, x + 1));
                        }
                    }
                }
                return (map, new_depth, new_width);
            }

            CarvePassagesFrom(0, 0);
            return GetObjectMap();               
        }
    }
}