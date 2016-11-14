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
    public class CProduct_Field
    {
        // data
        private Int32 _catagory_id;
        private String _field_name;
        private Int32 _product_id;
        private String _field_value;

        // constructors
        private CProduct_Field(Int32 catagory_id,
                               String field_name,
                               Int32 product_id,
                               String field_value)
        {
            this._catagory_id = catagory_id;
            this._field_name = field_name;
            this._product_id = product_id;
            this._field_value = field_value;
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

        public Int32 product_id
        {
            get
            {
                return _product_id;
            }
        }

        public String field_value
        {
            get
            {
                return _field_value;
            }
            set
            {
                _field_value = value;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 catagory_id,
                                                   String field_name,
                                                   Int32 product_id,
                                                   String field_value)  // For Registering New Product Field
        {
            try
            {
                // Check if Entry with Catagory ID and Name already exists
                String sql = "SELECT * FROM `product_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name and `product_id` = @product_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "Product Field Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Product Table
                sql = "INSERT INTO `product_field` " +
                      "(`catagory_id`, `field_name`, `product_id`, `field_value`) VALUES" +
                      "(@catagory_id, @field_name, @product_id, @field_value)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@field_value", field_value);
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

        public async static Task<CProduct_Field> Retrieve(Int32 catagory_id, String field_name, Int32 product_id)
        {
            CProduct_Field ret = null;
            try
            {
                String sql = "SELECT * FROM `product_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name and `product_id` = @product_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Product Field with catagory_id '" + catagory_id + "', product_id '" + product_id + "' and field_name '" + field_name + "' not found!";
                    return ret;
                }
                ret = new CProduct_Field(catagory_id,
                                         field_name,
                                         product_id,
                                         reader.GetString(reader.GetOrdinal("field_value")));
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

        public async static Task<Boolean> Remove(Int32 catagory_id, String field_name, Int32 product_id)
        {
            try
            {
                String sql = "DELETE FROM `product_field` WHERE `catagory_id` = @catagory_id and `field_name` = @field_name and `product_id` = @product_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@field_name", field_name);
                cmd.Parameters.AddWithValue("@product_id", product_id);
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

        public async Task<Boolean> Commit(String field_value)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `product_field` SET ");
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
                sql.Append(" WHERE `catagory_id` = @catagory_id and `field_name` = @field_name and `product_id` = @product_id");
                cmd.Parameters.AddWithValue("@catagory_id", this._catagory_id);
                cmd.Parameters.AddWithValue("@field_name", this._field_name);
                cmd.Parameters.AddWithValue("@product_id", this._product_id);
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
        }

        // non-core methods
        public async static Task<List<CProduct_Field>> RetrieveProductFieldList(Int32 product_id)
        {
            List<CProduct_Field> ret = new List<CProduct_Field>();
            try
            {
                String sql = "SELECT * FROM `product_field` WHERE `product_id` = @product_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CProduct_Field(reader.GetInt32(reader.GetOrdinal("catagory_id")),
                                           reader.GetString(reader.GetOrdinal("field_name")),
                                           product_id,
                                           reader.GetString(reader.GetOrdinal("field_value")))
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
