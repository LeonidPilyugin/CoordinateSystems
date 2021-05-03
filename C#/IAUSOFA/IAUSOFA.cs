using System;
using System.Runtime.InteropServices;

// Файл содержит класс IAUSOFA

namespace IAUSOFA
{
    /// <summary>
    /// Статический класс IAUSOFA содержит некоторые функции и константы этой библиотеки.<br/>
    /// Библиотека: https://www.iausofa.org/ <br/>
    /// Описания функций и их исходный код можно найти там.
    /// </summary>
    public static class IAUSOFA
    {
        #region consts
        /// <summary>
        /// Радиан в одном градусе
        /// </summary>
        public const double DD2R = 1.745329251994329576923691e-2;

        /// <summary>
        /// Метров в астрономической единице
        /// </summary>
        public const double DAU = 149597870.7e3;

        /// <summary>
        /// Радиан в угловой секунде
        /// </summary>
        public const double DAS2R = 4.848136811095359935899141e-6;
        #endregion

        #region functions
        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void iauPmat06(double date1, double date2, [In, Out] double[,] matrix);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double iauGst06a(double uta, double utb, double tta, double ttb);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double iauObl06(double date1, double date2);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void iauNut06a(double date1, double date2, ref double dpsi, ref double deps);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void iauNumat(double epsa, double dpsi, double deps, [In, Out] double[,] matrix);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int iauEpv00(double date1, double daye2,
            [In, Out] double[,] pvh, [In, Out] double[,] pvb);

        [DllImport("iausofa.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int iauPlan94(double date1, double date2, int np, [In, Out] double[,] pv);
        #endregion
    }
}
