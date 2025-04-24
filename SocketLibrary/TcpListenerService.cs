using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// Implementación concreta de ISocketListener usando TCP
public class TcpListenerService : ISocketListener
{
    private TcpListener _listener; // Listener de TCP
    private bool _isListening; // Flag de control

    public event EventHandler<string> MessageReceived;

    // Método asincrónico para comenzar la escucha
    public async Task StartListeningAsync(string ip, int port)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _listener.Start();
        _isListening = true;

        // Usa ThreadPool para manejar conexiones concurrentes asincrónicamente
        await Task.Run(() => ListenForClients());
    }

    private async Task ListenForClients()
    {
        while (_isListening)
        {
            try
            {
                var client = await _listener.AcceptTcpClientAsync(); // Acepta nueva conexión asincrónicamente
                await HandleClientAsync(client); // Maneja en hilo separado asincrónicamente
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el servidor de escucha: {ex.Message}");
            }
        }
    }

    // Método asincrónico para manejar la conexión del cliente
    private async Task HandleClientAsync(TcpClient client)
    {
        using var stream = client.GetStream();
        var buffer = new byte[1024]; // Buffer más pequeño para fragmentos
        var memoryStream = new MemoryStream();
        int bytesRead;

        try
        {
            // Leer hasta que no haya más datos
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                // Escribir los datos leídos en el MemoryStream
                await memoryStream.WriteAsync(buffer, 0, bytesRead);
            }

            // Convertir el contenido del MemoryStream a una cadena
            string request = Encoding.UTF8.GetString(memoryStream.ToArray());

            // Validación de datos recibidos (espacios y caracteres no imprimibles)
            if (!string.IsNullOrWhiteSpace(request))
            {
                MessageReceived?.Invoke(this, request); // Dispara el evento
            }
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