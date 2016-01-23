using System;

namespace Duelyst.DeckConstructor.VectorCalcs
{
    public static class VectorMath
    {
        public static double VDotProductZero(double x, double y, int presision = 0)
        {
            return Math.Round(Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2)), presision);
        }
    }
}
