﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using SunSystem;

namespace Connection
{
    public class GroundStationConnector : Connector
    {
        #region constructors
        public GroundStationConnector(string ID, CoordinateSystem coordinateSystem, View view, Body carrier) :
            base(ID, coordinateSystem, view, carrier)
        {

        }

        public GroundStationConnector(Connector connector) : base(connector)
        {

        }

        public GroundStationConnector() : base()
        {

        }
        #endregion

        #region functions
        protected override void Analize(Message message)
        {
            isAnalizing = true;
            var sender = message.FindPrevious(this);
            Console.WriteLine(DateTime.Now + ": " + ID + " got " + message.Data + " from " + sender.ID);

            /*if (message.Data == MessageType.Got)
            {
                RemoveReceiverFromReceivers(sender);
            }*/
            isAnalizing = false;
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
