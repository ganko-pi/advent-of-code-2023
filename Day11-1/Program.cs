using System.Text;
using Day11_1;

public class Program
{
    public static void Main()
    {
        string universe = ExpandUniverse(Input.input);
        List<Coordinate> galaxies = FindGalaxies(universe);

        int sumShortestDistances = 0;
        for (int i = 0; i < galaxies.Count; ++i)
        {
            for (int j = i + 1; j < galaxies.Count; ++j)
            {
                sumShortestDistances += CalculateManhattanDistance(galaxies[i], galaxies[j]);
            }
        }

        Console.WriteLine(sumShortestDistances);
    }

    private static string ExpandUniverse(string input)
    {
        string[] inputRows = input
            .Split(Environment.NewLine)
            .Where(row => !string.IsNullOrWhiteSpace(row))
            .ToArray();

        List<Coordinate> foundGalaxiesInUnexpandedUniverse = FindGalaxies(input);
        HashSet<int> columnsWithGalaxies = foundGalaxiesInUnexpandedUniverse
            .Select(galaxy => galaxy.X)
            .ToHashSet();
        HashSet<int> rowsWithGalaxies = foundGalaxiesInUnexpandedUniverse
            .Select(galaxy => galaxy.Y)
            .ToHashSet();

        HashSet<int> columnsToExpand = Enumerable
            .Range(0, inputRows[0].Length)
            .Except(columnsWithGalaxies)
            .ToHashSet();
        HashSet<int> rowsToExpand = Enumerable
            .Range(0, inputRows.Length)
            .Except(rowsWithGalaxies)
            .ToHashSet();

        StringBuilder expandedUniverse = new();
        for (int y = 0; y < inputRows.Length; ++y)
        {
            for (int x = 0; x < inputRows[y].Length; ++x)
            {
                expandedUniverse.Append(inputRows[y][x]);

                if (columnsToExpand.Contains(x))
                {
                    expandedUniverse.Append(inputRows[y][x]);
                }
            }

            expandedUniverse.Append(Environment.NewLine);

            if (rowsToExpand.Contains(y))
            {
                for (int x = 0; x < inputRows[0].Length; ++x)
                {
                    expandedUniverse.Append(inputRows[y][x]);

                    if (columnsToExpand.Contains(x))
                    {
                        expandedUniverse.Append(inputRows[y][x]);
                    }
                }

                expandedUniverse.Append(Environment.NewLine);
            }
        }

        return expandedUniverse.ToString();
    }

    private static List<Coordinate> FindGalaxies(string universe)
    {
        List<Coordinate> galaxies = new();

        int y = 0;
        foreach (string row in universe.Split(Environment.NewLine))
        {
            int x = 0;
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

        return galaxies;
    }

    private static int CalculateManhattanDistance(Coordinate galaxy1, Coordinate galaxy2)
    {
        return Math.Abs(galaxy2.X - galaxy1.X) + Math.Abs(galaxy2.Y - galaxy1.Y);
    }
}
