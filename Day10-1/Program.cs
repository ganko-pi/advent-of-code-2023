using Day10_1;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<Coordinate, Tile> tiles = ParseInput(Input.input);
        Dictionary<Coordinate, int> distancesToStart = GetDistancesToStart(tiles);

        int maxDistance = distancesToStart.Values.Max();

        Console.WriteLine(maxDistance);
    }

    private static Dictionary<Coordinate, Tile> ParseInput(string input)
    {
        Dictionary<Coordinate, Tile> tiles = new();

        int currentY = 0;
        foreach (string line in input.Split(Environment.NewLine))
        {
            int currentX = 0;
            foreach (char pipe in line)
            {
                Coordinate coordinate = new(currentX, currentY);
                Tile tile = pipe switch
                {
                    '|' => new(coordinate, Direction.NORTH, Direction.SOUTH),
                    '-' => new(coordinate, Direction.EAST, Direction.WEST),
                    'L' => new(coordinate, Direction.NORTH, Direction.EAST),
                    'J' => new(coordinate, Direction.NORTH, Direction.WEST),
                    '7' => new(coordinate, Direction.SOUTH, Direction.WEST),
                    'F' => new(coordinate, Direction.SOUTH, Direction.EAST),
                    'S' => new(coordinate, Direction.START, Direction.START),
                    _ => new(coordinate, Direction.NONE, Direction.NONE),
                };

                tiles.Add(coordinate, tile);

                ++currentX;
            }

            ++currentY;
        }

        return tiles;
    }

    private static Dictionary<Coordinate, int> GetDistancesToStart(
        Dictionary<Coordinate, Tile> tiles
    )
    {
        Dictionary<Coordinate, int> distancesToStart = new();
        Tile start = tiles.Values.First(
            tile => tile.Direction1 == Direction.START && tile.Direction2 == Direction.START
        );

        int distanceToStart = 0;
        distancesToStart.Add(start.Coordinate, distanceToStart);
        ++distanceToStart;

        HashSet<Tile> neighborTiles = new();
        List<Coordinate> neighborCoordinates = [
            GetNeighborCoordinate(start, Direction.NORTH),
            GetNeighborCoordinate(start, Direction.SOUTH),
            GetNeighborCoordinate(start, Direction.EAST),
            GetNeighborCoordinate(start, Direction.WEST),
        ];
        foreach (Coordinate neighborCoordinate in neighborCoordinates)
        {
            if (!tiles.TryGetValue(neighborCoordinate, out Tile? neighbor))
            {
                continue;
            }

            if (!IsConnected(start, neighbor))
            {
                continue;
            }

            neighborTiles.Add(neighbor);
        }

        while (SetNeighborDistancesToStart(neighborTiles, distancesToStart, distanceToStart))
        {
            ++distanceToStart;
            HashSet<Tile> newNeighborTiles = new();
            foreach (Tile tile in neighborTiles)
            {
                newNeighborTiles.UnionWith(GetNeighborTiles(tile, tiles));
            }

            List<Tile> tilesToRemove = new();
            foreach (Tile tile in newNeighborTiles)
            {
                if (distancesToStart.ContainsKey(tile.Coordinate))
                {
                    tilesToRemove.Add(tile);
                }
            }

            neighborTiles = newNeighborTiles.Except(tilesToRemove).ToHashSet();
        }

        return distancesToStart;
    }

    private static bool SetNeighborDistancesToStart(
        HashSet<Tile> neighborTiles,
        Dictionary<Coordinate, int> distancesToStart,
        int distanceToStartForNeighbors
    )
    {
        int neighborsWithoutDistance = 0;

        foreach (Tile tile in neighborTiles)
        {
            if (distancesToStart.TryAdd(tile.Coordinate, distanceToStartForNeighbors))
            {
                ++neighborsWithoutDistance;
            }
        }

        return neighborsWithoutDistance > 0;
    }

    private static HashSet<Tile> GetNeighborTiles(Tile tile, Dictionary<Coordinate, Tile> tiles)
    {
        HashSet<Coordinate> coordinates =
        [
            GetNeighborCoordinate(tile, tile.Direction1),
            GetNeighborCoordinate(tile, tile.Direction2),
        ];

        HashSet<Tile> neighbors = new();
        foreach (Coordinate coordinate in coordinates)
        {
            neighbors.Add(tiles[coordinate]);
        }

        return neighbors;
    }

    private static Coordinate GetNeighborCoordinate(Tile tile, Direction direction)
    {
        return direction switch
        {
            Direction.NORTH => new(tile.Coordinate.X, tile.Coordinate.Y - 1),
            Direction.SOUTH => new(tile.Coordinate.X, tile.Coordinate.Y + 1),
            Direction.EAST => new(tile.Coordinate.X + 1, tile.Coordinate.Y),
            Direction.WEST => new(tile.Coordinate.X - 1, tile.Coordinate.Y),
            _ => tile.Coordinate,
        };
    }

    private static bool IsConnected(Tile tile, Tile neighbor)
    {
        int offsetX = neighbor.Coordinate.X - tile.Coordinate.X;
        int offsetY = neighbor.Coordinate.Y - tile.Coordinate.Y;

        if (offsetX == 1)
        {
            return (
                    tile.Direction1 == Direction.EAST
                    || tile.Direction2 == Direction.EAST
                    || tile.Direction1 == Direction.START
                    || tile.Direction2 == Direction.START
                )
                && (
                    neighbor.Direction1 == Direction.WEST
                    || neighbor.Direction2 == Direction.WEST
                    || neighbor.Direction1 == Direction.START
                    || neighbor.Direction2 == Direction.START
                );
        }

        if (offsetX == -1)
        {
            return (
                    tile.Direction1 == Direction.WEST
                    || tile.Direction2 == Direction.WEST
                    || tile.Direction1 == Direction.START
                    || tile.Direction2 == Direction.START
                )
                && (
                    neighbor.Direction1 == Direction.EAST
                    || neighbor.Direction2 == Direction.EAST
                    || neighbor.Direction1 == Direction.START
                    || neighbor.Direction2 == Direction.START
                );
        }

        if (offsetY == 1)
        {
            return (
                    tile.Direction1 == Direction.SOUTH
                    || tile.Direction2 == Direction.SOUTH
                    || tile.Direction1 == Direction.START
                    || tile.Direction2 == Direction.START
                )
                && (
                    neighbor.Direction1 == Direction.NORTH
                    || neighbor.Direction2 == Direction.NORTH
                    || neighbor.Direction1 == Direction.START
                    || neighbor.Direction2 == Direction.START
                );
        }

        if (offsetY == -1)
        {
            return (
                    tile.Direction1 == Direction.NORTH
                    || tile.Direction2 == Direction.NORTH
                    || tile.Direction1 == Direction.START
                    || tile.Direction2 == Direction.START
                )
                && (
                    neighbor.Direction1 == Direction.SOUTH
                    || neighbor.Direction2 == Direction.SOUTH
                    || neighbor.Direction1 == Direction.START
                    || neighbor.Direction2 == Direction.START
                );
        }

        return false;
    }
}
