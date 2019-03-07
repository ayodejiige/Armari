using System;

namespace armari
{
    public class Logger
    {
        public delegate double LogFunc(string msg);

        private static readonly Logger m_instance = new Logger();
        public event EventHandler<EventArgsT<String>> MessageUpdated = delegate { };
        public event EventHandler<EventArgsT<String>> ErrorOccurred = delegate { };

        static Logger()
        {
        }

        private Logger()
        {
        }

        public static Logger Instance
        {
            get
            {
                return m_instance;
            }
        }

        public void Message(string msg)
        {
            //MessageUpdated(this, new EventArgsT<string>(msg));
            Console.WriteLine("INFO: ARMARI -> {0}", msg);
        }

        public void Error(string msg)
        {
            ErrorOccurred(this, new EventArgsT<string>(msg));
        }

    }
}