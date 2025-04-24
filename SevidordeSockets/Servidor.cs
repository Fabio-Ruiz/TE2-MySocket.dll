using System;
using System.Threading.Tasks;

namespace SocketLibraryApp
{
    class Server
    {
        static async Task Main(string[] args)
        {
            // Dirección y puerto donde escuchará el servidor
            string serverIp = "127.0.0.1";
            int serverPort = 8080;

            // Crear la biblioteca de sockets usando la fábrica
            var socketLibrary = SocketLibraryFactory.CreateSocketLibrary();

            // Suscribirse al evento de mensajes recibidos
            socketLibrary.MessageReceived += (sender, message) =>
            {
                Console.WriteLine("Mensaje recibido del cliente: " + message);
            };

            // Indicar en la consola que el servidor está escuchando
            Console.WriteLine($"Servidor escuchando en {serverIp}:{serverPort}...");

            // Iniciar la escucha del servidor
            await socketLibrary.StartListeningAsync(serverIp, serverPort);

            // Mantener el servidor activo
            Console.ReadLine();
        }
    }
}