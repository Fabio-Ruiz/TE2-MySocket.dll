
using System;

public interface ISocketListener
{
    void StartListening(string ip, int port);
    event EventHandler<string> MessageReceived;
    void Stop();
}
