using Rug.Osc;
using System;
using System.Threading;

namespace OSC_Receiver
{
    class Program
    {
        static OscReceiver receiver;
        static Thread thread;

        static void Main()
        {
            Console.WriteLine("Please enter receiving port number:");
            int port;
            // This is the port we are going to listen on 
            while (!int.TryParse(Console.ReadLine(), out port))
            {
                Console.WriteLine("Invalid value entered");
            }
            

            // Create the receiver
            receiver = new OscReceiver(port);

            // Create a thread to do the listening
            thread = new Thread(new ThreadStart(ListenLoop));

            // Connect the receiver
            receiver.Connect();

            // Start the listen thread
            thread.Start();

            // wait for a key press to exit
            Console.WriteLine("Press any key to exit");
            Console.ReadKey(true);

            // close the Reciver 
            receiver.Close();

            // Wait for the listen thread to exit
            thread.Join();
        }

        static void ListenLoop()
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {
                    // if we are in a state to recieve
                    if (receiver.State == OscSocketState.Connected)
                    {
                        // get the next message 
                        // this will block until one arrives or the socket is closed
                        OscPacket packet = receiver.Receive();

                        // Write the packet to the console 
                        Console.WriteLine(packet.ToString());

                        // DO SOMETHING HERE!
                       
                    }
                }
            }
            catch (Exception ex)
            {
                // if the socket was connected when this happens
                // then tell the user
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
