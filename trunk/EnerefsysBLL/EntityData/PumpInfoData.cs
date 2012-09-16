using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContext;
using EnerefsysDAL.Model;

namespace EnerefsysBLL.EntityData
{
    public static class PumpInfoData
    {
        public static int InsertPumpInfo(double pumpFlow, int pumpCount, string pumpType, double pumpDesignFlow)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                PumpInfo pumpInfo = new PumpInfo();
                pumpInfo.PumpCount = pumpCount;
                pumpInfo.PumpFlow = pumpFlow;
                pumpInfo.PumpType = pumpType;
                pumpInfo.PumpDesignFlow = pumpDesignFlow;

                // add entity object to the collection
                db.PumpInfoes.Add(pumpInfo);

                try
                {
                    // save changes to the database
                    return db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Could not save changes");
                }
            }
        }

        /// <summary>
        /// 得到流量
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static double getFlow(string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var flow = db.PumpInfoes
                        .Where(s => s.PumpType == type)
                        .First()
                        .PumpDesignFlow;
                    return flow;
                }
                catch
                {
                    return 0d;
                    throw new Exception("Could not save changes");
                }
            }
        }


        public static int DeleteAll()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                // get record that is to be deleted
                IEnumerable<PumpInfo> pumpInfos = from c in db.PumpInfoes
                                                                select c;
                foreach (var pumpInfo in pumpInfos)
                {
                    db.PumpInfoes.Remove(pumpInfo);
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


        public static PumpInfo GetMinPumpFlowByFlowType(double flow, string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
               
                var minValue = (from u in db.PumpInfoes
                                select u.PumpFlow).Min();
                PumpInfo pumpInfo = (from c in db.PumpInfoes
                                     where c.PumpFlow == minValue && c.PumpType == type
                                     select c).First();

                return pumpInfo;
           
            }
        }


        public static int GetPumpCountByFlow(int flow)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                int pumpCount = (from c in db.PumpInfoes
                                     where c.PumpFlow >= flow 
                                     select c.PumpCount).First();

                return pumpCount;

            }
        }


    }
}
