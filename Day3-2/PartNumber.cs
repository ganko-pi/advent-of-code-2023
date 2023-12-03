namespace Day3_1;

public record PartNumber(int Number, bool HasAdjacentGear = false, bool AlreadyInSum = false)
{
    public int Number { get; set; } = Number;
    public bool HasAdjacentGear { get; set; } = HasAdjacentGear;
    public List<PartNumber> RelatedPartNumbers { get; set; } = new();
    public bool AlreadyInSum { get; set; } = AlreadyInSum;
}
