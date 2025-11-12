namespace AppForSEII2526.API.Logging.LogViewer
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new Subscriber();
            subscriber.Start();

            Console.WriteLine("Presiona Ctrl+C para salir. Esperando logs...");

            // Mantiene la app viva mientras recibe logs
            System.Threading.Thread.Sleep(Timeout.Infinite);
        }
    }

}
