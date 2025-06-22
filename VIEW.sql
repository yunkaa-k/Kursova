USE ToysForBabys;

-- 1) Перегляд інформації про замовлення з клієнтами
CREATE VIEW vw_OrdersWithClientInfo AS
SELECT
    o.OrderID,
    o.OrderDate,
    o.TotalAmount,
    c.FirstName + ' ' + c.LastName AS ClientFullName,
    c.Email,
    c.Contacts
FROM Orders o
LEFT JOIN Clients c ON o.ClientID = c.ID;
GO

-- 2) Список клієнтів і кількість їхніх замовлень
CREATE VIEW vw_ClientsOrderStats AS
SELECT
    c.ID AS ClientID,
    c.FirstName + ' ' + c.LastName AS FullName,
    c.Email,
    COUNT(o.OrderID) AS TotalOrders,
    ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Clients c
LEFT JOIN Orders o ON c.ID = o.ClientID
GROUP BY c.ID, c.FirstName, c.LastName, c.Email;
GO

-- 3) Найпопулярніші товари (за кількістю замовлень)
CREATE VIEW vw_MostPopularItems AS
SELECT
    i.ItemID,
    i.Name AS ItemName,
    SUM(oi.Quantity) AS TotalSold,
    COUNT(DISTINCT oi.OrderID) AS OrdersCount
FROM OrderItems oi
JOIN Item i ON oi.ItemID = i.ItemID
GROUP BY i.ItemID, i.Name
GO

-- 4) Товари з низьким залишком на складі
CREATE VIEW vw_LowStockItems AS
SELECT
    ItemID,
    Name,
    Stock,
    Price
FROM Item
WHERE Stock < 30;
GO

