using Dapper;
using SV21T1020547.DomainModels;
using System.Data;

namespace SV21T1020547.DataLayers.SQLServer
{
    public class CustomerAccountDAL(string connectionString) : BaseDAL(connectionString), IUserAccountDAL
    {
        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? userAccount = null;

            using (var conn = OpenConnection())
            {
                var sql = @"SELECT  CustomerID as UserID,
                                    Email as UserName,
                                    CustomerName as DisplayName,
                                    N'' as Photo,
                                    N'' as RoleNames
                            FROM    Customers
                            WHERE   Email = @Email AND Password = @Password;";
                var parameters = new { Email = username, Password = password };
                userAccount = conn.QueryFirstOrDefault<UserAccount>(sql, parameters, commandType: CommandType.Text);
                conn.Close();
            }

            return userAccount;
        }

        public bool ChangePassword(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
