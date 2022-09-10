using System;
using System.Threading;
using System.Timers;

namespace BlockInput
{
    public partial class NativeMethods
    {
        public static void BlockInput(TimeSpan span)
        {
            try
            {
                NativeMethods.BlockInput(true);
                Thread.Sleep(span);
            }
            finally
            {
                NativeMethods.BlockInput(false);
            }
        }

        /// Return Type: BOOL->int
        ///fBlockIt: BOOL->int
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll", EntryPoint = "BlockInput")]
        [return: System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool BlockInput([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)] bool fBlockIt);

    }



    class Program
    {
        public static int seconds;
        public static DateTime starttime;
        private static void SetTimer()
        {

            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            starttime = DateTime.Now;
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Разблокировка через: {seconds - (DateTime.Now - starttime).TotalSeconds}...");


        }

        private static System.Timers.Timer aTimer;
        static void Main(string[] args)
        {
            Console.WriteLine("На сколько секунд заблочить? (0 - 10000)");
            string str = Console.ReadLine();

            try { seconds = Convert.ToInt32(str); if (seconds < 0 || seconds > 10000) { seconds = 10; Console.WriteLine("Baka."); } } catch (FormatException) { seconds = 10; }


            Console.WriteLine("Ввод заблокирован на " + seconds + " секунд.");

            SetTimer();


            NativeMethods.BlockInput(new TimeSpan(0, 0, seconds));
            Console.WriteLine("Ввод разблокирован.");

            aTimer.Stop();
            aTimer.Dispose();

            Console.ReadKey();

        }
    }
}

