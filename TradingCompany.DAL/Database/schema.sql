IF OBJECT_ID(N'dbo.suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.suppliers (
        id INT IDENTITY(1,1) NOT NULL,
        name NVARCHAR(120) NOT NULL,
        contact_name NVARCHAR(120) NOT NULL,
        phone NVARCHAR(30) NOT NULL,
        email NVARCHAR(254) NOT NULL,
        address NVARCHAR(250) NOT NULL,
        is_active BIT NOT NULL CONSTRAINT DF_suppliers_is_active DEFAULT (1),
        CONSTRAINT PK_suppliers PRIMARY KEY (id),
        CONSTRAINT UQ_suppliers_name UNIQUE (name),
        CONSTRAINT UQ_suppliers_email UNIQUE (email)
    );
END;

IF OBJECT_ID(N'dbo.warehouse_managers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.warehouse_managers (
        id INT IDENTITY(1,1) NOT NULL,
        username NVARCHAR(60) NOT NULL,
        password_hash NVARCHAR(255) NOT NULL,
        full_name NVARCHAR(150) NOT NULL,
        email NVARCHAR(254) NOT NULL,
        created_at DATETIME2(0) NOT NULL,
        CONSTRAINT PK_warehouse_managers PRIMARY KEY (id),
        CONSTRAINT UQ_warehouse_managers_username UNIQUE (username),
        CONSTRAINT UQ_warehouse_managers_email UNIQUE (email)
    );
END;

IF OBJECT_ID(N'dbo.products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.products (
        id INT IDENTITY(1,1) NOT NULL,
        supplier_id INT NOT NULL,
        sku NVARCHAR(40) NOT NULL,
        name NVARCHAR(150) NOT NULL,
        description NVARCHAR(500) NOT NULL,
        unit_price DECIMAL(12, 2) NOT NULL,
        unit_of_measure NVARCHAR(30) NOT NULL,
        minimum_stock_level INT NOT NULL,
        CONSTRAINT PK_products PRIMARY KEY (id),
        CONSTRAINT UQ_products_sku UNIQUE (sku),
        CONSTRAINT CK_products_unit_price CHECK (unit_price >= 0),
        CONSTRAINT CK_products_minimum_stock_level CHECK (minimum_stock_level >= 0),
        CONSTRAINT FK_products_suppliers FOREIGN KEY (supplier_id)
            REFERENCES dbo.suppliers(id)
            ON UPDATE CASCADE
    );
END;

IF OBJECT_ID(N'dbo.warehouse_stocks', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.warehouse_stocks (
        id INT IDENTITY(1,1) NOT NULL,
        product_id INT NOT NULL,
        quantity_on_hand INT NOT NULL,
        reserved_quantity INT NOT NULL CONSTRAINT DF_warehouse_stocks_reserved_quantity DEFAULT (0),
        updated_at DATETIME2(0) NOT NULL,
        CONSTRAINT PK_warehouse_stocks PRIMARY KEY (id),
        CONSTRAINT UQ_warehouse_stocks_product_id UNIQUE (product_id),
        CONSTRAINT CK_warehouse_stocks_quantity CHECK (quantity_on_hand >= 0),
        CONSTRAINT CK_warehouse_stocks_reserved CHECK (reserved_quantity >= 0 AND reserved_quantity <= quantity_on_hand),
        CONSTRAINT FK_warehouse_stocks_products FOREIGN KEY (product_id)
            REFERENCES dbo.products(id)
            ON UPDATE CASCADE
            ON DELETE CASCADE
    );
END;

IF OBJECT_ID(N'dbo.supply_orders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.supply_orders (
        id INT IDENTITY(1,1) NOT NULL,
        supplier_id INT NOT NULL,
        manager_id INT NOT NULL,
        status NVARCHAR(20) NOT NULL,
        order_date DATETIME2(0) NOT NULL,
        expected_delivery_date DATETIME2(0) NULL,
        notes NVARCHAR(500) NOT NULL CONSTRAINT DF_supply_orders_notes DEFAULT (N''),
        CONSTRAINT PK_supply_orders PRIMARY KEY (id),
        CONSTRAINT CK_supply_orders_status CHECK (status IN (N'Draft', N'Ordered', N'Shipped', N'Delivered', N'Cancelled')),
        CONSTRAINT FK_supply_orders_suppliers FOREIGN KEY (supplier_id)
            REFERENCES dbo.suppliers(id)
            ON UPDATE CASCADE,
        CONSTRAINT FK_supply_orders_warehouse_managers FOREIGN KEY (manager_id)
            REFERENCES dbo.warehouse_managers(id)
            ON UPDATE CASCADE
    );
END;

IF OBJECT_ID(N'dbo.supply_order_items', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.supply_order_items (
        id INT IDENTITY(1,1) NOT NULL,
        order_id INT NOT NULL,
        product_id INT NOT NULL,
        quantity_ordered INT NOT NULL,
        unit_price DECIMAL(12, 2) NOT NULL,
        CONSTRAINT PK_supply_order_items PRIMARY KEY (id),
        CONSTRAINT UQ_supply_order_items_order_product UNIQUE (order_id, product_id),
        CONSTRAINT CK_supply_order_items_quantity CHECK (quantity_ordered > 0),
        CONSTRAINT CK_supply_order_items_unit_price CHECK (unit_price >= 0),
        CONSTRAINT FK_supply_order_items_supply_orders FOREIGN KEY (order_id)
            REFERENCES dbo.supply_orders(id)
            ON UPDATE CASCADE
            ON DELETE CASCADE,
        CONSTRAINT FK_supply_order_items_products FOREIGN KEY (product_id)
            REFERENCES dbo.products(id)
            ON UPDATE NO ACTION
            ON DELETE NO ACTION
    );
END;
