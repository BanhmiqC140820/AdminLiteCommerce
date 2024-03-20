using SV20T1020085.DataLayers.SQL_Server;
using SV20T1020085.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020085.BusinessLayers
{
    public class ProductDataService
    {
        private static readonly ProductDAL productDB;

        static ProductDataService()
        {
            productDB = new ProductDAL(Configuration.ConnectionString);
        }

        public static List<Product> ListProducts(string searchValue = "")
        {
            return productDB.List(1, 0, searchValue, 0, 0, 0).ToList();
        }

        public static List<Product> ListProducts(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "",
                                                    int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0)
        {
            rowCount = productDB.count(searchValue, categoryID, supplierID, minPrice, maxPrice);
            return productDB.List(page, pageSize, searchValue, categoryID, supplierID, minPrice, maxPrice).ToList();
        }

        public static Product? GetProduct(int productID)
        {
            return productDB.Get(productID);
        }

        public static int AddProduct(Product data)
        {
            return productDB.Add(data);
        }

        public static bool UpdateProduct(Product data)
        {
            return productDB.Update(data);
        }

        public static bool DeleteProduct(int productID)
        {
            return productDB.Delete(productID);
        }

        public static bool InUserProduct(int productID)
        {
            return productDB.InUsed(productID);
        }

        public static List<ProductPhoto> ListPhotos(int productID)
        {
            return productDB.ListPhotos(productID).ToList();
        }

        public static ProductPhoto? GetProductPhoto(long photoID)
        {
            return productDB.GetPhoto(photoID);
        }

        public static ProductAttribute? GetProductAttribute(long attribute)
        {
            return productDB.GetAttribute(attribute);
        }

        public static long AddPhoto(ProductPhoto data)
        {
            return productDB.AddPhoto(data);
        }

        public static bool UpdatePhoto(ProductPhoto data)
        {
            return productDB.UpdatePhoto(data);
        }

        public static bool DeletePhoto(long photoID)
        {
            return productDB.DeletePhoto(photoID);
        }

        public static List<ProductAttribute> ListAtributes(int productID)
        {
            return productDB.ListAttributes(productID).ToList();
        }

        public static long AddAttribute(ProductAttribute data)
        {
            return productDB.AddAttribute(data);
        }

        public static bool UpdateAttribute(ProductAttribute data)
        {
            return productDB.UpdateAttribute(data);
        }

        public static bool DeleteAttribute(long attributeID)
        {
            return productDB.DeleteAttribute(attributeID);
        }
    }
}
