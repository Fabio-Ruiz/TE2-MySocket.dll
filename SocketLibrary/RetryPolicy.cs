
using System;
using System.Threading;

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
    public bool TryAction(Action action)
    {
        int attempts = 0;
        while (attempts < _maxRetries)
        {
            try
            {
                action(); // Ejecuta la acci�n
                return true; // Retorna true si tuvo �xito
            }
            catch (Exception ex)
            {
                attempts++;
                Console.WriteLine($"Intento {attempts} fallido: {ex.Message}");
                if (attempts >= _maxRetries) return false;
                Thread.Sleep(_delay); // Espera antes de reintentar
            }
        }
        return false;
    }
}
