using SV21T1020547.DataLayers;
using SV21T1020547.DataLayers.SQLServer;
using SV21T1020547.DomainModels;

namespace SV21T1020547.BusinessLayers
{
    public static class UserAccountDataService
    {
        private static readonly IUserAccountDAL employeeAccountDB;
        private static readonly IUserAccountDAL customerAccountDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static UserAccountDataService()
        {
            employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
            customerAccountDB = new CustomerAccountDAL(Configuration.ConnectionString);
        }

        public static UserAccount? Authorize(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
                return employeeAccountDB.Authorize(username, password);
            else
                return customerAccountDB.Authorize(username, password);
        }

        public static bool ChangePassword(UserTypes userTypes, string username, string password)
        {
            if (userTypes == UserTypes.Employee)
                return employeeAccountDB.ChangePassword(username, password);
            else
                return customerAccountDB.ChangePassword(username, password);
        }
    }

    public enum UserTypes
    {
        Employee,
        Customer,
    }
}
