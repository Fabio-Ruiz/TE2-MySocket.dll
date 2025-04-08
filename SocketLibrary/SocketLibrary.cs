
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SocketLibrary
{
    private readonly ISocketListener _listener;
    private readonly ISocketSender _sender;
    private readonly RetryPolicy _retryPolicy;

    public SocketLibrary(ISocketListener listener, ISocketSender sender, RetryPolicy retryPolicy)
    {
        _listener = listener;
        _sender = sender;
        _retryPolicy = retryPolicy;
    }

    public void StartListening(string ip, int port)
    {
        _listener.MessageReceived += (sender, message) => 
        {
            ProcessMessage(message);
        };
        _listener.StartListening(ip, port);
    }

    public void StopListening()
    {
        _listener.Stop();
    }

    public void SendMessage(string ip, int port, string message)
    {
        _retryPolicy.TryAction(() => _sender.SendMessage(ip, port, message));
    }

    private void ProcessMessage(string message)
    {
        Console.WriteLine($"Procesando mensaje: {message}");
    }
}
