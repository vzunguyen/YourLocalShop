# YourLocalShop - SWE30003 Assignment 3 - Group 5
## Grocery Shopping Web Application

### Overview
This project is a grocery shopping web application developed as part of coursework at Swinburne University.  
It demonstrates the use of **ASP.NET Core MVC** with a **Layered Architecture**, incorporating **MVC**, **Repository Pattern**, and **Client–Server Architecture** principles.  
The application allows customers to browse products by category, add items to a shopping cart, and proceed through checkout and order processing.

---

### Features
- Product catalogue with categories (Pantry, Cleaning, Dairy, Drinks, Snacks).
- Browsing, filtering, and searching of products.
- Shopping cart management (add, update, remove items).
- Checkout workflow with order creation and payment confirmation.
- Order history linked to customer accounts.
- Employee functionality for maintaining catalogue and categories.

---

### Platforms Used
- Rider IDE  
- Microsoft Edge / Mozilla Firefox  
- macOS / Windows  
- GitHub for version control  

---

### Deployment and Execution
The application is deployed as a **web application**.

#### Local Execution
1. Clone the repository:
   ```bash
   git clone https://github.com/vzunguyen/YourLocalShop.git
   cd YourLocalShop
2. Run the using the .NET CLI:
   ```bash
   dotnet run
3. The application will launch in your browser at:
   ```code
   http://localhost:5164
   (Port number may vary depending on configuration)

#### Rider IDE
- Open the solution in Rider IDE.
- Click the "Run" button to compile and execute.
- The application will launch in the browser automatically.

---

### Architecture
The system follows a Layered Architecure with four layers:
- Presentation Layer (MVC controllers and views)
- Service Layer (business logic services)
- Data Access Layer (repositories)
- Data Layer (domain models and JSON persistence)

---

### License
This project is for academic purpose. No commercial license is applied.
