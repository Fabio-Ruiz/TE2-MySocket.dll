
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// Clase principal que coordina las operaciones de escucha y envío
public class SocketLibrary
{
    private readonly ISocketListener _listener; // Servicio de escucha
    private readonly ISocketSender _sender; // Servicio de envío
    private readonly RetryPolicy _retryPolicy; // Política de reintentos
    // Constructor que recibe las dependencias (Inyección de Dependencias)
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
    // Envía un mensaje con reintentos automáticos
    public void SendMessage(string ip, int port, string message)
    {
        _retryPolicy.TryAction(() => _sender.SendMessage(ip, port, message));
    }
    // Procesa los mensajes recibidos (punto de extensión)
    private void ProcessMessage(string message)
    {
        Console.WriteLine($"Procesando mensaje: {message}");
    }
}
