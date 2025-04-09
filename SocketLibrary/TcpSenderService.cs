
using System;
using System.Net.Sockets;
using System.Text;
// Implementación concreta de ISocketSender usando TCP
public class TcpSenderService : ISocketSender
{
    public void SendMessage(string ip, int port, string message)
    {
        try
        {
            using (var client = new TcpClient())
            {
                client.SendTimeout = 5000; // Timeout de envío (5 segundos)
                client.Connect(ip, port);
                var stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);// Envía los datos
            } 
        }
        catch (TimeoutException)
        {
            Console.WriteLine("Tiempo de espera agotado al enviar el mensaje.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Error de socket al enviar mensaje: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error desconocido al enviar mensaje: {ex.Message}");
        }
    }
}
