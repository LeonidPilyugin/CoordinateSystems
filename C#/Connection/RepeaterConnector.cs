using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;

namespace Connection
{
    public class RepeaterConnector : Connector
    {
        #region constructors
        public RepeaterConnector(string ID, CoordinateSystem coordinateSystem, double maxDistance, Body carrier) :
            base(ID, coordinateSystem, maxDistance, carrier)
        {

        }

        public RepeaterConnector(Connector connector) : base(connector)
        {

        }

        public RepeaterConnector() : base()
        {

        }
        #endregion

        #region functions
        protected override void Analize(Message message)
        {
            isAnalizing = true;

            Connector sender = message.FindPrevious(this);
            Connector receiver = message.FindNext(this);
            Console.WriteLine(DateTime.Now + ": " + ID + " got " + message.Data + " from " + sender.ID);

            /*if (message.Data == MessageType.Got)
            {
                // Remove receiver from receivers
                RemoveReceiverFromReceivers((Repeater)sender);
            }
            else
            {
                Send(new Message(MessageType.Got, this, sender));
            }*/

            if (receiver != null)
            {
                Resend(message);
            }
            isAnalizing = false;
        }

        protected override bool CanAccess(Body receiver)
        {
            return base.CanAccess(receiver) || (!Message.IsCrossing(carrier, receiver) && receiver.ConvertTo(this).Length < MaxDistance);
        }

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
            }*/

            Send(message);
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
