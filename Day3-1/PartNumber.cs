namespace Day3_1;

public record PartNumber(int Number, bool HasAdjacentPart = false, bool AlreadyInSum = false)
{
    public int Number { get; set; } = Number;
    public bool HasAdjacentPart { get; set; } = HasAdjacentPart;
    public bool AlreadyInSum { get; set; } = AlreadyInSum;
}
