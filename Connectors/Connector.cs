using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using QuickGraph;
using QuickGraph.Algorithms;
using CoordinateSystems;
using sharpproject;
using static System.Math;

namespace sharpproject.Connectors
{
    //public enum CoordSystemType { ICS, GCS, HICS, Spacecraft };
    public abstract class Connector : Body
    {
        //public enum SendingResult { Successfully, ReceiverDontAnswer, ReceiverDontWork, CantSend };
        //public delegate void AnalizingFunction(Message message);

        //protected List<Repeater> repeaters;
        //protected List<Connector> workingConnectors;
        //protected Dictionary<Message, CancellationTokenSource> sentMessages;
        //protected Queue<Message> analisysQueue;
        //protected Queue<Message> sendingQueue;
        //protected bool isSending;
        protected bool isAnalizing;
        protected Body carrier;
        //protected static List<Connector> connectors;
        public Connector(string ID, CoordinateSystem coordinateSystem, double maxDistance, Body carrier)
            : base(ID, coordinateSystem)
        {
            //this.IsWorking = isWorking;
            this.MaxDistance = maxDistance;
            //this.WorkingConnectors = (workingConnectors == null) ? null : new List<Connector>(workingConnectors);
            //analisysQueue = new Queue<Message>();
            //sendingQueue = new Queue<Message>();
            //isSending = false;
            isAnalizing = false;
            this.carrier = carrier;
        }

        public Connector(Connector connector) : this(connector.ID, connector, connector.MaxDistance, connector.carrier)
        {

        }

        public Connector() : base()
        {
            MaxDistance = double.MaxValue;
            isAnalizing = false;
            carrier = null;
        }

        static Connector()
        {
            WorkingConnectors = new List<Connector>();
        }

        //public bool IsWorking { get; set; }
        public double MaxDistance { get; set; }
        public static List<Connector> WorkingConnectors { get; set; }

        protected virtual void TurnTo(Body point)
        {
            base.TurnTo(point.ConvertTo(this));
            Thread.Sleep(1000);
        }
        protected LinkedList<Connector> GetPath(Connector stop)
        {
            // get graph
            var graph = new AdjacencyGraph<Connector, TaggedEdge<Connector, double>>();
            var costs = new Dictionary<Edge<Connector>, double>();
            //var list = new List<Connector>(WorkingConnectors);

            // get vertexes
            foreach (Connector connector in WorkingConnectors)
            {
                graph.AddVertex(connector);
            }

            // get edges
            for (int i = 0; i < WorkingConnectors.Count; i++)
            {
                for (int j = 0; j < WorkingConnectors.Count; j++)
                {
                    if (i != j && WorkingConnectors[i].CanAccess(WorkingConnectors[j]))
                    {
                        var edge = new TaggedEdge<Connector, double>(WorkingConnectors[i], WorkingConnectors[j],
                            Message.Time(WorkingConnectors[i], WorkingConnectors[j]));
                        graph.AddVerticesAndEdge(edge);
                        costs.Add(edge, Message.Time(WorkingConnectors[i], WorkingConnectors[j]));
                        //Console.WriteLine(WorkingConnectors[i].ID + " " + WorkingConnectors[j].ID);
                    }
                }
            }

            var edgeCost = AlgorithmExtensions.GetIndexer(costs);
            var tryGetPath = graph.ShortestPathsDijkstra(edgeCost, this);
            // get path
            IEnumerable<TaggedEdge<Connector, double>> epath;
            if (!tryGetPath(stop, out epath))
            {
                // can't get path
                return null;
            }

            // convert path to linked list
            var path = epath.ToList();
            var result = new LinkedList<Connector>();
            foreach (TaggedEdge<Connector, double> edge in path)
            {
                result.AddLast(edge.Source);
            }
            result.AddLast(stop);
            return result;
        }

        protected void Send(Message message)
        {
            Console.WriteLine(DateTime.Now + ": " + ID + " send " + message.Data + " to " + message.FindNext(this).ID);

            TurnTo(message.FindNext(this).ConvertTo(this));
            message.Send(this);

            Console.WriteLine(DateTime.Now + ": " + ID + " sent " + message.Data + " to " + message.FindNext(this).ID);
        }

        public void Send(MessageType messageType, Connector receiver)
        {
            var path = GetPath(receiver);

            if(path != null)
            {
                Send(new Message(messageType, path));
            }
        }

        protected abstract void Analize(Message message);

        public virtual void GetMessage(Message message)
        {
            if (!isAnalizing)
            {
                Analize(message);
            }
        }

        protected virtual bool CanAccess(Body receiver)
        {
            return (!Message.IsCrossing(this, receiver)) && (receiver.ConvertTo(this).Length < MaxDistance);
        }





        /*public void AddMessage(Message message)
        {
            analisysQueue.Enqueue(message);
            AnalizeMessages();
        }*/

        /*protected void RemoveReceiverFromReceivers(Message message)
        {
            sentMessages[message].Cancel();
            sentMessages.Remove(message);
        }*/

        /*protected void Send(Message newMessage)
        {
            sendingQueue.Enqueue(newMessage);
            if(!isSending)
            {
                Task.Run(() =>
                {
                    isSending = true;
                    Message message;
                    do
                    {
                        message = sendingQueue.Dequeue();

                        Console.WriteLine(DateTime.Now + ": " + ID + " send " + message.Data + " to " + message.FindNext(this).ID);

                        TurnTo(message.FindNext(this).Vector);
                        message.Send(this);

                        Console.WriteLine(DateTime.Now + ": " + ID + " sent " + message.Data + " to " + message.FindNext(this).ID);
                    } while (sendingQueue.Count > 0 && IsWorking);

                    isSending = false;
                });
            }
        }

        /*protected void AnalizeMessages()
        {
            if (!isAnalizing)
            {
                Task.Run(() =>
                {
                    isAnalizing = true;
                    do
                    {
                        Analize(analisysQueue.Dequeue());
                    } while (analisysQueue.Count > 0 && IsWorking);
                    isAnalizing = false;
                });
            }
        }

        /*protected async Task<SendingResult> SendAsync(Message message, CancellationToken token)
        {
            Connector receiver = message.FindNext(this);
            Send(message);
            if (!receiver.IsWorking)
            {
                return SendingResult.ReceiverDontWork;
            }

            if (message.Data != MessageType.Got)
            {
                if (await WaitAnswerAsync(message, token))
                {
                    return SendingResult.Successfully;
                }
                else
                {
                    return SendingResult.ReceiverDontAnswer;
                }
            }
            else
            {
                return SendingResult.Successfully;
            }
        }

        protected async Task WaitAnswerAsync(Message message, CancellationToken token)
        {
            Connector receiver = message.FindNext(this);
            await Task.Delay(Message.Time(this, receiver) * 3, token);
            if(!token.IsCancellationRequested)
            {
                workingRepeaters.Remove((Repeater)receiver);
                sentMessages.Remove(message);
                SendNewPath(message);
            }
        }

        protected void SendNewPath(Message message)
        {

            // Make new path
            LinkedList<Connector> path = GetPath(message.Last);
            if (path == null)
            {

            }
            else
            {
                // In this case repeater sends message by new path
                ResendNewPath(message, path);
            }
        }

        protected void ResendNewPath(Message message, LinkedList<Connector> path)
        {
            // Copy path
            var newPath = new LinkedList<Connector>(path);

            // Add repeaters
            for (var node = message.Path.Find(this); node != message.Path.Find(this); node = node.Next)
            {
                newPath.AddFirst(node.Value);
            }

            // Create new message
            Message newMessage = new Message(message.Data, path);
            // Send new message
            Send(newMessage);
        }*/
    }
}
