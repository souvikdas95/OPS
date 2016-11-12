﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    public class CCustomer_Cart
    {
        // data
        private Int32 _customer_id;
	    private Int32 _product_id;
	    private Int32 _seller_id;
	    private Int32 _quantity;

        // consutructors
        private CCustomer_Cart(Int32 customer_id,
                               Int32 product_id,
                               Int32 seller_id,
                               Int32 quantity)
        {
            this._customer_id = customer_id;
            this._product_id = product_id;
            this._seller_id = seller_id;
            this._quantity = quantity;
        }

        // GET; SET; properties (wrappers)
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

        // core methods
        public async static Task<Boolean> Register(Int32 customer_id,
                                                   Int32 product_id,
                                                   Int32 seller_id,
                                                   Int32 quantity)  // For Registering New Inventory
        {
            try
            {
                // Check if Entry with Seller ID and Product ID already exists in Inventory
                String sql = "SELECT * FROM `customer_cart` WHERE `customer_id` = @customer_id and `product_id` = @product_id and `seller_id` = @seller_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Cart Already Exists!";
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Inventory Table
                sql = "INSERT INTO `customer_cart` " +
                      "(`customer_id`, `product_id`, `seller_id`, `quantity`) VALUES" +
                      "(@customer_id, @product_id, @seller_id, @quantity)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                cmd.Parameters.AddWithValue("@quantity", quantity);
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

        public async static Task<CCustomer_Cart> Retrieve(Int32 customer_id,
                                                          Int32 product_id,
                                                          Int32 seller_id)
        {
            CCustomer_Cart ret = null;
            try
            {
                String sql = "SELECT * FROM `customer_cart` WHERE `customer_id` = @customer_id and `product_id` = @product_id and `seller_id` = @seller_id LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Cart with customer_id '" + customer_id + "' and product_id '" + product_id + "' and seller_id '" + seller_id +  "' not found!";
                    return ret;
                }
                ret = new CCustomer_Cart(customer_id,
                                         product_id,
                                         seller_id,
                                         (await reader.IsDBNullAsync(reader.GetOrdinal("quantity"))) ? 0 : reader.GetInt32(reader.GetOrdinal("quantity")));
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

        public async static Task<Boolean> Remove(Int32 customer_id, Int32 product_id, Int32 seller_id)
        {
            try
            {
                String sql = "DELETE FROM `customer_cart` WHERE `customer_id` = @customer_id and `product_id` = @product_id and `seller_id` = @seller_id";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@customer_id", customer_id);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@seller_id", seller_id);
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

        public async Task<Boolean> Commit(Int32 quantity)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `customer_cart` SET ");
                if (!(this._quantity == quantity))
                {
                    hasChange = true;
                    sql.Append("`quantity` = @quantity");
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    this._quantity = quantity;
                }
                if (!hasChange)
                {
                    sql.Clear();
                    cmd.Dispose();
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `customer_id` = @customer_id AND `product_id` = @product_id AND `seller_id` = @seller_id");
                cmd.Parameters.AddWithValue("@customer_id", this._customer_id);
                cmd.Parameters.AddWithValue("@product_id", this._product_id);
                cmd.Parameters.AddWithValue("@seller_id", this._seller_id);
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
