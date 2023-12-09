string input =
    @"Time:        50     74     86     85
Distance:   242   1017   1691   1252
";

IEnumerable<long> times = input
    .Split(Environment.NewLine)
    .First(line => line.StartsWith("Time"))
    .Split(':')
    .Select(token => string.Concat(token.Where(character => !char.IsWhiteSpace(character))))
    .Where(token => !string.IsNullOrWhiteSpace(token))
    .Where(token => long.TryParse(token, out long _))
    .Select(long.Parse);

IEnumerable<long> distances = input
    .Split(Environment.NewLine)
    .First(line => line.StartsWith("Distance"))
    .Split(':')
    .Select(token => string.Concat(token.Where(character => !char.IsWhiteSpace(character))))
    .Where(token => !string.IsNullOrWhiteSpace(token))
    .Where(token => long.TryParse(token, out long _))
    .Select(long.Parse);

if (times.Count() != distances.Count())
{
    throw new Exception($"Invalid input. There are different counts for times ({times.Count()}) and distances ({distances.Count()}).");
}

Dictionary<long, long> recordDistancesPerTime = new();
for (int i = 0; i < times.Count(); ++i)
{
    recordDistancesPerTime.Add(times.ElementAt(i), distances.ElementAt(i));
}

long product = 1;
foreach (KeyValuePair<long, long> recordDistancePerTime in recordDistancesPerTime)
{
    long waysToBeatCurrentRecordDistance = 0;
    for (long i = 0; i <= recordDistancePerTime.Key; ++i)
    {
        long distance = i * (recordDistancePerTime.Key - i);
        if (distance > recordDistancePerTime.Value)
        {
            ++waysToBeatCurrentRecordDistance;
        }
    }

    product *= waysToBeatCurrentRecordDistance;
}

Console.WriteLine(product);
