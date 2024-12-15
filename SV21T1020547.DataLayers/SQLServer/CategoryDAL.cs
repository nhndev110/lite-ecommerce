using Dapper;
using SV21T1020547.DomainModels;
using System.Data;

namespace SV21T1020547.DataLayers.SQLServer
{
    public class CategoryDAL : BaseDAL, ICommonDAL<Category>
    {
        public CategoryDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Category data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                string sql = @"if exists(select * from Categories where CategoryName = @CategoryName)
                                    select -1;
                                else
                                begin
                                    insert into Categories (CategoryName, Description)
                                    values (@CategoryName, @Description);
                                    select SCOPE_IDENTITY();
                                end";
                var parameters = new { data.CategoryName, data.Description };
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
			                from	Categories
			                where	CategoryName like @searchValue";
                var parameters = new { searchValue };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                string sql = @"delete from Categories where CategoryID = @CategoryID";
                var parameters = new { CategoryID = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }

            return result;
        }

        public Category? Get(int id)
        {
            Category? data = null;

            using (var connection = OpenConnection())
            {
                string sql = @"select * from Categories where CategoryID = @CategoryID";
                var parameters = new { CategoryID = id };
                data = connection.QueryFirstOrDefault<Category>(sql: sql, param: parameters, commandType: CommandType.Text);
            }

            return data;
        }

        public bool InUsed(int id)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists (select * from Products where CategoryID = @CategoryID) select 1
                            else select 0";
                var parameters = new { CategoryID = id };
                result = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }

            return result;
        }

        public List<Category> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Category> data = [];
            searchValue = $"%{searchValue}%";
            using (var connection = OpenConnection())
            {
                var sql = @"select	*
                            from	(
			                            select	ROW_NUMBER() over (order by CategoryName) as RowNumber, *
			                            from	Categories
			                            where	CategoryName like @searchValue
		                            ) as t
                            where	(@pageSize = 0) or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)
                            order by RowNumber";
                var parameters = new { searchValue, page, pageSize };
                data = connection.Query<Category>(sql: sql,
                                                  param: parameters,
                                                  commandType: CommandType.Text).ToList();

                connection.Close();
            }

            return data;
        }

        public bool Update(Category data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if not exists(select * from Categories where CategoryName = @CategoryName)
                            begin
                                update Categories
                                set
	                                CategoryName = @CategoryName,
	                                Description = @Description
                                where CategoryID = @CategoryID
                            end";
                var parameters = new { data.CategoryName, data.Description, data.CategoryID };
                result = connection.Execute(sql: sql, param: parameters, commandType: CommandType.Text) > 0;
            }

            return result;
        }
    }
}
