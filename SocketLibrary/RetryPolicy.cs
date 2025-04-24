using System;
using System.Threading;
using System.Threading.Tasks;

public class RetryPolicy
{
    private int _maxRetries;
    private TimeSpan _delay;
    // Clase que implementa una pol�tica de reintentos para operaciones fallidas
    public RetryPolicy(int maxRetries, TimeSpan delay)
    {
        _maxRetries = maxRetries; // N�mero m�ximo de reintentos
        _delay = delay; // Tiempo de espera entre reintentos
    }
    // Intenta ejecutar una acci�n y aplica la pol�tica de reintentos si falla
    public async Task<bool> TryActionAsync(Func<Task> action)
    {
        int attempts = 0;
        while (attempts < _maxRetries)
        {
            try
            {
                await action(); // Ejecuta la acci�n asincr�nica
                return true; // Retorna true si tuvo �xito
            }
            catch (Exception ex)
            {
                attempts++;
                Console.WriteLine($"Intento {attempts} fallido: {ex.Message}");
                if (attempts >= _maxRetries) return false;
                await Task.Delay(_delay); // Espera antes de reintentar asincr�nicamente
            }
        }
        return false;
    }
}
