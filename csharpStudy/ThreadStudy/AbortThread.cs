using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestThread
{
    public static class AbortThread
    {
        /// <summary>
        /// start a thread, and abort in main thread.
        /// we can pass an object to Abort(), then we can get some information in sub thread
        /// </summary>
        public static void Abort()
        {
            var thread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine(string.Format(">sub thread Id : {0}", Thread.CurrentThread.ManagedThreadId));
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine(">Live");
                    }
                }
                catch (ThreadAbortException ex)
                {
                    //dynamic obj = ex.ExceptionState;
                    //Console.WriteLine(string.Format(">sub thread abort exception from main thread:{0}", obj.Message));
                    (ex.ExceptionState as Action)();

                }

                Console.WriteLine(">Sub thread finished");
            });

            thread.Start();

            Console.WriteLine(string.Format("Main thread Id : {0}", Thread.CurrentThread.ManagedThreadId));

            Console.WriteLine("main thread: Wait 3 seconds");
            Thread.Sleep(3000);
            Console.WriteLine("maint thread : Finish wait");

            //thread.Abort("My customer Abort Information" );
            //thread.Abort(new { Message = "My customer Abort Information" });
            Action action = new Action(() =>
            {
                Console.WriteLine(string.Format("Current thread Id : {0}", Thread.CurrentThread.ManagedThreadId));

                Console.WriteLine("I'm a action in main thread");
            });
            thread.Abort(action);

            Console.WriteLine(string.Format("sub thread IsAlive: {0}", thread.IsAlive));

            Console.WriteLine("main thread: Wait 3 seconds");
            Thread.Sleep(3000);
            Console.WriteLine("main thread: Finish wait");

            thread.Join();
            Console.WriteLine(string.Format("Sub thread IsAlive: {0}", thread.IsAlive));
            Console.WriteLine();
        }
    }
}
