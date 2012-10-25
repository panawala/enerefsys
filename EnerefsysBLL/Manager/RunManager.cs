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
        /// 得到常规
        /// </summary>
        /// <returns></returns>
        public static List<NormalRunResult> getNormalRunResults()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var runResults = context.NormalRunResults
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
        /// 根据条件查询结果
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="hour"></param>
        /// <param name="dryTemperature"></param>
        /// <param name="wetTemperature"></param>
        /// <param name="load"></param>
        /// <param name="temperature"></param>
        /// <param name="power"></param>
        /// <param name="price"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        public static List<RunResult> getRunResultsByCon(string month,string day,string hour,string dryTemperature,
            string wetTemperature,string load,string temperature,string power,string price,string totalPrice)
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    int smonth=0;
                    int sday=0;
                    int shour=0;
                    double sdryTemperature=0;
                    double swetTemperature=0;
                    double sload=0;
                    double stemperature=0;
                    double spower=0;
                    double sprice=0;
                    double stotalPrice=0;
                    if(!string.IsNullOrEmpty(month))
                        smonth=Convert.ToInt32(month);
                    if(!string.IsNullOrEmpty(day))
                        sday=Convert.ToInt32(day);
                    if(!string.IsNullOrEmpty(dryTemperature))
                        sdryTemperature=Convert.ToDouble(dryTemperature);
                    if(!string.IsNullOrEmpty(hour))
                        shour=Convert.ToInt32(hour);
                    if(!string.IsNullOrEmpty(wetTemperature))
                        swetTemperature=Convert.ToDouble(wetTemperature);
                    if(!string.IsNullOrEmpty(load))
                        sload=Convert.ToDouble(load);
                    if(!string.IsNullOrEmpty(temperature))
                        stemperature=Convert.ToDouble(temperature);
                    if(!string.IsNullOrEmpty(power))
                        spower=Convert.ToDouble(power);
                    if(!string.IsNullOrEmpty(power))
                        spower=Convert.ToDouble(power);
                    if(!string.IsNullOrEmpty(price))
                        sprice=Convert.ToDouble(price);
                    if(!string.IsNullOrEmpty(totalPrice))
                        stotalPrice=Convert.ToDouble(totalPrice);

                    var runResults = context.RunResults
                        .Where(s =>
                            (s.Month == smonth || smonth == 0)
                        && (s.Day == sday || sday == 0)
                        && (s.Hour == shour || shour == 0)
                        && (s.WetTemperature < swetTemperature + 2 && s.WetTemperature > swetTemperature - 2 || swetTemperature == 0)
                        && (s.DryTemperature < sdryTemperature + 2 && s.DryTemperature > sdryTemperature - 2 || sdryTemperature == 0)
                        && (s.Load < sload + 100 && s.Load > sload - 100 || sload == 0)
                        && (s.EnterTemperature < stemperature + 2 && s.EnterTemperature > stemperature - 2 || stemperature == 0)
                        && (s.Power < spower + 50 && s.Power > spower - 50 || spower == 0)
                        && (s.ElectronicPrice < sprice + 0.5 && s.ElectronicPrice > sprice - 0.5 || sprice == 0)
                        && (s.TotalPrice < stotalPrice + 50 && s.TotalPrice > stotalPrice - 50 || stotalPrice == 0)
                        )
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


        /// <summary>
        /// 插入常规运行结果
        /// </summary>
        /// <param name="standardLoad"></param>
        /// <param name="power"></param>
        /// <param name="totalPrice"></param>
        /// <returns></returns>
        public static int InsertIntoNormalRunResult(StandardLoad standardLoad, double power, double totalPrice)
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    context.NormalRunResults.Add(new NormalRunResult
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
        /// <summary>
        /// 删除所有的常规运行结果
        /// </summary>
        /// <returns></returns>
        public static int deleteallNormalRunResults()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var runResults = context.NormalRunResults;
                    foreach (var runResult in runResults)
                    {
                        context.NormalRunResults.Remove(runResult);
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
        /// 删除所有的运行结果
        /// </summary>
        /// <returns></returns>
        public static int deleteallRunResults()
        {
            using (var context = new EnerefsysContext())
            {
                try
                {
                    var runResults = context.RunResults;
                    foreach (var runResult in runResults)
                    {
                        context.RunResults.Remove(runResult);
                    }
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
