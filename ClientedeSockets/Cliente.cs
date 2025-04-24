using System;
using System.Threading.Tasks;

namespace SocketLibraryApp
{
    class Client
    {
        static async Task Main(string[] args)
        {
            // Dirección del servidor
            string serverIp = "127.0.0.1";
            int serverPort = 8080;
            string message = "¡Hola desde el cliente!";

            // Crear la biblioteca de sockets usando la fábrica
            var socketLibrary = SocketLibraryFactory.CreateSocketLibrary();

            // Enviar el mensaje al servidor
            await socketLibrary.SendMessageAsync(serverIp, serverPort, message);

            Console.WriteLine("Mensaje enviado al servidor: " + message);
        }
    }
}