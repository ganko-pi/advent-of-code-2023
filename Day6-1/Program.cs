string input =
    @"Time:        50     74     86     85
Distance:   242   1017   1691   1252
";

IEnumerable<int> times = input
    .Split(Environment.NewLine)
    .First(line => line.StartsWith("Time"))
    .Split(' ')
    .Where(token => !string.IsNullOrWhiteSpace(token))
    .Where(token => int.TryParse(token, out int _))
    .Select(int.Parse);

IEnumerable<int> distances = input
    .Split(Environment.NewLine)
    .First(line => line.StartsWith("Distance"))
    .Split(' ')
    .Where(token => !string.IsNullOrWhiteSpace(token))
    .Where(token => int.TryParse(token, out int _))
    .Select(int.Parse);

if (times.Count() != distances.Count())
{
    throw new Exception("Invalid input. There are different counts for times and distances.");
}

Dictionary<int, int> recordDistancesPerTime = new();
for (int i = 0; i < times.Count(); ++i)
{
    recordDistancesPerTime.Add(times.ElementAt(i), distances.ElementAt(i));
}

int product = 1;
foreach (KeyValuePair<int, int> recordDistancePerTime in recordDistancesPerTime)
{
    int waysToBeatCurrentRecordDistance = 0;
    for (int i = 0; i <= recordDistancePerTime.Key; ++i)
    {
        int distance = i * (recordDistancePerTime.Key - i);
        if (distance > recordDistancePerTime.Value)
        {
            ++waysToBeatCurrentRecordDistance;
        }
    }
    
    product *= waysToBeatCurrentRecordDistance;
}

Console.WriteLine(product);
