using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Keplerian;
using Connection;
using CoordinateSystems;

// Файл содержит класс OrbitRepeater

namespace Agents
{
    /// <summary>
    /// Ретранслятор, находящийся на орбите. Наследник класса <see cref="Repeater"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Поля:<br/>
    /// 1) <see cref="keplerian"/><br/>
    /// 2) <see cref="Repeater.connector"/><br/>
    /// 3) <see cref="Body.bodies"/><br/>
    /// 4) <see cref="Body.id"/><br/>
    /// 5) <see cref="CoordinateSystem.vector"/><br/>
    /// 6) <see cref="CoordinateSystem.velocity"/><br/>
    /// 7) <see cref="CoordinateSystem.basis"/><br/>
    /// 8) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="OrbitRepeater(EllipticKeplerian, Basis, RepeaterConnector)"/><br/>
    /// <br/>
    /// Свойства:<br/>
    /// 1) <see cref="Repeater.Connector"/><br/>
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
    /// 16) <see cref="Keplerian"/><br/>
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
    /// 10) <see cref="UpdateParams(double)"/><br/>
    /// </remarks>
    public class OrbitRepeater : Repeater
    {
        #region data
        /// <summary>
        /// Keplerian, который хранит параметры орбиты.
        /// </summary>
        protected EllipticKeplerian keplerian;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор задает полям значения:<br/>
        /// velocity — keplerian.Velocity<br/>
        /// vector — keplerian.Vector<br/>
        /// referenceSystem — keplerian.CentralBody<br/>
        /// connector — connector<br/>
        /// basis — basis<br/>
        /// </summary>
        /// 
        /// <param name="keplerian">Keplerian, который хранит параметры орбиты. Не должен быть null.</param>
        /// <param name="basis"> Базис. Не должен быть null.</param>
        /// <param name="connector"> Средство связи. Не должно быть null.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public OrbitRepeater(EllipticKeplerian keplerian, Basis basis, RepeaterConnector connector) :
            base(connector, keplerian.Vector, basis, keplerian.Velocity, keplerian.CentralBody)
        {
            this.keplerian = keplerian;
        }
        #endregion

        #region properties
        /// <summary>
        /// Keplerian, который хранит параметры орбиты.
        /// </summary>
        public EllipticKeplerian Keplerian
        {
            get
            {
                return keplerian;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Обновляет поля vector и velocity значениями для указанной юлианской даты.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public void UpdateParams(double julianDate)
        {
            vector = keplerian.GetVector(julianDate);
            velocity = keplerian.GetVelocity(julianDate);
        }
        #endregion
    }
}
