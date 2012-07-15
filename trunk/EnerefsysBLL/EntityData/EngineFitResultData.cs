using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnerefsysDAL.Model;
using DataContext;

namespace EnerefsysBLL.EntityData
{
    public static class EngineFitResultData
    {
        public static int Insert(float b1, float b2, float b3, float b4, float b5, float b6, double temperature, string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                EngineFitResult engineFitResult = new EngineFitResult();
                engineFitResult.B1 = b1;
                engineFitResult.B2 = b2;
                engineFitResult.B3 = b3;
                engineFitResult.B4 = b4;
                engineFitResult.B5 = b5;
                engineFitResult.B6 = b6;
                engineFitResult.Temperature = temperature;
                engineFitResult.Type = type;

                // add entity object to the collection
                db.EngineFitResults.Add(engineFitResult);

                try
                {
                    // save changes to the database
                    return db.SaveChanges();
                }
                catch(Exception e)
                {
                    throw new Exception("Could not save changes");
                }

            }

        }

        //根据类型删除数据库中所有数据
        public static int DeleteByType(string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                // get record that is to be deleted
                IEnumerable<EngineFitResult> engineFitResults = from c in db.EngineFitResults
                                                              where c.Type==type
                                                                select c;
                foreach (EngineFitResult engineFitResult in engineFitResults)
                {
                    db.EngineFitResults.Remove(engineFitResult);
                }

                try
                {
                    return db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Could not save changes");
                }
            }
        }

        //根据温度输出EngineFitResult
        public static EngineFitResult GetEntityByTemperature(double temperature)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                // get record that is to be select
                EngineFitResult engineFitResult = (from c in db.EngineFitResults
                                                   where c.Temperature == temperature
                                                   select c).First();
                return engineFitResult;
            }
        }


        //根据温度和类型输出EngineFitResult
        public static EngineFitResult GetEntityByTemperature(double temperature,string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                // get record that is to be selected
                EngineFitResult engineFitResult = (from c in db.EngineFitResults
                                                   where c.Temperature == temperature && c.Type == type
                                                   select c).First();
                return engineFitResult;
            }
        }



        //根据温度和类型输出EngineFitResult
        public static int GetBoundTemperature(double temperature, bool bound)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                if (bound)
                {
                    // 得到大于指定温度的最低温度
                    double Temperature = (from c in db.EngineFitResults
                                          where c.Temperature > temperature
                                          select c.Temperature).Min();
                    return Convert.ToInt32(Temperature);
                }
                else
                {
                    // 得到小于指定温度的最高温度
                    double Temperature = (from c in db.EngineFitResults
                                          where c.Temperature < temperature
                                          select c.Temperature).Max();
                    return Convert.ToInt32(Temperature);
                }
               
            }
        }


    }
}
