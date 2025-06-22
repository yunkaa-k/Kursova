USE ToysForBabys;

-- 1) Індекс на Email у таблиці Clients
CREATE NONCLUSTERED INDEX IX_Clients_Email
ON Clients(Email)

-- Перевірка використання індексу
SELECT ID, FirstName, LastName, Email
FROM Clients
WHERE Email = 'john.smith1@example.com';

-- 2) Індекс на OrderDate у таблиці Orders
CREATE NONCLUSTERED INDEX IX_Orders_OrderDate
ON Orders (OrderDate);

-- Перевірка використання індексу
SELECT OrderID, ClientID, TotalAmount
FROM Orders
WHERE OrderDate = '2024-01-01';

-- 3) Індекс на Price у таблиці Item
CREATE NONCLUSTERED INDEX IX_Item_Price
ON Item (Price);

-- Перевірка використання індексу
SELECT ItemID, Name, Price
FROM Item
WHERE Price = 100.00;