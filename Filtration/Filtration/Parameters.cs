using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtration
{
   public static class Parameters
   {
      public readonly static double a = 0; //нач. полож по x
      
      public readonly static double d = 1; // масса ракеты + топлива
      public readonly static double g = 9.8; 
      public readonly static double beta = 10; //расход топлива
      public readonly static double b = beta; // нач. полож по y
      public readonly static double sigma = 1; // нач. отклон ошибок
      public static Matrix x0 = new Matrix(new double[][] { new double[] { 0, 0, a * d, b * d - g, d - 1 / beta } });
      public static Matrix E = new Matrix(new double[][]
      {
         new double[] {1, 0, 0, 0, 0},
         new double[] {0, 1, 0, 0, 0},
         new double[] {0, 0, 1, 0, 0},
         new double[] {0, 0, 0, 1, 0},
         new double[] {0, 0, 0, 0, 1},
      });

      public static Matrix GetF(double tk1, double tk)
      {
         double T = tk1 - tk;
         var result = new Matrix();
         result.Mat = new double[][]
         {
            new double[] {1.0, 0.0, T, 0.0, a*T*T/2},
            new double[] {0, 1, 0, T, b*T*T/2},
            new double[] {0, 0, 1, 0, a*T},
            new double[] {0, 0, 0, 1, b*T},
            new double[] {0, 0, 0, 0, 1},
         };
         return result;
      }

      public static Matrix S0 = new Matrix(new double[][]
      {
         new double[] {1e-3, 0, 0, 0, 0},
         new double[] {0, 1e-3, 0, 0, 0},
         new double[] {0, 0, 1e-3, 0, 0},
         new double[] {0, 0, 0, 1e-3, 0},
         new double[] {0, 0, 0, 0, 1e-3},
      });
      public static Matrix R = new Matrix(new double[][]
      {
         new double[] {sigma*sigma},
      });
      public static Matrix H = new Matrix(new double[][]
      {
         new double[] {0, 1, 0, 0, 0},
      });
      public static Matrix vk = new Matrix(new double[][] { new double[] { 0, 0, 0, 0, 0 } });
   }
}
