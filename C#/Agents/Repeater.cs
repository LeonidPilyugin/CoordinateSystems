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
        protected RepeaterConnector connector;
        public Repeater(RepeaterConnector connector, CoordinateSystem coordinateSystem) :
            base(connector.ID, coordinateSystem)
        {
            this.connector = new RepeaterConnector(connector);
        }

        public RepeaterConnector Connector
        {
            get { return connector; }
            set { connector = new RepeaterConnector(value); }
        }
    }
}
