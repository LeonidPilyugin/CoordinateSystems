using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using System.Threading;
using Connection;

// Файл содержит класс Repeater

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
    /// 1) <see cref="Repeater(RepeaterConnector, Vector, Basis, Vector, CoordinateSystem)"/><br/>
    /// 2) <see cref="Repeater(Vector, Basis, Vector, CoordinateSystem)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Connector"/><br/>
    /// 2) <see cref="Body.Bodies"/><br/>
    /// 3) <see cref="Body.ID"/><br/>
    /// 4) <see cref="CoordinateSystem.Vector"/><br/>
    /// 5) <see cref="CoordinateSystem.Basis"/><br/>
    /// 6) <see cref="CoordinateSystem.ReferenceSystem"/><br/>
    /// 7) <see cref="CoordinateSystem.Velocity"/><br/>
    /// 8) <see cref="CoordinateSystem.TransitionMatrix"/><br/>
    /// 9) <see cref="CoordinateSystem.TransitionMatrixRelativelyRoot"/><br/>
    /// 10) <see cref="CoordinateSystem.BasisRelativelyReferenceSystem"/><br/>
    /// 11) <see cref="CoordinateSystem.BasisRelativelyRoot"/><br/>
    /// 12) <see cref="CoordinateSystem.RootBasis"/><br/>
    /// 13) <see cref="CoordinateSystem.ReferenceSystemBasis"/><br/>
    /// 14) <see cref="CoordinateSystem.VectorFromRoot"/><br/>
    /// 15) <see cref="CoordinateSystem.VelocityFromRoot"/><br/>
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
    public abstract class Repeater : Body
    {
        #region data
        /// <summary>
        /// Средство связи.
        /// </summary>
        protected RepeaterConnector connector;
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
        public Repeater(RepeaterConnector connector, Vector vector, Basis basis, Vector velocity,
            CoordinateSystem referenceSystem = null) : base(connector.ID + " carrier", vector,
                basis, velocity, referenceSystem)
        {
            Connector = connector;
            connector.Carrier = this;
            Body.bodies.Add(this);
            Body.bodies.Add(connector);
        }

        /// <summary>
        /// Конструктор задает полям переданные значения, но средство связи не задается.
        /// </summary>
        /// 
        /// <param name="vector"> Вектор относительно базовой системы координат.</param>
        /// <param name="basis"> Базис относительно базовой системы координат.</param>
        /// <param name="velocity"> Скорость относительно базовой системы координат.</param>
        /// <param name="referenceSystem"> Базовая система координат. Если она null, то базовая система координат — гелиоцентрическая.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        protected Repeater(Vector vector, Basis basis, Vector velocity,
            CoordinateSystem referenceSystem = null) : base("new repeater", vector,
                basis, velocity, referenceSystem)
        {

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
        public RepeaterConnector Connector
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
