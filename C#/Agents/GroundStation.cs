using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using System.Threading;
using Connection;

namespace Agents
{
    public class GroundStation : Body
    {
        protected GroundStationConnector connector;
        public GroundStation(GroundStationConnector connector, CoordinateSystem coordinateSystem) :
            base(connector.ID, coordinateSystem)
        {
            this.connector = new GroundStationConnector(connector);
        }

        public GroundStationConnector Connector
        {
            get { return connector; }
            set { connector = new GroundStationConnector(value); }
        }
    }
}
