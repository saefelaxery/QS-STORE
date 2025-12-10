using Microsoft.AspNetCore.Mvc;
using LaLaStore.Services;
using LaLaStore.Models;

namespace LaLaStore.Controllers;

public class CartController : Controller
{
    private readonly CartService _cartService;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public CartController(CartService cartService, ProductService productService, OrderService orderService)
    {
        _cartService = cartService;
        _productService = productService;
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        var items = _cartService.GetCartItems(HttpContext.Session, _productService);
        ViewBag.TotalPrice = _cartService.GetTotalPrice(HttpContext.Session, _productService);
        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int productId, string size, string color)
    {
        var product = _productService.GetProductById(productId);
        if (product == null)
        {
            return NotFound();
        }

        _cartService.AddToCart(HttpContext.Session, _productService, product, size, color);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RemoveFromCart(int productId, string size, string color)
    {
        _cartService.RemoveFromCart(HttpContext.Session, _productService, productId, size, color);
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateQuantity(int productId, string size, string color, int quantity)
    {
        _cartService.UpdateQuantity(HttpContext.Session, _productService, productId, size, color, quantity);
        return RedirectToAction("Index");
    }

    [AcceptVerbs("GET", "POST")]
    public IActionResult ClearCart()
    {
        _cartService.ClearCart(HttpContext.Session);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult GetCartCount()
    {
        var count = _cartService.GetTotalItems(HttpContext.Session, _productService);
        return Json(new { count });
    }

    [HttpGet]
    public IActionResult Checkout()
    {
        var items = _cartService.GetCartItems(HttpContext.Session, _productService);
        if (items.Count == 0)
        {
            return RedirectToAction("Index");
        }
        ViewBag.TotalPrice = _cartService.GetTotalPrice(HttpContext.Session, _productService);
        return View(new Order());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(Order order)
    {
        var items = _cartService.GetCartItems(HttpContext.Session, _productService);
        if (items.Count == 0)
        {
            return RedirectToAction("Index");
        }

        // If Visa selected, capture card details (store only safe info)
        if (order.PaymentMethod == "Visa")
        {
            var cardNumber = (Request.Form["CardNumber"].ToString() ?? string.Empty).Replace(" ", "");
            var cardExpiry = Request.Form["CardExpiry"].ToString() ?? string.Empty;
            var cardHolderName = Request.Form["CardHolderName"].ToString() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(cardNumber) ||
                string.IsNullOrWhiteSpace(cardExpiry) ||
                string.IsNullOrWhiteSpace(cardHolderName))
            {
                ModelState.AddModelError("PaymentMethod", "Please enter all Visa card details.");
            }
            else
            {
                order.CardLast4 = cardNumber.Length >= 4 ? cardNumber[^4..] : cardNumber;
                order.CardExpiry = cardExpiry;
                order.CardHolderName = cardHolderName;
            }
        }

        if (ModelState.IsValid)
        {
            var createdOrder = await _orderService.CreateOrderAsync(order, items);
            _cartService.ClearCart(HttpContext.Session);
            return RedirectToAction("OrderConfirmation", new { id = createdOrder.Id });
        }

        ViewBag.TotalPrice = _cartService.GetTotalPrice(HttpContext.Session, _productService);
        return View(order);
    }

    [HttpGet]
    public IActionResult OrderConfirmation(int id)
    {
        var order = _orderService.GetOrderById(id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }
}

