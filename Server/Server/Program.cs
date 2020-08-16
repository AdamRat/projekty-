using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Globalization;


namespace Server
{
    public class SynchronousSocketListener
    {

        // Incoming data from the client.  
        public static string data = null;

        
            public static void StartListening()
            {
                while (true)
                {
                
                    // Data buffer for incoming data.  
                    byte[] bytes = new Byte[1000000];

                    // Establish the local endpoint for the socket.  
                    // Dns.GetHostName returns the name of the
                    // host running the application.  
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 9081);

                    // Create a TCP/IP socket.  
                    Socket listener = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);


                    //System.Threading.Thread.Sleep(4000);


                    // Bind the socket to the local endpoint and
                    // listen for incoming connections.  
                    Guid guid = Generator();
                    try
                    {
                        listener.Bind(localEndPoint);
                        listener.Listen(100);

                        // Start listening for connections.  
                        while (true)
                        {
                            Console.WriteLine("Waiting for a connection...");
                            // Program is suspended while waiting for an incoming connection.  
                            Socket handler = listener.Accept();
                            data = null;

                            // An incoming connection needs to be processed.  
                            while (true)
                            {
                                int bytesRec = handler.Receive(bytes);
                                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                                // Show the data on the console. 
                                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                                Zapis(date, guid, data);
                                Console.WriteLine($"{date}     {guid}     -     {data}");
                                if (data.IndexOf("Koniec") > -1)
                                {
                                    guid = Generator();
                                    break;
                                }
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        Zapis(date, guid, e.ToString());
                        Console.WriteLine(e.ToString());
                    }

                }

            }
            public static void Zapis(string data, Guid guid, string wiadomosc)
            {
                DateTime datetodisplay = DateTime.Today;
                FileStream plik = new FileStream($@"C:\Users\adamr\OneDrive\Pulpit\Nowy folder\LOG_{datetodisplay.ToString("d")}.log", FileMode.Append, FileAccess.Write);
                try
                {
                    StreamWriter zapis = new StreamWriter(plik);
                    zapis.WriteLine($"{data}     {guid}     -     {wiadomosc}");
                    zapis.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
                plik.Close();
            }
            private static Guid Generator()
            {
                Guid guid = Guid.NewGuid();
                return guid;
            }
            public static int Main(String[] args)
            {
                StartListening();
                return 1;
            }
    }
}
