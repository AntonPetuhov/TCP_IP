using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCP_Client
{
    class Program
    {
        public static string IPadress = "10.129.188.7"; // test01-15
        public static int port = 8001;

        static byte[] ENQ = { 0x05 };

        public static void TCPCLient()
        {
            TcpClient tcpClient = new TcpClient();
            try
            {
                // подключение к test01-15
                tcpClient.Connect("test01-15", port);
                Console.WriteLine("Подключение установлено");
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void Main(string[] args)
        {
            //TCPCLient();
            
            IPAddress ip = IPAddress.Parse(IPadress);
            // конечная точка - объединение IP-адреса и порта
            IPEndPoint endpoint = new IPEndPoint(ip, port);

            Console.WriteLine(endpoint.Address); 
            Console.WriteLine(endpoint.Port);

            // сообщение для отправки на хост
            var message = "<ENQ>";

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // подключаемся к хосту
                socket.Connect(endpoint);
                Console.WriteLine("Подключение установлено.");
                Console.WriteLine($"Адрес сервера {socket.RemoteEndPoint}");
                Console.WriteLine($"Адрес клиента {socket.LocalEndPoint}");

                while (true)
                {
                    if (socket.Available == 0)
                    {
                        // конвертируем данные в массив байтов
                        //var messageBytes = Encoding.UTF8.GetBytes(message);
                        //int bytesSent = socket.Send(messageBytes);

                        Console.WriteLine($"---");
                        // отправка сообщения хосту
                        socket.Send(ENQ);
                        Console.WriteLine(message);
                    }
                    Thread.Sleep(5000);
                }
            }
            catch(SocketException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine($"Не удалось установить подключение к {endpoint.Address}");
            }
            
            Console.ReadLine();
            
        }
    }

}
