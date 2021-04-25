using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using System.Threading;
using Connection;
using SunSystem;

namespace Agents
{
    public class GroundStation : Body
    {
        #region data
        public GroundStationConnector Connector
        {
            get; set;
        }
        #endregion

        #region constructors
        public GroundStation(GroundStationConnector connector) :
            base(connector.ID, SunSystem.Planets.FixedEarth)
        {
            Connector = new GroundStationConnector(connector);
        }
        #endregion
    }
}
