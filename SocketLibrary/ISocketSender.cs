// Interfaz que define el contrato para enviar mensajes a trav�s de sockets
public interface ISocketSender
{
    // Env�a un mensaje a la IP y puerto especificados
    void SendMessage(string ip, int port, string message);
}
