using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020085.BusinessLayers;
using SV20T1020085.DomainModels;
using SV20T1020085.Web.Models;

namespace SV20T1020085.Web.Controllers
{
    [Authorize(Roles =$"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CategoryController : Controller
    {

        private const int PAGE_SIZE = 20;
        private const string CATEGORY_SEARCH = "category_search";

        public IActionResult Index()
        {
            //Lấy dầu vào tìm kiếm hiên đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH);

            //Trường hợp trong session chưa có điều kiên thì tạo điều kiên mới
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfCategories(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            ApplicationContext.SetSessionData(CATEGORY_SEARCH, input);
            var model = new CategorySearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };


            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung loại hàng";
            Category model = new Category()
            {
                CategoryID = 0
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin loại hàng";
            Category? model = CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Category data)
        {
            try
            {
                ViewBag.Title = data.CategoryID == 0 ? "Bổ sung loại hàng" : "Cập nhật thông tin loại hàng";
                //Kiểm soát đầu vào và đưa các thông báo lỗi trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.CategoryName))
                    ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được để trống");

                // thông qua thuộc tính IsValid cỉa ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {

                    return View("Edit", data);
                }

                if (data.CategoryID == 0)
                {
                    int id = CommonDataService.AddCategory(data);
                }
                else
                {
                    bool result = CommonDataService.UpdateCategory(data);
                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa thông tin loại hàng";

            if(Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var model= CommonDataService.GetCategory(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataService.IsUsedCategory(id);
            return View(model);
        }
    }
}
