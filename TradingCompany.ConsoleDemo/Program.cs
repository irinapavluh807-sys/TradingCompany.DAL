using System.Globalization;
using System.Text;
using TradingCompany.DAL.Data;
using TradingCompany.DAL.Dtos;
using TradingCompany.DAL.Models;
using TradingCompany.DAL.Repositories;

Console.OutputEncoding = Encoding.UTF8;

var connectionString = Environment.GetEnvironmentVariable("TRADING_COMPANY_SQLSERVER")
    ?? "Server=(localdb)\\MSSQLLocalDB;Initial Catalog=TradingCompanyDalDemo;Integrated Security=True;TrustServerCertificate=True;";
var connectionFactory = new SqlServerConnectionFactory(connectionString);
new DatabaseInitializer(connectionFactory).Initialize();

var managers = new WarehouseManagerRepository(connectionFactory);
var suppliers = new SupplierRepository(connectionFactory);
var products = new ProductRepository(connectionFactory);
var stocks = new WarehouseStockRepository(connectionFactory);
var orders = new SupplyOrderRepository(connectionFactory);
var orderItems = new SupplyOrderItemRepository(connectionFactory);

Console.WriteLine("TradingCompany DAL demo");
Console.WriteLine($"SQL Server connection: {connectionString}");
Console.WriteLine("Seeded users: manager1 .. manager25");

while (true)
{
    var currentManager = Login(managers);
    if (currentManager is null)
    {
        break;
    }

    var isLoggedIn = true;
    while (isLoggedIn)
    {
        Console.WriteLine();
        Console.WriteLine("Warehouse manager menu");
        Console.WriteLine("1. Show products and warehouse stock");
        Console.WriteLine("2. Create a supply order");
        Console.WriteLine("3. View/edit active supply orders");
        Console.WriteLine("4. Run automatic CRUD demo for all entities");
        Console.WriteLine("0. Logout");
        Console.Write("Choose: ");

        switch (Console.ReadLine())
        {
            case "1":
                ShowInventory(products);
                break;
            case "2":
                CreateSupplyOrder(currentManager, suppliers, products, orders, orderItems);
                break;
            case "3":
                EditActiveOrders(orders, orderItems);
                break;
            case "4":
                RunCrudDemo(managers, suppliers, products, stocks, orders, orderItems);
                break;
            case "0":
                isLoggedIn = false;
                Console.WriteLine("Logged out.");
                break;
            default:
                Console.WriteLine("Unknown menu item.");
                break;
        }
    }
}

static WarehouseManager? Login(WarehouseManagerRepository managers)
{
    while (true)
    {
        Console.WriteLine();
        Console.Write("Login as warehouse manager, or press Enter to exit: ");
        var username = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var manager = managers.GetByUsername(username.Trim());
        if (manager is not null)
        {
            Console.WriteLine($"Welcome, {manager.FullName}.");
            return manager;
        }

        Console.WriteLine("Manager was not found. Try manager1, manager2, ..., manager25.");
    }
}

static void ShowInventory(ProductRepository products)
{
    Console.Write("Search term (empty for all): ");
    var search = Console.ReadLine();
    var sortBy = ReadInventorySortField();

    Console.Write("Descending sort? y/N: ");
    var descending = string.Equals(Console.ReadLine(), "y", StringComparison.OrdinalIgnoreCase);

    var items = products.SearchInventory(search, sortBy, descending);
    Console.WriteLine();
    Console.WriteLine($"{"ID",3} {"SKU",-10} {"Product",-25} {"Supplier",-22} {"Stock",7} {"Reserved",8} {"Avail",7} {"Price",10}");
    Console.WriteLine(new string('-', 99));

    foreach (var item in items)
    {
        Console.WriteLine(
            $"{item.ProductId,3} {item.Sku,-10} {Trim(item.ProductName, 25),-25} {Trim(item.SupplierName, 22),-22} " +
            $"{item.QuantityOnHand,7} {item.ReservedQuantity,8} {item.AvailableQuantity,7} {item.UnitPrice,10:0.00}");
    }

    Console.WriteLine($"{items.Count} product(s) shown.");
}

static void CreateSupplyOrder(
    WarehouseManager manager,
    SupplierRepository suppliers,
    ProductRepository products,
    SupplyOrderRepository orders,
    SupplyOrderItemRepository orderItems)
{
    Console.WriteLine();
    Console.WriteLine("Suppliers:");
    foreach (var supplier in suppliers.GetAll())
    {
        Console.WriteLine($"{supplier.Id,3}. {supplier.Name}");
    }

    var supplierId = ReadRequiredInt("Supplier id: ");
    var supplierExists = suppliers.GetById(supplierId) is not null;
    if (!supplierExists)
    {
        Console.WriteLine("Supplier was not found.");
        return;
    }

    var order = new SupplyOrder
    {
        SupplierId = supplierId,
        ManagerId = manager.Id,
        Status = "Draft",
        OrderDate = DateTime.UtcNow,
        ExpectedDeliveryDate = DateTime.UtcNow.AddDays(7),
        Notes = "Created from console DAL demo"
    };
    order.Id = orders.Create(order);

    Console.WriteLine($"Order #{order.Id} created. Add products for this supplier.");
    var supplierProducts = products.GetAll().Where(product => product.SupplierId == supplierId).ToList();
    foreach (var product in supplierProducts)
    {
        Console.WriteLine($"{product.Id,3}. {product.Sku,-10} {product.Name,-25} {product.UnitPrice,8:0.00}");
    }

    while (supplierProducts.Count > 0)
    {
        var productId = ReadRequiredInt("Product id (0 to finish): ");
        if (productId == 0)
        {
            break;
        }

        var product = supplierProducts.FirstOrDefault(item => item.Id == productId);
        if (product is null)
        {
            Console.WriteLine("Choose a product that belongs to the selected supplier.");
            continue;
        }

        var quantity = ReadRequiredInt("Quantity: ");
        if (quantity <= 0)
        {
            Console.WriteLine("Quantity must be positive.");
            continue;
        }

        try
        {
            orderItems.Create(new SupplyOrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                QuantityOrdered = quantity,
                UnitPrice = product.UnitPrice
            });
            Console.WriteLine("Order item added.");
        }
        catch (InvalidOperationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not add item: {ex.Message}");
        }
    }
}

static void EditActiveOrders(SupplyOrderRepository orders, SupplyOrderItemRepository orderItems)
{
    var activeOrders = orders.GetActiveOrders();
    Console.WriteLine();
    Console.WriteLine($"{"ID",3} {"Supplier",8} {"Manager",7} {"Status",-10} {"Expected",-12} Notes");
    Console.WriteLine(new string('-', 75));

    foreach (var order in activeOrders)
    {
        Console.WriteLine(
            $"{order.Id,3} {order.SupplierId,8} {order.ManagerId,7} {order.Status,-10} " +
            $"{order.ExpectedDeliveryDate:yyyy-MM-dd} {order.Notes}");
    }

    if (activeOrders.Count == 0)
    {
        Console.WriteLine("There are no active orders.");
        return;
    }

    var orderId = ReadRequiredInt("Order id to edit (0 to cancel): ");
    if (orderId == 0)
    {
        return;
    }

    var selected = orders.GetById(orderId);
    if (selected is null)
    {
        Console.WriteLine("Order was not found.");
        return;
    }

    Console.WriteLine("Items:");
    foreach (var item in orderItems.GetByOrderId(selected.Id))
    {
        Console.WriteLine($"  Item #{item.Id}: product {item.ProductId}, qty {item.QuantityOrdered}, price {item.UnitPrice:0.00}");
    }

    Console.Write("New status (Draft/Ordered/Shipped/Delivered/Cancelled, empty keeps current): ");
    var status = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(status))
    {
        selected.Status = status.Trim();
    }

    Console.Write("New notes (empty keeps current): ");
    var notes = Console.ReadLine();
    if (!string.IsNullOrWhiteSpace(notes))
    {
        selected.Notes = notes.Trim();
    }

    Console.Write("Expected delivery date yyyy-MM-dd (empty keeps current): ");
    var dateText = Console.ReadLine();
    if (DateTime.TryParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsedDate))
    {
        selected.ExpectedDeliveryDate = parsedDate;
    }

    try
    {
        Console.WriteLine(orders.Update(selected) ? "Order updated." : "Order was not updated.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Could not update order: {ex.Message}");
    }
}

static void RunCrudDemo(
    WarehouseManagerRepository managers,
    SupplierRepository suppliers,
    ProductRepository products,
    WarehouseStockRepository stocks,
    SupplyOrderRepository orders,
    SupplyOrderItemRepository orderItems)
{
    var suffix = Guid.NewGuid().ToString("N")[..8];

    var managerId = managers.Create(new WarehouseManager
    {
        Username = $"demo_manager_{suffix}",
        PasswordHash = "demo-password-hash",
        FullName = "Demo Manager",
        Email = $"demo_manager_{suffix}@trading.local",
        CreatedAt = DateTime.UtcNow
    });

    var supplierId = suppliers.Create(new Supplier
    {
        Name = $"Demo Supplier {suffix}",
        ContactName = "Demo Contact",
        Phone = "+380501119999",
        Email = $"demo_supplier_{suffix}@trading.local",
        Address = "Demo address",
        IsActive = true
    });

    var productId = products.Create(new Product
    {
        SupplierId = supplierId,
        Sku = $"DEMO-{suffix}",
        Name = "Demo Product",
        Description = "Created during automatic CRUD demo",
        UnitPrice = 99.99m,
        UnitOfMeasure = "piece",
        MinimumStockLevel = 10
    });

    var stockId = stocks.Create(new WarehouseStock
    {
        ProductId = productId,
        QuantityOnHand = 50,
        ReservedQuantity = 5,
        UpdatedAt = DateTime.UtcNow
    });

    var orderId = orders.Create(new SupplyOrder
    {
        SupplierId = supplierId,
        ManagerId = managerId,
        Status = "Draft",
        OrderDate = DateTime.UtcNow,
        ExpectedDeliveryDate = DateTime.UtcNow.AddDays(5),
        Notes = "CRUD demo order"
    });

    var itemId = orderItems.Create(new SupplyOrderItem
    {
        OrderId = orderId,
        ProductId = productId,
        QuantityOrdered = 12,
        UnitPrice = 99.99m
    });

    Console.WriteLine("Created manager, supplier, product, stock row, order and order item.");

    var product = products.GetById(productId)!;
    product.UnitPrice = 109.50m;
    products.Update(product);

    var stock = stocks.GetById(stockId)!;
    stock.QuantityOnHand = 60;
    stocks.Update(stock);

    var order = orders.GetById(orderId)!;
    order.Status = "Ordered";
    orders.Update(order);

    Console.WriteLine("Read and updated product, stock and order.");

    orderItems.Delete(itemId);
    orders.Delete(orderId);
    stocks.Delete(stockId);
    products.Delete(productId);
    suppliers.Delete(supplierId);
    managers.Delete(managerId);

    Console.WriteLine("Deleted demo records in FK-safe order.");
}

static InventorySortField ReadInventorySortField()
{
    Console.WriteLine("Sort by: 1 - SKU, 2 - Name, 3 - Supplier, 4 - Stock, 5 - Price");
    Console.Write("Choose sort field: ");

    return Console.ReadLine() switch
    {
        "1" => InventorySortField.Sku,
        "3" => InventorySortField.SupplierName,
        "4" => InventorySortField.QuantityOnHand,
        "5" => InventorySortField.UnitPrice,
        _ => InventorySortField.Name
    };
}

static int ReadRequiredInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
        {
            return value;
        }

        Console.WriteLine("Enter a valid integer.");
    }
}

static string Trim(string value, int maxLength)
{
    return value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...";
}
