﻿using Dapper;
using SV21T1020547.DomainModels;
using System.Data;

namespace SV21T1020547.DataLayers.SQLServer
{
    public class ShipperDAL : BaseDAL, ICommonDAL<Shipper>
    {
        public ShipperDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Shipper data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Shippers where Phone = @Phone)
                                select -1;
                            else
                            begin
                                insert into Shippers(ShipperName, Phone)
                                values (@ShipperName, @Phone);
                                select SCOPE_IDENTITY();
                            end";
                var parameters = new
                {
                    ShipperName = data.ShipperName,
                    Phone = data.Phone,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select	count(*)
			                from	Shippers
			                where	ShipperName like @searchValue";
                var parameters = new { searchValue };
                count = connection.ExecuteScalar<int>(sql, param: parameters, commandType: CommandType.Text);
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"delete from Shippers where ShipperID = @ShipperID";
                var parameters = new
                {
                    ShipperID = id
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        public Shipper? Get(int id)
        {
            Shipper? data = null;

            using (var connection = OpenConnection())
            {
                var sql = @"select * from Shippers where ShipperID = @ShipperID";
                var parameters = new
                {
                    ShipperID = id
                };
                data = connection.QueryFirstOrDefault<Shipper>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }

            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where ShipperID = @ShipperID) select 1
                            else select 0";
                var parameters = new
                {
                    ShipperID = id
                };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        public List<Shipper> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Shipper> data = new List<Shipper>();
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select	*
                            from	(
			                            select	ROW_NUMBER() over (order by ShipperName) as RowNumber, *
			                            from	Shippers
			                            where	ShipperName like @searchValue
		                            ) as t
                            where	(@pageSize = 0) or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new { searchValue, page, pageSize };
                data = connection.Query<Shipper>(sql, param: parameters, commandType: CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Shipper data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Shippers where ShipperID <> @ShipperID and Phone = @Phone)
                            begin
                                update Shippers
                                set
                                    ShipperName = @ShipperName,
                                    Phone = @Phone
                                where ShipperID = @ShipperID
                            end";
                var parameters = new
                {
                    ShipperName = data.ShipperName,
                    Phone = data.Phone,
                    ShipperID = data.ShipperID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }
    }
}
