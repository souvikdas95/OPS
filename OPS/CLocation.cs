using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPS
{
    class CLocation
    {
        // data
        private Int32 _pincode;
        private String _city;
        private String _state;
        private String _country;

        // constructors
        private CLocation(Int32 pincode,
                               String city,
                               String state,
                               String country)
        {
            this._pincode = pincode;
            this._city = city;
            this._state = state;
            this._country = country;
        }

        // GET; SET; properties (wrappers)
        public Int32 pincode
        {
            get
            {
                return _pincode;
            }
        }

        public String city
        {
            get
            {
                return _city;
            }
        }

        public String state
        {
            get
            {
                return _state;
            }
        }

        public String country
        {
            get
            {
                return _country;
            }
        }

        // core methods
        public async static Task<Boolean> Register(Int32 pincode,
                                                   String city,
                                                   String state,
                                                   String country)  // For Registering New Location
        {
            try
            {
                // Check if Entry with Catagory ID and Name already exists 
                String sql = "SELECT * FROM `location` WHERE `pincode` = @pincode LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@pincode", pincode);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (await reader.ReadAsync())
                {
                    CUtils.LastLogMsg = "Location Already Exists!";
                    if (!reader.IsClosed)
                        reader.Close();
                    return false;
                }
                if (!reader.IsClosed)
                    reader.Close();

                // Insert new record / Register in Product Table
                sql = "INSERT INTO `location` " +
                      "(`pincode`, `city`, `state`, `country`) VALUES" +
                      "(@pincode, @city, @state, @country)";
                cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@pincode", pincode);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@country", country);
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

        public async static Task<CLocation> Retrieve(Int32 pincode)
        {
            CLocation ret = null;
            try
            {
                String sql = "SELECT * FROM `location` WHERE `pincode` = @pincode LIMIT 1";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@pincode", pincode);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                if (!(await reader.ReadAsync()))
                {
                    if (!reader.IsClosed)
                        reader.Close();
                    CUtils.LastLogMsg = "Location with pincode '" + pincode + "' not found!";
                    return ret;
                }
                ret = new CLocation(pincode,
                                    reader.GetString(reader.GetOrdinal("city")),
                                    reader.GetString(reader.GetOrdinal("state")),
                                    reader.GetString(reader.GetOrdinal("country")));
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

        public async static Task<Boolean> Remove(Int32 pincode)
        {
            try
            {
                String sql = "DELETE FROM `location` WHERE `pincode` = @pincode";
                MySqlCommand cmd = new MySqlCommand(sql, Program.conn);
                cmd.Parameters.AddWithValue("@pincode", pincode);
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

        public async Task<Boolean> Commit(String city,
                                          String state,
                                          String country)  // For Making Changes to Existing Class
        {
            try
            {
                Boolean hasChange = false;
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = Program.conn;
                StringBuilder sql = new StringBuilder("UPDATE `location` SET ");
                if (!this._city.Equals(city))
                {
                    hasChange = true;
                    sql.Append("`city` = @city");
                    cmd.Parameters.AddWithValue("@city", city);
                    this._city = city;
                }
                if (!(this._state.Equals(state)))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`state` = @state");
                    cmd.Parameters.AddWithValue("@state", state);
                    this._state = state;
                }
                if (!(this._country.Equals(country)))
                {
                    if (hasChange)
                        sql.Append(", ");
                    else
                        hasChange = true;
                    sql.Append("`country` = @country");
                    cmd.Parameters.AddWithValue("@country", country);
                    this._country = country;
                }
                if (!hasChange)
                {
                    CUtils.LastLogMsg = null;
                    return false;
                }
                sql.Append(" WHERE `pincode` = @pincode");
                cmd.Parameters.AddWithValue("@pincode", this._pincode);
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
        public async static Task<List<CLocation>> RetrieveLocationList()
        {
            List<CLocation> ret = new List<CLocation>();
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM `location`", Program.conn);
                DbDataReader reader = await cmd.ExecuteReaderAsync();
                cmd.Dispose();
                while (await reader.ReadAsync())
                {
                    ret.Add
                    (
                        new CLocation(reader.GetInt32(reader.GetOrdinal("pincode")),
                                      reader.GetString(reader.GetOrdinal("city")),
                                      reader.GetString(reader.GetOrdinal("state")),
                                      reader.GetString(reader.GetOrdinal("country")))
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
            return _pincode.ToString();
        }
    }
}
