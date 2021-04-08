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
    public class MainSpacecraft : Body
    {
        protected MainSpacecraftConnector connector;
        public MainSpacecraft(MainSpacecraftConnector connector, CoordinateSystem coordinateSystem) :
            base(connector.ID, coordinateSystem)
        {
            this.connector = new MainSpacecraftConnector(connector);
        }

        public MainSpacecraftConnector Connector
        {
            get { return connector; }
            set { connector = new MainSpacecraftConnector(value); }
        }
    }
}
