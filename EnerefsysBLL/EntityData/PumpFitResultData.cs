using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContext;
using EnerefsysDAL.Model;

namespace EnerefsysBLL.EntityData
{
    public static class PumpFitResultData
    {
        //插入PumpFitResult表
        public static int InsertPumpFitResult(double b1, double b2, double b3, string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                PumpFitResult pumpFitResult = new PumpFitResult();
                pumpFitResult.B1 = b1;
                pumpFitResult.B2 = b2;
                pumpFitResult.B3 = b3;
                pumpFitResult.Type = type;

                // add entity object to the collection
                db.PumpFitResults.Add(pumpFitResult);

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

        //插入PumpFitResult表
        public static int InsertPumpFitResult(double b1, double b2, double b3, double b4,string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                PumpFitResult pumpFitResult = new PumpFitResult();
                pumpFitResult.B1 = b1;
                pumpFitResult.B2 = b2;
                pumpFitResult.B3 = b3;
                pumpFitResult.B4 = b4;
                pumpFitResult.Type = type;

                // add entity object to the collection
                db.PumpFitResults.Add(pumpFitResult);

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

        public static int DeleteAll()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                // get record that is to be deleted
                IEnumerable<PumpFitResult> pumpFitResults = from c in db.PumpFitResults
                                                  select c;
                foreach (var pumpFitResult in pumpFitResults)
                {
                    db.PumpFitResults.Remove(pumpFitResult);
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




        public static List<string> GetPumpTypes()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                List<string> pumpTypes = new List<string>();
                // get record that is to be deleted
                IEnumerable<PumpFitResult> pumpFitResults = from c in db.PumpFitResults
                                                                select c;
                foreach (var pumpFitResult in pumpFitResults)
                {
                    pumpTypes.Add(pumpFitResult.Type);
                }
                return pumpTypes;
            }
        }

        //得到一定类型下的水泵的二次项系数
        public static List<double> GetParamsByType(string type)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                List<double> results = new List<double>();
                // get record that is to be deleted
                PumpFitResult pumpFitResult = (from c in db.PumpFitResults
                                                                where c.Type==type
                                                                select c).First();
                results.Add(pumpFitResult.B1);
                results.Add(pumpFitResult.B2);
                results.Add(pumpFitResult.B3);
                results.Add(pumpFitResult.B4);
                return results;

              
            }
        }

    }
}
