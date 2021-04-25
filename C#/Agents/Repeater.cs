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
    public class Repeater : Body
    {
        #region data
        public RepeaterConnector Connector
        {
            get; set;
        }
        #endregion

        #region constructor
        public Repeater(RepeaterConnector connector, CoordinateSystem coordinateSystem) :
            base(connector.ID, coordinateSystem)
        {
            Connector = new RepeaterConnector(connector);
        }
        #endregion
    }
}
