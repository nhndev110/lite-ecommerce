using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV21T1020547.BusinessLayers;
using SV21T1020547.DomainModels;
using SV21T1020547.Web.Models;
using System.Globalization;

namespace SV21T1020547.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.SALE}")]
    public class OrderController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string ORDER_SEARCH_CONDITION = "OrderSearchCondition";
        private const int PRODUCT_PAGE_SIZE = 5;
        private const string PRODUCT_SEARCH_CONDITION = "ProductSearchCondition";
        private const string SHOPPING_CART = "ShoppingCart";

        public IActionResult Index()
        {
            var condition = ApplicationContext.GetSessionData<OrderSearchInput>(ORDER_SEARCH_CONDITION);
            if (condition == null)
            {
                var cultureInfo = new CultureInfo("en-US");
                condition = new OrderSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    Status = 0,
                    TimeRange = $"{DateTime.Today.AddDays(-7).ToString("dd/MM/yyyy", cultureInfo)} - {DateTime.Today.ToString("dd/MM/yyyy", cultureInfo)}",
                };
            }

            return View(condition);
        }

        public IActionResult Search(OrderSearchInput condition)
        {
            List<Order> data = OrderDataService.ListOrders(
                out int rowCount,
                condition.Page,
                condition.PageSize,
                condition.Status,
                condition.FromTime,
                condition.ToTime,
                condition.SearchValue ?? ""
            );

            var model = new OrderSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                Status = condition.Status,
                TimeRange = condition.TimeRange,
                RowCount = rowCount,
                Data = data,
            };

            ApplicationContext.SetSessionData(ORDER_SEARCH_CONDITION, condition);

            return View(model);
        }

        public IActionResult Details(int id = 0)
        {
            var order = OrderDataService.GetOrder(id);
            if (order == null)
                return RedirectToAction("Index");

            var details = OrderDataService.ListOrderDetails(id);
            var model = new OrderDetailModel()
            {
                Order = order,
                Details = details
            };

            return View(model);
        }


        public IActionResult Create()
        {
            var condition = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH_CONDITION);
            if (condition == null)
            {
                condition = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PRODUCT_PAGE_SIZE,
                    SearchValue = "",
                };
            }
            return View(condition);
        }

        public IActionResult SearchProduct(ProductSearchInput condition)
        {
            var data = ProductDataService.ListProducts(
                out int rowCount,
                condition.Page,
                condition.PageSize,
                condition.SearchValue ?? ""
            );
            var model = new ProductSearchResult()
            {
                Page = condition.Page,
                PageSize = condition.PageSize,
                SearchValue = condition.SearchValue ?? "",
                RowCount = rowCount,
                Data = data,
            };
            ApplicationContext.SetSessionData(PRODUCT_SEARCH_CONDITION, condition);
            return View(model);
        }

        private static List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = [];
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }
            return shoppingCart;
        }

        public IActionResult AddToCart(CartItem item)
        {
            if (item.SalePrice < 0 || item.Quantity < 0)
                return Json("Giá bán và số lượng không hợp lệ");

            var shoppingCart = GetShoppingCart();
            var existsProduct = shoppingCart.FirstOrDefault(m => m.ProductID == item.ProductID);
            if (existsProduct == null)
            {
                shoppingCart.Add(item);
            }
            else
            {
                existsProduct.Quantity += item.Quantity;
                existsProduct.SalePrice = item.SalePrice;
            }
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult RemoveFromCart(int id = 0)
        {
            var shoppingCart = GetShoppingCart();
            int index = shoppingCart.FindIndex(n => n.ProductID == id);
            if (index >= 0)
                shoppingCart.RemoveAt(index);
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult ClearCart()
        {
            var shoppingCart = GetShoppingCart();
            shoppingCart.Clear();
            ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            return Json("");
        }

        public IActionResult ShoppingCart()
        {
            return View(GetShoppingCart());
        }

        public IActionResult Init(int customerID = 0, string deliveryProvince = "", string deliveryAddress = "")
        {
            var shoppingCart = GetShoppingCart();
            if (shoppingCart.Count == 0)
                return Json("Giỏ hàng trống. Vui lòng chọn mặt hàng cần bán");

            if (customerID == 0 || string.IsNullOrWhiteSpace(deliveryProvince) || string.IsNullOrWhiteSpace(deliveryAddress))
                return Json("Vui lòng nhập đầy đủ thông tin khách hàng và nơi giao hàng");

            int employeeID = 1; //TODO: Thay bởi ID nhân viên đang login vào hệ thống

            List<OrderDetail> orderDetails = [];
            shoppingCart.ForEach(
                item => orderDetails.Add(
                    new OrderDetail()
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        SalePrice = item.SalePrice
                    }
                )
            );

            int orderID = OrderDataService.InitOrder(
                employeeID,
                customerID,
                deliveryProvince,
                deliveryAddress,
                orderDetails
            );

            ClearCart();

            return Json(orderID);
        }

        public IActionResult EditDetail(int id, int productID = 0)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Đơn hàng không tồn tại" });

            if (productID < 0)
                return StatusCode(400, new { message = "Không tìm thấy sản phẩm" });

            OrderDetail? orderDetail = OrderDataService.GetOrderDetail(id, productID);

            if (orderDetail == null)
                return StatusCode(404, new { message = "Mặt hàng này không tồn tại trong đơn hàng" });

            return View(orderDetail);
        }

        [HttpPost]
        public IActionResult UpdateDetail(int id, int productID, int quantity, decimal salePrice)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            if (productID < 1 || ProductDataService.GetProduct(productID) == null)
                return StatusCode(400, new { message = "Không tìm thấy sản phẩm" });

            if (quantity < 1)
                return StatusCode(400, new { message = "Vui lòng nhập số lượng lớn hơn 0" });

            if (salePrice < 1)
                return StatusCode(400, new { message = "Vui lòng nhập đơn giá lớn hơn 0" });

            bool isUpdated = OrderDataService.SaveOrderDetail(id, productID, quantity, salePrice);

            if (!isUpdated)
                return StatusCode(404, new { message = "Có lỗi khi cập nhật chi tiết đơn hàng" });

            return StatusCode(200);
        }

        public IActionResult DeleteDetail(int id, int productID)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            if (productID < 1 || ProductDataService.GetProduct(productID) == null)
                return StatusCode(400, new { message = "Không tìm thấy sản phẩm" });

            bool isDeleted = OrderDataService.DeleteOrderDetail(id, productID);
            if (!isDeleted)
                return StatusCode(403, new { message = "Không thể xoá sản phẩm này trong đơn hàng" });

            return StatusCode(200);
        }

        public IActionResult Delete(int id)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            bool isDeleted = OrderDataService.DeleteOrder(id);
            if (!isDeleted)
                return StatusCode(403, new { message = "Không thể xoá đơn hàng này" });

            return StatusCode(200, new { redirectURL = Url.Action("Index", "Order") });
        }

        public IActionResult Accept(int id)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            bool isAccepted = OrderDataService.AcceptOrder(id);
            if (!isAccepted)
                return StatusCode(403, new { message = "Không thể duyệt đơn hàng này" });

            return StatusCode(200);
        }

        public IActionResult Finish(int id)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            bool isFinished = OrderDataService.FinishOrder(id);
            if (!isFinished)
                return StatusCode(403, new { message = "Không thể hoàn tất đơn hàng này" });

            return StatusCode(200);
        }

        public IActionResult Cancel(int id)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            bool isCanceled = OrderDataService.CancelOrder(id);
            if (!isCanceled)
                return StatusCode(403, new { message = "Không thể huỷ đơn hàng này" });

            return StatusCode(200);
        }

        public IActionResult Reject(int id)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            bool isRejected = OrderDataService.RejectOrder(id);
            if (!isRejected)
                return StatusCode(403, new { message = "Không thể từ chối đơn hàng này" });

            return StatusCode(200);
        }

        public IActionResult Shipping(int id, int shipperID = 0)
        {
            if (id < 1)
                return StatusCode(400, new { message = "Không tìm thấy đơn hàng" });

            Order? order = OrderDataService.GetOrder(id);
            if (order == null)
                return StatusCode(404, new { message = "Không tìm thấy đơn hàng" });

            if (OrderDataService.ListOrderDetails(id).Count < 1)
                return StatusCode(400, new { message = "Đơn hàng này không có sản phẩm nào để giao hàng" });

            if (Request.Method == "GET")
                return View(order);

            if (shipperID < 1)
                return StatusCode(400, new { message = "Vui lòng chọn đơn vị giao hàng" });

            if (CommonDataService.GetShipper(shipperID) == null)
                return StatusCode(400, new { message = "Không tìm thấy người giao hàng" });

            bool isShipped = OrderDataService.ShipOrder(id, shipperID);
            if (!isShipped)
                return StatusCode(403, new { message = "Không thể vận chuyển đơn hàng này" });

            return StatusCode(200);
        }
    }
}
