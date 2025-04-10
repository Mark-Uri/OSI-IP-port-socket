using System.Net;
using System.Net.Sockets;
using System.Text;

class Client
{
    private const int DEFAULT_BUFLEN = 512;
    private const string DEFAULT_PORT = "27015";

    static void Main()
    {
        Console.Title = "CLIENT SIDE";
        try
        {
            var ipAddress = IPAddress.Loopback; // IP-адрес локального хоста (127.0.0.1), который используется для подключения к серверу на текущем устройстве
            var remoteEndPoint = new IPEndPoint(ipAddress, int.Parse(DEFAULT_PORT));

            var clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            clientSocket.Connect(remoteEndPoint); // инициирует подключение клиента к серверу по указанному конечному пункту (IP-адрес и порт)
            Console.WriteLine("Подключение к серверу установлено.");

            while (true)
            {
                Console.Write("Напишите запрос к серверу (или наберите exit для выхода): ");
                var message = Console.ReadLine();
                if (message == "exit")
                {
                    clientSocket.Shutdown(SocketShutdown.Send);
                    clientSocket.Close();
                    Console.WriteLine("Соединение с сервером закрыто.");
                    break;
                }
                else
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    clientSocket.Send(messageBytes);
                    Console.WriteLine($"Сообщение отправлено: {message}");

                    var buffer = new byte[DEFAULT_BUFLEN];
                    int bytesReceived = clientSocket.Receive(buffer);
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
                    Console.WriteLine($"Ответ от сервера: {response}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }
}
