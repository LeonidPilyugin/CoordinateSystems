using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CoordinateSystems
{
    public static class IAUSOFA
    {
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
    }
}
