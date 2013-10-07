using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Elide.Forms
{
    public static class Dpi
    {
        private static double factorX = .0d;
        private static double factorY = .0d;

        public static double GetFactorX()
        {
            if (factorX < 1.0d)
            {
                using (var c = new Control())
                using (var g = c.CreateGraphics())
                    factorX = g.DpiX / 96d;
            }

            return factorX;
        }
        
        public static double GetFactorY()
        {
            if (factorY < 1.0d)
            {
                using (var c = new Control())
                using (var g = c.CreateGraphics())
                    factorY = g.DpiY / 96d;
            }

            return factorY;
        }

        public static int ScaleY(int num)
        {
            return (Int32)(num * GetFactorY());
        }

        public static int ScaleX(int num)
        {
            return (Int32)(num * GetFactorX());
        }
    }
}
