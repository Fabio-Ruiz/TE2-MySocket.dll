using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


class Prueba { 
    public static void Main()
    {
        var socketLibrary = SocketLibraryFactory.CreateSocketLibrary();

        // Inicia el socketLibrary escuchando en el puerto 5000 de la IP 127.0.0.1
        socketLibrary.StartListening("127.0.0.1", 5000);

        // Envía un mensaje de ejemplo
        socketLibrary.SendMessage("127.0.0.1", 5000, "Hola desde el cliente!");

        Console.WriteLine("Presiona cualquier tecla para detener...");
        Console.ReadKey();

        // Detiene el socketLibrary
        socketLibrary.StopListening();
    }
}
