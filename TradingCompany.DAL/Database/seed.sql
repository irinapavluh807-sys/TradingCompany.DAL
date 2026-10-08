SET IDENTITY_INSERT dbo.warehouse_managers ON;

MERGE dbo.warehouse_managers AS target
USING (VALUES
(1, N'manager1', N'demo-password-hash', N'Olena Koval', N'manager1@trading.local', CONVERT(datetime2(0), '2026-01-01T08:00:00')),
(2, N'manager2', N'demo-password-hash', N'Andrii Bondar', N'manager2@trading.local', CONVERT(datetime2(0), '2026-01-02T08:00:00')),
(3, N'manager3', N'demo-password-hash', N'Iryna Melnyk', N'manager3@trading.local', CONVERT(datetime2(0), '2026-01-03T08:00:00')),
(4, N'manager4', N'demo-password-hash', N'Taras Shevchuk', N'manager4@trading.local', CONVERT(datetime2(0), '2026-01-04T08:00:00')),
(5, N'manager5', N'demo-password-hash', N'Marta Lysenko', N'manager5@trading.local', CONVERT(datetime2(0), '2026-01-05T08:00:00')),
(6, N'manager6', N'demo-password-hash', N'Roman Hnatiuk', N'manager6@trading.local', CONVERT(datetime2(0), '2026-01-06T08:00:00')),
(7, N'manager7', N'demo-password-hash', N'Sofia Tkachenko', N'manager7@trading.local', CONVERT(datetime2(0), '2026-01-07T08:00:00')),
(8, N'manager8', N'demo-password-hash', N'Dmytro Kravets', N'manager8@trading.local', CONVERT(datetime2(0), '2026-01-08T08:00:00')),
(9, N'manager9', N'demo-password-hash', N'Natalia Moroz', N'manager9@trading.local', CONVERT(datetime2(0), '2026-01-09T08:00:00')),
(10, N'manager10', N'demo-password-hash', N'Petro Savchuk', N'manager10@trading.local', CONVERT(datetime2(0), '2026-01-10T08:00:00')),
(11, N'manager11', N'demo-password-hash', N'Viktoria Marchuk', N'manager11@trading.local', CONVERT(datetime2(0), '2026-01-11T08:00:00')),
(12, N'manager12', N'demo-password-hash', N'Yurii Kovalenko', N'manager12@trading.local', CONVERT(datetime2(0), '2026-01-12T08:00:00')),
(13, N'manager13', N'demo-password-hash', N'Kateryna Oliinyk', N'manager13@trading.local', CONVERT(datetime2(0), '2026-01-13T08:00:00')),
(14, N'manager14', N'demo-password-hash', N'Bohdan Rudenko', N'manager14@trading.local', CONVERT(datetime2(0), '2026-01-14T08:00:00')),
(15, N'manager15', N'demo-password-hash', N'Anastasiia Sokol', N'manager15@trading.local', CONVERT(datetime2(0), '2026-01-15T08:00:00')),
(16, N'manager16', N'demo-password-hash', N'Mykhailo Polishchuk', N'manager16@trading.local', CONVERT(datetime2(0), '2026-01-16T08:00:00')),
(17, N'manager17', N'demo-password-hash', N'Oksana Martynenko', N'manager17@trading.local', CONVERT(datetime2(0), '2026-01-17T08:00:00')),
(18, N'manager18', N'demo-password-hash', N'Serhii Mazur', N'manager18@trading.local', CONVERT(datetime2(0), '2026-01-18T08:00:00')),
(19, N'manager19', N'demo-password-hash', N'Liliia Horbenko', N'manager19@trading.local', CONVERT(datetime2(0), '2026-01-19T08:00:00')),
(20, N'manager20', N'demo-password-hash', N'Pavlo Danyliuk', N'manager20@trading.local', CONVERT(datetime2(0), '2026-01-20T08:00:00')),
(21, N'manager21', N'demo-password-hash', N'Yevheniia Klymenko', N'manager21@trading.local', CONVERT(datetime2(0), '2026-01-21T08:00:00')),
(22, N'manager22', N'demo-password-hash', N'Ihor Tkach', N'manager22@trading.local', CONVERT(datetime2(0), '2026-01-22T08:00:00')),
(23, N'manager23', N'demo-password-hash', N'Daryna Chorna', N'manager23@trading.local', CONVERT(datetime2(0), '2026-01-23T08:00:00')),
(24, N'manager24', N'demo-password-hash', N'Vladyslav Bilyk', N'manager24@trading.local', CONVERT(datetime2(0), '2026-01-24T08:00:00')),
(25, N'manager25', N'demo-password-hash', N'Halyna Sydorenko', N'manager25@trading.local', CONVERT(datetime2(0), '2026-01-25T08:00:00'))
) AS source (id, username, password_hash, full_name, email, created_at)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, username, password_hash, full_name, email, created_at)
    VALUES (source.id, source.username, source.password_hash, source.full_name, source.email, source.created_at);

SET IDENTITY_INSERT dbo.warehouse_managers OFF;

SET IDENTITY_INSERT dbo.suppliers ON;

MERGE dbo.suppliers AS target
USING (VALUES
(1, N'Dnipro Foods LLC', N'Oleh Karp', N'+380501110001', N'sales@dniprofoods.local', N'12 River St, Dnipro', CONVERT(bit, 1)),
(2, N'Carpathian Goods', N'Nadia Verba', N'+380501110002', N'orders@carpathiangoods.local', N'7 Mountain Ave, Uzhhorod', CONVERT(bit, 1)),
(3, N'Kyiv Packaging', N'Ivan Sereda', N'+380501110003', N'contact@kyivpack.local', N'31 Industrial Rd, Kyiv', CONVERT(bit, 1)),
(4, N'Lviv Coffee Trade', N'Hanna Lev', N'+380501110004', N'supply@lvivcoffee.local', N'5 Market Sq, Lviv', CONVERT(bit, 1)),
(5, N'Odesa Import Group', N'Artem Lito', N'+380501110005', N'office@odesaimport.local', N'90 Port Blvd, Odesa', CONVERT(bit, 1)),
(6, N'Poltava Dairy', N'Maksym Bilous', N'+380501110006', N'sales@poltavadairy.local', N'18 Farm Rd, Poltava', CONVERT(bit, 1)),
(7, N'Kharkiv Hardware', N'Svitlana Dub', N'+380501110007', N'orders@khhardware.local', N'42 Factory St, Kharkiv', CONVERT(bit, 1)),
(8, N'Chernihiv Paper', N'Denys Byk', N'+380501110008', N'paper@chernihivpaper.local', N'11 Forest Lane, Chernihiv', CONVERT(bit, 1)),
(9, N'Vinnytsia Fresh', N'Alla Hrom', N'+380501110009', N'fresh@vinnytsiafresh.local', N'24 Garden St, Vinnytsia', CONVERT(bit, 1)),
(10, N'Zaporizhzhia Tools', N'Yevhen Dnister', N'+380501110010', N'tools@zptools.local', N'66 Metal Ave, Zaporizhzhia', CONVERT(bit, 1)),
(11, N'Rivne Textiles', N'Mariia Kvit', N'+380501110011', N'orders@rivnetextiles.local', N'14 Textile St, Rivne', CONVERT(bit, 1)),
(12, N'Ternopil Grains', N'Stepan Hai', N'+380501110012', N'grain@ternopilgrains.local', N'20 Mill Rd, Ternopil', CONVERT(bit, 1)),
(13, N'Sumy Chemicals', N'Valentyn Horikh', N'+380501110013', N'chem@sumychem.local', N'3 Lab St, Sumy', CONVERT(bit, 1)),
(14, N'Ivano Decor', N'Nina Chumak', N'+380501110014', N'decor@ivanodecor.local', N'8 Craft St, Ivano-Frankivsk', CONVERT(bit, 1)),
(15, N'Cherkasy Plastics', N'Petro Lypa', N'+380501110015', N'plastic@cherkasyplast.local', N'55 Polymer Rd, Cherkasy', CONVERT(bit, 1)),
(16, N'Mykolaiv Marine', N'Borys Shyp', N'+380501110016', N'marine@mykolaivmarine.local', N'2 Dock St, Mykolaiv', CONVERT(bit, 1)),
(17, N'Zhytomyr Snacks', N'Inna Horobets', N'+380501110017', N'snacks@zhytomyrsnacks.local', N'17 Snack Ave, Zhytomyr', CONVERT(bit, 1)),
(18, N'Kropyvnytskyi Office', N'Oleksii Rad', N'+380501110018', N'office@kropoffice.local', N'29 Desk Rd, Kropyvnytskyi', CONVERT(bit, 1)),
(19, N'Lutsk Beverages', N'Yana Sova', N'+380501110019', N'drink@lutskbev.local', N'44 Bottle St, Lutsk', CONVERT(bit, 1)),
(20, N'Kherson Cleaning', N'Mila Bashta', N'+380501110020', N'clean@khersonclean.local', N'10 Clean Rd, Kherson', CONVERT(bit, 1)),
(21, N'Bucha Household', N'Orest Ladan', N'+380501110021', N'home@buchahousehold.local', N'21 Home St, Bucha', CONVERT(bit, 1)),
(22, N'Fastiv Electronics', N'Liubov Iskra', N'+380501110022', N'electro@fastivelectronics.local', N'12 Circuit Rd, Fastiv', CONVERT(bit, 1)),
(23, N'Bila Tserkva Bakery', N'Mykola Pich', N'+380501110023', N'bakery@btbakery.local', N'6 Bread Ave, Bila Tserkva', CONVERT(bit, 1)),
(24, N'Mukachevo Tea House', N'Roksolana Lis', N'+380501110024', N'tea@mukachevotea.local', N'33 Tea Lane, Mukachevo', CONVERT(bit, 1)),
(25, N'Kamianets Ceramics', N'Arsen Hlynka', N'+380501110025', N'ceramics@kamceramics.local', N'4 Clay St, Kamianets-Podilskyi', CONVERT(bit, 1))
) AS source (id, name, contact_name, phone, email, address, is_active)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, name, contact_name, phone, email, address, is_active)
    VALUES (source.id, source.name, source.contact_name, source.phone, source.email, source.address, source.is_active);

SET IDENTITY_INSERT dbo.suppliers OFF;

SET IDENTITY_INSERT dbo.products ON;

MERGE dbo.products AS target
USING (VALUES
(1, 1, N'PRD-1001', N'Sunflower Oil 1L', N'Refined sunflower oil bottle', CONVERT(decimal(12, 2), 62.50), N'bottle', 120),
(2, 2, N'PRD-1002', N'Buckwheat 1kg', N'Packed buckwheat groats', CONVERT(decimal(12, 2), 48.90), N'pack', 150),
(3, 3, N'PRD-1003', N'Cardboard Box M', N'Medium shipping cardboard box', CONVERT(decimal(12, 2), 18.20), N'piece', 300),
(4, 4, N'PRD-1004', N'Arabica Coffee 250g', N'Ground arabica coffee', CONVERT(decimal(12, 2), 139.00), N'pack', 80),
(5, 5, N'PRD-1005', N'Olive Mix 500g', N'Imported mixed olives', CONVERT(decimal(12, 2), 112.30), N'jar', 60),
(6, 6, N'PRD-1006', N'Cheese Classic 200g', N'Semi-hard packaged cheese', CONVERT(decimal(12, 2), 83.40), N'pack', 90),
(7, 7, N'PRD-1007', N'Steel Screw Set', N'Assorted steel screws', CONVERT(decimal(12, 2), 55.00), N'box', 200),
(8, 8, N'PRD-1008', N'A4 Paper 500', N'Office paper ream', CONVERT(decimal(12, 2), 178.00), N'ream', 100),
(9, 9, N'PRD-1009', N'Apple Juice 1L', N'Pasteurized apple juice', CONVERT(decimal(12, 2), 44.70), N'carton', 130),
(10, 10, N'PRD-1010', N'Hammer 500g', N'Metal hammer with wooden handle', CONVERT(decimal(12, 2), 245.00), N'piece', 40),
(11, 11, N'PRD-1011', N'Cotton Towel', N'White cotton towel', CONVERT(decimal(12, 2), 96.00), N'piece', 75),
(12, 12, N'PRD-1012', N'Wheat Flour 2kg', N'Fine wheat flour', CONVERT(decimal(12, 2), 57.80), N'bag', 160),
(13, 13, N'PRD-1013', N'Dish Soap 500ml', N'Liquid dish soap', CONVERT(decimal(12, 2), 39.90), N'bottle', 140),
(14, 14, N'PRD-1014', N'Ceramic Mug', N'Decorated ceramic mug', CONVERT(decimal(12, 2), 88.50), N'piece', 70),
(15, 15, N'PRD-1015', N'Plastic Container 1L', N'Food storage container', CONVERT(decimal(12, 2), 64.10), N'piece', 110),
(16, 16, N'PRD-1016', N'Rope 20m', N'Marine utility rope', CONVERT(decimal(12, 2), 156.00), N'coil', 35),
(17, 17, N'PRD-1017', N'Potato Chips 120g', N'Salted potato chips', CONVERT(decimal(12, 2), 36.60), N'pack', 180),
(18, 18, N'PRD-1018', N'Notebook A5', N'Lined office notebook', CONVERT(decimal(12, 2), 41.20), N'piece', 210),
(19, 19, N'PRD-1019', N'Mineral Water 1.5L', N'Still mineral water', CONVERT(decimal(12, 2), 24.90), N'bottle', 240),
(20, 20, N'PRD-1020', N'Floor Cleaner 1L', N'Lemon floor cleaner', CONVERT(decimal(12, 2), 73.30), N'bottle', 95),
(21, 21, N'PRD-1021', N'Kitchen Sponge Pack', N'Five-piece kitchen sponge pack', CONVERT(decimal(12, 2), 32.40), N'pack', 170),
(22, 22, N'PRD-1022', N'LED Bulb 10W', N'Energy-saving LED bulb', CONVERT(decimal(12, 2), 68.90), N'piece', 120),
(23, 23, N'PRD-1023', N'Rye Bread 600g', N'Fresh packaged rye bread', CONVERT(decimal(12, 2), 29.80), N'loaf', 160),
(24, 24, N'PRD-1024', N'Black Tea 100g', N'Loose leaf black tea', CONVERT(decimal(12, 2), 91.70), N'pack', 85),
(25, 25, N'PRD-1025', N'Ceramic Plate', N'White ceramic dinner plate', CONVERT(decimal(12, 2), 104.20), N'piece', 65)
) AS source (id, supplier_id, sku, name, description, unit_price, unit_of_measure, minimum_stock_level)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, supplier_id, sku, name, description, unit_price, unit_of_measure, minimum_stock_level)
    VALUES (source.id, source.supplier_id, source.sku, source.name, source.description, source.unit_price, source.unit_of_measure, source.minimum_stock_level);

SET IDENTITY_INSERT dbo.products OFF;

SET IDENTITY_INSERT dbo.warehouse_stocks ON;

MERGE dbo.warehouse_stocks AS target
USING (VALUES
(1, 1, 510, 40, CONVERT(datetime2(0), '2026-09-01T09:00:00')),
(2, 2, 430, 30, CONVERT(datetime2(0), '2026-09-01T09:05:00')),
(3, 3, 980, 120, CONVERT(datetime2(0), '2026-09-01T09:10:00')),
(4, 4, 220, 15, CONVERT(datetime2(0), '2026-09-01T09:15:00')),
(5, 5, 145, 20, CONVERT(datetime2(0), '2026-09-01T09:20:00')),
(6, 6, 175, 25, CONVERT(datetime2(0), '2026-09-01T09:25:00')),
(7, 7, 620, 50, CONVERT(datetime2(0), '2026-09-01T09:30:00')),
(8, 8, 310, 35, CONVERT(datetime2(0), '2026-09-01T09:35:00')),
(9, 9, 470, 45, CONVERT(datetime2(0), '2026-09-01T09:40:00')),
(10, 10, 90, 6, CONVERT(datetime2(0), '2026-09-01T09:45:00')),
(11, 11, 205, 18, CONVERT(datetime2(0), '2026-09-01T09:50:00')),
(12, 12, 520, 55, CONVERT(datetime2(0), '2026-09-01T09:55:00')),
(13, 13, 390, 28, CONVERT(datetime2(0), '2026-09-01T10:00:00')),
(14, 14, 165, 14, CONVERT(datetime2(0), '2026-09-01T10:05:00')),
(15, 15, 280, 30, CONVERT(datetime2(0), '2026-09-01T10:10:00')),
(16, 16, 75, 5, CONVERT(datetime2(0), '2026-09-01T10:15:00')),
(17, 17, 690, 80, CONVERT(datetime2(0), '2026-09-01T10:20:00')),
(18, 18, 740, 65, CONVERT(datetime2(0), '2026-09-01T10:25:00')),
(19, 19, 860, 95, CONVERT(datetime2(0), '2026-09-01T10:30:00')),
(20, 20, 235, 22, CONVERT(datetime2(0), '2026-09-01T10:35:00')),
(21, 21, 455, 38, CONVERT(datetime2(0), '2026-09-01T10:40:00')),
(22, 22, 300, 24, CONVERT(datetime2(0), '2026-09-01T10:45:00')),
(23, 23, 510, 60, CONVERT(datetime2(0), '2026-09-01T10:50:00')),
(24, 24, 190, 16, CONVERT(datetime2(0), '2026-09-01T10:55:00')),
(25, 25, 150, 12, CONVERT(datetime2(0), '2026-09-01T11:00:00'))
) AS source (id, product_id, quantity_on_hand, reserved_quantity, updated_at)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, product_id, quantity_on_hand, reserved_quantity, updated_at)
    VALUES (source.id, source.product_id, source.quantity_on_hand, source.reserved_quantity, source.updated_at);

SET IDENTITY_INSERT dbo.warehouse_stocks OFF;

SET IDENTITY_INSERT dbo.supply_orders ON;

MERGE dbo.supply_orders AS target
USING (VALUES
(1, 1, 1, N'Draft', CONVERT(datetime2(0), '2026-09-02T08:00:00'), CONVERT(datetime2(0), '2026-09-09T08:00:00'), N'Restock base oils'),
(2, 2, 2, N'Ordered', CONVERT(datetime2(0), '2026-09-03T08:00:00'), CONVERT(datetime2(0), '2026-09-10T08:00:00'), N'Weekly grain supply'),
(3, 3, 3, N'Shipped', CONVERT(datetime2(0), '2026-09-04T08:00:00'), CONVERT(datetime2(0), '2026-09-11T08:00:00'), N'Packaging materials'),
(4, 4, 4, N'Delivered', CONVERT(datetime2(0), '2026-09-05T08:00:00'), CONVERT(datetime2(0), '2026-09-12T08:00:00'), N'Coffee shipment received'),
(5, 5, 5, N'Cancelled', CONVERT(datetime2(0), '2026-09-06T08:00:00'), CONVERT(datetime2(0), '2026-09-13T08:00:00'), N'Supplier postponed batch'),
(6, 6, 6, N'Draft', CONVERT(datetime2(0), '2026-09-07T08:00:00'), CONVERT(datetime2(0), '2026-09-14T08:00:00'), N'Dairy replenishment'),
(7, 7, 7, N'Ordered', CONVERT(datetime2(0), '2026-09-08T08:00:00'), CONVERT(datetime2(0), '2026-09-15T08:00:00'), N'Hardware stock'),
(8, 8, 8, N'Shipped', CONVERT(datetime2(0), '2026-09-09T08:00:00'), CONVERT(datetime2(0), '2026-09-16T08:00:00'), N'Paper for office customers'),
(9, 9, 9, N'Delivered', CONVERT(datetime2(0), '2026-09-10T08:00:00'), CONVERT(datetime2(0), '2026-09-17T08:00:00'), N'Fresh juice arrived'),
(10, 10, 10, N'Draft', CONVERT(datetime2(0), '2026-09-11T08:00:00'), CONVERT(datetime2(0), '2026-09-18T08:00:00'), N'Tool replenishment'),
(11, 11, 11, N'Ordered', CONVERT(datetime2(0), '2026-09-12T08:00:00'), CONVERT(datetime2(0), '2026-09-19T08:00:00'), N'Textile order'),
(12, 12, 12, N'Shipped', CONVERT(datetime2(0), '2026-09-13T08:00:00'), CONVERT(datetime2(0), '2026-09-20T08:00:00'), N'Flour batch'),
(13, 13, 13, N'Delivered', CONVERT(datetime2(0), '2026-09-14T08:00:00'), CONVERT(datetime2(0), '2026-09-21T08:00:00'), N'Cleaning chemicals'),
(14, 14, 14, N'Draft', CONVERT(datetime2(0), '2026-09-15T08:00:00'), CONVERT(datetime2(0), '2026-09-22T08:00:00'), N'Decor goods'),
(15, 15, 15, N'Ordered', CONVERT(datetime2(0), '2026-09-16T08:00:00'), CONVERT(datetime2(0), '2026-09-23T08:00:00'), N'Containers'),
(16, 16, 16, N'Shipped', CONVERT(datetime2(0), '2026-09-17T08:00:00'), CONVERT(datetime2(0), '2026-09-24T08:00:00'), N'Marine goods'),
(17, 17, 17, N'Delivered', CONVERT(datetime2(0), '2026-09-18T08:00:00'), CONVERT(datetime2(0), '2026-09-25T08:00:00'), N'Snack restock'),
(18, 18, 18, N'Draft', CONVERT(datetime2(0), '2026-09-19T08:00:00'), CONVERT(datetime2(0), '2026-09-26T08:00:00'), N'Office supplies'),
(19, 19, 19, N'Ordered', CONVERT(datetime2(0), '2026-09-20T08:00:00'), CONVERT(datetime2(0), '2026-09-27T08:00:00'), N'Water delivery'),
(20, 20, 20, N'Shipped', CONVERT(datetime2(0), '2026-09-21T08:00:00'), CONVERT(datetime2(0), '2026-09-28T08:00:00'), N'Cleaning stock'),
(21, 21, 21, N'Draft', CONVERT(datetime2(0), '2026-09-22T08:00:00'), CONVERT(datetime2(0), '2026-09-29T08:00:00'), N'Household stock'),
(22, 22, 22, N'Ordered', CONVERT(datetime2(0), '2026-09-23T08:00:00'), CONVERT(datetime2(0), '2026-09-30T08:00:00'), N'Electronics restock'),
(23, 23, 23, N'Shipped', CONVERT(datetime2(0), '2026-09-24T08:00:00'), CONVERT(datetime2(0), '2026-10-01T08:00:00'), N'Bakery goods'),
(24, 24, 24, N'Delivered', CONVERT(datetime2(0), '2026-09-25T08:00:00'), CONVERT(datetime2(0), '2026-10-02T08:00:00'), N'Tea shipment'),
(25, 25, 25, N'Ordered', CONVERT(datetime2(0), '2026-09-26T08:00:00'), CONVERT(datetime2(0), '2026-10-03T08:00:00'), N'Ceramic goods')
) AS source (id, supplier_id, manager_id, status, order_date, expected_delivery_date, notes)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, supplier_id, manager_id, status, order_date, expected_delivery_date, notes)
    VALUES (source.id, source.supplier_id, source.manager_id, source.status, source.order_date, source.expected_delivery_date, source.notes);

SET IDENTITY_INSERT dbo.supply_orders OFF;

SET IDENTITY_INSERT dbo.supply_order_items ON;

MERGE dbo.supply_order_items AS target
USING (VALUES
(1, 1, 1, 180, CONVERT(decimal(12, 2), 62.50)),
(2, 2, 2, 220, CONVERT(decimal(12, 2), 48.90)),
(3, 3, 3, 500, CONVERT(decimal(12, 2), 18.20)),
(4, 4, 4, 120, CONVERT(decimal(12, 2), 139.00)),
(5, 5, 5, 80, CONVERT(decimal(12, 2), 112.30)),
(6, 6, 6, 95, CONVERT(decimal(12, 2), 83.40)),
(7, 7, 7, 300, CONVERT(decimal(12, 2), 55.00)),
(8, 8, 8, 170, CONVERT(decimal(12, 2), 178.00)),
(9, 9, 9, 210, CONVERT(decimal(12, 2), 44.70)),
(10, 10, 10, 50, CONVERT(decimal(12, 2), 245.00)),
(11, 11, 11, 105, CONVERT(decimal(12, 2), 96.00)),
(12, 12, 12, 260, CONVERT(decimal(12, 2), 57.80)),
(13, 13, 13, 190, CONVERT(decimal(12, 2), 39.90)),
(14, 14, 14, 75, CONVERT(decimal(12, 2), 88.50)),
(15, 15, 15, 140, CONVERT(decimal(12, 2), 64.10)),
(16, 16, 16, 45, CONVERT(decimal(12, 2), 156.00)),
(17, 17, 17, 310, CONVERT(decimal(12, 2), 36.60)),
(18, 18, 18, 360, CONVERT(decimal(12, 2), 41.20)),
(19, 19, 19, 420, CONVERT(decimal(12, 2), 24.90)),
(20, 20, 20, 135, CONVERT(decimal(12, 2), 73.30)),
(21, 21, 21, 240, CONVERT(decimal(12, 2), 32.40)),
(22, 22, 22, 180, CONVERT(decimal(12, 2), 68.90)),
(23, 23, 23, 260, CONVERT(decimal(12, 2), 29.80)),
(24, 24, 24, 95, CONVERT(decimal(12, 2), 91.70)),
(25, 25, 25, 75, CONVERT(decimal(12, 2), 104.20))
) AS source (id, order_id, product_id, quantity_ordered, unit_price)
ON target.id = source.id
WHEN NOT MATCHED BY TARGET THEN
    INSERT (id, order_id, product_id, quantity_ordered, unit_price)
    VALUES (source.id, source.order_id, source.product_id, source.quantity_ordered, source.unit_price);

SET IDENTITY_INSERT dbo.supply_order_items OFF;
