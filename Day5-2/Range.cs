namespace Day5_2;

public record Range(long StartIncluded, long EndExcluded) : IComparable<Range>
{

    public long StartIncluded { get; set; } = StartIncluded;
    
    public long EndExcluded { get; set; } = EndExcluded;
    
    public int CompareTo(Range? other)
    {
        if (other == null)
        {
            return 1;
        }

        return StartIncluded.CompareTo(other.StartIncluded);
    }
}
