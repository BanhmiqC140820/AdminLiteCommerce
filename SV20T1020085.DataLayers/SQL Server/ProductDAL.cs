using Dapper;
using SV20T1020085.DomainModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020085.DataLayers.SQL_Server
{
    public class ProductDAL : _BaseDAL, IProductDAL
    {
        public ProductDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Product data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO Products(ProductName, ProductDescription, SupplierID, CategoryID, Unit, Price, Photo, IsSelling)
                            VALUES(@ProductName, @ProductDescription, @SupplierID, @CategoryID, @Unit, @Price, @Photo, @IsSelling);
                            SELECT @@identity";
                var parameters = new
                {
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }

            return id;
        }

        public long AddAttribute(ProductAttribute data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductAttributes(ProductID, AttributeName, AttributeValue, DisplayOrder)
                            VALUES(@ProductID, @AttributeName, @AttributeValue, @DisplayOrder);
                            SELECT @@identity";

                var parameters = new
                {
                    ProductID = data.ProductID,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder,
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return id;
        }

        public long AddPhoto(ProductPhoto data)
        {
            int id = 0;

            using (var connection = OpenConnection())
            {
                var sql = @"INSERT INTO ProductPhotos(ProductID, Photo, Description, DisplayOrder, IsHidden)
                            VALUES(@ProductID, @Photo, @Description, @DisplayOrder, @IsHidden);
                            SELECT @@identity";

                var parameters = new
                {
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description,
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden,
                };

                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return id;
        }

        public int count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"select  count(*)
                            from    Products
                            where   (@SearchValue = N'' or ProductName like @SearchValue)
                                and (@CategoryID = 0 or CategoryID = @CategoryID)
                                and (@SupplierID = 0 or SupplierId = @SupplierID)
                                and (Price >= @MinPrice)
                                and (@MaxPrice <= 0 or Price <= @MaxPrice)";
                var parameters = new
                {
                    searchValue = searchValue,
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int productID)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"delete from Products where ProductID = @productId";
                var parameters = new
                {
                    ProductID = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteAttribute(long atrributeID)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductAttributes where AtrributeID = @atrributeID";
                var parameters = new
                {
                    atrributeID = atrributeID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeletePhoto(long photoID)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"delete from ProductPhotos where photoID = @photoID";
                var parameters = new
                {
                    photoID = photoID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Product? Get(int productID)
        {
            Product? data = null;

            using (var connection = OpenConnection())
            {
                var sql = @"select * from Products where ProductId = @productId";

                var parameters = new
                {
                    productId = productID
                };

                data = connection.QueryFirstOrDefault<Product>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductAttribute? GetAttribute(long attributeID)
        {
            ProductAttribute? data = null;

            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes where attributeID = @attributeID";

                var parameters = new
                {
                    attributeID = attributeID
                };

                data = connection.QueryFirstOrDefault<ProductAttribute>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public ProductPhoto? GetPhoto(long PhotoID)
        {
            ProductPhoto? data = null;

            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos where PhotoID = @PhotoID";

                var parameters = new
                {
                    PhotoID = PhotoID
                };

                data = connection.QueryFirstOrDefault<ProductPhoto>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public bool InUsed(int productID)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails where ProductID = @productID)
                                select 1
                            else 
                                select 0";

                var parameters = new
                {
                    productID = productID
                };

                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Product> List(int page = 1, int pageSize = 0, string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            List<Product> data = new List<Product>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as
                            (
                                select  *,
                                        row_number() over(order by ProductName) as RowNumber
                                from    Products
                                where   (@SearchValue = N'' or ProductName like @SearchValue)
                                    and (@CategoryID = 0 or CategoryID = @CategoryID)
                                    and (@SupplierID = 0 or SupplierId = @SupplierID)
                                    and (Price >= @MinPrice)
                                    and (@MaxPrice <= 0 or Price <= @MaxPrice)
                            )
                            select * from cte
                            where   (@PageSize = 0)
                                or (RowNumber between (@Page - 1)*@PageSize + 1 and @Page * @PageSize)";

                var parameters = new
                {
                    page = page,
                    pageSize = pageSize,
                    searchValue = searchValue,
                    CategoryID = categoryID,
                    SupplierID = supplierID,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };
                data = connection.Query<Product>(sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
                connection.Close();
            }
            return data;
        }

        public IList<ProductAttribute> ListAttributes(int productID)
        {
            List<ProductAttribute> data = new List<ProductAttribute>();

            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductAttributes
                            where (@productID = 0 or ProductID = @productID)";

                var parameters = new
                {
                    productID = productID
                };

                data = connection.Query<ProductAttribute>(sql: sql, param: parameters).ToList();
                connection.Close();
            }

            return data;
        }

        public IList<ProductPhoto> ListPhotos(int productID)
        {
            List<ProductPhoto> data = new List<ProductPhoto>();

            using (var connection = OpenConnection())
            {
                var sql = @"select * from ProductPhotos
                            where (@productID = 0 or ProductID = @productID) ";

                var parameters = new
                {
                    productID = productID
                };

                data = connection.Query<ProductPhoto>(sql: sql, param: parameters).ToList();
                connection.Close();
            }

            return data;
        }

        public bool Update(Product data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE Products
                            SET ProductName = @ProductName,
                                ProductDescription = @ProductDescription,
                                SupplierID = @SupplierID,
                                CategoryID = @CategoryID,
                                Unit = @Unit,
                                Price = @Price,
                                Photo = @Photo,
                                IsSelling = @IsSelling
                            WHERE ProductID = @ProductID";

                var parameters = new
                {
                    ProductID = data.ProductID,
                    ProductName = data.ProductName,
                    ProductDescription = data.ProductDescription,
                    SupplierID = data.SupplierID,
                    CategoryID = data.CategoryID,
                    Unit = data.Unit,
                    Price = data.Price,
                    Photo = data.Photo,
                    IsSelling = data.IsSelling,
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }

        public bool UpdateAttribute(ProductAttribute data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductAttributes
                            SET AttributeName = @AttributeName,
                                AttributeValue = @AttributeValue,
                                DisplayOrder = @DisplayOrder
                            WHERE AttributeID = @AttributeID";

                var parameters = new
                {
                    AttributeID = data.AttributeID,
                    ProductID =data.ProductID,
                    AttributeName = data.AttributeName,
                    AttributeValue = data.AttributeValue,
                    DisplayOrder = data.DisplayOrder,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool UpdatePhoto(ProductPhoto data)
        {
            bool result = false;

            using (var connection = OpenConnection())
            {
                var sql = @"UPDATE ProductPhotos
                            SET Photo = @Photo,
                                Description = @Description,
                                DisplayOrder = @DisplayOrder,
                                IsHidden = @IsHidden
                            WHERE PhotoID = @PhotoID";

                var parameters = new
                {
                    PhotoID = data.PhotoID,
                    ProductID = data.ProductID,
                    Photo = data.Photo,
                    Description = data.Description,
                    DisplayOrder = data.DisplayOrder,
                    IsHidden = data.IsHidden
                };

                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }

            return result;
        }
    }
}
