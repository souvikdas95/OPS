using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OPS
{
    class CUser
    {
        // data
        private Int32 _id;
        private String _username;
        private String _password;
        private String _email;
        private Byte _type;
        private Byte _status;

        // current user
        public static CUser cur_user = null;

        // subuser tables names
        public static readonly String[] szUserTables =  
        {
            "user_customer",
            "user_seller",
            "user_ordermanager",
            "user_productmanager"
        };

        // constructors
        private CUser(Int32 id,
                    String username,
                    String password,
                    String email,
                    Byte type,
                    Byte status)
        {
            this._id = id;
            this._username = username;
            this._password = password;
            this._email = email;
            this._type = type;
            this._status = status;
        }

        // GET; SET; properties (wrappers)
        public Int32 id
        {
            get
            {
                return _id;
            }
        }

        public String username
        {
            get
            {
                return _username;
            }
        }

        public String password
        {
            get
            {
                return _password;
            }
        }

        public String email
        {
            get
            {
                return _email;
            }
        }

        public Byte type
        {
            get
            {
                return _type;
            }
        }

        public Byte status
        {
            get
            {
                return _status;
            }
        }

        // methods
        /*public async Task<Boolean> login()
        {
            try
            {
                String sql = "UPDATE `user` SET `status` = '1' WHERE `id` = '" + _id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();
                _status = 1;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
                return false;
            }
            CUtils.LastLogMsg = null;
            return true;
        }

        public async Task<Boolean> logout()
        {
            try
            {
                String sql = "UPDATE `user` SET `status` = '0' WHERE `id` = '" + _id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();
                _status = 0;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
                return false;
            }
            CUtils.LastLogMsg = null;
            return true;
        }*/

        public async static Task<Boolean> Register(String username,
                                                               String password,
                                                               String email,
                                                               Byte type)
        {
            try
            {
                // Check if Entry with Username or Email ID already exists
                String sql = "SELECT * FROM `user` WHERE `username` = '" + username + "' OR `email` = '" + email + "' LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "User Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in User Table
                sql = "INSERT INTO `user` " +
                      "(`username`, `password`, `email`, `type`, `status`) " +
                      "VALUES ('" + username + "', '" + password + "', '" + email + "', '" + (1 << type) + "', '0')";
                cmd = new MySqlCommand(sql, Program.conn);
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();

                // Note: Corresponding entries in subuser tables are created automatically using triggers
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
                return false;
            }
            return true;
        }

        public async static Task<CUser> Retrieve(Int32 id)  // overload #1
        {
            CUser ret = null;
            try
            {
                String sql = "SELECT * FROM `user` WHERE `id` = '" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "User with id '" + id + "' not found!";
                    return ret;
                }
                ret = new CUser(id,
                                reader.GetString(reader.GetOrdinal("username")),
                                reader.GetString(reader.GetOrdinal("password")),
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetByte(reader.GetOrdinal("type")),
                                (await reader.IsDBNullAsync(reader.GetOrdinal("status"))) ? (Byte)0 : reader.GetByte(reader.GetOrdinal("status")));

                if (!reader.IsClosed)
                    reader.Close();
                CUtils.LastLogMsg = null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
            }
            return ret;
        }

        public async static Task<CUser> Retrieve(String username, String password, Byte cur_type)   // overload #2
        {
            CUser ret = null;
            try
            {
                String sql = "SELECT * FROM `user` WHERE `username` = '" + username + "' LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "User with username '" + username + "' not found!";
                    return ret;
                }
                if ((reader.GetByte(reader.GetOrdinal("type")) & ((byte)(1 << cur_type))) == (byte)0)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "User is not registered for the selected Account/User Type!";
                    return ret;
                }
                if (!(reader.GetString(reader.GetOrdinal("password")).Equals(password)))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Incorrect Password!";
                    return ret;
                }
                ret = new CUser(reader.GetInt32(reader.GetOrdinal("id")),
                                username,
                                password,
                                reader.GetString(reader.GetOrdinal("email")),
                                reader.GetByte(reader.GetOrdinal("type")),
                                (await reader.IsDBNullAsync(reader.GetOrdinal("status"))) ? (Byte)0 : reader.GetByte(reader.GetOrdinal("status")));
                if (!reader.IsClosed)
                    reader.Close();
                CUtils.LastLogMsg = null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
            }
            return ret;
        }

        public async static Task<Boolean> Remove(Int32 id)
        {
            try
            {
                String sql = "DELETE FROM `user` WHERE `id` = '" + id + "'";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();
                CUtils.LastLogMsg = null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
                return false;
            }
            return true;
        }

        public async Task<Boolean> Commit(String password,
                                          String email,
                                          Byte type)  // For Making Changes to Existing Class
        {
            try
            {
                MySqlCommand cmd;
                Boolean hasChange = false;
                StringBuilder sql = new StringBuilder("UPDATE `user` SET ");
                if (!this._password.Equals(password))
                {
                    hasChange = true;
                    sql.Append("`password` = '" + password + "'");
                    this._password = password;
                }
                if (!this._email.Equals(email))
                {
                    // Check for existing emails
                    cmd = new MySqlCommand("SELECT CASE WHEN EXISTS (SELECT * FROM `user` WHERE `email` = '" + email + "') THEN 1 ELSE 0 END", Program.conn);
                    if ((Int32)(Int64)(await cmd.ExecuteScalarAsync()) == 1)
                    {
                        CUtils.LastLogMsg = "Email ID already Exists!";
                        return false;
                    }

                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`email` = '" + email + "'");
                    this._email = email;
                }
                if (!(this.type == type))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`type` = '" + type + "'");
                    this._type = type;
                }
                if (!hasChange)
                {
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `id` = '" + this._id + "'");
                cmd = new MySqlCommand(sql.ToString(), Program.conn);
                sql.Clear();
                await cmd.ExecuteNonQueryAsync();
                cmd.Dispose();
                CUtils.LastLogMsg = null;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message + " " + ex.StackTrace);
#endif
                CUtils.LastLogMsg = "Unahandled Exception!";
                return false;
            }
            return true;
        }

        // util methods
        public override String ToString()
        {
            return _username;
        }
    }
}
