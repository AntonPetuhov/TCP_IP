using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCP_Server
{
    class Program
    {
        public static string IPadress = "10.129.188.7"; // test01-15
        public static int port = 8001;

        #region functions
        static string TranslateBytes(byte BytePar)
        {
            switch (BytePar)
            {
                case 0x02:
                    return "<STX>";
                case 0x03:
                    return "<ETX>";
                case 0x04:
                    return "<EOT>";
                case 0x05:
                    return "<ENQ>";
                case 0x06:
                    return "<ACK>";
                case 0x15:
                    return "<NAK>";
                case 0x16:
                    return "<SYN>";
                case 0x17:
                    return "<ETB>";
                case 0x0A:
                    return "<LF>";
                case 0x0D:
                    return "<CR>";
                default:
                    return "<HZ>";
            }
        }
        #endregion
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse(IPadress);
            // локальная точка EndPoint, на которой сокет будет принимать подключения от клиентов
            EndPoint endpoint = new IPEndPoint(ip, port);
            // создаем сокет
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // связываем сокет с локальной точкой endpoint 
            socket.Bind(endpoint);
            // получаем конечную точку, с которой связан сокет
            Console.WriteLine(socket.LocalEndPoint);

            // запуск прослушивания подключений
            socket.Listen(1000);
            Console.WriteLine("Сервер запущен. Ожидание подключений...");
            //После начала прослушивания сокет готов принимать подключения
            // получаем входящее подключение
            Socket client = socket.Accept();

            // получаем адрес клиента
            Console.WriteLine($"Адрес подключенного клиента: {client.RemoteEndPoint}");

            while (true)
            {   
                if (client.Available == 0)
                {
                    Console.WriteLine("waiting...");
                }
                else
                {
                    // тоже работает
                    /*
                    StringBuilder builder = new StringBuilder();
                    int received_bytes_first = 0; // количество полученных байтов
                    byte[] received_data_first = new byte[1024]; // буфер для получаемых данных
                    do
                    {
                        received_bytes_first = client.Receive(received_data_first);
                        builder.Append(Encoding.GetEncoding(1251).GetString(received_data_first, 0, received_bytes_first));
                    }
                    while (client.Available > 0);
                    Console.WriteLine(builder);
                    */

                    // буфер для получения данных
                    var receivedBytes = new byte[512];
                    // получаем данные
                    var bytes = client.Receive(receivedBytes);
                    // преобразуем полученные данные в строку
                    string MessageFromClient = Encoding.UTF8.GetString(receivedBytes, 0, bytes);
                    // выводим данные на консоль
                    Console.WriteLine(MessageFromClient);
                    Console.WriteLine($"C:{TranslateBytes(receivedBytes[0])};");
                }

                Thread.Sleep(1000);
                /*
                    // получение сообщения от клиента
                    byte[] received_data = new byte[256]; // буфер для ответа
                StringBuilder received_builder = new StringBuilder();
                int received_bytes = 0; // количество полученных байт
                do
                {
                    received_bytes = socket.Receive(received_data, received_data.Length, 0);
                    received_builder.Append(Encoding.GetEncoding(1251).GetString(received_data, 0, received_bytes));
                }
                while (socket.Available > 0);
                // выводим данные на консоль
                Console.WriteLine(received_builder);
                */
            }

            Console.ReadLine();
        }
    }
}
