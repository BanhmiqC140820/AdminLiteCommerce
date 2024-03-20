using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using SV20T1020085.BusinessLayers;
using SV20T1020085.DomainModels;
using SV20T1020085.Web.Models;
using System.Reflection;

namespace SV20T1020085.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search";

        public IActionResult Index()
        {
            //Lấy dầu vào tìm kiếm hiên đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            //Trường hợp trong session chưa có điều kiên thì tạo điều kiên mới
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    minPrice = 0,
                    maxPrice = 0,
                };
            }

            return View(input);
        }

        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "",
                                                           input.CategoryID, input.SupplierID, input.minPrice, input.maxPrice);
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                data = data
            };
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.IsEdit = false;

            Product model = new Product()
            {
                ProductID = 0
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            ViewBag.IsEdit = true;
            Product? model = ProductDataService.GetProduct(id);
            ViewBag.photos = ProductDataService.ListPhotos(id);
            ViewBag.attributes = ProductDataService.ListAtributes(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Product data, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                ViewBag.IsEdit = data.ProductID == 0 ? false : true;

                if (uploadPhoto != null)
                {
                    string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");
                    string filePath = Path.Combine(folder, filename);// Đương dẫn đến file cần lưu

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = filename;

                }

                if (string.IsNullOrWhiteSpace(data.ProductName))
                {
                    ModelState.AddModelError("ProductName", "Tên mặt hàng không được để trống");
                }
                if (string.IsNullOrWhiteSpace(data.SupplierID.ToString()))
                {
                    ModelState.AddModelError(nameof(data.SupplierID), "Tên nhà cung cấp không được để trống");
                }
                if (string.IsNullOrWhiteSpace(data.CategoryID.ToString()))
                {
                    ModelState.AddModelError(nameof(data.CategoryID), "Tên loại hàng không được để trống");
                }
                if (string.IsNullOrWhiteSpace(data.Unit))
                {
                    ModelState.AddModelError("Unit", "Đơn vị tính không được để trống");
                }
                if (string.IsNullOrWhiteSpace(data.Price.ToString()))
                {
                    ModelState.AddModelError("Price", "Giá không được để trống");
                }
                // thông qua thuộc tính IsValid cỉa ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {
                    return View("Edit", data);
                }

                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại sau");
                return View("Edit", data);

            }
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa thông tin mặt hàng";
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            bool allowDelete = !ProductDataService.InUserProduct(id);
            ViewBag.AllowDelete = allowDelete;
            return View(model);
        }
       
        public IActionResult Photo(int id, string method, int photoID = 0)
        {
         
            ProductPhoto? model = new ProductPhoto();
            switch (method)
            {
                case "add":
                    
                    ViewBag.Title = "Bổ sung ảnh";
                    model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = id
                    };
                    break;
                case "edit":
                    ViewBag.Title = " Thay đổi ảnh";
                    model = ProductDataService.GetProductPhoto(Convert.ToInt64(photoID));
                    if (model == null)
                        return RedirectToAction("Edit", new { id = id });
                    break;

                case "delete":
                    // Todo: Xóa ảnh (xóa ảnh trực tiếp, không cần confirm)
                    ProductDataService.DeletePhoto(Convert.ToInt64(photoID));

                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile uploadPhoto)
        {
            
            //Xử lý ảnh upload
            if (uploadPhoto != null)
            {
                string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");
                string filePath = Path.Combine(folder, filename);// Đương dẫn đến file cần lưu

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = filename;
            }
            if (string.IsNullOrWhiteSpace(data.Photo))
            {
                ModelState.AddModelError("Photo", "Ảnh không được để trống");
            }
            if (string.IsNullOrWhiteSpace(data.Description))
            {
                ModelState.AddModelError("Description", "Mô tả không được để trống");
            }

            if (string.IsNullOrWhiteSpace(data.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }

            if (!ModelState.IsValid)
            {
                return View("Photo", data);
            }
            if (data.PhotoID == 0)
            {
               
                long id = ProductDataService.AddPhoto(data);
            }
            else
            {
           
                bool result = ProductDataService.UpdatePhoto(data);
            }
            return RedirectToAction("Edit", new{ id = data.ProductID });
        }

        public IActionResult Attribute(int id, String method, int attributeId = 0)
        {
            ProductAttribute? model = new ProductAttribute();
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = id
                    };
                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    model = ProductDataService.GetProductAttribute(Convert.ToInt64(attributeId));
                  
                    if (model == null)
                        return RedirectToAction("Edit", new { id = id });
                    return View(model);
                case "delete":
                    ProductDataService.DeleteAttribute(Convert.ToInt64(attributeId));

                    return RedirectToAction("Edit", new { id = id });
                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute? model)
        {
            if (string.IsNullOrWhiteSpace(model.AttributeName))
            {
                ModelState.AddModelError("AttributeName", "Tên thuộc tính không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.AttributeValue))
            {
                ModelState.AddModelError("AttributeValue", "Giá trị không được để trống");
            }
            if (string.IsNullOrWhiteSpace(model.DisplayOrder.ToString()))
            {
                ModelState.AddModelError("DisplayOrder", "Thứ tự không được để trống");
            }

            if (!ModelState.IsValid)
            {

                return View("Attribute", model);
            }

            if (model.AttributeID == 0)
            {
            
                long id = ProductDataService.AddAttribute(model);
            }
            else
            {
                bool result = ProductDataService.UpdateAttribute(model);   
            }
            return RedirectToAction("Edit", new { id = model.ProductID });

        }
    }
}
