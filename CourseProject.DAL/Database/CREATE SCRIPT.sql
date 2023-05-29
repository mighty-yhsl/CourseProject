CREATE TABLE Customer(
Id int not null primary key identity(1,1),
CustomerName NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
CustomerSurname NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
Phone NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
Email NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
Addres NVARCHAR(80) Collate Cyrillic_General_CI_AS not null
);

CREATE TABLE Seller(
Id int not null primary key identity(1,1),
SellerName  NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
SellerSurname NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
Phone NVARCHAR(40) Collate Cyrillic_General_CI_AS not null,
Email NVARCHAR(40) Collate Cyrillic_General_CI_AS not null
);

CREATE TABLE Category(
Id int not null primary key identity(1,1),
CategoryName NVARCHAR(40) Collate Cyrillic_General_CI_AS not null
);

CREATE TABLE Manufacturer(
Id int not null primary key identity(1,1),
ManufacturerName NVARCHAR(40) Collate Cyrillic_General_CI_AS not null
);

CREATE TABLE StatusOrder(
Id int not null primary key identity(1,1),
StatusOrderName NVARCHAR(40) Collate Cyrillic_General_CI_AS not null
);

CREATE TABLE CustomerOrder(
Id int not null primary key identity(1,1),
[Description] NVARCHAR(256) Collate Cyrillic_General_CI_AS null,
CreateDate DATETIME NOT NULL DEFAULT(GETDATE()),
UpdateDate DATETIME NOT NULL,
SellerId int not null,
CustomerId int not null,
StatusId int not null,
Foreign Key(SellerId) REFERENCES Seller(Id),
Foreign Key(CustomerId) REFERENCES Customer(Id),
Foreign Key(StatusId) REFERENCES StatusOrder(Id)
);

CREATE TABLE OrderDetails(
Id int not null primary key identity(1,1),
CustomerOrderId int not null,
TotalAmount int not null DEFAULT(1),
TotalPrice DECIMAL NOT NULL DEFAULT(0),
Foreign Key(CustomerOrderId) REFERENCES CustomerOrder(Id) ON DELETE CASCADE
);

CREATE TABLE Transport(
Id int not null primary key identity(1,1),
[Name] NVARCHAR(64) Collate Cyrillic_General_CI_AS not null,
Speed int not null,
Weightt int not null,
EnginePower int not null,
Amount int not null DEFAULT(1),
Price DECIMAL NOT NULL DEFAULT(0),
CategoryId int not null,
ManufacturerId int not null,
Foreign Key(CategoryId) REFERENCES Category(Id),
Foreign Key(ManufacturerId) REFERENCES Manufacturer(Id)
);

