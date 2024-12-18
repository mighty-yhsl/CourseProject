CREATE TABLE Service (
    ServiceId INT PRIMARY KEY IDENTITY(1,1),
    ServiceName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE()
);

-- Створення таблиці ServiceStatus
CREATE TABLE ServiceStatus (
    ServiceStatusId INT PRIMARY KEY IDENTITY(1,1),
    ServiceStatusName NVARCHAR(255) NOT NULL
);

-- Створення таблиці ServiceInstance
CREATE TABLE ServiceInstance (
    ServiceInstanceId INT PRIMARY KEY IDENTITY(1,1),
    ServiceId INT NOT NULL, -- Зовнішній ключ на Service
    ServiceStatusId INT NOT NULL,  -- Зовнішній ключ на ServiceStatus
    Address NVARCHAR(MAX),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME DEFAULT GETDATE(), -- виправлено написання
    CONSTRAINT FK_ServiceInstance_Service FOREIGN KEY (ServiceId)
        REFERENCES Service(ServiceId),
    CONSTRAINT FK_ServiceInstance_Status FOREIGN KEY (ServiceStatusId)
        REFERENCES ServiceStatus(ServiceStatusId)
);