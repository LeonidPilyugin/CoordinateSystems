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
        #region data
        public MainSpacecraftConnector Connector
        {
            get; set;
        }
        #endregion

        #region constructors
        public MainSpacecraft(MainSpacecraftConnector connector, CoordinateSystem coordinateSystem) :
            base(connector.ID, coordinateSystem)
        {
            Connector = new MainSpacecraftConnector(connector);
        }
        #endregion
    }
}
