USE ToysForBabys;

-- 1) Додавання нового клієнта
CREATE PROCEDURE AddClient
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @Contacts NVARCHAR(50),
    @Email NVARCHAR(255)
AS
BEGIN
    INSERT INTO Clients (FirstName, LastName, Contacts, Email)
    VALUES (@FirstName, @LastName, @Contacts, @Email);
END;

EXEC AddClient
    @FirstName = N'Bob',
    @LastName = N'Bobov',
    @Contacts = N'+380931323456',
    @Email = N'BB0bov@example.com';

-- 2) Оновлення запасу товару
CREATE PROCEDURE UpdateItemStock
    @ItemID INT,
    @NewStock INT
AS
BEGIN
    UPDATE Item
    SET Stock = @NewStock
    WHERE ItemID = @ItemID;
END;

EXEC UpdateItemStock
    @ItemID = 1,
    @NewStock = 70;

-- 3) Отримати всі товари за категорією
CREATE PROCEDURE GetItemsByCategory
    @CategoryID INT
AS
BEGIN
    SELECT ItemID, Name, Price, Stock
    FROM Item
    WHERE CategoryID = @CategoryID
END;

EXEC GetItemsByCategory
    @CategoryID = 3;

-- 4) Отримати всі замовлення клієнта з деталями
CREATE PROCEDURE GetClientOrders
    @ClientID INT
AS
BEGIN
    SELECT o.OrderID, o.OrderDate, oi.ItemID, i.Name, oi.Quantity, oi.Price
    FROM Orders o
    JOIN OrderItems oi ON o.OrderID = oi.OrderID
    JOIN Item i ON oi.ItemID = i.ItemID
    WHERE o.ClientID = @ClientID
END;

EXEC GetClientOrders
    @ClientID = 100;

-- 5) Знайти постачальників для певного товару
CREATE PROCEDURE GetSuppliersForItem
    @ItemID INT
AS
BEGIN
    SELECT s.SupplierID, s.Name, s.Phone, s.Email
    FROM Supplies sp
    JOIN Suppliers s ON sp.SupplierID = s.SupplierID
    WHERE sp.ItemID = @ItemID
END;

EXEC GetSuppliersForItem
    @ItemID = 1;

-- 6) Пошук товарів за ключовим словом у назві
CREATE PROCEDURE SearchItemsByKeyword
    @Keyword NVARCHAR(100)
AS
BEGIN
    SELECT ItemID, Name, Description, Price, Stock
    FROM Item
    WHERE Name LIKE '%' + @Keyword + '%';
END;
GO

EXEC SearchItemsByKeyword @Keyword = 'Wooden';
