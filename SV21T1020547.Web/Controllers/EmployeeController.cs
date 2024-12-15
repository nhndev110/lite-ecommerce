using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020547.BusinessLayers;
using SV21T1020547.DomainModels;
using SV21T1020547.Web.Models;

namespace SV21T1020547.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN}")]
    public class EmployeeController : Controller
    {
        private const int PAGE_SIZE = 12;
        private const string PASSWORD_DEFAULT = "123456";
        private const string EMPLOYEE_SEARCH_CONDITION = "EmployeeSearchCondition";

        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(EMPLOYEE_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new PaginationSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = ""
                };
            }

            return View(condition);
        }

        public IActionResult Search(PaginationSearchInput condition)
        {
            var data = CommonDataService.ListOfEmployees(out int rowCount, condition.Page, condition.PageSize, condition.SearchValue);
            EmployeeSearchResult model = new()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };

            ApplicationContext.SetSessionData(EMPLOYEE_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Nhân viên";
            var data = new Employee() { EmployeeID = 0, IsWorking = true };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Nhân viên";
            var data = CommonDataService.GetEmployee(id);
            if (data == null) return RedirectToAction("Index");
            return View(data);
        }

        [HttpPost]
        public IActionResult Save(Employee data, string _Birthdate, IFormFile? uploadPhoto)
        {
            ViewBag.Title = data.EmployeeID == 0 ? "Bổ sung nhân viên" : "Cập nhật thông tin nhân viên";

            if (string.IsNullOrWhiteSpace(data.FullName))
                ModelState.AddModelError(nameof(data.FullName), "Tên nhân viên không được để trống");

            if (string.IsNullOrWhiteSpace(data.Email))
                ModelState.AddModelError(nameof(data.Email), "Vui lòng nhập Email của nhân viên");

            if (string.IsNullOrWhiteSpace(_Birthdate))
                ModelState.AddModelError(nameof(data.BirthDate), "Vui lòng nhập ngày sinh của nhân viên");

            DateTime? d = _Birthdate.ToDateTime();

            if (d != null)
                data.BirthDate = d.Value;
            else
                ModelState.AddModelError(nameof(data.BirthDate), "Ngày sinh nhập không hợp lệ");

            if (string.IsNullOrWhiteSpace(data.Phone)) data.Phone = "";

            if (string.IsNullOrWhiteSpace(data.Address)) data.Address = "";

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}-{uploadPhoto.FileName}";
                string filePath = Path.Combine(ApplicationContext.WebRootPath, @"images\users", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }
            if (data.EmployeeID == 0)
            {
                data.Password = PASSWORD_DEFAULT;
                int id = CommonDataService.AddEmployee(data);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateEmployee(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.Email), "Email bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteEmployee(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetEmployee(id);
            if (data == null) return RedirectToAction("Index");
            return View(data);
        }
    }
}
