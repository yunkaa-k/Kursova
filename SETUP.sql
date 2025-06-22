USE ToysForBabys;

-- Категорії
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(255) NOT NULL UNIQUE
);
GO

-- Товари
CREATE TABLE Item (
    ItemID INT PRIMARY KEY IDENTITY (1,1),
    Name NVARCHAR(255),
    CategoryID INT NOT NULL,
    Description NVARCHAR(1000),
    Price DECIMAL(10, 2) NOT NULL,
    Stock INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);
GO

-- Клієнти
CREATE TABLE Clients (
    ID INT IDENTITY(1,1) PRIMARY KEY ,
    FirstName NVARCHAR(255) NOT NULL,
    LastName NVARCHAR(255) NOT NULL,
    Contacts NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL
);
GO

-- Замовлення
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    ClientID INT,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClientID) REFERENCES Clients(ID)
        ON DELETE SET NULL
        ON UPDATE CASCADE
);
GO

-- Деталі замовлень
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    ItemID INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(8,2) NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
        ON DELETE CASCADE,
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
        ON DELETE CASCADE
);
GO

-- Доставки
CREATE TABLE OrderDelivery (
    OrderDeliveryID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT NOT NULL,
    DeliveryDate DATE NOT NULL,
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID)
        ON DELETE CASCADE
);
GO

-- Постачальники
CREATE TABLE Suppliers (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    ContactName NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL,
    Address NVARCHAR(500) NOT NULL
);
GO

-- Поставки
CREATE TABLE Supplies (
    SupplyID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT NOT NULL,
    ItemID INT NOT NULL,
    SupplyPrice DECIMAL(10, 2) NOT NULL,
    LastDeliveryDate DATE,
    FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
        ON DELETE CASCADE,
    FOREIGN KEY (ItemID) REFERENCES Item(ItemID)
        ON DELETE CASCADE
);
GO

-- Працівники
CREATE TABLE Employees (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    HireDate DATE NOT NULL,
    Phone NVARCHAR(20) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL
);
GO

-- Склади
CREATE TABLE Warehouses (
    WarehouseID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Location NVARCHAR(500) NOT NULL,
    Phone NVARCHAR(50) NOT NULL
);
GO

DROP TABLE IF EXISTS Warehouses;
DROP TABLE IF EXISTS Supplies;
DROP TABLE IF EXISTS Suppliers;
DROP TABLE IF EXISTS OrderDelivery;
DROP TABLE IF EXISTS OrderItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Clients;
DROP TABLE IF EXISTS Item;
DROP TABLE IF EXISTS Categories;
DROP TABLE IF EXISTS Employees;