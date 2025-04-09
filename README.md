# **SocketLibraryApp** - Biblioteca de Comunicación por Sockets  

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
2. Agrega el proyecto como referencia  

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

