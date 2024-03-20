using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020085.BusinessLayers;
using SV20T1020085.DomainModels;
using SV20T1020085.Web.Models;
using System.Drawing.Printing;

namespace SV20T1020085.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SHIPPER_SEARCH = "shipper_search";

        public IActionResult Index()
        {
            //Lấy dầu vào tìm kiếm hiên đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);

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
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);
            var model = new ShipperSearchResult()
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
            ViewBag.Title = "Bổ sung thông tin người giao hàng";
            Shipper model = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", model);
        }
        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Cập nhật thông tin người giao hàng";
            Shipper? model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Shipper shipper)
        {
            try
            {
                ViewBag.Title = shipper.ShipperID == 0 ? "Bổ sung thông tin người giao hàng" : "Cập nhật thông tin người giao hàng";
                if (string.IsNullOrWhiteSpace(shipper.ShipperName))
                    ModelState.AddModelError(nameof(shipper.ShipperName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(shipper.Phone))
                    ModelState.AddModelError(nameof(shipper.Phone), "Tên giao dịch không được để trống");
                // thông qua thuộc tính IsValid cỉa ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {

                    return View("Edit", shipper);
                }

                if (shipper.ShipperID == 0)
                {
                    int id = CommonDataService.AddShipper(shipper);
                }
                else
                {
                    bool result =CommonDataService.UpdateShipper(shipper);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            };
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa thông tin người giao hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetShipper(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !CommonDataService.IsUsedShipper(id);

            return View(model);
        }
    }
}
