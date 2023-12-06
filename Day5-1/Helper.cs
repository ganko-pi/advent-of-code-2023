namespace Day5_1;

public static class Helper
{
    public static long Map(string input, int substringStartIndex, int substringEndIndex, long itemToMap)
    {
        List<string> linesWithMappings = input[substringStartIndex..substringEndIndex]
            .Split(Environment.NewLine)
            .Where((line, index) => index > 0)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        foreach (string lineWithMapping in linesWithMappings)
        {
            List<string> lineWithMappingSplitted = lineWithMapping
                .Split(' ')
                .Where(str => !string.IsNullOrWhiteSpace(str))
                .ToList();
            long length = long.Parse(lineWithMappingSplitted[2]);
            long mappingFromStart = long.Parse(lineWithMappingSplitted[1]);
            long mappingToStart = long.Parse(lineWithMappingSplitted[0]);
            if (itemToMap >= mappingFromStart && itemToMap < mappingFromStart + length)
            {
                return mappingToStart + (itemToMap - mappingFromStart);
            }
        }

        return itemToMap;
    }
}
