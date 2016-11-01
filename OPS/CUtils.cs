using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Drawing;

namespace OPS
{
    class CUtils
    {
        public static String LastLogMsg;

        public static Boolean IsValidUsername(String s)
        {
            if (s.Length < 8 || s.Length > 63)
            {
                LastLogMsg = "Invalid Username Length!";
                return false;
            }
            try
            {
                LastLogMsg = null;
                return Regex.IsMatch(s, "^[a-zA-Z0-9\\._-]+$");
            }
            catch(Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return false;
            }

        }

        public static Boolean IsValidPassword(String s)
        {

            if (s.Length < 8 || s.Length > 63)
            {
                LastLogMsg = "Invalid Password Length!";
                return false;
            }
            try
            {
                LastLogMsg = null;
                return Regex.IsMatch(s, "^(?=.*[A-Za-z])(?=.*\\d)[A-Za-z\\d]+$");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return false;
            }
        }

        public static Boolean IsValidEmail(String s)
        {
            if (s.Length < 3 || s.Length > 254)
            {
                LastLogMsg = "Invalid Email Length!";
                return false;
            }
            try
            {
                LastLogMsg = null;
                return Regex.IsMatch(s, "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return false;
            }
        }

        public async static Task<Boolean> IsValidUserType(Byte type)    // Numeric (not Bitsum)
        {
            if (type < 0 || type >=  CUser.szUserTables.Length)
            {
                LastLogMsg = "Invalid User Type!";
                return false;
            }
            try
            {
                String sql = "select case when exists(select * from information_schema.tables where `table_schema` = @table_schema and `table_name` = @table_name) then 1 else 0 end";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("table_schema", "db_ops");
                cmd.Parameters.AddWithValue("table_name", CUser.szUserTables[type]);
                if ((Int32)(Int64)(await cmd.ExecuteScalarAsync()) == 0)
                {
                    LastLogMsg = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return false;
            }
            LastLogMsg = null;
            return true;
        }

        public static Boolean IsValidCustomerName(String s)
        {
            if (s.Length < 3 || s.Length > 127)
            {
                LastLogMsg = "Invalid Customer Name Length!";
                return false;
            }
            try
            {
                LastLogMsg = null;
                return Regex.IsMatch(s, "^[a-zA-Z]([a-zA-Z\\s])+[a-zA-Z]$");
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return false;
            }
        }

        public static String sha256(String s)
        {
            try
            {
                SHA256Managed crypt = new SHA256Managed();
                Byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(s), 0, Encoding.UTF8.GetByteCount(s));
                crypt.Clear();
                crypt.Dispose();
                StringBuilder hash = new StringBuilder();
                foreach (Byte x in crypto)
                {
                    hash.Append(x.ToString("x2"));
                }
                LastLogMsg = null;
                return hash.ToString();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                LastLogMsg = "Unhandled Exception!";
                return null;
            }
        }

        public static Boolean IsNumeric(String str)
        {
            LastLogMsg = null;
            try
            {
                Double d = Double.Parse(str);
            }
            catch (Exception ex)    // non-mutant type
            {
                return false;
            }
            return true;
        }
    }
}
