using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    class CSeller_Inventory
    {
        // data
        private Int32 _seller_id;
        private Int32 _product_id;
	    private Double _price;
        private Int32 _quantity;
        private Int32 _pincode;
        private Double _warranty;

        // constructors
        private CSeller_Inventory(Int32 seller_id,
                                  Int32 product_id,
                                  Double price,
                                  Int32 quantity,
                                  Int32 pincode,
                                  Double warranty)
        {
            this._seller_id = seller_id;
            this._product_id = product_id;
            this._price = price;
            this._quantity = quantity;
            this._pincode = pincode;
            this._warranty = warranty;
        }

        // GET; SET; properties (wrappers)
        public Int32 seller_id
        {
            get
            {
                return _seller_id;
            }
        }

        public Int32 product_id
        {
            get
            {
                return _product_id;
            }
        }

        public Double price
        {
            get
            {
                return _price;
            }
        }

        public Int32 quantity
        {
            get
            {
                return _quantity;
            }
        }

        public Int32 pincode
        {
            get
            {
                return _pincode;
            }
        }

        public Double warranty
        {
            get
            {
                return _warranty;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 seller_id,
                                                   Int32 product_id,
                                                   Double price,
                                                   Int32 quantity,
                                                   Int32 pincode,
                                                   Double warranty)  // For Registering New Inventory
        {
            try
            {
                // Check if Entry with Seller ID and Product ID already exists in Inventory
                String sql = "SELECT * FROM `seller_inventory` WHERE `seller_id` = @seller_id and `product_id` = @product_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Inventory Already Exists!";
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Inventory Table
                sql = "INSERT INTO `seller_inventory` " +
                      "(`seller_id`, `product_id`, `price`, `quantity`, `pincode`, `warranty`) VALUES" +
                      "(@seller_id, @product_id, @price, @quantity, @pincode, @warranty)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@pincode", pincode);
                cmd.Parameters.AddWithValue("@warranty", warranty);
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

        public async static Task<CSeller_Inventory> Retrieve(Int32 seller_id, Int32 product_id)
        {
            CSeller_Inventory ret = null;
            try
            {
                String sql = "SELECT * FROM `seller_inventory` WHERE `seller_id` = @seller_id and `product_id` = @product_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Inventory with seller_id '" + seller_id + "' and product_id '" + product_id + "' not found!";
                    return ret;
                }
                ret = new CSeller_Inventory(seller_id,
                                            product_id,
                                            (await reader.IsDBNullAsync(reader.GetOrdinal("price"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("price")),
                                            (await reader.IsDBNullAsync(reader.GetOrdinal("quantity"))) ? 0 : reader.GetInt32(reader.GetOrdinal("quantity")),
                                            (await reader.IsDBNullAsync(reader.GetOrdinal("pincode"))) ? 0 : reader.GetInt32(reader.GetOrdinal("pincode")),
                                            (await reader.IsDBNullAsync(reader.GetOrdinal("warranty"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("warranty")));
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

        public async static Task<Boolean> Remove(Int32 seller_id, Int32 product_id)
        {
            try
            {
                String sql = "DELETE FROM `seller_inventory` WHERE `seller_id` = @seller_id and `product_id` = @product_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
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

        public async Task<Boolean> Commit(Double price,
                                          Int32 quantity,
                                          Int32 pincode,
                                          Double warranty)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `seller_inventory` SET ");
                if (!(this._price == price))
                {
                    hasChange = true;
                    sql.Append("`price` = @price");
                    cmd.Parameters.AddWithValue("@price", price);
                    this._price = price;
                }
                if (!(this._quantity == quantity))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`quantity` = @quantity");
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    this._quantity = quantity;
                }
                if (!(this._pincode == pincode))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`pincode` = @pincode");
                    cmd.Parameters.AddWithValue("@pincode", pincode);
                    this._pincode = pincode;
                }
                if (!(this._warranty == warranty))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`warranty` = @warranty");
                    cmd.Parameters.AddWithValue("@warranty", warranty);
                    this._warranty = warranty;
                }
                if (!hasChange)
                {
                    sql.Clear();
                    cmd.Dispose();
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `seller_id` = @seller_id AND `product_id` = @product_id");
                cmd.Parameters.AddWithValue("@seller_id", this._seller_id);
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
    }
}
