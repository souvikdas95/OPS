using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    class CCatagory
    {
        // data
        private Int32 _id;
        private String _name;
        private String _description;
        private Int32 _parent_id;

        // constructors
        private CCatagory(Int32 id,
                         String name,
                         String description,
                         Int32 parent_id)
        {
            this._id = id;
            this._name = name;
            this._description = description;
            this._parent_id = parent_id;
        }

        // GET; SET; properties (wrappers)
        public Int32 id
        {
            get
            {
                return _id;
            }
        }

        public String name
        {
            get
            {
                return _name;
            }
        }

        public String description
        {
            get
            {
                return _description;
            }
        }

        public Int32 parent_id
        {
            get
            {
                return _parent_id;
            }
        }

        // core methods
        public async static Task<Boolean> Register(String name,
                                                   String description,
                                                   Int32 parent_id)  // For Registering New Catagory
        {
            try
            {
                // Check if Entry with Name and Parent ID already exists
                String sql = "SELECT * FROM `user` WHERE `name` = '" + name + "'";
                if (parent_id != 0)
                    sql += " AND `parent_id` = '" + parent_id + "'";
                else
                    sql += " AND `parent_id` = '0' OR `parent_id` is NULL";
                sql += " LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "Catagory Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in User Table
                sql = "INSERT INTO `catagory` " +
                             "(`name`, `description`, `parent_id`) VALUES" +
                             "('" + name + "', '" + description + "', '" + parent_id + "')";
                cmd = new MySqlCommand(sql, Program.conn);
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

        public async static Task<CCatagory> Retrieve(Int32 id)
        {
            CCatagory ret = null;
            try
            {
                String sql = "SELECT * FROM `catagory` WHERE `id` = '" + id + "' LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Catagory with id '" + id + "' not found!";
                    return ret;
                }
                ret = new CCatagory(id,
                                    reader.GetString(reader.GetOrdinal("name")),
                                    (await reader.IsDBNullAsync(reader.GetOrdinal("description"))) ? String.Empty : reader.GetString(reader.GetOrdinal("description")),
                                    (await reader.IsDBNullAsync(reader.GetOrdinal("parent_id"))) ? 0 : reader.GetInt32(reader.GetOrdinal("parent_id")));
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
                String sql = "DELETE FROM `catagory` WHERE `id` = '" + id + "'";
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

        public async Task<Boolean> Commit(String name,
                                          String description,
                                          Int32 parent_id)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                StringBuilder sql = new StringBuilder("UPDATE `catagory` SET ");
                if (!this._name.Equals(name))
                {
                    hasChange = true;
                    sql.Append("`name` = '" + name + "'" );
                    this._name = name;
                }
                if (!this._description.Equals(description))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`description` = '" + description + "'");
                    this._description = description;
                }
                if (!(this._parent_id == parent_id))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`parent_id` = '" + parent_id + "'");
                    this._parent_id = parent_id;
                }
                if (!hasChange)
                {
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `id` = '" + this._id + "'");
                MySqlCommand cmd = new MySqlCommand(sql.ToString(), Program.conn);
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

        // non-core methods
        public async static Task<List<CCatagory>> RetrieveCatagoryList(Int32 parent_id)
        {
            List<CCatagory> ret = new List<CCatagory>();
            try
            {
                String sql = "SELECT * FROM `catagory`";
                if (parent_id != 0)  // child catagories
                    sql += " WHERE `parent_id` = '" + parent_id + "'";
                else
                    sql += " WHERE `parent_id` = '0' OR `parent_id` is NULL";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CCatagory(reader.GetInt32(reader.GetOrdinal("id")),
                                      reader.GetString(reader.GetOrdinal("name")),
                                      (await reader.IsDBNullAsync(reader.GetOrdinal("description"))) ? String.Empty : reader.GetString(reader.GetOrdinal("description")),
                                      parent_id)
                    );
                }
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
                return ret;
            }
            return ret;
        }

        // util methods
        public override string ToString()
        {
            return _name;
        }
    }
}
