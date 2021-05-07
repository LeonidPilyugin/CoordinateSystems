﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoordinateSystems;
using Connection;
using SunSystem;

// Файл содержит класс LagrangianRepeater

namespace Agents
{
    /// <summary>
    /// Ретранслятор, находящийся на орбите. Наследник класса <see cref="Repeater"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// Типы:<br/>
    /// 1) <see cref="GetParams"/><br/>
    /// 2) <see cref="LagrangianPoint"/><br/>
    /// <br/>
    /// Поля:<br/>
    /// 1) <see cref="getParamsMethod"/><br/>
    /// 2) <see cref="Repeater.connector"/><br/>
    /// 3) <see cref="Body.bodies"/><br/>
    /// 4) <see cref="Body.id"/><br/>
    /// 5) <see cref="CoordinateSystem.vector"/><br/>
    /// 6) <see cref="CoordinateSystem.velocity"/><br/>
    /// 7) <see cref="CoordinateSystem.basis"/><br/>
    /// 8) <see cref="CoordinateSystem.referenceSystem"/><br/>
    /// <br/>
    /// Конструкторы:<br/>
    /// 1) <see cref="LagrangianRepeater(GetParams, RepeaterConnector, Basis, Vector, Vector, Body)"/><br/>
    /// 2) <see cref="LagrangianRepeater(LagrangianPoint, RepeaterConnector, Basis, Vector, Vector, Body)"/><br/>
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
    public class LagrangianRepeater : Repeater
    {
        #region types
        /// <summary>
        /// Делегат описывает функцию получения координат ретранслятора.
        /// </summary>
        /// 
        /// <param name="julianDate"> Юлианская дата.</param>
        public delegate (Vector Vector, Vector Velocity) GetParams(double julianDate);

        /// <summary>
        /// Точки Лагранжа.
        /// </summary>
        public enum LagrangianPoint
        {
            /// <summary> 1 точка Лагранжа системы Солнце-Земля.</summary>
            SE1,
            /// <summary> 2 точка Лагранжа системы Солнце-Земля.</summary>
            SE2,
            /// <summary> 3 точка Лагранжа системы Солнце-Земля.</summary>
            SE3,
            /// <summary> 4 точка Лагранжа системы Солнце-Земля.</summary>
            SE4,
            /// <summary> 5 точка Лагранжа системы Солнце-Земля.</summary>
            SE5,
            /// <summary> 1 точка Лагранжа системы Земля-Луна.</summary>
            EM1,
            /// <summary> 2 точка Лагранжа системы Земля-Луна.</summary>
            EM2,
            /// <summary> 3 точка Лагранжа системы Земля-Луна.</summary>
            EM3,
            /// <summary> 4 точка Лагранжа системы Земля-Луна.</summary>
            EM4,
            /// <summary> 5 точка Лагранжа системы Земля-Луна.</summary>
            EM5
        }
        #endregion

        #region data
        /// <summary>
        /// Функция, описывающая положение ретранслятора.
        /// </summary>
        protected GetParams getParamsMethod;
        #endregion

        #region constructors
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// 
        /// <param name="function"> Функция, определяющая координаты и скорость точки.</param>
        /// <param name="connector"> Средство связи.</param>
        /// <param name="basis"> Базис.</param>
        /// <param name="vector"> Координаты.</param>
        /// <param name="velocity"> Скорость.</param>
        /// <param name="centralBody"> Тело, в системе отсчета которого заданы координаты.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public LagrangianRepeater(GetParams function, RepeaterConnector connector, Basis basis,
            Vector vector, Vector velocity, Body centralBody) : base(connector, vector,
                basis, velocity, centralBody)
        {
            if(function == null)
            {
                throw new ArgumentNullException("function mustn't be null");
            }
            getParamsMethod += function;
        }

        /// <summary>
        /// Конструктор сам задает функцию по указанной точке Лагранжа.
        /// </summary>
        /// 
        /// <param name="lagrangianPoint"> Точка Лагранжа.</param>
        /// <param name="connector"> Средство связи.</param>
        /// <param name="basis"> Базис.</param>
        /// <param name="vector"> Координаты.</param>
        /// <param name="velocity"> Скорость.</param>
        /// <param name="centralBody"> Тело, в системе отсчета которого заданы координаты.</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// Вызывается при передаче null.
        /// </exception>
        public LagrangianRepeater(LagrangianPoint lagrangianPoint, RepeaterConnector connector, Basis basis,
            Vector vector, Vector velocity, Body centralBody) : base(connector, vector,
                basis, velocity, centralBody)
        {
            switch(lagrangianPoint)
            {
                case LagrangianPoint.EM1: getParamsMethod += LagrangianPoints.GetEarthMoon1; break;
                case LagrangianPoint.EM2: getParamsMethod += LagrangianPoints.GetEarthMoon2; break;
                case LagrangianPoint.EM3: getParamsMethod += LagrangianPoints.GetEarthMoon3; break;
                case LagrangianPoint.EM4: getParamsMethod += LagrangianPoints.GetEarthMoon4; break;
                case LagrangianPoint.EM5: getParamsMethod += LagrangianPoints.GetEarthMoon5; break;

                case LagrangianPoint.SE1: getParamsMethod += LagrangianPoints.GetSunEarth1; break;
                case LagrangianPoint.SE2: getParamsMethod += LagrangianPoints.GetSunEarth2; break;
                case LagrangianPoint.SE3: getParamsMethod += LagrangianPoints.GetSunEarth3; break;
                case LagrangianPoint.SE4: getParamsMethod += LagrangianPoints.GetSunEarth4; break;
                case LagrangianPoint.SE5: getParamsMethod += LagrangianPoints.GetSunEarth5; break;
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
            (vector, velocity) = getParamsMethod(julianDate);
        }
        #endregion
    }
}
