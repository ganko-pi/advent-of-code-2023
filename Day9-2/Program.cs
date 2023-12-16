using Day9_2;

public class Program
{
    public static void Main()
    {
        long sum = 0;
        foreach (string line in Input.input.Split(Environment.NewLine))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            List<long> sequence = LineToSequence(line);
            sum += GetPreviousValue(sequence);
        }

        Console.WriteLine(sum);
    }

    private static List<long> LineToSequence(string line)
    {
        return line.Split(' ').Select(numberStr => long.Parse(numberStr)).ToList();
    }

    private static long GetPreviousValue(List<long> sequence)
    {
        List<long> differences = CalculateDifferences(sequence);
        if (differences.All(difference => difference == 0))
        {
            return sequence.First();
        }

        return sequence.First() - GetPreviousValue(differences);
    }

    private static List<long> CalculateDifferences(List<long> sequence)
    {
        List<long> differences = new();
        long previousValue = sequence.First();
        for (int i = 1; i < sequence.Count; ++i)
        {
            differences.Add(sequence[i] - previousValue);
            previousValue = sequence[i];
        }

        return differences;
    }
}