﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
namespace Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class OracleConnHelper
    {
        //定义一个SqlServer Connection对象
        private static OracleConnection  conn;
     
        //连接字符串
        private static String oracleConnStr;
      /// <summary>
      /// 
      /// </summary>
        public OracleConnHelper() {

        }
        #region 初始化连接字符串
        private static void initConStr() {
            XmlFlexflow xff = new XmlFlexflow();
            string host_Name =  xff.ReadXmlNodeValue("SERVER_NAME");
            string user_Id = xff.ReadXmlNodeValue("USER_ID");
            string password = xff.ReadXmlNodeValue("PASSWORD");

            oracleConnStr = String.Format(@"Data Source=(DESCRIPTION="
              + "(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0} )(PORT=1521)))"
              + "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));"
              + "User Id={1};Password={2};", host_Name, user_Id, password);
        }
        #endregion
        #region 打开连接对象
        private static void openConn() {
            initConStr();
            conn = new OracleConnection(oracleConnStr);
            try
            {
                conn.Open();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.ToString(),"提示：",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
        #endregion
        /// <summary>
        /// 获取已经打开的连接对象。
        /// </summary>
        /// <returns></returns>
        public static OracleConnection getConn()
        {
            openConn();
            return conn;
        }
        /// <summary>
        /// 获取一个单独的连接对象。
        /// </summary>
        /// <returns></returns>
        public static OracleConnection getTheConn()
        {
            initConStr();
            OracleConnection theConn = new OracleConnection(oracleConnStr);
            try
            {
                theConn.Open();
                return theConn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }
        /// <summary>
        /// 关闭连接对象。
        /// </summary>
        public static void closeConn(){
            if (conn != null &&  conn.State == ConnectionState.Open)
                conn.Close();
        }
        /// <summary>
        /// 关闭指定的链接对象
        /// </summary>
        /// <param name="conn"></param>
        public static void closeConn(OracleConnection conn) {
            if (conn != null && conn.State ==ConnectionState.Open)
                conn.Close();
        }
    }
}
