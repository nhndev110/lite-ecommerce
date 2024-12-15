using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020547.BusinessLayers;
using SV21T1020547.DomainModels;
using SV21T1020547.Web.Models;

namespace SV21T1020547.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.ADMIN},{WebUserRoles.MANAGER}")]
    public class ShipperController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string SHIPPER_SEARCH_CONDITION = "ShipperSearchCondition";

        public IActionResult Index()
        {
            PaginationSearchInput? condition = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH_CONDITION);
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
            var data = CommonDataService.ListOfShippers(out int rowCount, condition.Page, condition.PageSize, condition.SearchValue);
            ShipperSearchResult model = new ShipperSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };

            ApplicationContext.SetSessionData(SHIPPER_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung Người giao hàng";
            var data = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", data);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin Người giao hàng";
            var data = CommonDataService.GetShipper(id);
            if (data == null) return RedirectToAction("Index");
            return View(data);
        }

        [HttpPost]
        public IActionResult Save(Shipper data)
        {
            ViewBag.Title = data.ShipperID == 0 ? "Bổ sung Người giao hàng" : "Cập nhật thông tin Người giao hàng";

            if (string.IsNullOrWhiteSpace(data.ShipperName))
                ModelState.AddModelError(nameof(data.ShipperName), "Tên người giao hàng không được để trống");
            if (string.IsNullOrWhiteSpace(data.Phone))
                ModelState.AddModelError(nameof(data.Phone), "Vui lòng nhập điện thoại của người giao hàng");

            if (!ModelState.IsValid)
            {
                return View("Edit", data);
            }

            if (data.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(data);
                if (id <= 0)
                {
                    ModelState.AddModelError(nameof(data.Phone), "Số điện thoại bị trùng");
                    return View("Edit", data);
                }
            }
            else
            {
                bool result = CommonDataService.UpdateShipper(data);
                if (!result)
                {
                    ModelState.AddModelError(nameof(data.Phone), "Số điện thoại bị trùng");
                    return View("Edit", data);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var data = CommonDataService.GetShipper(id);
            if (data == null) return RedirectToAction("Index");
            return View(data);
        }
    }
}
