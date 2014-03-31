﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SkeletalTracking
{
    class Server
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public Server()
        {
            this.tcpListener = new TcpListener(System.Net.IPAddress.Parse(GetIP4Address()), 8000);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        public string GetIP4Address()
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IP4Address = IPA.ToString();
                    break;
                }
            }

            return IP4Address;
        }

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (true)
            {
                System.Console.WriteLine("Listening...");
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                System.Console.WriteLine("Client connected");

                //create a thread to handle communications with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }
        public delegate void ServerDelegate(string s);
        public event ServerDelegate ServerEvent;
        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                String mes = encoder.GetString(message, 0, bytesRead);
                System.Console.WriteLine(mes);
                ServerEvent(mes);
                //Server reply to the client
                //byte[] buffer = encoder.GetBytes(mes);
                //clientStream.Write(buffer, 0, buffer.Length);
                //clientStream.Flush();
            }
            tcpClient.Close();
        }
    }
}

