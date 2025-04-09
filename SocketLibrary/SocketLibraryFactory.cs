using System;
// Fábrica para crear instancias preconfiguradas de SocketLibrary
public static class SocketLibraryFactory
{
    public static SocketLibrary CreateSocketLibrary()
    {
        var listener = new TcpListenerService(); // Implementación concreta de listener
        var sender = new TcpSenderService(); // Implementación concreta de sender
        var retryPolicy = new RetryPolicy(3, TimeSpan.FromSeconds(2)); // 3 reintentos, 2 segundos entre ellos
        return new SocketLibrary(listener, sender, retryPolicy);
    }
}
