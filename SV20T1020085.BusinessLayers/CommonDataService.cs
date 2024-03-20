using SV20T1020085.DataLayers;
using SV20T1020085.DataLayers.SQL_Server;
using SV20T1020085.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020085.BusinessLayers
{
    /// <summary>
    /// Cung cấp chức năng xử lý dữ liệu chung
    /// (tỉnh thành, khách hàng, nhà cung cấp, loại hàng, người giao hàng, nhân viên)
    /// </summary>
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Category> categoryDB;

        /// <summary>
        /// Ctor (static contructor hoạt động như thế nào ?)
        /// </summary>
        static CommonDataService()
        {
            string connectionString = Configuration.ConnectionString;

            provinceDB = new ProvinceDAL(connectionString);
            supplierDB = new SupplierDAL(connectionString);
            customerDB = new CustomerDAL(connectionString);
            shipperDB = new ShipperDAL(connectionString);  
            employeeDB = new EmployeeDAL(connectionString);
            categoryDB = new CategoryDAL(connectionString);


        }
        /// <summary>
        /// DS tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }
        /// <summary>
        /// Lấy danh sách tất cả nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers()
        {
            return supplierDB.List().ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhà cung cấp
        /// </summary>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 nhà cung caaos theo mã nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier? GetSupplier(int id)
        {
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Bổ sung nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// Xóa nhà cung cấp có mã là id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem nhà cung cấp có mã id hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.IsUsed(id);
        }

        /// <summary>
        /// Tìm kiếm và lấy danh sách khách hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        public static List<Customer> ListOfCustomers()
        {
            return customerDB.List(1,0,"").ToList();
        }
        /// <summary>
        /// Lấy thông tin khách hàng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            return customerDB.Get(id);
        }
        /// <summary>
        /// Thêm mới khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer customer)
        {
            return customerDB.Add(customer);
        }
        /// <summary>
        /// Cập nhật khách hàng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer customer)
        {
            return customerDB.Update(customer);
        }
        /// <summary>
        /// Xóa khách hàng với ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id)
        {
            if (customerDB.IsUsed(id))
            {
                return false;
            }
            return customerDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra khách hàng có dữ liệu liên quan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUserCustomer(int id)
        {
            return customerDB.IsUsed(id);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Bổ sung người giao hàng mới
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Bổ sung người giao hàng mới
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper shipper)
        {
            return shipperDB.Add(shipper);
        }
        /// <summary>
        /// Cập nhật người giao hàng
        /// </summary>
        /// <param name="shipper"></param>
        /// <returns></returns>
        public static bool UpdateShipper (Shipper shipper)
        {
            return shipperDB.Update(shipper);
        }
        /// <summary>
        /// Xóa người giao hàng có mã id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }
        /// <summary>
        ///  Kiểm tra xem người giao hàng có mã id hiện có dữ liệu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper (int id)
        {
            return shipperDB.IsUsed(id);
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách nhân viên
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "") {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize,searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 nhân viên theo mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee? GetEmployee(int id)
        {
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Bổ sung nhân viên mới
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static int AddEmployee (Employee employee)
        {
            return employeeDB.Add(employee);
        }
        /// <summary>
        /// Cập nhật nhân viên
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee employee)
        {
            return employeeDB.Update(employee);
        }
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra xem nhân viên có mã id hiện có dữ liêu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee (int id)
        {
            return employeeDB.IsUsed(id);
        }

        /// <summary>
        /// lấy danh sách loại hàng
        /// </summary>
        /// <returns></returns>
        public static List<Category> ListOfCategories()
        {
            return categoryDB.List().ToList();
        }
        /// <summary>
        /// Tìm kiếm và lấy danh sách loại hàng
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories (out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 loại hàng theo mã loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            return categoryDB.Get(id);
        }
        /// <summary>
        /// Bổ sung loại hàng mới
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static int AddCategory (Category category)
        {
            return categoryDB.Add(category);
        }
        /// <summary>
        /// Cập nhật loại hàng
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static bool UpdateCategory (Category category)
        {
            return categoryDB.Update(category);
        }
        /// <summary>
        /// Xóa loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory (int id)
        {
            return categoryDB.Delete(id);
        }
        /// <summary>
        ///  Kiểm tra xem loại hàng có mã id hiện có dữ liêu liên quan hay không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory (int id)
        {
            return categoryDB.IsUsed(id);
        }
    }
}
