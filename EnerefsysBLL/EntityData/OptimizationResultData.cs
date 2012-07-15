using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnerefsysDAL.Model;
using DataContext;

namespace EnerefsysDAL
{
    public static class OptimizationResultData
    {
        //得到所有的优化结果列表
        public static List<OptimizationResult> GetAll()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    return db.OptimizationResults.ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        //插入优化结果
        public static int Insert(string day, string time, double temperature, double load, string engineType,
            double engineValue, double engineLoadRatio, double enginePower, double flow, double systemMinPower,
            double freezePumpPower,double coolingPumpPower,double coolingPower)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    db.OptimizationResults.Add(new OptimizationResult
                    {
                        Day = day,
                        Time = time,
                        Temperature = temperature,
                        Load = load,
                        EngineType = engineType,
                        EngineValue = engineValue,
                        EngineLoadRatio = engineLoadRatio,
                        EnginePower = enginePower,
                        Flow = flow,
                        SystemMinPower = systemMinPower,
                        FreezePumpPower=freezePumpPower,
                        CoolingPumpPower=coolingPumpPower,
                        CoolingPower=coolingPower
                    });
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }


        

    }
}
