using System.Text.Json;
using YourLocalShop.Models;

namespace YourLocalShop.Data;

public class OrdersRepository
{
    private readonly string _ordersPath= "Data/orders.json";

    // Load all orders
    public List<Order> GetAll()
    {
        if (!File.Exists(_ordersPath))
            return new List<Order>();

        string json = File.ReadAllText(_ordersPath);
        return JsonSerializer.Deserialize<List<Order>>(json) ?? new List<Order>();
    }

    // Save all orders back to file
    private void SaveAll(List<Order> orders)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(orders, options);
        File.WriteAllText(_ordersPath, json);
    }

    // Add a new order
    public void Add(Order order)
    {
        var orders = GetAll();
        
        // Assign a new ID if needed (prevent duplicate IDs)
        if (order.Id == 0)
        {
            order.Id = orders.Count > 0 ? orders.Max(o => o.Id) + 1 : 1;
        }
        
        orders.Add(order);
        SaveAll(orders);
    }

    // Update an existing order (e.g. status after payment)
    public void Update(Order updatedOrder)
    {
        var orders = GetAll();
        var index = orders.FindIndex(o => o.Id == updatedOrder.Id);
        if (index != -1)
        {
            orders[index] = updatedOrder;
            SaveAll(orders);
        }
    }

    // Find by ID
    public Order? GetById(int id)
    {
        return GetAll().FirstOrDefault(o => o.Id == id);
    }
}
