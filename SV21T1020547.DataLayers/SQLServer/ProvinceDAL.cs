﻿using Dapper;
using SV21T1020547.DomainModels;
using System.Data;

namespace SV21T1020547.DataLayers.SQLServer
{
    public class ProvinceDAL : BaseDAL, ISimpleQueryDAL<Province>
    {
        public ProvinceDAL(string connectionString) : base(connectionString)
        {
        }

        public List<Province> List()
        {
            List<Province> data = new List<Province>();
            using (var connection = OpenConnection())
            {
                var sql = @"select	*
			                from	Provinces";
                data = connection.Query<Province>(sql: sql, commandType: CommandType.Text).ToList();
            }

            return data;
        }
    }
}
