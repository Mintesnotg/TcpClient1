using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TcpClientServer;

namespace TcpClient1
{
     class TCPClient
    {

        static void Main()
        {

            Thread serverThread = new Thread(() =>
            {
                TCPServer server = new TCPServer(5000);
                server.Start();
            });

            serverThread.IsBackground = true;
            serverThread.Start();

            while (true) {


                try
                {
                    Console.WriteLine("Enter command GET_TEMP/GET_STATUS/EXIT :- ");
                    string command = Console.ReadLine();
                    if (command.ToUpper() == "EXIT") break;

                    using (TcpClient client = new TcpClient("127.0.0.1", 5000))
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] commandBytes = Encoding.ASCII.GetBytes(command);
                        stream.Write(commandBytes, 0, commandBytes.Length);

                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("Server Response: " + response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error communicating with server: " + ex.Message);
                }

            }
        }
    }
}
