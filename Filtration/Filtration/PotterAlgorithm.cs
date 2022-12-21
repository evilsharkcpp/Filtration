﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtration
{
   internal class PotterAlgorithm
   {
      Matrix x_kk;
      Matrix s_kk;
      Matrix p_kk;
      Matrix x_k1k1;
      Matrix s_k1k1;
      Matrix p_k1k1;
      Matrix x_k1k;
      Matrix s_k1k;
      Matrix p_k1k;
      Matrix yk1;
      void Extrapolation(double tk1, double tk)
      {
         var F_k1k = Parameters.GetF(tk1, tk);
         x_k1k = F_k1k * x_kk;
         s_k1k = F_k1k * s_kk;
         p_k1k = s_k1k * s_k1k.Transpose();
      }
      void Filtration(double tk1, double tk)
      {
         var Fk1 = s_k1k.Transpose() * Parameters.H.Transpose();
         double a = (Fk1.Transpose()*Fk1 + Parameters.R).Mat[0][0];
         a = 1.0 / a;
         var tmp = Parameters.R * a;
         tmp.Mat[0][0] = Math.Sqrt(tmp.Mat[0][0]);
         Matrix gamma = 1 + tmp;
         gamma.Mat[0][0] = 1.0 / gamma.Mat[0][0];
         var K = a * s_k1k * Fk1;
         x_k1k1 = x_k1k + K * ((Parameters.H * x_kk + Parameters.vk) - Parameters.H * x_k1k);
         s_k1k1 = s_k1k * (Parameters.E - a * gamma.Mat[0][0] * Fk1 * Fk1.Transpose());
         p_k1k1 = s_k1k1 * s_k1k1.Transpose();
      }
      double Norm(Matrix x)
      {
         double sum = 0;
         for (int i = 0; i < x[0].Length; i++)
            sum += x[0][i] * x[0][i];
         return Math.Sqrt(sum);
      }
      public void Iteration()
      {
         p_kk = Parameters.S0 * Parameters.S0.Transpose();
         x_kk = Parameters.x0.Transpose();
         s_kk = Parameters.S0;
         List<double> results = new List<double>();
         Matrix x_tr = Parameters.x0.Transpose();
         Matrix y_tr;
         Matrix y;
         for (int i = 1; i < 3;i++)
         {
            Extrapolation(i, i - 1);
            Filtration(i, i - 1);
            x_tr = Parameters.GetF(i, i - 1) * x_tr;
            y_tr = Parameters.H * x_tr;
            y = Parameters.H * x_k1k1 + Parameters.vk;
            var a = y_tr - y;
            var b = Norm(y_tr);
            results.Add(Norm(a) / b);
            p_kk = p_k1k1;
            x_kk = x_k1k1;
            s_kk = s_k1k1;
         }
      }
   }
}