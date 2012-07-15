using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataContext;
using EnerefsysDAL.Model;

namespace EnerefsysDAL
{
    public static class ElectronicData
    {
        public static int update(int Id, double price)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var electronicEntity = db.Electronics
                        .Where(s => s.ElectronicID == Id)
                        .First();
                   
                    electronicEntity.ElectronicPrice = price;
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }
        //根据Id,更新电价信息
        public static int update(int Id,string startTime,string endTime,double price,double defaultValue)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var electronicEntity = db.Electronics
                        .Where(s => s.ElectronicID == Id)
                        .First();
                    electronicEntity.StartTime = startTime;
                    electronicEntity.EndTime = endTime;
                    electronicEntity.ElectronicPrice = price;
                    electronicEntity.DefaultValue = defaultValue;

                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        //增加电价信息
        public static int Insert(string startTime, string endTime, float price,float defaultValue)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    db.Electronics.Add(new Electronic { 
                    StartTime=startTime,
                    EndTime=endTime,
                    ElectronicPrice=price,
                    DefaultValue=defaultValue
                    });

                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        //根据电价Id删除
        public static int Delete(int Id)
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                try
                {
                    var electronic = db.Electronics
                        .Where(s => s.ElectronicID == Id)
                        .First();

                    db.Electronics.Remove(electronic);
                    return db.SaveChanges();
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        public static List<Electronic> GetAllElectroinc()
        {
            using (EnerefsysContext db = new EnerefsysContext())
            {
                
                try
                {
                    return db.Electronics.ToList();
                    //var electronicData = db.Electronic;
                    //foreach (var electronic in electronicData)
                    //{
                    //    eletronicEntities.Add(new ElectronicEntity { 
                    //    Id=Convert.ToInt32(electronic.Id),
                    //    StartTime=electronic.StartTime,
                    //    EndTime=electronic.EndTime,
                    //    ElectronicPrice=electronic.ElectronicPrice,
                    //    DefaultValue=electronic.DefaultValue
                    //    });
                    //}
                    //return eletronicEntities;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

    }
   
}
