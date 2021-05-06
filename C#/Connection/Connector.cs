using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using QuickGraph;
using QuickGraph.Algorithms;
using CoordinateSystems;
using static System.Math;

// Файл содержит абстрактный класс Connector.

namespace Connection
{
    /// <summary>
    /// Класс Connector описывает средство связи. Наследник класса <see cref="Body"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="isAnalizing"/><br/>
    /// 2) <see cref="view"/><br/>
    /// 3) <see cref="workingConnectors"/><br/>
    /// 4) <see cref="Body.id"/><br/>
    /// 5) <see cref="Body.bodies"/><br/>
    /// 6) <see cref="CoordinateSystem.vector"/><br/>
    /// 7) <see cref="CoordinateSystem.velocity"/><br/>
    /// 8) <see cref="CoordinateSystem.basis"/><br/>
    /// 9) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="Connector(Connector)"/><br/>
    /// 2) <see cref="Connector(string, Vector, Basis, View)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="View"/><br/>
    /// 2) <see cref="WorkingConnectors"/><br/>
    /// 3) <see cref="Body.Bodies"/><br/>
    /// 4) <see cref="Body.ID"/><br/>
    /// 5) <see cref="Vector"/><br/>
    /// 6) <see cref="Basis"/><br/>
    /// 7) <see cref="CoordinateSystem.ReferenceSystem"/><br/>
    /// 8) <see cref="CoordinateSystem.Velocity"/><br/>
    /// 9) <see cref="CoordinateSystem.TransitionMatrix"/><br/>
    /// 10) <see cref="CoordinateSystem.TransitionMatrixRelativelyRoot"/><br/>
    /// 11) <see cref="CoordinateSystem.BasisRelativelyReferenceSystem"/><br/>
    /// 12) <see cref="CoordinateSystem.BasisRelativelyRoot"/><br/>
    /// 13) <see cref="CoordinateSystem.RootBasis"/><br/>
    /// 14) <see cref="CoordinateSystem.ReferenceSystemBasis"/><br/>
    /// 15) <see cref="CoordinateSystem.VectorFromRoot"/><br/>
    /// 16) <see cref="CoordinateSystem.VelocityFromRoot"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="TurnTo(Body)"/><br/>
    /// 2) <see cref="GetPath(Connector)"/><br/>
    /// 3) <see cref="Send(Message)"/><br/>
    /// 4) <see cref="Send(MessageType, Connector)"/><br/>
    /// 5) <see cref="Analize(Message)"/><br/>
    /// 6) <see cref="GetMessage(Message)"/><br/>
    /// 7) <see cref="CanAccess(Body)"/><br/>
    /// 8) <see cref="Body.IsInside(Vector)"/><br/>
    /// 9) <see cref="Body.IsCrossing(Vector, Vector)"/><br/>
    /// 10) <see cref="CoordinateSystem.ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 11) <see cref="CoordinateSystem.GetVectorFromRoot(Vector)"/><br/>
    /// 12) <see cref="CoordinateSystem.GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 13) <see cref="CoordinateSystem.GetVelocityFromRoot(Vector)"/><br/>
    /// 14) <see cref="CoordinateSystem.GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public abstract class Connector : Body
    {
        //public enum SendingResult { Successfully, ReceiverDontAnswer, ReceiverDontWork, CantSend };
        //public delegate void AnalizingFunction(Message message);

        #region data
        //protected List<Repeater> repeaters;
        //protected List<Connector> workingConnectors;
        //protected Dictionary<Message, CancellationTokenSource> sentMessages;
        //protected Queue<Message> analisysQueue;
        //protected Queue<Message> sendingQueue;
        //protected static List<Connector> connectors;
        //public bool IsWorking { get; set; }
        //protected bool isSending;

        /// <summary>
        /// true, если спутник занят, false, если нет.
        /// </summary>
        protected bool isAnalizing;

        /// <summary>
        /// Носитель.
        /// </summary>
        //protected Body carrier;

        /// <summary>
        /// Область видимости.
        /// </summary>
        protected View view;

        /// <summary>
        /// Все работающие средства связи.
        /// </summary>
        protected static List<Connector> workingConnectors;
        #endregion

        #region constructors
        /// <summary>
        /// Задает полям переданные значения, но:<br/>
        /// velocity = new Vector(0.0, 0.0, 0.0)<br/>
        /// referenceSystem = carrier<br/>
        /// isAnalizing = false<br/>
        /// </summary>
        /// 
        /// <param name="id"> Идентификатор</param>
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="view"> Область видимости.</param>
        /// <param name="carrier"> Носитель.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Connector(string id, Vector vector, Basis basis, View view)
        {
            //this.IsWorking = isWorking;
            //this.WorkingConnectors = (workingConnectors == null) ? null : new List<Connector>(workingConnectors);
            //analisysQueue = new Queue<Message>();
            //sendingQueue = new Queue<Message>();
            //isSending = false;

            ID = id;
            Vector = vector;
            Basis = basis;
            Velocity = new Vector(0.0, 0.0, 0.0);
            View = view;
            isAnalizing = false;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="connector"> Копируемое средство связи.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Connector(Connector connector) :
            this(connector.ID, connector.vector, connector.basis, connector.view)
        {

        }

        static Connector()
        {
            workingConnectors = new List<Connector>();
        }
        #endregion

        #region properties
        /// <summary>
        /// Область видимости. Не должна быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public View View
        {
            get
            {
                return view;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("View mustn't be null");
                }
                view = value;
            }
        }

        /// <summary>
        /// Все работающие средства связи.
        /// </summary>
        public List<Connector> WorkingConnectors
        {
            get
            {
                return workingConnectors;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Метод <see cref="CoordinateSystem.TurnTo(Vector, List{Vector})"/> не наследуется и вызывает исключение.
        /// </summary>
        /// 
        /// <param name="target"></param>
        /// <param name="points"></param>
        /// 
        /// <exception cref="NotImplementedException">
        /// Вызывается при вызове метода.
        /// </exception>
        new public void TurnTo(Vector target, List<Vector> points = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Поворачивает средство связи осью X к телу.
        /// </summary>
        /// 
        /// <param name="point"> Тело, к которому поворачивается средство связи.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если передается null.
        /// </exception>
        protected virtual void TurnTo(Body point)
        {
            base.TurnTo(point.ConvertTo(this));
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Получает кратчайший путь через к указанному средству связи.
        /// </summary>
        /// 
        /// <param name="stop"> Средство связи, к которому ищется путь.</param>
        /// 
        /// <returns>
        /// Кратчайший путь в виде связного списка:
        /// [это средство связи], [следующее], ..., [получатель]
        /// </returns>
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

        /// <summary>
        /// Отправляет сообщение следующему в списке.
        /// </summary>
        /// 
        /// <param name="message"> отправляемое сообщение.</param>
        protected void Send(Message message)
        {
            Console.WriteLine(DateTime.Now + ": " + ID + " send " + message.Type + " to " + message.FindNext(this).ID);

            TurnTo(message.FindNext(this).ConvertTo(this));
            message.Send(this);

            Console.WriteLine(DateTime.Now + ": " + ID + " sent " + message.Type + " to " + message.FindNext(this).ID);
        }

        /// <summary>
        /// Отправляет сообщение указанного типа указанному средству связи.
        /// </summary>
        /// 
        /// <param name="messageType"> Тип сообщения.</param>
        /// <param name="receiver"> Получатель.</param>
        /// 
        /// <returns>
        /// true, если сообщение можно передать.<br/>
        /// false, если нет.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если хотя бы один аргумент null.
        /// </exception>
        public bool Send(MessageType messageType, Connector receiver)
        {
            var path = GetPath(receiver);

            if(path != null)
            {
                Send(new Message(messageType, path));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Функция, анализирующая сообщение.
        /// </summary>
        /// 
        /// <param name="message"> Анализируемое сообщение.</param>
        protected abstract void Analize(Message message);

        /// <summary>
        /// Функция, принимающая сообщение (сообщение вызывает эту функцию при достижении этого средства связи).
        /// </summary>
        /// 
        /// <param name="message"> Вызывающее сообщение.</param>
        public virtual void GetMessage(Message message)
        {
            if (!isAnalizing)
            {
                Analize(message);
            }
        }

        /// <summary>
        /// Проверяет, дойдет ли сообщение до получателя, если его отправить.
        /// </summary>
        /// 
        /// <param name="receiver"> Получатель.</param>
        /// 
        /// <returns>
        /// true, если дойдет, false, если нет.
        /// </returns>
        protected virtual bool CanAccess(Body receiver)
        {
            return (!Body.IsCrossing(this, receiver)) && (receiver.ConvertTo(this).Length < view.Length);
        }
        #endregion




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
        }
        */
    }
}
