using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using Agents;

// Файл содержит класс GroundStationConnector.

namespace Connection
{
    /// <summary>
    /// Класс GroundStationConnector описывает средство связи, расположенное на поверхности Земли. Наследник класса <see cref="Connector"/>.
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
    /// 1) <see cref="GroundStationConnector(GroundStationConnector)"/><br/>
    /// 2) <see cref="GroundStationConnector(string, Basis, View, GroundStation)"/><br/>
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
    /// 2) <see cref="Connector.TurnTo(Body)"/><br/>
    /// 3) <see cref="Connector.GetPath(Connector)"/><br/>
    /// 4) <see cref="Connector.Send(Message)"/><br/>
    /// 5) <see cref="Connector.Send(MessageType, Connector)"/><br/>
    /// 6) <see cref="Connector.GetMessage(Message)"/><br/>
    /// 7) <see cref="Connector.CanAccess(Body)"/><br/>
    /// 8) <see cref="Body.IsInside(Vector)"/><br/>
    /// 9) <see cref="Body.IsCrossing(Vector, Vector)"/><br/>
    /// 10) <see cref="CoordinateSystem.ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 11) <see cref="CoordinateSystem.GetVectorFromRoot(Vector)"/><br/>
    /// 12) <see cref="CoordinateSystem.GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 13) <see cref="CoordinateSystem.GetVelocityFromRoot(Vector)"/><br/>
    /// 14) <see cref="CoordinateSystem.GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public class GroundStationConnector : Connector
    {
        #region data
        /// <summary>
        /// Средство связи.
        /// </summary>
        protected GroundStation carrier;
        #endregion

        #region constructors
        /// <summary>
        /// Задает полям переданные значения, но:<br/>
        /// velocity = new Vector(0.0, 0.0, 0.0)<br/>
        /// vector = new Vector(0.0, 0.0, 0.0)<br/>
        /// referenceSystem = carrier<br/>
        /// isAnalizing = false<br/>
        /// carrier = <see cref="SunSystem.Planets.FixedEarth"/><br/>
        /// </summary>
        /// 
        /// <param name="ID"> Идентификатор</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="view"> Область видимости.</param>
        /// <param name="groundStation"> Наземная станция.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public GroundStationConnector(string ID, Basis basis, View view, GroundStation groundStation) :
            base(ID, new Vector(0.0, 0.0, 0.0), basis, view)
        {
            Carrier = groundStation;
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
        public GroundStationConnector(GroundStationConnector connector) : base(connector)
        {
            carrier = connector.carrier;
            referenceSystem = carrier;
        }
        #endregion

        #region methods
        /// <inheritdoc/>
        protected override void Analize(Message message)
        {
            // что-то делает
        }
        #endregion

        #region properties
        /// <summary>
        /// Средство связи. Не должно быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public GroundStation Carrier
        {
            get
            {
                return carrier;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("Carrier mustn't be null");
                }

                carrier = value;
            }
        }
        #endregion


        /*public void Send(MessageType messageType, MainSpacecraftConnector spacecraft)
        {
            var path = GetPath(spacecraft);
            Send(new Message(messageType, path));
            if (path == null)
            {
                return SendingResult.CantSend;
            }
            else
            {
                Message message = new Message(messageType, path);
                Repeater receiver = (Repeater)message.FindNext(this);
                var cts = new CancellationTokenSource();
                receivers.Add(receiver, cts);
                var SendingResult = SendAsync(message, cts.Token).Result;
                if (SendingResult == SendingResult.ReceiverDontWork || SendingResult == SendingResult.ReceiverDontAnswer)
                {
                    receivers.Remove(receiver);
                    workingRepeaters.Remove(receiver);
                    return Send(messageType, spacecraft);
                }
                return SendingResult.Successfully;
            }
        }*/
    }
}
