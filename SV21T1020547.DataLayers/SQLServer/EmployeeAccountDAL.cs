using Dapper;
using SV21T1020547.DomainModels;
using System.Data;

namespace SV21T1020547.DataLayers.SQLServer
{
    public class EmployeeAccountDAL(string connectionString) : BaseDAL(connectionString), IUserAccountDAL
    {
        public UserAccount? Authorize(string username, string password)
        {
            UserAccount? userAccount = null;

            using (var conn = OpenConnection())
            {
                var sql = @"SELECT  EmployeeID as UserID,
                                    Email as UserName,
                                    FullName as DisplayName,
                                    IsWorking as Active,
                                    Photo, RoleNames
                            FROM    Employees
                            WHERE   Email = @Email AND Password = @Password;";
                var parameters = new { Email = username, Password = password };
                userAccount = conn.QueryFirstOrDefault<UserAccount>(sql, parameters, commandType: CommandType.Text);
                conn.Close();
            }

            return userAccount;
        }

        public bool ChangePassword(string username, string password)
        {
            bool isChanged = false;

            using (var conn = OpenConnection())
            {
                var sql = @"UPDATE  Employees
                            SET     Password = @Password
                            WHERE   Email = @Username";
                var parameters = new
                {
                    Password = password,
                    Username = username,
                };
                isChanged = conn.Execute(sql, parameters, commandType: CommandType.Text) > 1;
                conn.Close();
            }

            return isChanged;
        }
    }
}
