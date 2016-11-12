using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    public class CCatagory_Field
    {
        // data
        private Int32 _catagory_id;
        private String _field_name;

        // constructors
        private CCatagory_Field(Int32 catagory_id,
                               String field_name)
        {
            this._catagory_id = catagory_id;
            this._field_name = field_name;
        }

        // GET; SET; properties (wrappers)
        public Int32 catagory_id
        {
            get
            {
                return _catagory_id;
            }
        }

        public String field_name
        {
            get
            {
                return _field_name;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 catagory_id,
                                                   String field_name)  // For Registering New Catagory Field
        {
            try
            {
                // Check if Entry with Catagory ID and Name already exists
                String sql = "SELECT * FROM `catagory_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "Catagory Field Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Product Table
                sql = "INSERT INTO `catagory_field` " +
                      "(`catagory_id`, `field_name`) VALUES" +
                      "(@catagory_id, @field_name)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
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

        /*public async static Task<CCatagory_Field> Retrieve(Int32 catagory_id, String field_name, Int32 catagory_id)
        {
            CCatagory_Field ret = null;
            try
            {
                String sql = "SELECT * FROM `catagory_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Catagory Field with catagory_id '" + catagory_id + "' and field_name '" + field_name + "' not found!";
                    return ret;
                }
                ret = new CCatagory_Field(catagory_id,
                                         field_name);
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
        }*/

        public async static Task<Boolean> Remove(Int32 catagory_id, String field_name)
        {
            try
            {
                String sql = "DELETE FROM `catagory_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
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

        /*public async Task<Boolean> Commit(String field_value)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `catagory_field` SET ");
                if (!(this._field_value.Equals(field_value)))
                {
                    hasChange = true;
                    sql.Append("`field_value` = @field_value");
                    cmd.Parameters.AddWithValue("@field_value", field_value);
                    this._field_value = field_value;
                }
                if (!hasChange)
                {
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `catagory_id` = @catagory_id and `field_name` = @field_name and `catagory_id` = @catagory_id");
                cmd.Parameters.AddWithValue("@catagory_id", this._catagory_id);
                cmd.Parameters.AddWithValue("@field_name", this._field_name);
                cmd.Parameters.AddWithValue("@catagory_id", this._catagory_id);
                cmd.CommandText = sql.ToString();
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
        }*/

        // non-core methods
        public async static Task<List<CCatagory_Field>> RetrieveCatagoryFieldList(Int32 catagory_id)
        {
            List<CCatagory_Field> ret = new List<CCatagory_Field>();
            try
            {
                String sql = "SELECT * FROM `catagory_field` WHERE `catagory_id` = @catagory_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CCatagory_Field(catagory_id,
                                           reader.GetString(reader.GetOrdinal("field_name")))
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
            }
            return ret;
        }
    }
}
