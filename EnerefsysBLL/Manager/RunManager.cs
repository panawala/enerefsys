using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnerefsysDAL.Model;
using DataContext;
using System.Data;

namespace EnerefsysBLL.Manager
{
    public class RunManager
    {
        /// <summary>
        /// 根据时间得到数据源
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static StandardLoad getStandardLoad(int month, int day, int hour)
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var standardLoad = context.StandardLoads
                  .Where(s => s.Month == month
                  && s.Day == day
                  && s.Hour == hour)
                  .First();
                    return standardLoad;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到所有的负荷数据
        /// </summary>
        /// <returns></returns>
        public static List<StandardLoad> getAllStandardLoads()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var standardLoads = context.StandardLoads
                        .ToList();
                    return standardLoads;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 删除所有的负荷数据
        /// </summary>
        /// <returns></returns>
        public static int deleteAll()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var standardLoads = context.StandardLoads;
                    foreach (var standardLoad in standardLoads)
                    {
                        context.StandardLoads.Remove(standardLoad);
                    }
                    return context.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }


        /// <summary>
        /// 通过excel导入数据库
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static int InsertFromExcel(DataTable dataTable)
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        var standardLoad = new StandardLoad
                        {
                            Day = Convert.ToInt32(dataRow["Day"].ToString()),
                            Month=Convert.ToInt32(dataRow["Month"].ToString()),
                            Hour=Convert.ToInt32(dataRow["Hour"].ToString()),
                            DryTemperature=Convert.ToDouble(dataRow["DryTemperature"].ToString()),
                            WetTemperature = Convert.ToDouble(dataRow["WetTemperature"].ToString()),
                            Load=Convert.ToDouble(dataRow["Load"].ToString()),
                            EnterTemperature = Convert.ToDouble(dataRow["EnterTemperature"].ToString()),
                            ElectronicPrice = Convert.ToDouble(dataRow["ElectronicPrice"].ToString())
                        };
                        context.StandardLoads.Add(standardLoad);
                    }
                    return context.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }

        }

        /// <summary>
        /// 得到所有的运行结果
        /// </summary>
        /// <returns></returns>
        public static List<RunResult> getRunResults()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var runResults = context.RunResults
                        .ToList();
                    return runResults;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 插入报表数据
        /// </summary>
        /// <param name="standardLoad"></param>
        /// <param name="power"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        public static int InsertIntoRunResult(StandardLoad standardLoad, double power, double totalPrice)
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    context.RunResults.Add(new RunResult
                        {
                            Month = standardLoad.Month,
                            Day = standardLoad.Day,
                            Hour = standardLoad.Hour,
                            DryTemperature = standardLoad.DryTemperature,
                            WetTemperature = standardLoad.WetTemperature,
                            Load = standardLoad.Load,
                            EnterTemperature = standardLoad.EnterTemperature,
                            ElectronicPrice = standardLoad.ElectronicPrice,
                            Power = power,
                            TotalPrice = totalPrice
                        });
                    return context.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }


    }
}
