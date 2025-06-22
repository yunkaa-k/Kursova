USE ToysForBabys;

-- Отримати повне ім'я клієнта
CREATE FUNCTION GetClientFullName(@ClientID INT)
RETURNS NVARCHAR(511)
AS
BEGIN
    DECLARE @FullName NVARCHAR(511)
    SELECT @FullName = FirstName + ' ' + LastName
    FROM Clients
    WHERE ID = @ClientID
    RETURN @FullName
END;

SELECT dbo.GetClientFullName(1) AS FullName;

-- Повертає номер телефону складу за назвою
CREATE FUNCTION GetWarehousePhone (@WarehouseName NVARCHAR(255))
RETURNS NVARCHAR(50)
AS
BEGIN
    DECLARE @Phone NVARCHAR(50);
    SELECT @Phone = Phone
    FROM Warehouses
    WHERE Name = @WarehouseName;

    RETURN ISNULL(@Phone, N'Unknown');
END;
GO

SELECT dbo.GetWarehousePhone(N'Toys Warehouse AA') AS Phone;

-- Повертає середню ціну товарів у вказаній категорії
CREATE FUNCTION GetAverageItemPriceByCategory (@CategoryID INT)
RETURNS DECIMAL(10,2)
AS
BEGIN
    DECLARE @AvgPrice DECIMAL(10,2);
    SELECT @AvgPrice = AVG(Price)
    FROM Item
    WHERE CategoryID = @CategoryID;

    RETURN ISNULL(@AvgPrice, 0);
END;
GO

SELECT dbo.GetAverageItemPriceByCategory(2) AS AvgPrice;

-- Повертає повне ім’я працівника за його ID
CREATE FUNCTION GetEmployeeFullName (@EmployeeID INT)
RETURNS NVARCHAR(255)
AS
BEGIN
    DECLARE @FullName NVARCHAR(255);
    SELECT @FullName = FirstName + N' ' + LastName
    FROM Employees
    WHERE EmployeeID = @EmployeeID;

    RETURN @FullName;
END;
GO

SELECT dbo.GetEmployeeFullName(20) AS EmployeeName;

