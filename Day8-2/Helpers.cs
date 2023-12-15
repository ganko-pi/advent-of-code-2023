namespace Day8_2;

public class Helpers
{
    public static long GreatestCommonDivider(long n1, long n2)
    {
        if (n2 == 0)
        {
            return n1;
        }
        else
        {
            return GreatestCommonDivider(n2, n1 % n2);
        }
    }

    public static long LeastCommonMultiple(List<long> numbers)
    {
        return numbers.Aggregate((S, val) => S * val / GreatestCommonDivider(S, val));
    }
}
