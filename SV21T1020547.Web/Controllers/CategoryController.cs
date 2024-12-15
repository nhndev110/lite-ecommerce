using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020547.BusinessLayers;
using SV21T1020547.DomainModels;
using SV21T1020547.Web.Models;

namespace SV21T1020547.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN},{WebUserRoles.MANAGER}")]
    public class CategoryController : Controller
    {
        private const int PAGE_SIZE = 10;
        private const string CATEGORY_SEARCH_CONDITION = "CategorySearchCondition";

        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(CATEGORY_SEARCH_CONDITION);
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
            var data = CommonDataService.ListOfCategories(out int rowCount, condition.Page, condition.PageSize, condition.SearchValue);
            CategorySearchResult model = new()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };

            ApplicationContext.SetSessionData(CATEGORY_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Loại hàng";
            var data = new Category()
            {
                CategoryID = 0
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Loại hàng";
            var data = CommonDataService.GetCategory(id);
            if (data == null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Save(Category data)
        {
            ViewBag.Title = data.CategoryID == 0 ? "Bổ sung Loại hàng" : "Cập nhật thông tin Loại hàng";

            if (string.IsNullOrWhiteSpace(data.CategoryName))
                ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.Description))
                ModelState.AddModelError(nameof(data.Description), "Mô tả loại hàng không được để trống");

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (data.CategoryID == 0)
            {
                int id = CommonDataService.AddCategory(data);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateCategory(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.CategoryName), "Tên loại hàng bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetCategory(id);
            if (data == null) return RedirectToAction("Index");
            return View(data);
        }
    }
}
