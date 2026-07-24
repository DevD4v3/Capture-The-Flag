namespace CTF.Application.Players.Vitalities;

public class Vitality
{
    public float Amount { get; private set; }
    private Vitality(float amount) => Amount = amount;

    public static Result<Vitality> Create(float amount)
    {
        if (amount < 0 || amount > 100)
            return Result<Vitality>.Failure(Messages.InvalidVitality);

        return Result<Vitality>.Success(new Vitality(amount));
    }
}
