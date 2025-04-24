
# **SocketLibraryApp** - Biblioteca de Comunicación por Sockets  
**Autores**: Jose Fabio Ruiz Morales y Javier Hernández Castillo

---

## **Descripción**  
**SocketLibraryApp** es una biblioteca de comunicación basada en sockets TCP para .NET que simplifica el envío y recepción de mensajes, incorporando:  
 -**Manejo automático de reconexiones** (Retry Policy)  
 -**Desacoplamiento entre red y lógica de negocio**  
 -**Configuración flexible** (timeouts, reintentos, buffers)  
 -**Implementación limpia con patrones de diseño** (Factory, Observer, Strategy)  

---

##  **Instalación**  

### **Manual** (Compilar desde fuente)  
1. Clonar el repositorio  
2. Agregar "SocketLibraryApp.dll" al proyecto como referencia
3. La biblioteca compilada que debe agregarse como referencia se encuntra en la siguiente ruta: TE2-MySocket.dll\SocketLibrary\bin\Debug\net8.0\SocketLibraryApp.dll

---

##  **Guía Rápida**  

### **1. Configuración Básica**  
```csharp
using SocketLibraryApp;

// Crear instancia con configuración por defecto
var socketApp = SocketLibraryFactory.CreateSocketLibrary();

// Iniciar servidor
socketApp.StartListening("127.0.0.1", 8080); 

// Enviar mensaje
socketApp.SendMessage("127.0.0.1", 8080, "Hola Servidor!");
```

### **2. Escucha de Mensajes (Servidor)**  
```csharp
// Opción 1: Usar evento (sin heredar)
socketApp.StartListening("0.0.0.0", 8080);
socketApp.MessageReceived += (sender, message) => 
{
    Console.WriteLine($"[Servidor] Recibido: {message}");
};

// Opción 2: Sobrescribir ProcessMessage (recomendado para lógica compleja)
public class MySocketServer : SocketLibrary 
{
    protected override void ProcessMessage(string message)
    {
        if (message == "PING") 
        {
            SendMessage("127.0.0.1", 8080, "PONG");
        }
    }
}
```

### **3. Mensaje de Consola al Iniciar el Servidor**  
Cuando el servidor comienza a escuchar, se mostrará un mensaje indicando que está escuchando en la dirección y puerto especificados.

```csharp
// Iniciar el servidor
await socketLibrary.StartListeningAsync(serverIp, serverPort);

// Mostrar en consola que el servidor está escuchando
Console.WriteLine($"Servidor escuchando en {serverIp}:{serverPort}...");
```

Este mensaje aparece justo después de que el servidor comienza a escuchar en el puerto y la IP especificados.

---

## **Configuración Avanzada**  

### **Personalizar Política de Reintentos**  
```csharp
var retryPolicy = new RetryPolicy(
    maxRetries: 5, 
    delay: TimeSpan.FromSeconds(1)
);

var customApp = new SocketLibrary(
    new TcpListenerService(),
    new TcpSenderService(),
    retryPolicy
);
```


---

## **Manejo de Errores**  
La biblioteca maneja automáticamente:  
- Timeouts (configurables en TcpSenderService)  
- Fallos de conexión (con reintentos)  
- Excepciones de socket (registradas en consola)  

**Ejemplo de recuperación manual**:  
```csharp
try 
{
    socketApp.SendMessage("192.168.1.100", 8080, "Test");
} 
catch (SocketException ex) 
{
    Console.WriteLine($"Error crítico: {ex.SocketErrorCode}");
}
```

---

## **Notas**  
- La biblioteca está diseñada para ser flexible y fácil de usar, con un enfoque en la simplicidad. El código de usuario no necesita modificar la lógica interna de la biblioteca para que funcione correctamente.
- Asegúrese de que el puerto que usa el servidor no esté bloqueado por un firewall y de que ambos, cliente y servidor, estén corriendo en el mismo entorno de red.

---
