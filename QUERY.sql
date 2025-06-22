USE ToysForBabys;

-- 1) Всі товари з їх категоріями
SELECT i.ItemID, i.Name AS ItemName, c.Name AS CategoryName, i.Price, i.Stock
FROM Item i
JOIN Categories c ON i.CategoryID = c.CategoryID;

-- 2) Товари, ціна яких більше 50
SELECT Name, Price FROM Item WHERE Price > 50 ORDER BY Price DESC;

-- 3) Загальна сума замовлень кожного клієнта
SELECT c.ID, c.FirstName, c.LastName, SUM(o.TotalAmount) AS TotalSpent
FROM Clients c
JOIN Orders o ON c.ID = o.ClientID
GROUP BY c.ID, c.FirstName, c.LastName;

-- 4) Найдорожчий товар в кожній категорії (підзапит)
SELECT c.Name AS CategoryName, i.Name AS ItemName, i.Price
FROM Item i
JOIN Categories c ON i.CategoryID = c.CategoryID
WHERE i.Price = (SELECT MAX(Price) FROM Item WHERE CategoryID = c.CategoryID);

-- 5) Товари з кількістю на складі менше 30
SELECT Name, Stock FROM Item WHERE Stock < 30;

-- 6) Замовлення з датою останнього тижня
SELECT * FROM Orders WHERE OrderDate >= DATEADD(day, -7, GETDATE());

-- 7) Деталі замовлення для конкретного замовлення
SELECT oi.OrderItemID, i.Name, oi.Quantity, oi.Price
FROM OrderItems oi
JOIN Item i ON oi.ItemID = i.ItemID
WHERE oi.OrderID = 1;

-- 8) Поставки певного постачальника
SELECT s.SupplyID, i.Name AS ItemName, s.SupplyPrice, s.LastDeliveryDate
FROM Supplies s
JOIN Item i ON s.ItemID = i.ItemID
WHERE s.SupplierID = 2;

-- 9) Постачальники, які постачають товар з ціною більше 70
SELECT sup.SupplierID, sup.Name
FROM Suppliers sup
JOIN Supplies sp ON sup.SupplierID = sp.SupplierID
JOIN Item i ON sp.ItemID = i.ItemID
WHERE i.Price > 70;

-- 10) Клієнти, які ще не зробили жодного замовлення (підзапит)
SELECT * FROM Clients
WHERE ID NOT IN (SELECT DISTINCT ClientID FROM Orders WHERE ClientID IS NOT NULL);

-- 11) Загальна кількість проданих одиниць товару (всі замовлення)
SELECT SUM(Quantity) AS TotalItemsSold FROM OrderItems;

-- 12) Найпопулярніший товар за кількістю проданих одиниць (підзапит)
SELECT TOP 1 i.Name, SUM(oi.Quantity) AS TotalSold
FROM OrderItems oi
JOIN Item i ON oi.ItemID = i.ItemID
GROUP BY i.Name
ORDER BY TotalSold DESC;

-- 13) Працівники, найняті після 2023 року
SELECT * FROM Employees WHERE HireDate > '2023-01-01';

-- 14) Замовлення із сумою більше середньої по всім замовленням (підзапит)
SELECT * FROM Orders
WHERE TotalAmount > (SELECT AVG(TotalAmount) FROM Orders);

-- 15) Всі доставки із замовленнями
SELECT od.OrderDeliveryID, o.OrderID, o.TotalAmount, od.DeliveryDate
FROM OrderDelivery od
JOIN Orders o ON od.OrderID = o.OrderID;

-- 16) Кількість товарів у кожній категорії
SELECT c.Name, COUNT(i.ItemID) AS ItemCount
FROM Categories c
LEFT JOIN Item i ON c.CategoryID = i.CategoryID
GROUP BY c.Name;

-- 17) Товари, що не мають поставок (підзапит)
SELECT * FROM Item
WHERE ItemID NOT IN (SELECT DISTINCT ItemID FROM Supplies);

-- 18) Вивести всі замовлення разом з даними клієнтів
SELECT o.OrderID, o.OrderDate, c.FirstName, c.LastName, c.Email
FROM Orders o
JOIN Clients c ON o.ClientID = c.ID;


-- 19) Перелік замовлених товарів разом з кількістю і ціною
SELECT o.OrderID, i.Name AS ItemName, oi.Quantity, oi.Price
FROM Orders o
JOIN OrderItems oi ON o.OrderID = oi.OrderID
JOIN Item i ON oi.ItemID = i.ItemID;

-- 20) Список постачальників, які ніколи не постачали жодного товару
SELECT sup.SupplierID, sup.Name
FROM Suppliers sup
LEFT JOIN Supplies s ON sup.SupplierID = s.SupplierID
WHERE s.SupplyID IS NULL;

-- 21) Товари з постачальниками, що мають поставку дорожчу за 170
SELECT i.Name AS ItemName, sup.Name AS SupplierName, s.SupplyPrice
FROM Supplies s
JOIN Item i ON s.ItemID = i.ItemID
JOIN Suppliers sup ON s.SupplierID = sup.SupplierID
WHERE s.SupplyPrice > 170;
