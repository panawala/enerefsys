using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace EnerefsysBLL.Utility
{
    public class Utility
    {
        //从excel表中得到每个sheet的名字
        public static List<string> GetSheetNames(string fileName)
        {
            List<string> retSheetNames = new List<string>();
            string strHead = "NO";
            string excelFilepath = fileName;
            string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelFilepath + ";Extended Properties=\"Excel 12.0;HDR=" + strHead + ";\"";
            if ((System.IO.Path.GetExtension(excelFilepath)).ToLower() == ".xls")
            {
                strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + excelFilepath + ";Extended Properties=\"Excel 8.0;HDR=" + strHead + ";IMEX=1\"";
            }

            using (OleDbConnection conn = new OleDbConnection(strCon))
            {
                //打开oledb连接
                conn.Open();

                //返回在目录中定义的工作表
                System.Data.DataTable schemeTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);

                for (int i = 0; i < schemeTable.Rows.Count; i++)
                {
                    //工作表名 schemeTable.Rows[i][2].ToString().Trim()
                    string temp = (schemeTable.Rows[i][2].ToString().Trim());
                    //int tmp = GetNumber(temp);
                    //retSheetNames.Add(tmp.ToString());
                    temp = temp.Substring(1,temp.Length - 3);
                    retSheetNames.Add(temp);
                }
                return retSheetNames;
            }
        }

        private static int GetNumber(string str)
        {
            int result = 0;
            if (str != null && str != string.Empty)
            {
                // 正则表达式剔除非数字字符（不包含小数点.） 
                str = Regex.Replace(str, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型 
                if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
                {
                    result = Convert.ToInt32(str);
                }
            }
            return result;
        }

        //求得一元二次方程的最小值的解,解的范围默认0-1
        public static double GetMinSolute(double a,double b, double c)
        {
            //抛物线口朝上
            if (a > 0)
            {
                double axis = -b / (2 * a);
                if (axis > 0 && axis <= 1)
                {
                    return axis;
                }
                else if (axis > 1)
                {
                    return 1;
                }
                else
                    return 0;
            }
            //抛物线口朝下
            else if (a < 0)
            {
                double axis = -b / (2 * a);
                if (axis < 0.5)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            //直线
            else
            {
                //直线向上
                if (b > 0)
                    return 0;
                //直线向下
                else if (b < 0)
                    return 1;
                //常数返回0
                else
                    return 0;
            }
        }

        //求得一元二次方程的最小值的解,解的范围默认0-1
        public static double GetMinSolute(double threeOption,double a, double b, double c)
        {
            double rtResut = 0.3;
            for (int i = 30; i < 101; i++)
            {
                double ii = i * 0.01;
                double iresult = threeOption * ii * ii * ii + a * ii * ii + b * ii + c;
                if (iresult < rtResut && iresult >= 0.3 && iresult <= 1)
                    rtResut = iresult;
            }
            return rtResut;
        }

        //base64加密
        public static string GetBase64Hash(string input)
        {
            byte[] bytes = Encoding.Default.GetBytes(input);
            string base64Result = Convert.ToBase64String(bytes);
            return base64Result;
        }
        //base64解密
        public static string GetFromBase64Hash(string input)
        {
            byte[] outputb = Convert.FromBase64String(input);
            string orgStr = Encoding.Default.GetString(outputb);
            return orgStr;
        }
        //md5加密
        public static string GetMD5Hash(String input)
        {
            byte[] result = Encoding.Default.GetBytes(input);    //tbPass为输入密码的文本框
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            string md5Str = BitConverter.ToString(output).Replace("-", "");
            return md5Str;
        }


        public readonly static string filePath = @"C:\Windows\Help\Windows\txt.ini";
        /// <summary>
        /// ini文件读写操作
        /// </summary>
        /// <param name="section">section名</param>
        /// <param name="key">键</param>
        /// <param name="val">值</param>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key, string def, StringBuilder retVal,int size, string filePath);

        //对ini文件进行写操作的函数
        public static void IniWriteValue(string Section, string Key, string Value, string filepath)
        {
            WritePrivateProfileString(Section, Key, Value, filepath);
        }
        //对ini文件进行读操作的函数
        public static string IniReadValue(string Section, string Key, string filepath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,255, filepath);
            return temp.ToString();
        }



        //根据列表和负荷得到组合序列
        public static List<List<int>> GetConsist(List<double> list, double load)
        {
            List<List<int>> result = new List<List<int>>();
            List<int> elemlist = new List<int>();
            int count = list.Count();
            string index = "";
            for (int i = GetPower(2, count) - 1; i >= 0; i--)
            {

                try
                {
                    index = GetBinary(i, count);
                }
                catch (Exception e)
                {
                    Console.Out.WriteLine(e.ToString());
                }
                if (IsValidate(list, index, load))
                {
                    elemlist = GetList(index);
                    result.Add(elemlist);
                }
            }
            return result;
        }

        private static int GetPower(int basement, int exponent)
        {
            int result = 1;
            for (int i = 0; i < exponent; i++)
            {
                result *= basement;
            }
            return result;
        }

        private static bool IsValidate(List<double> list, string index, double load)
        {
            List<int> elem = GetList(index);
            double sum = 0.0;
            for (int i = 0; i < elem.Count; i++)
            {
                sum += list[elem[i]];
            }
            return sum >= load ? true : false;
        }

        private static string GetBinary(int index, int count)
        {
            string result = Convert.ToString(index, 2);
            if (result.Length != count)
            {
                result = "";
            }
            return result;
        }

        private static List<int> GetList(string list)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] != '0')
                    result.Add(i);
            }
            return result;
        }













    }
}
