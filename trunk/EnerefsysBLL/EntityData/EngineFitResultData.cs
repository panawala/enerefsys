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

        /// <summary>
        /// 7个参数版本
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2"></param>
        /// <param name="b3"></param>
        /// <param name="b4"></param>
        /// <param name="b5"></param>
        /// <param name="b6"></param>
        /// <param name="b7"></param>
        /// <param name="temperature"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int Insert(float b1, float b2, float b3, float b4, float b5, float b6,float b7, double temperature, string type)
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
                engineFitResult.B7 = b7;
                engineFitResult.Temperature = temperature;
                engineFitResult.Type = type;

                // add entity object to the collection
                db.EngineFitResults.Add(engineFitResult);

                try
                {
                    // save changes to the database
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    throw new Exception("Could not save changes");
                }

            }

        }

        /// <summary>
        /// 得到最低温度
        /// </summary>
        /// <returns></returns>
        public static double getMinTemperature()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var engineFitResult = context.EngineFitResults
                        .Select(s => s.Temperature)
                        .Min();
                    return engineFitResult;
                }
                catch (Exception e)
                {
                    return 0d;
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
                if (temperature < EngineFitResultData.getMinTemperature())
                    temperature = EngineFitResultData.getMinTemperature();
                // get record that is to be selected
                var engineFitResults = (from c in db.EngineFitResults
                                        where c.Temperature == temperature && c.Type == type
                                        select c);
                if (engineFitResults != null && engineFitResults.Count() > 0)
                {
                    return engineFitResults.First(); 
                }
                //如果不存在对应的温度，使用加权平均，
                else
                {
                    int Min_Temp = EngineFitResultData.GetBoundTemperature(Convert.ToDouble(temperature), true,type);
                    int Max_Temp = EngineFitResultData.GetBoundTemperature(Convert.ToDouble(temperature), false,type);

                    EngineFitResult minEngineFitResult = (from c in db.EngineFitResults
                                                          where c.Temperature == Min_Temp && c.Type == type
                                                          select c).First();
                    EngineFitResult maxEngineFitResult = (from c in db.EngineFitResults
                                                          where c.Temperature == Max_Temp && c.Type == type
                                                          select c).First();

                    EngineFitResult engineFitResult = new EngineFitResult()
                    {
                        B1=(minEngineFitResult.B1+maxEngineFitResult.B1)/2,
                        B2 = (minEngineFitResult.B2 + maxEngineFitResult.B2) / 2,
                        B3 = (minEngineFitResult.B3 + maxEngineFitResult.B3) / 2,
                        B4 = (minEngineFitResult.B4 + maxEngineFitResult.B4) / 2,
                        B5 = (minEngineFitResult.B5 + maxEngineFitResult.B5) / 2,
                        B6 = (minEngineFitResult.B6 + maxEngineFitResult.B6) / 2,
                        Temperature=temperature,
                        Type=type
                    };
                    return engineFitResult;
                }
                
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


        //根据温度和类型输出EngineFitResult
        public static int GetBoundTemperature(double temperature, bool bound,string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                if (bound)
                {
                    double Temperature = 0d;
                    var minTemp = from c in db.EngineFitResults
                                  where c.Temperature > temperature && c.Type == type
                                  select c.Temperature;
                    if (minTemp != null && minTemp.Count() != 0)
                    {
                        // 得到大于指定温度的最低温度
                        Temperature = minTemp.Min();
                    }
                    //如果数据库中不存在比当前温度更低的温度，则取其中最大的温度
                    else
                    {
                        Temperature = (from c in db.EngineFitResults
                                  where c.Type == type
                                  select c.Temperature).Max();
                    }

                    return Convert.ToInt32(Temperature);
                }
                else
                {                 
                    double Temperature = 0d;

                    var maxTemp = from c in db.EngineFitResults
                                  where c.Temperature < temperature && c.Type == type
                                  select c.Temperature;
                    if (maxTemp != null && maxTemp.Count() != 0)
                    {
                        // 得到小于指定温度的最高温度
                        Temperature = maxTemp.Max();
                    }
                    //如果数据库中不存在比当前温度更低的温度，则取其中的最低温度。
                    else
                    {
                        Temperature = (from c in db.EngineFitResults
                                       where c.Type == type
                                       select c.Temperature).Min();
                    }

                    return Convert.ToInt32(Temperature);
                }

            }
        }


    }
}
