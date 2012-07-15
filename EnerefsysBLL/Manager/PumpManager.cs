using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using EnerefsysDAL;
using EnerefsysDAL.Model;

namespace BLL
{
    public class PumpManager
    {
        //在水泵的数值模拟系数表中插入数据
        public static int InsertPumpInfo(object pumpFlow, object pumpCount, object pumpType, object pumpDesignFlow)
        {
            /*string sql = "INSERT INTO PumpInfo (Id,PumpFlow,PumpCount,PumpType,PumpDesignFlow) VALUES";
            sql += "(@Id,@PumpFlow,@PumpCount,@PumpType,@PumpDesignFlow)";
            SQLiteParameter[] sps = new SQLiteParameter[]
            {
                new SQLiteParameter("@Id",null),
                new SQLiteParameter("@PumpFlow",pumpFlow),
                new SQLiteParameter("@PumpCount",pumpCount),
                new SQLiteParameter("@PumpType",pumpType),
                new SQLiteParameter("@PumpDesignFlow",pumpDesignFlow)
            };
            return DBHelper.ExecuteNonQuery(sql, sps);*/
            return PumpInfoData.InsertPumpInfo(Convert.ToDouble(pumpFlow), Convert.ToInt32(pumpCount), pumpType.ToString(), Convert.ToDouble(pumpDesignFlow));
        }

        //在水泵的数值模拟系数表中插入数据
        public static int Insert(object b1, object b2, object b3,object type)
        {
            /*string sql = "INSERT INTO PumpFitResult (Id,B1,B2,B3,Type) VALUES";
            sql += "(@Id,@B1, @B2, @B3, @Type)";
            SQLiteParameter[] sps = new SQLiteParameter[]
            {
                new SQLiteParameter("@Id",null),
                new SQLiteParameter("@B1",b1),
                new SQLiteParameter("@B2",b2),
                new SQLiteParameter("@B3",b3),
                new SQLiteParameter("@Type",type)
            };
            return DBHelper.ExecuteNonQuery(sql, sps);*/
            return PumpFitResultData.InsertPumpFitResult(Convert.ToDouble(b1), Convert.ToDouble(b2), Convert.ToDouble(b3), type.ToString());
        }

        public static int DeletePump()
        {
            /*string sql = "DELETE FROM PumpFitResult";
            DBHelper.ExecuteNonQuery(sql);
            string sql1 = "DELETE FROM PumpInfo";
            return DBHelper.ExecuteNonQuery(sql1);*/

            PumpFitResultData.DeleteAll();
            return PumpInfoData.DeleteAll();

        }

        //得到水泵的所有类型
        public static List<string> GetPumpTypes()
        {
            /*string sql = "SELECT Type FROM PumpFitResult";
            DataTable dt = DBHelper.GetTable(sql);
            List<string> result = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i]["Type"].ToString());
            }
            return result;*/
            return PumpFitResultData.GetPumpTypes();
        }

        //得到一定类型下的水泵的二次项系数
        public static List<double> GetParamsByType(string type)
        {
            /*string sql = "SELECT B1,B2,B3 FROM PumpFitResult WHERE Type = @Type";
            SQLiteParameter[] sps = new SQLiteParameter[]
            {
                new SQLiteParameter("@Type",type)
            };
            DataTable dt = DBHelper.GetTable(sql, sps);
            double b1 = Convert.ToDouble(dt.Rows[0]["B1"]);
            double b2 = Convert.ToDouble(dt.Rows[0]["B2"]);
            double b3 = Convert.ToDouble(dt.Rows[0]["B3"]);
            List<double> result = new List<double>();
            result.Add(b1);
            result.Add(b2);
            result.Add(b3);
            return result;*/
            return PumpFitResultData.GetParamsByType(type);
        }

        //根据流量和类型算出所需水泵数量，
        public static int GetCountByFlowType(double flow,string type)
        {
            /*string sql = "SELECT MIN(PumpFlow) AS Min_Temp,PumpDesignFlow FROM PumpInfo WHERE PumpFlow >= @PumpFlow AND PumpType = @PumpType";
            SQLiteParameter[] sps= new SQLiteParameter[]
            {
                new SQLiteParameter("@PumpFlow",flow),
                new SQLiteParameter("@PumpType",type)
            };
            DataTable dt = DBHelper.GetTable(sql, sps);
            //如果表中查找到，返回相应的台数
            if (dt != null)
            {
                int Min_Temp = Convert.ToInt32(dt.Rows[0]["Min_Temp"]);
                string sql1 = "SELECT PumpCount FROM PumpInfo WHERE PumpFlow = @PumpFlow";
                SQLiteParameter[] sps1 = new SQLiteParameter[]
                {
                    new SQLiteParameter("@PumpFlow",Min_Temp),
                };
                DataTable dt1 = DBHelper.GetTable(sql1, sps1);
                return Convert.ToInt32(dt1.Rows[0]["PumpCount"]);
            }
            //如果表中没有查到，则直接除以设计流量
            else
            {
                double pumpFlow = Convert.ToDouble(dt.Rows[0]["PumpDesignFlow"]);
                return Convert.ToInt32(flow / pumpFlow);
            }*/



            PumpInfo pumpInfo = PumpInfoData.GetMinPumpFlowByFlowType(flow, type);

            //如果表中查找到，返回相应的台数
            if (pumpInfo != null)
            {
                return PumpInfoData.GetPumpCountByFlow(Convert.ToInt32(pumpInfo.PumpFlow));
            }
            //如果表中没有查到，则直接除以设计流量
            else
            {
                double pumpFlow = Convert.ToDouble(pumpInfo.PumpFlow);
                return Convert.ToInt32(flow / pumpFlow);
            }

            
        }

    }
}
