using Microsoft.AspNetCore.Mvc;
using YourLocalShop.Models;
using YourLocalShop.Models.ViewModels;
using YourLocalShop.Services;
using System.Text.Json;
using System.Security.Claims;
using YourLocalShop.Data;

namespace YourLocalShop.Controllers;

public class OrdersController : Controller
{
    private readonly ICartService _cart;
    private readonly UsersRepository _users;
    private readonly OrdersRepository _ordersRepo = new();

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
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
    Console.WriteLine(string.Join(", ", errors));
            return View(vm);
        }

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

        _ordersRepo.Add(order);
        
        // Save the order ID into TempData
        TempData["OrderId"] = order.Id;
        
        return RedirectToAction("Invoice");
    }

    // GET: /Orders/Invoice
    [HttpGet]
    public IActionResult Invoice()
    {
        var orderId = TempData["OrderId"] as int?;
        if (orderId == null) return RedirectToAction("Index", "ShoppingCart");

        var order = _ordersRepo.GetById(orderId.Value);
        TempData.Keep("OrderId"); // keep it alive for the next request
        
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
        var orderId = TempData["OrderId"] as int?;
        if (orderId == null) return RedirectToAction("Index", "ShoppingCart");

        var order = _ordersRepo.GetById(orderId.Value);
        
        // Check built-in validation first
        if (!ModelState.IsValid)
        {
            TempData.Keep("OrderId");
            return View("Invoice", new InvoiceVm { Order = order!, Payment = vm.Payment });
        }
        
        // Check if card number is valid 
        if (!vm.Payment.IsValidCardNumber())
        {
            ModelState.AddModelError("Payment.CardNumber", "Card number not valid: failed Luhn check.");
        }
        // Only run expiry date (future-date) check if regex passed
        if (ModelState.TryGetValue("Payment.Expiry", out var entry) && entry.Errors.Count == 0)
        {
            if (!vm.Payment.IsExpiryValid())
            {
                ModelState.AddModelError("Payment.Expiry", "Expiry date must be in the future.");
            }
        }

        if (!ModelState.IsValid)
        {
            TempData.Keep("OrderId");
            return View("Invoice", new InvoiceVm { Order = order!, Payment = vm.Payment });
        }

        // Payment passed --> mark order as paid
        order.ReceiptId = $"RCP-{DateTime.UtcNow.Ticks}";
        order.Status = "Successful";
        
        _ordersRepo.Update(order);
        
        return View("Receipt", order);
    }
    
    // GET: /Orders/Receipt
    [HttpGet]
    public IActionResult Receipt()
    {
        var orderId = TempData["OrderId"] as int?;
        if (orderId == null) return RedirectToAction("Index", "ShoppingCart");

        var order = _ordersRepo.GetById(orderId.Value);
        return View(order);
    }
}
