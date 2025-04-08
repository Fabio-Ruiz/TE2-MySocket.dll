
using System;
using System.Threading;

public class RetryPolicy
{
    private int _maxRetries;
    private TimeSpan _delay;

    public RetryPolicy(int maxRetries, TimeSpan delay)
    {
        _maxRetries = maxRetries;
        _delay = delay;
    }

    public bool TryAction(Action action)
    {
        int attempts = 0;
        while (attempts < _maxRetries)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                attempts++;
                Console.WriteLine($"Intento {attempts} fallido: {ex.Message}");
                if (attempts >= _maxRetries) return false;
                Thread.Sleep(_delay);
            }
        }
        return false;
    }
}
