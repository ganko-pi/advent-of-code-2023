namespace Day5_2;

public static class Helper
{
    public static List<Range> AddSorted(List<Range> ranges, Range rangeToAdd)
    {
        List<Range> rangesToReturn = new(ranges);

        int indexToInsert = rangesToReturn.BinarySearch(rangeToAdd);
        if (indexToInsert < 0)
        {
            indexToInsert = ~indexToInsert;
        }

        if (
            (rangesToReturn.Count > 0)
            && (indexToInsert > 0)
            && (rangesToReturn[indexToInsert - 1].EndExcluded > rangeToAdd.EndExcluded)
        )
        {
            // rangeToAdd is already completly contained in list
            return rangesToReturn;
        }

        if (
            (rangesToReturn.Count > 0)
            && (indexToInsert > 0)
            && (rangesToReturn[indexToInsert - 1].EndExcluded > rangeToAdd.StartIncluded)
        )
        {
            rangeToAdd.StartIncluded = rangesToReturn[indexToInsert - 1].StartIncluded;
            rangesToReturn.RemoveAt(indexToInsert - 1);
            --indexToInsert;
        }

        if (
            (indexToInsert < rangesToReturn.Count)
            && (rangesToReturn[indexToInsert].StartIncluded < rangeToAdd.EndExcluded)
        )
        {
            rangeToAdd.EndExcluded = rangesToReturn[indexToInsert].EndExcluded;
            rangesToReturn.RemoveAt(indexToInsert);
        }

        rangesToReturn.Insert(indexToInsert, rangeToAdd);

        return rangesToReturn;
    }

    public static List<Range> Map(
        string input,
        int substringStartIndex,
        int substringEndIndex,
        List<Range> rangesToMap
    )
    {
        List<string> linesWithMappings = input[substringStartIndex..substringEndIndex]
            .Split(Environment.NewLine)
            .Where((line, index) => index > 0)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        Dictionary<Range, Range> mappings = new();

        foreach (string lineWithMapping in linesWithMappings)
        {
            List<string> lineWithMappingSplitted = lineWithMapping
                .Split(' ')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .ToList();
            long length = long.Parse(lineWithMappingSplitted[2]);
            long mappingFromStart = long.Parse(lineWithMappingSplitted[1]);
            long mappingToStart = long.Parse(lineWithMappingSplitted[0]);

            Range mappingFrom = new(mappingFromStart, mappingFromStart + length);
            Range mappingTo = new(mappingToStart, mappingToStart + length);
            mappings.Add(mappingFrom, mappingTo);
        }

        List<Range> mappedRanges = new();
        foreach (Range rangeToMap in rangesToMap)
        {
            Range rangeToOperate = new(rangeToMap.StartIncluded, rangeToMap.EndExcluded);
            while (rangeToOperate.StartIncluded < rangeToOperate.EndExcluded)
            {
                KeyValuePair<Range, Range> nextSmallerMapping = mappings
                    .OrderByDescending(mapping => mapping.Key)
                    .FirstOrDefault(range => range.Key.CompareTo(rangeToOperate) <= 0);

                if (
                    nextSmallerMapping.Equals(default(KeyValuePair<Range, Range>))
                    || nextSmallerMapping.Key.EndExcluded <= rangeToOperate.StartIncluded
                )
                {
                    KeyValuePair<Range, Range> nextLargerMapping = mappings
                        .OrderBy(mapping => mapping.Key)
                        .FirstOrDefault(range => range.Key.CompareTo(rangeToOperate) > 0);

                    if (nextLargerMapping.Equals(default(KeyValuePair<Range, Range>)))
                    {
                        mappedRanges = AddSorted(mappedRanges, rangeToOperate);

                        rangeToOperate = new(rangeToOperate.EndExcluded, rangeToOperate.EndExcluded);

                        continue;
                    }

                    long rangeToAddEnd = Math.Min(
                        rangeToOperate.EndExcluded,
                        nextLargerMapping.Key.StartIncluded
                    );
                    Range rangeToAdd = new(rangeToOperate.StartIncluded, rangeToAddEnd);
                    mappedRanges = AddSorted(mappedRanges, rangeToAdd);

                    rangeToOperate = new(rangeToAddEnd, rangeToOperate.EndExcluded);

                    continue;
                }

                long offset = rangeToOperate.StartIncluded - nextSmallerMapping.Key.StartIncluded;
                long length;
                if (rangeToOperate.EndExcluded < nextSmallerMapping.Key.EndExcluded)
                {
                    length = rangeToOperate.EndExcluded - rangeToOperate.StartIncluded;
                }
                else
                {
                    length = nextSmallerMapping.Key.EndExcluded - rangeToOperate.StartIncluded;
                }

                Range rangeMappedToAdd =
                    new(
                        nextSmallerMapping.Value.StartIncluded + offset,
                        nextSmallerMapping.Value.StartIncluded + offset + length
                    );
                mappedRanges = AddSorted(mappedRanges, rangeMappedToAdd);

                rangeToOperate = new(
                    nextSmallerMapping.Key.StartIncluded + offset + length,
                    rangeToOperate.EndExcluded
                );
            }
        }

        return mappedRanges;
    }
}
