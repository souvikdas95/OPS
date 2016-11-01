using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    class CProduct
    {
        // data
        private Int32 _id;
        private Int32 _catagory_id;
        private String _name;
        private String _description;
        private Double _minprice;
        private Int32 _sales;
	    private Int32 _raters;
	    private Double _rating;
        private Int32 _quantity;
        private Image _image;

        // constructors
        private CProduct(Int32 id,
                        Int32 catagory_id,
                        String name,
                        String description,
                        Double minprice,
                        Int32 sales,
                        Int32 raters,
                        Double rating,
                        Int32 quantity,
                        Image image)
        {
            this._id = id;
            this._catagory_id = catagory_id;
            this._name = name;
            this._description = description;
            this._minprice = minprice;
            this._sales = sales;
            this._raters = raters;
            this._rating = rating;
            this._quantity = quantity;
            this._image = image;
        }

        // GET; SET; properties (wrappers)
        public Int32 id
        {
            get
            {
                return _id;
            }
        }

        public Int32 catagory_id
        {
            get
            {
                return _catagory_id;
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

        public Double minprice
        {
            get
            {
                return _minprice;
            }
        }

        public Int32 sales
        {
            get
            {
                return _sales;
            }
        }

        public Int32 raters
        {
            get
            {
                return _raters;
            }
        }

        public Double rating
        {
            get
            {
                return _rating;
            }
        }

        public Int32 quantity
        {
            get
            {
                return _quantity;
            }
        }

        public Image image
        {
            get
            {
                return _image;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 catagory_id,
                                                   String name,
                                                   Int32 description,
                                                   Image image)  // For Registering New Product
        {
            try
            {
                // Check if Entry with Catagory ID and Name already exists
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                String sql = "SELECT * FROM `product` WHERE `name` = @name";
                cmd.Parameters.AddWithValue("@name", name);
                if (catagory_id != 0)
                {
                    sql += " AND `catagory_id` = @catagory_id";
                    cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                }
                else
                    sql += " AND `catagory_id` = '0' OR `catagory_id` is NULL";
                sql += " LIMIT 1";
                cmd.CommandText = sql;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "Product Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Product Table
                MemoryStream memStream = new MemoryStream();
                image.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                sql = "INSERT INTO `product` " +
                      "(`catagory_id`, `name`, `description`, `image`) VALUES" +
                      "(@catagory_id, @name, @description, @image)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@image", Encoding.UTF8.GetString(memStream.ToArray()));
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

        public async static Task<CProduct> Retrieve(Int32 id)
        {
            CProduct ret = null;
            try
            {
                String sql = "SELECT * FROM `product` WHERE `id` = @id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@id", id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Product with id '" + id + "' not found!";
                    return ret;
                }
                ret = new CProduct(id,
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("catagory_id"))) ? 0 : reader.GetInt32(reader.GetOrdinal("catagory_id")),
                                   reader.GetString(reader.GetOrdinal("name")),
                                   reader.GetString(reader.GetOrdinal("description")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("minprice"))) ? 0 : reader.GetDouble(reader.GetOrdinal("minprice")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("sales"))) ? 0 : reader.GetInt32(reader.GetOrdinal("sales")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("raters"))) ? 0 : reader.GetInt32(reader.GetOrdinal("raters")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("rating"))) ? 0 : reader.GetDouble(reader.GetOrdinal("rating")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("quantity"))) ? 0 : reader.GetInt32(reader.GetOrdinal("quantity")),
                                   Image.FromStream(reader.GetStream(reader.GetOrdinal("image"))));
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
                String sql = "DELETE FROM `product` WHERE `id` = @id";
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

        public async Task<Boolean> Commit(Int32 catagory_id,
                                          String name,
                                          String description,
                                          Image image)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `catagory` SET ");
                if (!(this._catagory_id == catagory_id))
                {
                    hasChange = true;
                    sql.Append("`catagory_id` = @catagory_id");
                    cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                    this._name = name;
                }
                if (!this._name.Equals(name))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`name` = @name");
                    cmd.Parameters.AddWithValue("@name", name);
                    this._name = name;
                }
                if (!this._description.Equals(description))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`description` = @description");
                    cmd.Parameters.AddWithValue("@description", description);
                    this._description = description;
                }
                if (!(this._image.Equals(image)))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    MemoryStream memStream = new MemoryStream();
                    image.Save(memStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    sql.Append("`image` = @image");
                    cmd.Parameters.AddWithValue("@image", Encoding.UTF8.GetString(memStream.ToArray()));
                    this._image = image;
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
        public async static Task<List<CProduct>> RetrieveProductList(Int32 catagory_id)
        {
            List<CProduct> ret = new List<CProduct>();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                String sql = "SELECT * FROM `product`";
                if (catagory_id != 0)
                {
                    sql += " WHERE `catagory_id` = @catagory_id";
                    cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                }
                else
                    sql += " WHERE `catagory_id` = '0' OR `catagory_id` is NULL";
                cmd.CommandText = sql;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CProduct(reader.GetInt32(reader.GetOrdinal("id")),
                                   catagory_id,
                                   reader.GetString(reader.GetOrdinal("name")),
                                   reader.GetString(reader.GetOrdinal("description")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("minprice"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("minprice")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("sales"))) ? 0 : reader.GetInt32(reader.GetOrdinal("sales")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("raters"))) ? 0 : reader.GetInt32(reader.GetOrdinal("raters")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("rating"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("rating")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("quantity"))) ? 0 : reader.GetInt32(reader.GetOrdinal("quantity")),
                                   Image.FromStream(reader.GetStream(reader.GetOrdinal("image"))))
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

        public async static Task<BindingList<CProduct>> SearchProductBindingList(Int32 catagory_id, String keyword)
        {
            BindingList<CProduct> ret = new BindingList<CProduct>();
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                String sql = "SELECT * FROM `product`";
                if (catagory_id != 0)
                {
                    sql += " WHERE `catagory_id` = @catagory_id";
                    cmd.Parameters.AddWithValue("@catagory_id", catagory_id);
                }
                else
                    sql += " WHERE (`catagory_id` = '0' OR `catagory_id` is NULL)";
                sql += " AND `name` LIKE CONCAT('%', '" + keyword + "', '%')";
                cmd.CommandText = sql;
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CProduct(reader.GetInt32(reader.GetOrdinal("id")),
                                   catagory_id,
                                   reader.GetString(reader.GetOrdinal("name")),
                                   reader.GetString(reader.GetOrdinal("description")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("minprice"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("minprice")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("sales"))) ? 0 : reader.GetInt32(reader.GetOrdinal("sales")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("raters"))) ? 0 : reader.GetInt32(reader.GetOrdinal("raters")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("rating"))) ? 0.0 : reader.GetDouble(reader.GetOrdinal("rating")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("quantity"))) ? 0 : reader.GetInt32(reader.GetOrdinal("quantity")),
                                   (await reader.IsDBNullAsync(reader.GetOrdinal("image"))) ? global::OPS.Properties.Resources.noimage : Image.FromStream(reader.GetStream(reader.GetOrdinal("image"))))
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
