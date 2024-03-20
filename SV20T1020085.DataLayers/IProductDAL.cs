using SV20T1020085.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020085.DataLayers
{
    public interface IProductDAL
    {
        /// <summary>
        /// TÌm kiếm và lấy danh sách mặt hàng dưới dạng phân trang
        /// </summary>
        /// <param name="page">TRang cần hiển thị</param>
        /// <param name="pageSize">Số dòng trên mỗi trang (0 nếu không phân trang)</param>
        /// <param name="searchValue">Tên mặt hàng cân tìm (chuỗi rỗng nếu không tìm kiếm )</param>
        /// <param name="categoryID">Mã loại hàng cần tìm (0 nếu không tìm theo loại hàng)</param>
        /// <param name="supplierID">Mã nhà cung cấp cần tìm(0 nếu không tìm thấy nhà cung cấp)</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lớn nhất trong khoảng giá cần tìm</param>
        /// <returns></returns>
        IList<Product> List(int page=1, int pageSize=0,
                            string searchValue="", int categoryID=0, int supplierID=0,
                            decimal minPrice=0, decimal maxPrice =0);
        /// <summary>
        /// Đếm số lương mặt hàng tìm kiếm được
        /// </summary>
        /// <param name="searchValue">Tên mặt hàng cân tìm (chuỗi rỗng nếu không tìm kiếm )</param>
        /// <param name="categoryID">Mã loại hàng cần tìm (0 nếu không tìm theo loại hàng)</param>
        /// <param name="supplierID">Mã nhà cung cấp cần tìm(0 nếu không tìm thấy nhà cung cấp)</param>
        /// <param name="minPrice">Mức giá nhỏ nhất trong khoảng giá cần tìm</param>
        /// <param name="maxPrice">Mức giá lớn nhất trong khoảng giá cần tìm</param>
        /// <returns></returns>
        int count(string searchValue = "", int categoryID = 0, int supplierID = 0, decimal minPrice = 0, decimal maxPrice = 0);
        /// <summary>
        /// Lấy thông tim mặt hàng theo mã hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        Product? Get(int productID);
        /// <summary>
        /// Bổ sung mặt hàng mới (hàm trả về mã của mặt hàng được bổ sung)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        int Add(Product data);
        /// <summary>
        /// Cập nhật thông tin mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(Product data);
        /// <summary>
        /// Xóa mặt hàng
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool Delete(int productID);
        /// <summary>
        /// Kiểm tra xem mặt hàng hiện có đơn hàng liên quan hay không ?
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        bool InUsed(int  productID);
        /// <summary>
        /// Lấy danh sách ảnh của mặt hàng (sắp xếp theo thứ tự của DisplayOrder)
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductPhoto> ListPhotos(int productID);
        /// <summary>
        /// Lấy thông tin 1 ảnh dựa vào ID
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        ProductPhoto? GetPhoto(long PhotoID);
        /// <summary>
        /// Bổ sung 1 ảnh cho mặt hàng (hàm trả về mã của ảnh được bổ sung)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddPhoto(ProductPhoto data);
        /// <summary>
        /// Cập nhật ảnh của mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdatePhoto(ProductPhoto data);
        /// <summary>
        /// Xóa ảnh của mặt hàng
        /// </summary>
        /// <param name="photoID"></param>
        /// <returns></returns>
        bool DeletePhoto(long photoID);
        /// <summary>
        /// Lấy danh sách các thuộc tính của mặt hàng, sắp xếp theo thứ tự của DisplayOrder
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        IList<ProductAttribute> ListAttributes (int productID);
        /// <summary>
        /// Lấy thông tin của thuộc  tính theo mã thuộc tính
        /// </summary>
        /// <param name="atrributeID"></param>
        /// <returns></returns>
        ProductAttribute? GetAttribute(long atrributeID);
        /// <summary>
        /// Bổ sung thuộc tính mặt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        long AddAttribute(ProductAttribute data);
        /// <summary>
        /// Cập nhật thuộc tính của măt hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool UpdateAttribute( ProductAttribute data);
        /// <summary>
        /// Xóa thuộc tính
        /// </summary>
        /// <param name="atrributeID"></param>
        /// <returns></returns>
        bool DeleteAttribute(long atrributeID);
    }
}
