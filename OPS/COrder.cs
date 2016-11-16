using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    class COrder
    {
        // data
        private Int32 _id;
        private Int32 _customer_id;
        private Int32 _product_id;
        private Int32 _seller_id;
        private Int32 _quantity;
        private Int64 _contact_no;
        private String _street;
        private Int32 _pincode;
        private Double _total;
        private DateTime _started_at;
        private Byte _order_status;

        // constructors
        private COrder(Int32 id,
                       Int32 customer_id,
                       Int32 product_id,
                       Int32 seller_id,
                       Int32 quantity,
                       Int64 contact_no,
                       String street,
                       Int32 pincode,
                       Double total,
                       DateTime started_at,
                       Byte order_status)
        {
            this._id = id;
            this._customer_id = customer_id;
            this._product_id = product_id;
            this._seller_id = seller_id;
            this._quantity = quantity;
            this._contact_no = contact_no;
            this._street = street;
            this._pincode = pincode;
            this._total = total;
            this._started_at = started_at;
            this._order_status = order_status;
        }

        // GET; SET; properties (wrappers)
        public Int32 id
        {
            get
            {
                return _id;
            }
        }

        public Int32 customer_id
        {
            get
            {
                return _customer_id;
            }
        }

        public Int32 product_id
        {
            get
            {
                return _product_id;
            }
        }

        public Int32 seller_id
        {
            get
            {
                return _seller_id;
            }
        }

        public Int32 quantity
        {
            get
            {
                return _quantity;
            }
        }

        public Int64 contact_no
        {
            get
            {
                return _contact_no;
            }
        }

        public String street
        {
            get
            {
                return _street;
            }
        }

        public Int32 pincode
        {
            get
            {
                return _pincode;
            }
        }

        public Double total
        {
            get
            {
                return _total;
            }
        }

        public DateTime started_at
        {
            get
            {
                return _started_at;
            }
        }

        public Byte order_status
        {
            get
            {
                return _order_status;
            }
            set
            {
                _order_status = value;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 customer_id,
                                                   Int32 product_id,
                                                   Int32 seller_id,
                                                   Int32 quantity,
                                                   Int64 contact_no,
                                                   String street,
                                                   Int32 pincode,
                                                   Double total)  // For Registering New Order
        {
            try
            {
                // Insert new record / Register in Product Table
                String sql = "INSERT INTO `order` " +
                      "(`customer_id`, `product_id`, `seller_id`, `quantity`, `contact_no`, `street`, `pincode`, `total`) VALUES" +
                      "(@customer_id, @product_id, @seller_id, @quantity, @contact_no, @street, @pincode, @total)";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@contact_no", contact_no);
                cmd.Parameters.AddWithValue("@street", street);
                cmd.Parameters.AddWithValue("@pincode", pincode);
                cmd.Parameters.AddWithValue("@total", total);
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

        public async static Task<COrder> Retrieve(Int32 id)
        {
            COrder ret = null;
            try
            {
                String sql = "SELECT * FROM `order` WHERE `id` = @id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@id", id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Order with id '" + id + "' not found!";
                    return ret;
                }
                ret = new COrder(id,
                                 reader.GetInt32(reader.GetOrdinal("customer_id")),
                                 reader.GetInt32(reader.GetOrdinal("product_id")),
                                 reader.GetInt32(reader.GetOrdinal("seller_id")),
                                 reader.GetInt32(reader.GetOrdinal("quantity")),
                                 reader.GetInt64(reader.GetOrdinal("contact_no")),
                                 reader.GetString(reader.GetOrdinal("street")),
                                 reader.GetInt32(reader.GetOrdinal("pincode")),
                                 reader.GetDouble(reader.GetOrdinal("total")),
                                 reader.GetDateTime(reader.GetOrdinal("started_at")),
                                 (await reader.IsDBNullAsync(reader.GetOrdinal("order_status"))) ? (Byte)0 : reader.GetByte(reader.GetOrdinal("order_status")));
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
                String sql = "DELETE FROM `order` WHERE `id` = @id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@id", id);
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

        public async Task<Boolean> Commit(Byte order_status)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `order` SET ");
                if (!(this._order_status == order_status))
                {
                    hasChange = true;
                    sql.Append("`order_status` = @order_status");
                    cmd.Parameters.AddWithValue("@order_status", order_status);
                    this._order_status = order_status;
                }
                if (!hasChange)
                {
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `id` = @id");
                cmd.Parameters.AddWithValue("@id", this._id);
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
        public async static Task<List<COrder>> RetrieveOrdertByCustomerID(Int32 customer_id)
        {
            List<COrder> ret = new List<COrder>();
            try
            {
                String sql = "SELECT * FROM `order` WHERE `customer_id` = @customer_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new COrder(reader.GetInt32(reader.GetOrdinal("id")),
                                 customer_id,
                                 reader.GetInt32(reader.GetOrdinal("product_id")),
                                 reader.GetInt32(reader.GetOrdinal("seller_id")),
                                 reader.GetInt32(reader.GetOrdinal("quantity")),
                                 reader.GetInt64(reader.GetOrdinal("contact_no")),
                                 reader.GetString(reader.GetOrdinal("street")),
                                 reader.GetInt32(reader.GetOrdinal("pincode")),
                                 reader.GetDouble(reader.GetOrdinal("total")),
                                 reader.GetDateTime(reader.GetOrdinal("started_at")),
                                 (await reader.IsDBNullAsync(reader.GetOrdinal("order_status"))) ? (Byte)0 : reader.GetByte(reader.GetOrdinal("order_status")))
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

        public async static Task<List<COrder>> RetrieveOrdertBySellerID(Int32 seller_id)
        {
            List<COrder> ret = new List<COrder>();
            try
            {
                String sql = "SELECT * FROM `order` WHERE `seller_id` = @seller_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new COrder(reader.GetInt32(reader.GetOrdinal("id")),
                                 reader.GetInt32(reader.GetOrdinal("customer_id")),
                                 reader.GetInt32(reader.GetOrdinal("product_id")),
                                 seller_id,
                                 reader.GetInt32(reader.GetOrdinal("quantity")),
                                 reader.GetInt64(reader.GetOrdinal("contact_no")),
                                 reader.GetString(reader.GetOrdinal("street")),
                                 reader.GetInt32(reader.GetOrdinal("pincode")),
                                 reader.GetDouble(reader.GetOrdinal("total")),
                                 reader.GetDateTime(reader.GetOrdinal("started_at")),
                                 (await reader.IsDBNullAsync(reader.GetOrdinal("order_status"))) ? (Byte)0 : reader.GetByte(reader.GetOrdinal("order_status")))
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
