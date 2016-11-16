using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace OPS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        public static MySqlConnection conn = null;

        public static String szGoogleMapsAPI_key
        {
            get
            {
                return "AIzaSyCr9yTU0JEvh0KlU6pmBMVwjW1cWDzXfJ8";
            }
        }

        [STAThread]
        static void Main()
        {
            System.Threading.Thread.GetDomain().UnhandledException +=
                (sender, eventArgs) => CurrentDomain_ProcessExit((Exception)eventArgs.ExceptionObject);
            AppDomain.CurrentDomain.ProcessExit +=
                (sender, eventArgs) => CurrentDomain_ProcessExit(null);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());
        }

        static /*async */void CurrentDomain_ProcessExit(Exception ex)
        {
            // Logout Current User
            if (CUser.cur_user != null)
            {
                /*await CUser.cur_user.logout();*/
                CUser.cur_user = null;
            }

            // Force Disconnect On Process Exit or Unhandled Errors
            DisconnectSQL();
        }

        public async static void ConnectSQL()
        {
            try
            {
                MySqlConnectionStringBuilder szConn = new MySqlConnectionStringBuilder();
                szConn.Server = "localhost";
                szConn.UserID = "root";
                szConn.Password = "protected";
                szConn.PersistSecurityInfo = false;
                szConn.Database = "db_ops";
                conn = new MySqlConnection(szConn.ToString());
                await conn.OpenAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async static void DisconnectSQL()
        {
            try
            {
                if (conn == null || conn.State == ConnectionState.Closed)
                    return;
                await conn.CloseAsync();
                conn.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
