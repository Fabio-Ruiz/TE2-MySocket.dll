using System;

public static class SocketLibraryFactory
{
    public static SocketLibrary CreateSocketLibrary()
    {
        var listener = new TcpListenerService();
        var sender = new TcpSenderService();
        var retryPolicy = new RetryPolicy(3, TimeSpan.FromSeconds(2));
        return new SocketLibrary(listener, sender, retryPolicy);
    }
}
