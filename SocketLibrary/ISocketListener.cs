using System;
using System.Threading.Tasks;
// Interfaz que define el contrato para un servicio de escucha de sockets
public interface ISocketListener
{
    // Inicia la escucha en la IP y puerto especificados
    Task StartListeningAsync(string ip, int port);

    // Evento que se dispara cuando se recibe un mensaje
    event EventHandler<string> MessageReceived;
    // Detiene la escucha
    void Stop();
}