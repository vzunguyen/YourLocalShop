using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;
using YourLocalShop.Models.ViewModels;
using YourLocalShop.Services;
using System.Text.Json;
using System.Security.Claims;

namespace YourLocalShop.Controllers;

public class OrdersController : Controller
{
    private readonly ICartService _cart;
    private readonly UsersRepository _users;

    public OrdersController(ICartService cart, UsersRepository users)
    {
        _cart = cart;
        _users = users;
    }

    // GET: /Orders/Checkout
    [HttpGet]
    public IActionResult Checkout()
    {
        var cart = _cart.Get();
        if (!cart.Items.Any())
            return RedirectToAction("Index", "ShoppingCart");

        // get logged-in customer from claims
        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = _users.FindByEmail(email);
        
        Customer customer;
        if (user is Customer existingCustomer)
        {
            customer = existingCustomer;
        }
        else
        {
            // fallback: hydrate from claims if available
            customer = new Customer
            {
                FirstName = User.FindFirstValue(ClaimTypes.GivenName) ?? "",
                LastName = User.FindFirstValue(ClaimTypes.Surname) ?? "",
                Email = email ?? "",
                Phone = User.FindFirstValue(ClaimTypes.MobilePhone) ?? ""
            };
        }

        var vm = new CheckoutVm
        {
            Customer = customer ?? new Customer(), // pre-filled if found, blank otherwise
            Items = cart.Items,
            Total = cart.Total
        };

        return View(vm); // Views/Orders/Checkout.cshtml
    }

    // POST: /Orders/Checkout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(CheckoutVm vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var email = User.FindFirstValue(ClaimTypes.Email);
        var user = _users.FindByEmail(email);
        var customer = user as Customer;

        var order = new Order
        {
            CustomerId = customer?.Id ?? 0,
            Customer = vm.Customer, // recipient details (can override default address)
            OrderDate = DateTime.Now,
            Items = vm.Items,
            InvoiceId = $"INV-{DateTime.UtcNow.Ticks}"
        };

        // Keep order alive across multiple requests
        TempData["Order"] = JsonSerializer.Serialize(order);
        
        return RedirectToAction("Invoice");
    }

    // GET: /Orders/Invoice
    [HttpGet]
    public IActionResult Invoice()
    {
        var orderJson = TempData["Order"] as string;
        if (orderJson == null) return RedirectToAction("Index", "ShoppingCart");

        var order = JsonSerializer.Deserialize<Order>(orderJson);
        TempData.Keep("Order"); // keep for next request
        
        var vm = new InvoiceVm
        {
            Order = order!,
            Payment = new Payment()
        };
        
        return View(vm); // Views/Orders/Invoice.cshtml
    }

    // POST: /Orders/Pay
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Pay(InvoiceVm vm)
    {
        var orderJson = TempData["Order"] as string;
        if (orderJson == null) return RedirectToAction("Index", "ShoppingCart");

        var order = JsonSerializer.Deserialize<Order>(orderJson);
        
        // Check basic model validation (expiry format, CVC length)
        if (!ModelState.IsValid || !vm.Payment.IsValidCardNumber())
        {
            ModelState.AddModelError("CardNumber", "Card number not valid: failed Luhn check.");
            TempData.Keep("Order"); // keep order for redisplay
            
            // Re-wrap order + payment for redisplay
            var retryVm = new InvoiceVm
            {
                Order = order!,
                Payment = vm.Payment
            };
            return View("Invoice", retryVm);
        }

        // Payment passed --> mark order as paid
        order.ReceiptId = $"RCP-{DateTime.UtcNow.Ticks}";
        return View("Receipt", order);
    }
    
    // GET: /Orders/Receipt
    [HttpGet]
    public IActionResult Receipt()
    {
        var orderJson = TempData["Order"] as string;
        if (orderJson == null) return RedirectToAction("Index", "ShoppingCart");

        var order = JsonSerializer.Deserialize<Order>(orderJson);
        return View(order);
    }
}
