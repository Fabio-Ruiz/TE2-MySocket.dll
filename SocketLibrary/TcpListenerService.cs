
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TcpListenerService : ISocketListener
{
    private TcpListener _listener;
    private bool _isListening;

    public event EventHandler<string> MessageReceived;

    public void StartListening(string ip, int port)
    {
        _listener = new TcpListener(IPAddress.Parse(ip), port);
        _listener.Start();
        _isListening = true;

        ThreadPool.QueueUserWorkItem(ListenForClients);
    }

    private void ListenForClients(object obj)
    {
        while (_isListening)
        {
            try
            {
                var client = _listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleClient, client);
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
        byte[] buffer = new byte[2048];
        int bytesRead;

        try
        {
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            MessageReceived?.Invoke(this, request);
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
            client.Close();
        }
    }

    public void Stop()
    {
        _isListening = false;
        _listener.Stop();
    }
}
