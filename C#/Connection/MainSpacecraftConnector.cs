using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;

namespace Connection
{
    public class MainSpacecraftConnector : Connector
    {
        #region constructors
        public MainSpacecraftConnector(string ID, CoordinateSystem coordinateSystem, View view, Body carrier) :
            base(ID, coordinateSystem, view, carrier)
        {

        }

        public MainSpacecraftConnector(Connector connector) : base(connector)
        {

        }

        public MainSpacecraftConnector() : base()
        {

        }
        #endregion

        /*public void Send(MessageType messageType, GroundStationConnector station)
        {
            var path = GetPath(station);
            Send(new Message(messageType, path));
        }

        public async Task<SendingResult> Send(MessageType messageType, GroundStation station)
        {
            var path = GetPath(station);
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
                    return await Send(messageType, station);
                }
                return SendingResult.Successfully;
            }
        }*/

        #region functions
        protected override void Analize(Message message)
        {
            isAnalizing = true;
            Connector sender = message.FindPrevious(this);
            Console.WriteLine(DateTime.Now + ": " + ID + " got " + message.Data + " from " + sender.ID);

            /*if (message.Data == MessageType.Got)
            {
                RemoveReceiverFromReceivers(sender);
            }*/
            isAnalizing = false;
        }

        protected override bool CanAccess(Body receiver)
        {
            return base.CanAccess(receiver) || (!Message.IsCrossing(carrier, receiver) && receiver.ConvertTo(this).Length < view.Length);
        }

        protected override void TurnTo(Body point)
        {
            if (base.CanAccess(point))
            {
                base.TurnTo(point);
            }
            else
            {
                carrier.TurnTo(point.ConvertTo(this));
                base.TurnTo(point);
            }
        }
        #endregion
    }
}
