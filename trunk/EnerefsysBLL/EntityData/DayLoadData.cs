using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContext;
using EnerefsysDAL.Model;

namespace EnerefsysDAL
{
    public static class DayLoadData
    {
        public static int Insert(string beginTime, string endTime, float load, float ratio, string buildingType)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    db.DayLoads.Add(new DayLoad
                    {
                        BeginTime = beginTime,
                        EndTime = endTime,
                        Load = load,
                        Ratio = ratio,
                        BuildingType = buildingType
                    });

                   return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        //得到所有的日负荷表
        public static List<DayLoad> GetAllDayloads()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    return db.DayLoads.ToList();
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        //根据Id,更新负荷和比率
        public static int Update(int Id, double load, double ratio)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var dayLoad = db.DayLoads
                        .Where(s => s.DayLoadID == Id)
                        .First();
                    dayLoad.Load = load;
                    dayLoad.Ratio = ratio;
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 根据指定时间，获得指定时间的流量
        /// </summary>
        /// <param name="day"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static double GetItemByTime(string day, string time)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var dayLoad = db.DayLoads.AsEnumerable()
                        .Where(s => Convert.ToDouble(s.BeginTime.Substring(0, 2)) <= Convert.ToDouble(time)
                        && Convert.ToDouble(s.EndTime.Substring(0, 2)) > Convert.ToDouble(time))
                        .First();
                    return dayLoad.Load;
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }


    }
}
