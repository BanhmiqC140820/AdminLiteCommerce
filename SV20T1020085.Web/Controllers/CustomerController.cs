using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020085.BusinessLayers;
using SV20T1020085.DomainModels;
using SV20T1020085.Web.Models;

namespace SV20T1020085.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator},{WebUserRoles.Employee}")]
    public class CustomerController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string CUSTOMER_SEARCH = "customer_search";//tên biến dùng để lưu trong session


        public IActionResult Index()
        {
            //Lấy dầu vào tìm kiếm hiên đang lưu lại trong session
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(CUSTOMER_SEARCH);
            
            //Trường hợp trong session chưa có điều kiên thì tạo điều kiên mới
            if (input == null)
            {
                input = new PaginationSearchInput()
                {
                    Page =1,
                    PageSize = PAGE_SIZE,
                    SearchValue =""
                };
            }
                
            return View(input);
        }

        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data =CommonDataService.ListOfCustomers(out rowCount,input.Page,input.PageSize,input.SearchValue??"");
            ApplicationContext.SetSessionData(CUSTOMER_SEARCH, input);
            var model = new CustomerSearchResult() {
                Page =input.Page,
                PageSize =input.PageSize,
                SearchValue =input.SearchValue??"",
                RowCount = rowCount,
                Data = data
            };


            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung khách hàng";
            Customer model = new Customer()
            {
                CustomerId = 0
            };

            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin khách hàng";
            Customer? model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Customer data)
        {
            try
            {
                ViewBag.Title = data.CustomerId == 0 ? "Bổ sung khách hàng" : "Cập nhật thông tin khách hàng";
                //Kiểm soát đầu vào và đưa các thông báo lỗi trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.CustomerName))
                    ModelState.AddModelError("CustomerName", "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.ContactName))
                    ModelState.AddModelError("ContactName","Tên giao dịch không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError("Email", "Vui lòng nhập Email của khách hàng");
                if (string.IsNullOrWhiteSpace(data.Province))
                    ModelState.AddModelError("Province", "Vui lòng chọn tỉnh thành");
                // thông qua thuộc tính IsValid cỉa ModelState để kiểm tra xem có tồn tại lỗi hay không
                if(!ModelState.IsValid)
                {
                   
                    return View("Edit", data);
                }

                if (data.CustomerId == 0)
                {
                    int id = CommonDataService.AddCustomer(data);
                    if(id<=0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng");
                        return View("Edit", data);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateCustomer(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với khách hàng khác");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", "Không thể lưu được dữ liệu. Vui lòng thử lại sau");
                return View("Edit",data);
            }
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Title = "Xóa thông tin khách hàng";
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCustomer(id);
                return RedirectToAction("Index");
            }

            var model = CommonDataService.GetCustomer(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }

            bool allowDelete = !CommonDataService.IsUserCustomer(id);
            ViewBag.AllowDelete = allowDelete;

            return View(model);
        }
    }
}
