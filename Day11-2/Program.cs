using Day11_2;

public class Program
{
    public static void Main()
    {
        List<Coordinate> galaxies = FindGalaxiesInUnexpandedUniverse(Input.input);

        long sumShortestDistances = 0;
        for (int i = 0; i < galaxies.Count; ++i)
        {
            for (int j = i + 1; j < galaxies.Count; ++j)
            {
                sumShortestDistances += CalculateManhattanDistance(galaxies[i], galaxies[j]);
            }
        }

        Console.WriteLine(sumShortestDistances);
    }

    private static List<Coordinate> FindGalaxiesInUnexpandedUniverse(string universe)
    {
        string[] universeRows = universe
            .Split(Environment.NewLine)
            .Where(row => !string.IsNullOrWhiteSpace(row))
            .ToArray();

        List<Coordinate> galaxies = new();

        long y = 0;
        foreach (string row in universeRows)
        {
            long x = 0;
            foreach (char column in row)
            {
                if (column == '#')
                {
                    Coordinate coordinate = new(x, y);
                    galaxies.Add(coordinate);
                }

                ++x;
            }

            ++y;
        }

        HashSet<int> columnsWithGalaxies = galaxies
            .Select(galaxy => (int)galaxy.X)
            .ToHashSet();
        HashSet<int> rowsWithGalaxies = galaxies
            .Select(galaxy => (int)galaxy.Y)
            .ToHashSet();

        HashSet<int> columnsToExpand = Enumerable
            .Range(0, universeRows[0].Length)
            .Except(columnsWithGalaxies)
            .ToHashSet();
        HashSet<int> rowsToExpand = Enumerable
            .Range(0, universeRows.Length)
            .Except(rowsWithGalaxies)
            .ToHashSet();

        long offsetFactor = 1000000 - 1;
        for (int i = 0; i < galaxies.Count; ++i)
        {
            Coordinate currentGalaxy = galaxies[i];

            long numberOfColumnsBeforeCurrentGalaxy = columnsToExpand.Where(column => column < currentGalaxy.X).Count();
            long numberOfRowsBeforeCurrentGalaxy = rowsToExpand.Where(row => row < currentGalaxy.Y).Count();

            Coordinate currentGalaxyCorrected = new(currentGalaxy.X + numberOfColumnsBeforeCurrentGalaxy * offsetFactor, currentGalaxy.Y + numberOfRowsBeforeCurrentGalaxy * offsetFactor);
            galaxies[i] = currentGalaxyCorrected;
        }

        return galaxies;
    }

    private static long CalculateManhattanDistance(Coordinate galaxy1, Coordinate galaxy2)
    {
        return Math.Abs(galaxy2.X - galaxy1.X) + Math.Abs(galaxy2.Y - galaxy1.Y);
    }
}
