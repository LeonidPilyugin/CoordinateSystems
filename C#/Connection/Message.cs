using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;

namespace Connection
{
    public enum MessageType { M1, M2, M3, Got, Fail };
    public class Message
    {
        public const double LightSpeed = 299792458.0;

        protected MessageType data;
        protected LinkedList<Connector> path;

        public Message(MessageType data, LinkedList<Connector> path)
        {
            this.data = data;
            this.path = new LinkedList<Connector>(path);
        }

        public Message(MessageType message, params Connector[] connectors)
        {
            data = message;
            path = new LinkedList<Connector>();
            foreach(Connector connector in connectors)
            {
                path.AddLast(connector);
            }
        }

        public Message(Message message)
        {
            data = message.data;
            path = new LinkedList<Connector>(message.path);
        }

        public MessageType Data { get { return data; } }
        public LinkedList<Connector> Path { get { return path; } }
        public Connector Last { get { return Path.Last.Value; } }
        public Connector First { get { return Path.First.Value; } }

        static public int Time(Connector r1, Connector r2)
        {
            return (int)(Distance(r1, r2) / LightSpeed * 1000.0);
        }

        static public double Distance(Connector r1, Connector r2)
        {
            return (r1.VectorFromSun - r2.VectorFromSun).Length;
        }

        public void Send(Connector sender)
        {
            Task.Run(() =>
            {
                // wait
                System.Threading.Thread.Sleep(Time(sender, FindNext(sender)));
                // call receiver
                FindNext(sender).GetMessage(this);
            });
        }

        public Connector FindNext(Connector repeater)
        {
            return Path.Find(repeater).Next?.Value;
        }

        public Connector FindPrevious(Connector repeater)
        {
            return Path.Find(repeater).Previous?.Value;
        }

        public static bool IsCrossing(Body body1, Body body2)
        {
            var result = false;
            Vector v1 = body1.VectorFromSun;
            Vector v2 = body2.VectorFromSun;

            foreach (Body body in Body.Bodies)
            {
                if(body.VectorFromSun != v1 && body.VectorFromSun != v2)
                    result |= body.IsCrossing(body1.ConvertTo(body), body2.ConvertTo(body));
            }

            return result;
        }
    }
}
