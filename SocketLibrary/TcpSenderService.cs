using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
// Implementación concreta de ISocketSender usando TCP
public class TcpSenderService : ISocketSender
{
    public async Task SendMessageAsync(string ip, int port, string message)
    {
        try
        {
            using (var client = new TcpClient())
            {
                client.SendTimeout = 5000; // Timeout de envío (5 segundos)
                await client.ConnectAsync(ip, port); // Conexión asincrónica
                var stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(buffer, 0, buffer.Length); // Envío asincrónico
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