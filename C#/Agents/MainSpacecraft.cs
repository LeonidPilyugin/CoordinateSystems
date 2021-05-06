using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using System.Threading;
using Connection;

// Файл содержит класс MainSpacecraft

namespace Agents
{
    /// <summary>
    /// Класс MainSpacecraft описывает главный космический аппарат. Наследник класса <see cref="Body"/>.
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
    public class MainSpacecraft : Body
    {
        #region data
        /// <summary>
        /// Средство связи.
        /// </summary>
        protected MainSpacecraftConnector connector;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает полям переданные значения, но:<br/>
        /// id = connector.id<br/>
        /// </summary>
        /// 
        /// <param name="connector"> Средство связи. Не должно быть null.</param>
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат.</param>
        /// <param name="referenceSystem"> Базовая система координат. Если она null, то базовая система координат — гелиоцентрическая.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public MainSpacecraft(MainSpacecraftConnector connector, Vector vector, Basis basis, Vector velocity,
            CoordinateSystem referenceSystem = null) : base(connector.ID, vector, basis, velocity, referenceSystem)
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
        public MainSpacecraftConnector Connector
        {
            get
            {
                return connector;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("Connector mustn't be null");
                }

                connector = value;
            }
        }
        #endregion
    }
}
