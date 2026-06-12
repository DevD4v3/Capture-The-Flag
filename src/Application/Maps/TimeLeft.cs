namespace CTF.Application.Maps;

/// <summary>
/// Represents the time left on the current map.
/// </summary>
public class TimeLeft
{
    private const int MaxRoundTime = 3600;
    private const int DefaultRoundTime = 900;

    /// <summary>
    /// Represents the interval in seconds.
    /// </summary>
    private int _interval = DefaultRoundTime;

    /// <summary>
    /// Represents the time left in a text draw.
    /// </summary>
    // This property can never be mutable.
    // If this property is modified from the outside, it may cause buffer overflow.
    public string TextDraw { get; } = "00:00";

    public TimeLeft() => UpdateTextDraw();

    /// <summary>
    /// Checks if the countdown has ended.
    /// </summary>
    public bool IsCompleted() => _interval == 0;

    public Result SetInterval(Minutes minutes)
    {
        if (minutes.Value < 0 || minutes.Value > (MaxRoundTime / 60))
        {
            var message = Smart.Format(Messages.InvalidInterval, new { Max = MaxRoundTime / 60 });
            return Result.Failure(message);
        }

        _interval = minutes.Value * 60;
        UpdateTextDraw();
        return Result.Success();
    }

    public Result SetInterval(Seconds seconds)
    {
        if (seconds.Value < 0 || seconds.Value > MaxRoundTime)
        {
            var message = Smart.Format(Messages.InvalidInterval, new { Max = MaxRoundTime });
            return Result.Failure(message);
        }

        _interval = seconds.Value;
        UpdateTextDraw();
        return Result.Success();
    }

    /// <summary>
    /// Reduces the time remaining until it reaches zero.
    /// </summary>
    public void Decrease()
    {
        if(_interval == 0) 
            return;

        _interval--;
        UpdateTextDraw();
    }

    public void Reset()
    {
        _interval = DefaultRoundTime;
        UpdateTextDraw();
    }

    /// <summary>
    /// The purpose is to manipulate the buffer directly with pointers 
    /// to avoid memory reallocations caused by string interpolation.
    /// </summary>
    /// <remarks>
    /// This decision was made because the text will be updated every 1s by a timer.
    /// </remarks>
    private unsafe void UpdateTextDraw()
    {
        int minutes = _interval / 60;
        int seconds = _interval % 60;

        int digit1 = minutes % 10;
        int digit0 = minutes / 10 % 10;

        int digit4 = seconds % 10;
        int digit3 = seconds / 10 % 10;

        fixed (char* text = TextDraw)
        {
            text[0] = (char)(digit0 + '0');
            text[1] = (char)(digit1 + '0');
            text[3] = (char)(digit3 + '0');
            text[4] = (char)(digit4 + '0');
        }
    }
}

public readonly ref struct Minutes
{
    public int Value { get; }
    public Minutes(int value) => Value = value;
}

public readonly ref struct Seconds
{
    public int Value { get; }
    public Seconds(int value) => Value = value;
}
