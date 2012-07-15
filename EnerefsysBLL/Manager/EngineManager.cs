using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using EnerefsysDAL;
using EnerefsysDAL.Model;
using EnerefsysBLL.Entity;
using EnerefsysBLL.EntityData;
using EnerefsysBLL.Utility;

namespace EnerefsysBLL.Manager
{
    public class EngineManager
    {
        //在主机的数值模拟系数表中插入数据
        public static int Insert(object b1, object b2, object b3, object b4, object b5, object b6, object temperature,object type)
        {
            return EngineFitResultData.Insert(ObjectToFloat(b1), ObjectToFloat(b2), ObjectToFloat(b3), ObjectToFloat(b4), ObjectToFloat(b5), ObjectToFloat(b6), Convert.ToDouble(temperature), type.ToString());
        }
        private static float ObjectToFloat(object objValue)
        {
            float fValue = 0.0f;
            fValue =float.Parse(objValue.ToString());
            return fValue;
        }
        //根据类型删除数据库中所有数据
        public static int DeleteByType(object type)
        {
            return EngineFitResultData.DeleteByType(type.ToString());
        }

        //根据温度得到数据库中的各种系数，并根据负荷求得最优解
        public static SoluteResult GetReslutByTem(object temperature,int load)
        {
            EngineFitResult engineFitResult = EngineFitResultData.GetEntityByTemperature(Convert.ToDouble(temperature));
            SoluteResult sr = GetFlowMinW(engineFitResult,load);
            return sr;
        }


        //根据类型和所分配的负荷得到关于流量的二次项系数a,b,c
        public static List<double> GetParamsByType(double temperature, double load,string type)
        {
           
            EngineFitResult engineFitResult = EngineFitResultData.GetEntityByTemperature(Convert.ToDouble(temperature), type);
            double a = engineFitResult.B4;
            double b = engineFitResult.B5 + engineFitResult.B6 * load;
            double c = engineFitResult.B1 + engineFitResult.B2 * load * load + engineFitResult.B3 * load;

            List<double> result = new List<double>();
            result.Add(a);
            result.Add(b);
            result.Add(c);
            return result;
        }


        //根据温度的加权平均计算出最优解
        public static SoluteResult GetReslutByTemByWeight(object temperature, int load)
        {
          
            EngineFitResult engineFitResult=EngineFitResultData.GetEntityByTemperature(Convert.ToDouble(temperature));

            if (engineFitResult != null)
            {
                SoluteResult sr = GetFlowMinW(engineFitResult,load);
                return sr;
            }
             //如果不存在对应的温度，使用加权平均，
            else
            {
                int Min_Temp = EngineFitResultData.GetBoundTemperature(Convert.ToDouble(temperature), true);
                int Max_Temp = EngineFitResultData.GetBoundTemperature(Convert.ToDouble(temperature), false);

                SoluteResult sr1 = GetReslutByTem(Min_Temp, load);
                SoluteResult sr2 = GetReslutByTem(Max_Temp, load);

                double result = (sr2.Result * (Convert.ToDouble(temperature) - Convert.ToDouble(Min_Temp)) + sr1.Result * (Convert.ToDouble(Max_Temp) - Convert.ToDouble(temperature))) / (Convert.ToDouble(Max_Temp) - Convert.ToDouble(Min_Temp));
                double solute = (sr2.Solute * (Convert.ToDouble(temperature) - Convert.ToDouble(Min_Temp)) + sr1.Solute * (Convert.ToDouble(Max_Temp) - Convert.ToDouble(temperature))) / (Convert.ToDouble(Max_Temp) - Convert.ToDouble(Min_Temp));
                return new SoluteResult(result, solute);
            }

            
        }


        //特定温度下给出负荷（L），求出是最小效率(W)的流量(F)
        public static SoluteResult GetFlowMinW(int load,double b1,double b2,double b3,double b4,double b5,double b6)
        {
            //得到一元二次函数的三个系数
            double a = b4;
            double b = b5 + b6 * load;
            double c = b1 + b2 * load * load + b3 * load;

            //求得一元二次函数的最小值
            double solute = Utility.Utility.GetMinSolute(a, b, c);
            double result = b1 + b2 * load * load + b3 * load + b4 * solute * solute + b5 * solute + b6 * load * solute;
            SoluteResult sr = new SoluteResult(result, solute);
            return sr;
        }

        //特定温度下给出负荷（L），求出是最小效率(W)的流量(F)
        public static SoluteResult GetFlowMinW(EngineFitResult engineFitResult,int load)
        {
            //得到一元二次函数的三个系数
            double a = engineFitResult.B4;
            double b = engineFitResult.B5 + engineFitResult.B6 * load;
            double c = engineFitResult.B1 + engineFitResult.B2 * load * load + engineFitResult.B3 * load;

            //求得一元二次函数的最小值
            double solute = Utility.Utility.GetMinSolute(a, b, c);
            double result = engineFitResult.B1 + engineFitResult.B2 * load * load + engineFitResult.B3 * load + engineFitResult.B4 * solute * solute + engineFitResult.B5 * solute + engineFitResult.B6 * load * solute;
            SoluteResult sr = new SoluteResult(result, solute);
            return sr;
        }






    }
}