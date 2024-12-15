using SV21T1020547.DataLayers;
using SV21T1020547.DomainModels;

namespace SV21T1020547.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Category> categoryDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ISimpleQueryDAL<Province> provinceDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            customerDB = new DataLayers.SQLServer.CustomerDAL(connectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(connectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(connectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(connectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(connectionString);
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(connectionString);
        }

        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List();
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng dưới dạng phân trang
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue);
        }

        public static List<Customer> ListOfCustomers(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return customerDB.List(page, pageSize, searchValue);
        }

        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }

        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }

        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }

        public static bool DeleteCustomer(int id)
        {
            return customerDB.Delete(id);
        }

        public static bool InUsedCustomer(int id)
        {
            return customerDB.InUsed(id);
        }

        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue);
        }

        public static List<Category> ListOfCategories(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return categoryDB.List(page, pageSize, searchValue);
        }

        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }

        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }

        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }

        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }

        public static bool InUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }

        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue);
        }

        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }

        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }

        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }

        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }

        public static bool InUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }

        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue);
        }

        public static List<Supplier> ListOfSuppliers(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return supplierDB.List(page, pageSize, searchValue);
        }

        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }

        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }

        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }

        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }

        public static bool InUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }

        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue);
        }

        public static List<Shipper> ListOfShippers(int page = 1, int pageSize = 0, string searchValue = "")
        {
            return shipperDB.List(page, pageSize, searchValue);
        }

        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }

        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }

        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }

        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }

        public static bool InUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }
    }
}
