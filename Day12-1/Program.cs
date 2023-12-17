using Day12_1;

public class Program
{
    public static void Main()
    {
        int sum = 0;
        foreach (string line in Input.input.Split(Environment.NewLine))
        {
            sum += NumberOfPossibleConfigurations(line);
        }

        Console.WriteLine(sum);
    }

    private static int NumberOfPossibleConfigurations(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return 0;
        }

        string conditionRecord = line.Split(' ')[0];
        List<int> contiguousGroupsOfDamagedSprings = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();

        int indexOfNextUnknownSpring = conditionRecord.ToList().FindIndex(spring => spring == '?');
        return NumberOfPossibleConfigurations(conditionRecord, contiguousGroupsOfDamagedSprings, indexOfNextUnknownSpring, '.')
            + NumberOfPossibleConfigurations(conditionRecord, contiguousGroupsOfDamagedSprings, indexOfNextUnknownSpring, '#');
    }

    private static int NumberOfPossibleConfigurations(string condtionRecord, List<int> contiguousGroupsOfDamagedSprings, int indexForSpringToReplace, char replaceWith)
    {
        List<char> lineAsList = [.. condtionRecord];
        lineAsList[indexForSpringToReplace] = replaceWith;
        string alteredConditionRecord = new(lineAsList.ToArray());

        if (indexForSpringToReplace == condtionRecord.Length - 1)
        {
            return IsValidConfiguration(alteredConditionRecord, contiguousGroupsOfDamagedSprings) ? 1 : 0;
        }

        int indexOfNextUnknownSpring = alteredConditionRecord[(indexForSpringToReplace + 1)..].ToList().FindIndex(spring => spring == '?');
        if (indexOfNextUnknownSpring == -1)
        {
            return IsValidConfiguration(alteredConditionRecord, contiguousGroupsOfDamagedSprings) ? 1 : 0;
        }

        indexOfNextUnknownSpring += indexForSpringToReplace + 1;

        return NumberOfPossibleConfigurations(alteredConditionRecord, contiguousGroupsOfDamagedSprings, indexOfNextUnknownSpring, '.')
            + NumberOfPossibleConfigurations(alteredConditionRecord, contiguousGroupsOfDamagedSprings, indexOfNextUnknownSpring, '#');
    }

    private static bool IsValidConfiguration(string conditionRecord, List<int> contiguousGroupsOfDamagedSprings)
    {
        int indexOfNextDamagedSpring = conditionRecord.ToList().FindIndex(spring => spring == '#');
        List<int> lengths = new();

        while (indexOfNextDamagedSpring != -1)
        {
            int indexOfNextOperationalSpring = conditionRecord[indexOfNextDamagedSpring..].ToList().FindIndex(spring => spring == '.') + indexOfNextDamagedSpring;
            
            if (indexOfNextOperationalSpring < indexOfNextDamagedSpring)
            {
                lengths.Add(conditionRecord.Length - indexOfNextDamagedSpring);
                break;
            }

            lengths.Add(indexOfNextOperationalSpring - indexOfNextDamagedSpring);

            indexOfNextDamagedSpring = conditionRecord[indexOfNextOperationalSpring..].ToList().FindIndex(spring => spring == '#') + indexOfNextOperationalSpring;

            if (indexOfNextDamagedSpring < indexOfNextOperationalSpring)
            {
                break;
            }
        }

        return lengths.SequenceEqual(contiguousGroupsOfDamagedSprings);
    }
}