using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    public class CUser_Customer
    {
        // data
        private Int32 _user_id;
        private String _name;
        private DateTime _dob;

        // constructors
        private CUser_Customer(Int32 user_id,
                               String name,
                               DateTime dob)
        {
            this._user_id = user_id;
            this._name = name;
            this._dob = dob;
        }

        // GET; SET; properties (wrappers)
        public Int32 user_id
        {
            get
            {
                return _user_id;
            }
        }

        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public DateTime dob
        {
            get
            {
                return _dob;
            }
            set
            {
                _dob = value;
            }
        }

        // core methods
        public async static Task<CUser_Customer> Retrieve(Int32 user_id)
        {
            CUser_Customer ret = null;
            try
            {
                String sql = "SELECT * FROM `user_customer` WHERE `user_id` = @user_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@user_id", user_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Customer with user_id '" + user_id + "' not found!";
                    return ret;
                }
                ret = new CUser_Customer(user_id,
                                        (await reader.IsDBNullAsync(reader.GetOrdinal("name"))) ? String.Empty : reader.GetString(reader.GetOrdinal("name")),
                                        (await reader.IsDBNullAsync(reader.GetOrdinal("dob"))) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("dob")));

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

        public async Task<Boolean> Commit(String name,
                                          DateTime dob)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `user_customer` SET ");
                if (!this._name.Equals(name))
                {
                    hasChange = true;
                    sql.Append("`name` = @name");
                    cmd.Parameters.AddWithValue("@name", name);
                    this._name = name;
                }
                if (!(this._dob.Equals(dob)))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`dob` = @dob");
                    cmd.Parameters.AddWithValue("@dob", dob.ToString("yyyy-MM-dd"));
                    this._dob = dob;
                }
                if (!hasChange)
                {
                    sql.Clear();
                    cmd.Dispose();
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `user_id` = @user_id");
                cmd.Parameters.AddWithValue("@user_id", this._user_id);
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
    }
}