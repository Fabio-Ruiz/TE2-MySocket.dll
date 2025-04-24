using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    // Inicia el servidor de escucha de manera asincr�nica
    public async Task StartListeningAsync(string ip, int port)
    {
        // Suscribe el evento de mensaje recibido
        _listener.MessageReceived += (sender, message) =>
        {
            ProcessMessage(message);
        };

        // Inicia la escucha asincr�nica
        await _listener.StartListeningAsync(ip, port);
    }

    // Detiene el servidor de escucha
    public void StopListening()
    {
        _listener.Stop();
    }

    // Env�a un mensaje con reintentos autom�ticos
    public async Task SendMessageAsync(string ip, int port, string message)
    {
        // Utiliza la pol�tica de reintentos para enviar el mensaje con reintentos
        await _retryPolicy.TryActionAsync(async () => await _sender.SendMessageAsync(ip, port, message));
    }

    // Procesa los mensajes recibidos (punto de extensi�n)
    private void ProcessMessage(string message)
    {
        // Aqu� se puede agregar la l�gica para procesar los mensajes recibidos.
        // Por ahora, solo se muestra en consola.
        Console.WriteLine($"Procesando mensaje: {message}");
    }

    // Exponer el evento MessageReceived para que otros puedan suscribirse
    public event EventHandler<string> MessageReceived
    {
        add { _listener.MessageReceived += value; }  // Suscribir al evento
        remove { _listener.MessageReceived -= value; }  // Desuscribir del evento
    }
}