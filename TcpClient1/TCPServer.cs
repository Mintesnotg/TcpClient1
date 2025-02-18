using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpClientServer
{


    class TCPServer
    {

        private TcpListener server;


        //public TCPServer()
        //{
                
        //}
        public TCPServer(int port)
        {

            try
            {
                server = new TcpListener(IPAddress.Any, port);
                server.Start();
                Console.WriteLine("Server started...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error starting server: " + ex.Message);

            }

        }

        public void Start()
        {

            try
            {
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(HandleClient, client); // use concurrency 

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error accepting client: " + ex.Message);
                
            }

        }

        private void HandleClient(object obj) {


            try
            {
                TcpClient client = (TcpClient)obj;
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                string req = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                string response = "Unknown Command";

                switch (req.Trim().ToUpper())
                {

                    case "GET_TEMP":
                        response = "Temprature:25°C";
                        break;
                    case "GET_STATUS":
                        response = "Status :OK";
                        break;
                }

                byte[] responeBytes = Encoding.ASCII.GetBytes(response);
                stream.Write(responeBytes, 0, responeBytes.Length);
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error handling client: " + ex.Message);

                throw;
            }



        }


    }
}
