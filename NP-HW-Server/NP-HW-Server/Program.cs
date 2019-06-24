using NP_HW_Server.Service;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NP_HW_Server
{
    class Program
    {
        static TcpListener server;
        static Thread thread;
        static ManualResetEvent eventStop;
        
        static void Main(string[] args)
        {
            eventStop = new ManualResetEvent(false);
            server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            Console.WriteLine("Ожидани подключения");
            thread = new Thread(ServerThreadProcedure);
            thread.Start(server);
        }
        static void ServerThreadProcedure(object obj)
        {
            TcpListener server = (TcpListener)obj;

            while (true)
            {
                IAsyncResult asyncResult = server.BeginAcceptSocket(AsyncServerProc, server);
                while (asyncResult.AsyncWaitHandle.WaitOne(200) == false)
                {
                    if (eventStop.WaitOne(0) == true)
                        return;
                }

            }
        }

        static void AsyncServerProc(IAsyncResult iAsync)
        {

            TcpListener server = (TcpListener)iAsync.AsyncState;
            TcpClient client = server.EndAcceptTcpClient(iAsync);
            Console.WriteLine("Подключился клиент");
            Console.WriteLine("IP адрес клиента" + client.Client.RemoteEndPoint.ToString() + "\n");
            ThreadPool.QueueUserWorkItem(ClientThreadProc, client);

        }

        static void ClientThreadProc(object obj)
        {
            TcpClient client = (TcpClient)obj;
            Console.WriteLine("Рабочий поток клиента запущен");
            var buffer = new byte[1024 * 4];

           // string clientName;
            string messageClient = "";
            string messageServer = "";
            var deserialize = new Deserialize();
            int reciveSize;

            //clientName = Encoding.UTF8.GetString(buffer);
            Console.WriteLine($"Клиент подключен \r\n");
            client.Client.Send(Encoding.ASCII.GetBytes($"Hello"));

            while (true)
            {
                reciveSize = client.Client.Receive(buffer);
                messageClient = Encoding.UTF8.GetString(buffer, 0, reciveSize);
                messageServer = deserialize.Execute(messageClient).Data[0].Parts[0].NameRus;
                client.Client.Send(Encoding.GetEncoding(1251).GetBytes(messageServer));
            }
        }
    }
}
