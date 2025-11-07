using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace YourLocalShop.Models;

public class UsersRepository 
{
    private readonly string _customersPath = "Data/customers.json";
    private readonly ConcurrentDictionary<string, Customer> _customers;
    public PasswordHasher<User> Hasher { get; } = new();

    // Keep the single admin in memory only
    private readonly Employee _admin;
    
    public UsersRepository()
    {
        // Load customers from JSON
        if (File.Exists(_customersPath))
        {
            var json = File.ReadAllText(_customersPath);
            var list = JsonSerializer.Deserialize<List<Customer>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Customer>();

            _customers = new ConcurrentDictionary<string, Customer>(
                list.ToDictionary(c => c.Email, c => c, StringComparer.OrdinalIgnoreCase));
        }
        else
        {
            _customers = new ConcurrentDictionary<string, Customer>(StringComparer.OrdinalIgnoreCase);
            Save();
        }

        // seed the admin
        _admin = new Employee
        {
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@shop.com",
            EmployeeRole = "Admin",
            PasswordHash = Hasher.HashPassword(new Employee(), "Admin1234!")
        };
    }

    public User? FindByEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;

        if (email.Equals(_admin.Email, StringComparison.OrdinalIgnoreCase))
            return _admin;

        return _customers.GetValueOrDefault(email);
    }

    public void Add(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
            throw new ArgumentException("Customer must have an email.");

        _customers[customer.Email] = customer;
        Save();
    }

    public void Update(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
            throw new ArgumentException("Customer must have an email.");

        _customers[customer.Email] = customer;
        Save();
    }
    
    public Customer RegisterCustomer(string firstName, string lastName, string email, string phone, string password)
    {
        var customer = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Phone = phone
        };
        customer.PasswordHash = Hasher.HashPassword(customer, password);
        Add(customer);
        return customer;
    }

    private void Save()
    {
        var list = _customers.Values.ToList();
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_customersPath, json);
    }
}