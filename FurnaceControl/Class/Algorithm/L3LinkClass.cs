using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace FurnaceControl
{
    internal class L3LinkClass : TimerClass
    {
        public Socket L3Socket;
        private readonly MainClass m_MainClass;

        private IPHostEntry ipHostInfo;
        private IPAddress ipAddress;
        private IPEndPoint ipep;

        public L3LinkClass(MainClass mc, int timer_interval)
        {
            this.m_MainClass = mc;
            this.Start(timer_interval, "L3LinkClassTimer");

            //this.InitSocket();

            var t1 = new Thread(new ThreadStart(this.receiveTCPThread));
            t1.Start();
        }

        /**
         * 주기적으로 실행하는 함수 
         **/
        public override void Run()
        {
            try
            {

                //this.m_MainClass.m_SysLogClass.SystemLog(this, "L3LinkClassTimer");

                //L3Socket.Send(data);
            }
            catch (Exception)
            {
            }
        }

        private void InitSocket()
        {
            this.L3Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.ipHostInfo = Dns.Resolve(Dns.GetHostName());
            this.ipAddress = this.ipHostInfo.AddressList[0];
            this.ipep = new IPEndPoint(this.ipAddress, 11000);
        }

        private void receiveTCPThread()
        {
            byte[] bytes = new byte[1024];

            try
            {
                this.L3Socket.Connect(this.ipep);

                while (true)
                {
                    Console.WriteLine("Socket connected to {0}", this.L3Socket.RemoteEndPoint.ToString());

                    // Encode the data string into a byte array.
                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

                    // Send the data through the socket.
                    int bytesSent = this.L3Socket.Send(msg);

                    // Receive the response from the remote device.
                    int bytesRec = this.L3Socket.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}", Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    Thread.Sleep(4000);
                }
                // Release the socket.
                this.L3Socket.Shutdown(SocketShutdown.Both);
                this.L3Socket.Close();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }
    }
}
