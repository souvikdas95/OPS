using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    public class CUser_Seller
    {
        // data
        private Int32 _user_id;
        private String _name;
        private Int32 _sales;
        private Int32 _raters;
        private Double _rating;

        // constructors
        private CUser_Seller(Int32 user_id,
                            String name,
                            Int32 sales,
                            Int32 raters,
                            Double rating)
        {
            this._user_id = user_id;
            this._name = name;
            this._sales = sales;
            this._raters = raters;
            this._rating = rating;
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

        public Int32 sales
        {
            get
            {
                return _sales;
            }
            set
            {
                _sales = value;
            }
        }

        public Int32 raters
        {
            get
            {
                return _raters;
            }
            set
            {
                _raters = value;
            }
        }

        public Double rating
        {
            get
            {
                return _rating;
            }
            set
            {
                _rating = value;
            }
        }

        // core methods
        public async static Task<CUser_Seller> Retrieve(Int32 user_id)
        {
            CUser_Seller ret = null;
            try
            {
                String sql = "SELECT * FROM `user_seller` WHERE `user_id` = @user_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@user_id", user_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Seller with user_id '" + user_id + "' not found!";
                    return ret;
                }
                ret = new CUser_Seller(user_id,
                                      (await reader.IsDBNullAsync(reader.GetOrdinal("name"))) ? String.Empty : reader.GetString(reader.GetOrdinal("name")),
                                      (await reader.IsDBNullAsync(reader.GetOrdinal("sales"))) ? 0 : reader.GetInt32(reader.GetOrdinal("sales")),
                                      (await reader.IsDBNullAsync(reader.GetOrdinal("raters"))) ? 0 : reader.GetInt32(reader.GetOrdinal("raters")),
                                      (await reader.IsDBNullAsync(reader.GetOrdinal("rating"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("rating")));

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
                                          Int32 sales,
                                          Int32 raters,
                                          Double rating)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `user_seller` SET ");
                if (!this._name.Equals(name))
                {
                    hasChange = true;
                    sql.Append("`name` = @name");
                    cmd.Parameters.AddWithValue("@name", name);
                    this._name = name;
                }
                if (!(this._sales == sales))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`sales` = @sales");
                    cmd.Parameters.AddWithValue("@sales", sales);
                    this._sales = sales;
                }
                if (!(this._raters == raters))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`raters` = @raters");
                    cmd.Parameters.AddWithValue("@raters", raters);
                    this._raters = raters;
                }
                if (!(this._rating == rating))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`rating` = @rating");
                    cmd.Parameters.AddWithValue("@rating", rating);
                    this._rating = rating;
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