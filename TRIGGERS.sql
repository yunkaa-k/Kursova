USE Toysforbabys;

-- 1) Тригер на автоматичне оновлення дати останньої поставки
CREATE TRIGGER trg_UpdateLastDeliveryDate
ON Supplies
AFTER INSERT
AS
BEGIN
    UPDATE Supplies
    SET LastDeliveryDate = GETDATE()
    FROM Supplies
    JOIN inserted ON Supplies.SupplyID = inserted.SupplyID;
END;

-- 2) Тригер на перевірку що ціна товару > 0
CREATE TRIGGER trg_Item_PriceCheck
ON Item
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE Price <= 0)
    BEGIN
        THROW 50000, 'The price of the product must be greater than 0.', 1;
    END
END;
GO

-- 3) Тригер на перевірку кількості товарів в замовленні які > 0
CREATE TRIGGER trg_OrderItem_QuantityCheck
ON OrderItems
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE Quantity <= 0)
    BEGIN
        THROW 50000, 'The number of items in the order must be greater than 0.', 1;
    END
END;
GO

-- 4) Тригер на перевірку ціни постачання > 0
CREATE TRIGGER trg_SupplyPriceCheck
ON Supplies
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted WHERE SupplyPrice <= 0)
    BEGIN
        THROW 50000, 'The delivery price must be greater than 0.', 1;
    END
END;
GO

-- 5) Тригер на перевірку наявності обов’язкових полів у Item
CREATE TRIGGER trg_Item_RequiredFields
ON Item
AFTER INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted
        WHERE Name IS NULL OR Description IS NULL OR Stock IS NULL
    )
    BEGIN
        THROW 50000, 'The product must have a name, description, and quantity in stock.', 1;
    END
END;
GO

-- 6) Тригер на перевірку
CREATE TRIGGER trg_Clients_Insert_Validation
ON Clients
INSTEAD OF INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM inserted i
        JOIN Clients c ON i.Contacts = c.Contacts OR i.Email = c.Email
    )
    BEGIN
        THROW 50013, 'The contacts or email are already being used by another client.', 1;
        RETURN;
    END

    INSERT INTO Clients (FirstName, LastName, Contacts, Email)
    SELECT FirstName, LastName, Contacts, Email FROM inserted;
END;
GO

-- Тест 1)
INSERT INTO Supplies (SupplierID, ItemID, SupplyPrice)
VALUES (21, 1, 120.00);

-- Тест 2)
INSERT INTO Item (Name, CategoryID, Description, Price, Stock)
VALUES (N'Test1', 1, N'Test1', 0, 10);

-- Тест 3)
INSERT INTO OrderItems (OrderID, ItemID, Quantity, Price)
VALUES (1, 1, 0, 100.00);

-- Тест 4)
INSERT INTO Supplies (SupplierID, ItemID, SupplyPrice, LastDeliveryDate)
VALUES (1, 1, 0, GETDATE());

-- Тест 5)
INSERT INTO Item (Name, CategoryID, Description, Price, Stock)
VALUES (NULL, 1, N'Опис', 100.00, 5);

INSERT INTO Item (Name, CategoryID, Description, Price, Stock)
VALUES (N'Іграшка', 1, NULL, 100.00, 5);

-- Тест 6)
INSERT INTO Clients (FirstName, LastName, Contacts, Email)
VALUES (N'Oleg', N'Kizyanov', N'+380501112233', N'john.smith1@example.com');
