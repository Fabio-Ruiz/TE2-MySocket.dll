// Interfaz que define el contrato para enviar mensajes a través de sockets
public interface ISocketSender
{
    // Envía un mensaje a la IP y puerto especificados
    void SendMessage(string ip, int port, string message);
}
