﻿using Microsoft.Data.SqlClient;

namespace SV21T1020547.DataLayers.SQLServer
{
    public abstract class BaseDAL
    {
        protected string _connectionString = "";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="connectionString">Chuỗi tham số kết nối với CSDL</param>
        public BaseDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Tạo và mở một kết nối đến CSDL (SQL Server)
        /// </summary>
        /// <returns></returns>
        protected SqlConnection OpenConnection()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
