
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// Implementación concreta de ISocketListener usando TCP
public class TcpListenerService : ISocketListener
{
    private TcpListener _listener; // Listener de TCP
    private bool _isListening; // Flag de control

    public event EventHandler<string> MessageReceived;

    public void StartListening(string ip, int port)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _listener.Start();
        _isListening = true;
        // Usa ThreadPool para manejar conexiones concurrentes
        ThreadPool.QueueUserWorkItem(ListenForClients);
    }

    private void ListenForClients(object obj)
    {
        while (_isListening)
        {
            try
            {
                var client = _listener.AcceptTcpClient(); // Acepta nueva conexión
                ThreadPool.QueueUserWorkItem(HandleClient, client); // Maneja en hilo separado
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor de escucha: {ex.Message}");
            }
        }
    }

    private void HandleClient(object obj)
    {
        var client = (TcpClient)obj;
        using var stream = client.GetStream();
        byte[] buffer = new byte[2048]; // Buffer de 2KB
        int bytesRead;

        try
        {
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            MessageReceived?.Invoke(this, request); // Dispara el evento
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Tiempo de espera agotado al leer del cliente.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Error de socket: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido al manejar cliente: {ex.Message}");
        }
        finally
        {
            client.Close(); // Asegura cierre del cliente
        }
    }

    public void Stop()
    {
        _isListening = false;
        _listener.Stop();
    }
}
