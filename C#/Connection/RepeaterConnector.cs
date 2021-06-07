using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using Agents;

// Файл содержит класс RepeaterConnector.

namespace Connection
{
    /// <summary>
    /// Класс RepeaterConnector описывает средство связи, закрепленное на ретрансляторе.
    /// Наследник класса <see cref="Connector"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="Connector.isAnalizing"/><br/>
    /// 2) <see cref="carrier"/><br/>
    /// 3) <see cref="Connector.view"/><br/>
    /// 4) <see cref="Connector.workingConnectors"/><br/>
    /// 5) <see cref="Body.id"/><br/>
    /// 6) <see cref="Body.bodies"/><br/>
    /// 7) <see cref="CoordinateSystem.vector"/><br/>
    /// 8) <see cref="CoordinateSystem.velocity"/><br/>
    /// 9) <see cref="CoordinateSystem.basis"/><br/>
    /// 10) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="RepeaterConnector(RepeaterConnector)"/><br/>
    /// 2) <see cref="RepeaterConnector(string, Vector, Basis, View, Repeater)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Connector.View"/><br/>
    /// 2) <see cref="Connector.WorkingConnectors"/><br/>
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
    /// 17) <see cref="Carrier"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="Analize(Message)"/><br/>
    /// 2) <see cref="TurnTo(Body)"/><br/>
    /// 3) <see cref="CanAccess(Body)"/><br/>
    /// 4) <see cref="Resend(Message)"/><br/>
    /// 5) <see cref="Connector.GetPath(Connector)"/><br/>
    /// 6) <see cref="Connector.Send(Message)"/><br/>
    /// 7) <see cref="Connector.Send(MessageType, Connector)"/><br/>
    /// 8) <see cref="Connector.GetMessage(Message)"/><br/>
    /// 9) <see cref="Body.IsInside(Vector)"/><br/>
    /// 10) <see cref="Body.IsCrossing(Vector, Vector)"/><br/>
    /// 11) <see cref="CoordinateSystem.ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 12) <see cref="CoordinateSystem.GetVectorFromRoot(Vector)"/><br/>
    /// 13) <see cref="CoordinateSystem.GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 14) <see cref="CoordinateSystem.GetVelocityFromRoot(Vector)"/><br/>
    /// 15) <see cref="CoordinateSystem.GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public class RepeaterConnector : Connector
    {
        #region data
        /// <summary>
        /// Средство связи.
        /// </summary>
        protected Repeater carrier;
        #endregion

        #region constructors
        /// <summary>
        /// Задает полям переданные значения, но:<br/>
        /// velocity = new Vector(0.0, 0.0, 0.0)<br/>
        /// referenceSystem = carrier<br/>
        /// isAnalizing = false<br/>
        /// </summary>
        /// 
        /// <param name="ID"> Идентификатор</param>
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="view"> Область видимости.</param>
        /// <param name="carrier"> Аппарат, на котором закреплено средство связи.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public RepeaterConnector(string ID, Vector vector, Basis basis, View view, Repeater carrier) :
            base(ID, vector, basis, view)
        {
            Carrier = carrier;
            referenceSystem = Carrier;
        }

        /// <summary>
        /// Конструктор копирования.
        /// </summary>
        /// 
        /// <param name="connector"> Копируемое средство связи. Не должно быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public RepeaterConnector(RepeaterConnector connector) : base(connector)
        {
            Carrier = carrier;
            referenceSystem = Carrier;
        }
        #endregion

        #region methods
        /// <inheritdoc/>
        protected override void Analize(Message message)
        {
            // что-то делает
        }

        /// <inheritdoc/>
        public override bool CanAccess(Body receiver)
        {
            return base.CanAccess(receiver) || (!Body.IsCrossing(carrier, receiver) && receiver.ConvertTo(this).Length < view.Length);
        }

        /// <inheritdoc/>
        protected override void TurnTo(Body point)
        {
            if(base.CanAccess(point))
            {
                base.TurnTo(point);
            }
            else
            {
                carrier.TurnTo(point.ConvertTo(this));
                base.TurnTo(point);
            }
        }

        /*/// <summary>
        /// Пересылает сообщение следующему в списке аппарату.
        /// </summary>
        /// 
        /// <param name="message"> Пересылаемое сообщение.</param>
        protected void Resend(Message message)
        {
            /*var receiver = (Repeater)message.FindNext(this);
            if (message.FindNext(this) != null)
            {
            var cts = new CancellationTokenSource();
            receivers.Add(receiver, cts);
            var SendingResult = SendAsync(message, cts.Token).Result;
            if (SendingResult == SendingResult.ReceiverDontWork || SendingResult == SendingResult.ReceiverDontAnswer)
            {
                receivers.Remove(receiver);
                workingRepeaters.Remove(receiver);
                SendAnotherWay(message);
            }
            }
            else
            {
            Send(message);
            }
            if(CanAccess(message.FindNext(this)))
            {
                Send(message);
            }
            else
            {
                // Получить новый путь
                LinkedList<Connector> newPath = null;
                while(newPath == null)
                {
                    newPath = GetPath(message.Last);
                }

                // Добавить к нему начало этого пути.
                for(LinkedListNode<Connector> temp = message.Path.Find(this);
                    temp != message.Path.First; temp = temp.Previous)
                {
                    newPath.AddFirst(temp.Value);
                }

                Send(new Message(message.Type, newPath));
            }
        }*/
        #endregion

        #region properties
        /// <summary>
        /// Средство связи. Не должно быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public Repeater Carrier
        {
            get
            {
                return carrier;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Carrier mustn't be null");
                }

                carrier = value;
            }
        }
        #endregion

        /*protected void SendAnotherWay(Message message)
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
            Resend(newMessage);
        }*/
    }
}
