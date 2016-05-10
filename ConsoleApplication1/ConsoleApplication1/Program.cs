using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class ItemBase
    {

    }

    public class Widget:ItemBase
    {

    }

    class Worker
    {
        void DoWork(object obj)
        {
            Console.WriteLine("in dowork(object)");
            Console.ReadLine();
        }
        void DoWork(Widget widget)
        {
            Console.WriteLine("in dowork(widget)");
            Console.ReadLine();
        }
        void DoWork(ItemBase  itemBase)
        {
            Console.WriteLine("in dowork(itemBase)");
            Console.ReadLine();
        }

        public void Run()
        {
            object o = new Widget();
            DoWork((Widget)o);
        }

      
    }


    public class Alert
    {
        public event EventHandler<EventArgs> SendMessage;
        public void Execute()
        {
            SendMessage(this, new EventArgs());
        }
    }

    public class Subscriber
    {
        Alert alrert = new Alert();
        public void Subscribe()
        {
            alrert.SendMessage += (sender, e) => { Console.WriteLine("first"); };
            alrert.SendMessage += (sender, e) => { Console.WriteLine("second"); };
            alrert.SendMessage += (sender, e) => { Console.WriteLine("third"); };
            alrert.SendMessage += (sender, e) => { Console.WriteLine("third"); };
        }

        public void Execute()
        {
            alrert.Execute();
        }
    }




    class Program
    {
        static void Main(string[] args)
        {
            //Worker w = new Worker();
            //w.Run();

            Subscriber sub = new Subscriber();
            sub.Subscribe();
            sub.Execute();
            Console.ReadLine();

        }
    }
}
