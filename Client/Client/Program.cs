using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class SynchronousSocketClient
    {

        public static void StartClient()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1000000];

            // Connect to a remote device.  
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                //IPAddress address = IPAddress.Parse(ipAddress);
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9081);
                // Create a TCP/IP  socket.  
                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                
                // Connect the socket to the remote endpoint. Catch any errors.  
                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());


                    //byte[] username = Encoding.ASCII.GetBytes(Environment.UserName);
                    //int bytesSenting = sender.Send(username);
                    string message = "";
                    Console.WriteLine("Podaj co chcesz przesłać. Napisz Koniec aby zakończyć.");
                    do {

                        message = Messages();
                        // Encode the data string into a byte array.  
                        byte[] msg = Encoding.ASCII.GetBytes(Environment.UserName + ":     " + message);
                        
                        // Send the data through the socket.  
                        int bytesSent = sender.Send(msg);
                        
                    }while (message != "Koniec");
                    // Receive the response from the remote device.  
                    int bytesRec = sender.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes, 0, bytesRec));

                    // Release the socket.  
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

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
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
                StartClient();
                return 0;
        }

        private static string Messages()
        {
            return Console.ReadLine();
        }
    }
}
