namespace Day7_1;

public record Hand : IComparable<Hand>
{
    public Label FirstCard { get; }
    public Label SecondCard { get; }
    public Label ThirdCard { get; }
    public Label FourthCard { get; }
    public Label FifthCard { get; }
    public Type Type { get; }

    public Hand(
        Label firstCard,
        Label secondCard,
        Label thirdCard,
        Label fourthCard,
        Label fifthCard
    )
    {
        FirstCard = firstCard;
        SecondCard = secondCard;
        ThirdCard = thirdCard;
        FourthCard = fourthCard;
        FifthCard = fifthCard;

        Type = DetermineType(FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard);
    }

    public Hand(Label[] labels)
    {
        if (labels.Length != 5)
        {
            throw new Exception($"Length of {nameof(labels)} is {labels.Length} instead of 5.");
        }

        FirstCard = labels[0];
        SecondCard = labels[1];
        ThirdCard = labels[2];
        FourthCard = labels[3];
        FifthCard = labels[4];

        Type = DetermineType(FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard);
    }

    public Hand(string id)
    {
        if (id.Length != 5)
        {
            throw new Exception($"Id {id} is not valid for a {typeof(Hand)}.");
        }

        List<Label> labels = new();
        foreach (char labelString in id)
        {
            if (Enum.TryParse($"L_{labelString}", out Label label))
            {
                labels.Add(label);
                continue;
            }

            throw new Exception($"Character {labelString} in id {id} is cannot be parsed as a {typeof(Label)}.");
        }

        FirstCard = labels[0];
        SecondCard = labels[1];
        ThirdCard = labels[2];
        FourthCard = labels[3];
        FifthCard = labels[4];

        Type = DetermineType(FirstCard, SecondCard, ThirdCard, FourthCard, FifthCard);
    } 

    private Type DetermineType(
        Label firstCard,
        Label secondCard,
        Label thirdCard,
        Label fourthCard,
        Label fifthCard
    )
    {
        Dictionary<Label, int> countPerLabel = new();
        IncrementCountOrAdd(countPerLabel, firstCard);
        IncrementCountOrAdd(countPerLabel, secondCard);
        IncrementCountOrAdd(countPerLabel, thirdCard);
        IncrementCountOrAdd(countPerLabel, fourthCard);
        IncrementCountOrAdd(countPerLabel, fifthCard);

        if (countPerLabel.Count == 1)
        {
            return Type.FIVE_OF_A_KIND;
        }

        if (
            countPerLabel.Count == 2
            && !countPerLabel
                .FirstOrDefault(keyValuePair => keyValuePair.Value == 4)
                .Equals(default(KeyValuePair<Label, int>))
        )
        {
            return Type.FOUR_OF_A_KIND;
        }

        if (
            countPerLabel.Count == 2
            && !countPerLabel
                .FirstOrDefault(keyValuePair => keyValuePair.Value == 3)
                .Equals(default(KeyValuePair<Label, int>))
        )
        {
            return Type.FULL_HOUSE;
        }

        if (
            countPerLabel.Count == 3
            && !countPerLabel
                .FirstOrDefault(keyValuePair => keyValuePair.Value == 3)
                .Equals(default(KeyValuePair<Label, int>))
        )
        {
            return Type.THREE_OF_A_KIND;
        }

        if (
            countPerLabel.Count == 3
            && !countPerLabel
                .FirstOrDefault(keyValuePair => keyValuePair.Value == 2)
                .Equals(default(KeyValuePair<Label, int>))
        )
        {
            return Type.TWO_PAIR;
        }

        if (countPerLabel.Count == 4)
        {
            return Type.ONE_PAIR;
        }

        return Type.HIGH_CARD;
    }

    private void IncrementCountOrAdd(Dictionary<Label, int> dictionary, Label label)
    {
        if (dictionary.ContainsKey(label))
        {
            ++dictionary[label];
        }
        else
        {
            dictionary.Add(label, 1);
        }
    }

    public int CompareTo(Hand? other)
    {
        if (other == null)
        {
            return 1;
        }

        if (Type != other.Type)
        {
            return Type.CompareTo(other.Type);
        }

        if (FirstCard != other.FirstCard)
        {
            return FirstCard.CompareTo(other.FirstCard);
        }

        if (SecondCard != other.SecondCard)
        {
            return SecondCard.CompareTo(other.SecondCard);
        }

        if (ThirdCard != other.ThirdCard)
        {
            return ThirdCard.CompareTo(other.ThirdCard);
        }

        if (FourthCard != other.FourthCard)
        {
            return FourthCard.CompareTo(other.FourthCard);
        }

        return FifthCard.CompareTo(other.FifthCard);
    }
}
