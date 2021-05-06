using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using System.Threading;
using Connection;
using SunSystem;

// Файл содержит класс GroundStation

namespace Agents
{
    /// <summary>
    /// Класс GroundStation описывает наземную станцию. Наследник класса <see cref="Body"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="connector"/><br/>
    /// 2) <see cref="Body.bodies"/><br/>
    /// 3) <see cref="Body.id"/><br/>
    /// 4) <see cref="CoordinateSystem.vector"/><br/>
    /// 5) <see cref="CoordinateSystem.velocity"/><br/>
    /// 6) <see cref="CoordinateSystem.basis"/><br/>
    /// 7) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="GroundStation(double, double, GroundStationConnector)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Connector"/><br/>
    /// <br/>
    /// Методы:<br/>
    /// 1) <see cref="Body.IsInside(Vector)"/><br/>
    /// 2) <see cref="Body.IsCrossing(Vector, Vector)"/><br/>
    /// 3) <see cref="Body.IsCrossing(CoordinateSystem, CoordinateSystem, Vector, Vector)"/><br/>
    /// 4) <see cref="CoordinateSystem.ConvertTo(CoordinateSystem, Vector)"/><br/>
    /// 5) <see cref="CoordinateSystem.TurnTo(Vector, List&lt;Vector&gt;)"/><br/>
    /// 6) <see cref="CoordinateSystem.GetVectorFromRoot(Vector)"/><br/>
    /// 7) <see cref="CoordinateSystem.GetVectorRelativelyReferenceSystem(Vector)"/><br/>
    /// 8) <see cref="CoordinateSystem.GetVelocityFromRoot(Vector)"/><br/>
    /// 9) <see cref="CoordinateSystem.GetVelocityRelativelyReferenceSystem(Vector)"/><br/>
    /// </remarks>
    public class GroundStation : Body
    {
        #region data
        /// <summary>
        /// Средство связи.
        /// </summary>
        protected GroundStationConnector connector;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает полям переданные значения, но:<br/>
        /// id = connector.id<br/>
        /// vector = Coordinates.GetGCStoTCSvector(latitude, longitude)<br/>
        /// bais = new Basis()<br/>
        /// velocity = new Vector(0.0, 0.0, 0.0)<br/>
        /// centralBody = <see cref="SunSystem.Planets.FixedEarth"/><br/>
        /// </summary>
        /// 
        /// <param name="latitude"> Широта. Измеряется в радианах. -<see cref="Math.PI"/> / 2 &lt;= latitude &lt;= <see cref="Math.PI"/> / 2.</param>
        /// <param name="longitude">  Долгота. Измеряется в радианах. -<see cref="Math.PI"/> &lt;= latitude &lt;= <see cref="Math.PI"/>.</param>
        /// <param name="connector"> Средство связи. Не должно быть null.</param>
        /// 
        /// <exception cref="ArgumentException">
        /// Вызывается, если передается широта, по модулю большая <see cref="Math.PI"/> / 2 или
        /// долгота, по модулю большая <see cref="Math.PI"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public GroundStation(double latitude, double longitude, GroundStationConnector connector) :
            base(connector.ID, Coordinates.GetGCStoTCSvector(latitude, longitude), new Basis(),
                new Vector(0.0, 0.0, 0.0), SunSystem.Planets.FixedEarth)
        {
            Connector = connector;
        }
        #endregion

        #region properties
        /// <summary>
        /// Средство связи. Не должно быть null.
        /// </summary>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается, если передается null.
        /// </exception>
        public GroundStationConnector Connector
        {
            get
            {
                return connector;
            }

            set
            {
                if(value == null)
                {
                    throw new ArgumentNullException("Connector mustn't be null");
                }

                connector = value;
            }
        }
        #endregion
    }
}
