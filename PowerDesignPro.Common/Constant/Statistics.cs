using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{
    public static class Statistics
    {
        public static double lRegression(decimal[] firstSeries, double[] secondSeries, double convert)
        {
            try
            {
                decimal slopeV = (decimal)slope(firstSeries, secondSeries);
                decimal interceptV = (decimal)intercept(firstSeries, secondSeries);

                return (double)(interceptV + slopeV * (decimal)convert);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private static double slope(decimal[] seriesX, double[] seriesY)
        {
            int N = seriesX.Length;
            double EXY = 0, EX = 0, EY = 0, EX2 = 0;

            for (int i = 0; i < N; i++)
            {
                EXY += ((double)seriesX[i] * seriesY[i]);
                EX += (double)(seriesX[i]);
                //EY += (double)(seriesY[i]);
                EY += seriesY[i];
                EX2 += (double)(seriesX[i] * seriesX[i]);
            }
            var numerator = (N * EXY) - (EX * EY);
            var denominator = (N * EX2) - (EX * EX);
            var rValue = numerator / denominator;

            //return (double)rValue;
            return rValue;
        }

        private static double intercept(decimal[] firstSeries, double[] secondSeries)
        {
            int N = firstSeries.Length;
            double EX = 0, EY = 0;
            var b = slope(firstSeries, secondSeries);

            for (int i = 0; i < N; i++)
            {
                EX += (double)firstSeries[i];
                //EY += (double)secondSeries[i];
                EY += secondSeries[i];
            }

            var numerator = (EY - (b) * EX);
            var rValue = numerator / N;

            return rValue;
        }
    }
}
