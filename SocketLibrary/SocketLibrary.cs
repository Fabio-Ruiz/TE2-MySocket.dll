
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// Clase principal que coordina las operaciones de escucha y env�o
public class SocketLibrary
{
    private readonly ISocketListener _listener; // Servicio de escucha
    private readonly ISocketSender _sender; // Servicio de env�o
    private readonly RetryPolicy _retryPolicy; // Pol�tica de reintentos
    // Constructor que recibe las dependencias (Inyecci�n de Dependencias)
    public SocketLibrary(ISocketListener listener, ISocketSender sender, RetryPolicy retryPolicy)
    {
        _listener = listener;
        _sender = sender;
        _retryPolicy = retryPolicy;
    }
    // Inicia el servidor de escucha
    public void StartListening(string ip, int port)
    {
        // Suscribe el evento de mensaje recibido
        _listener.MessageReceived += (sender, message) => 
        {
            ProcessMessage(message);
        };
        _listener.StartListening(ip, port);
    }
    // Detiene el servidor de escucha
    public void StopListening()
    {
        _listener.Stop();
    }
    // Env�a un mensaje con reintentos autom�ticos
    public void SendMessage(string ip, int port, string message)
    {
        _retryPolicy.TryAction(() => _sender.SendMessage(ip, port, message));
    }
    // Procesa los mensajes recibidos (punto de extensi�n)
    private void ProcessMessage(string message)
    {
        Console.WriteLine($"Procesando mensaje: {message}");
    }
}
