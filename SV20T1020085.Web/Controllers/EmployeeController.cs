using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020085.BusinessLayers;
using SV20T1020085.DomainModels;
using SV20T1020085.Web.Models;

namespace SV20T1020085.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}")]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string EMPLOYEE_SEARCH = "employee_search";
        
        public IActionResult Index()
        {
            PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH);

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
            var data = CommonDataService.ListOfEmployees(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH, input);
            var model = new EmployeeSearchResult()
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
            ViewBag.Title = "Bổ sung nhân viên";
            Employee model = new Employee()
            {
                EmployeeID = 0,
                BirthDate = new DateTime(1990, 1, 1),
                Photo = "nophoto.png"
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Title = "Chỉnh sửa nhân viên";
            Employee? model = CommonDataService.GetEmployee(id);
            if (model == null)
                return RedirectToAction("Index");
            if (string.IsNullOrEmpty(model.Photo))
            {
                model.Photo = "nophoto.png";
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetEmployee(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.allowDelete = !CommonDataService.IsUsedEmployee(id);
            //return Json(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult Save(Employee data, string birthDateInput, IFormFile? uploadPhoto)
        {
            try
            {
                ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";
                //Xủ lý ngày sinh
                DateTime? birthDate = birthDateInput.ToDateTime();
                if (birthDate.HasValue)
                {
                    data.BirthDate = birthDate.Value;
                }
                else
                {
                    ModelState.AddModelError(nameof(data.BirthDate), "Vui lòng nhập ngày sinh");
                }
                //Xử lý ảnh upload
                if (uploadPhoto != null)
                {
                    string filename = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";
                    string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\employees");
                    string filePath = Path.Combine(folder, filename);// Đương dẫn đến file cần lưu

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadPhoto.CopyTo(stream);
                    }
                    data.Photo = filename;

                }

               
                //Kiểm soát đầu vào và đưa các thông báo lỗi trong ModelState (nếu có)
                if (string.IsNullOrWhiteSpace(data.FullName))
                    ModelState.AddModelError(nameof(data.FullName), "Tên không được để trống");
                if (string.IsNullOrWhiteSpace(data.Email))
                    ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của nhân viên");
            
                // thông qua thuộc tính IsValid cỉa ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {

                    return View("Edit", data);
                }

                if (data.EmployeeID == 0)
                {

                    int id = CommonDataService.AddEmployee(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng");
                        return View("Edit", data);
                    }
                }
                else
                {
                    bool result = CommonDataService.UpdateEmployee(data);
                    if (!result)
                    {
                        ModelState.AddModelError(nameof(data.Email), "Địa chỉ email bị trùng với nhân viên khác");
                        return View("Edit", data);
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }
    }
}
