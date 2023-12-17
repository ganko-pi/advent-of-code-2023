using Day12_2;

public class Program
{
    public static void Main()
    {
        long sum = 0;
        foreach (string line in Input.input.Split(Environment.NewLine))
        {
            string unfoldedLine = UnfoldLine(line);

            sum += NumberOfPossibleConfigurations(unfoldedLine);
        }

        Console.WriteLine(sum);
    }

    private static string UnfoldLine(string line)
    {
        
        if (string.IsNullOrWhiteSpace(line))
        {
            return string.Empty;
        }

        string conditionRecord = line.Split(' ')[0];
        string contiguousGroupsOfDamagedSprings = line.Split(' ')[1];

        string unfoldedConditionRecord = string.Join('?', conditionRecord, conditionRecord, conditionRecord, conditionRecord, conditionRecord);
        string unfoldedContiguousGroupsOfDamagedSprings = string.Join(',', contiguousGroupsOfDamagedSprings, contiguousGroupsOfDamagedSprings, contiguousGroupsOfDamagedSprings, contiguousGroupsOfDamagedSprings, contiguousGroupsOfDamagedSprings);

        return string.Join(' ', unfoldedConditionRecord, unfoldedContiguousGroupsOfDamagedSprings);
    }

    private static long NumberOfPossibleConfigurations(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return 0L;
        }

        string conditionRecord = line.Split(' ')[0];
        List<int> contiguousGroupsOfDamagedSprings = line.Split(' ')[1].Split(',').Select(int.Parse).ToList();

        int indexOfPreviousUnknownSpring = conditionRecord.LastIndexOf('?');
        return NumberOfPossibleConfigurations(conditionRecord, contiguousGroupsOfDamagedSprings, indexOfPreviousUnknownSpring, '.')
            + NumberOfPossibleConfigurations(conditionRecord, contiguousGroupsOfDamagedSprings, indexOfPreviousUnknownSpring, '#');
    }

    private static long NumberOfPossibleConfigurations(string conditionRecord, List<int> contiguousGroupsOfDamagedSprings, int indexForSpringToReplace, char replaceWith)
    {
        List<char> lineAsList = [.. conditionRecord];
        lineAsList[indexForSpringToReplace] = replaceWith;
        string alteredConditionRecord = new(lineAsList.ToArray());

        if (!CouldBeValidConfiguration(alteredConditionRecord, contiguousGroupsOfDamagedSprings))
        {
            return 0L;
        }

        if (indexForSpringToReplace == 0)
        {
            return IsValidConfiguration(alteredConditionRecord, contiguousGroupsOfDamagedSprings) ? 1L : 0L;
        }

        int indexOfPreviousUnknownSpring = alteredConditionRecord[..indexForSpringToReplace].LastIndexOf('?');
        if (indexOfPreviousUnknownSpring == -1)
        {
            return IsValidConfiguration(alteredConditionRecord, contiguousGroupsOfDamagedSprings) ? 1L : 0L;
        }

        return NumberOfPossibleConfigurations(alteredConditionRecord, contiguousGroupsOfDamagedSprings, indexOfPreviousUnknownSpring, '.')
            + NumberOfPossibleConfigurations(alteredConditionRecord, contiguousGroupsOfDamagedSprings, indexOfPreviousUnknownSpring, '#');
    }

    private static bool IsValidConfiguration(string conditionRecord, List<int> contiguousGroupsOfDamagedSprings)
    {
        if (conditionRecord.Contains('?'))
        {
            return false;
        }

        if (conditionRecord.Count(c => c == '#') != contiguousGroupsOfDamagedSprings.Sum())
        {
            return false;
        }

        return CouldBeValidConfiguration(conditionRecord, contiguousGroupsOfDamagedSprings);
    }

    private static bool CouldBeValidConfiguration(string conditionRecord, List<int> contiguousGroupsOfDamagedSprings)
    {
        if (conditionRecord.Count(c => c == '#') + conditionRecord.Count(c => c == '?') < contiguousGroupsOfDamagedSprings.Sum())
        {
            return false;
        }

        int lastQuestionMarkIndex = conditionRecord.LastIndexOf('?');
        if (lastQuestionMarkIndex == -1)
        {
            lastQuestionMarkIndex = 0;
        }

        int indexOfPreviousDamagedSpring = conditionRecord[lastQuestionMarkIndex..].LastIndexOf('#');
        LinkedList<int> contiguousGroupsOfDamagedSpringsInConditionRecord = new();

        while (indexOfPreviousDamagedSpring != -1)
        {
            int indexOfPreviousOperationalSpring = conditionRecord[lastQuestionMarkIndex..(indexOfPreviousDamagedSpring + lastQuestionMarkIndex)].LastIndexOf('.');
            
            if (indexOfPreviousOperationalSpring == -1)
            {
                break;
            }

            contiguousGroupsOfDamagedSpringsInConditionRecord.AddFirst(indexOfPreviousDamagedSpring - indexOfPreviousOperationalSpring);

            indexOfPreviousDamagedSpring = conditionRecord[lastQuestionMarkIndex..(indexOfPreviousOperationalSpring + lastQuestionMarkIndex)].LastIndexOf('#');

            if (indexOfPreviousDamagedSpring == -1)
            {
                break;
            }
        }

        for (int i = contiguousGroupsOfDamagedSprings.Count - contiguousGroupsOfDamagedSpringsInConditionRecord.Count - 1; i >= 0; --i)
        {
            contiguousGroupsOfDamagedSpringsInConditionRecord.AddFirst(contiguousGroupsOfDamagedSprings[i]);
        }

        return contiguousGroupsOfDamagedSpringsInConditionRecord.SequenceEqual(contiguousGroupsOfDamagedSprings);
    }
}
