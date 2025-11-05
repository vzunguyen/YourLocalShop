namespace YourLocalShop.Models;

public class Employee : User
{
    public string EmployeeRole { get; set; } = "Admin";
    public override string Role => "Employee";
}