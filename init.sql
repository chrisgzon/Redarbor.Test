CREATE DATABASE redarbor;
GO

USE redarbor;
GO

IF OBJECT_ID('dbo.employees', 'U') IS NOT NULL DROP TABLE dbo.employees;
IF OBJECT_ID('dbo.statuses', 'U') IS NOT NULL DROP TABLE dbo.statuses;
IF OBJECT_ID('dbo.portals', 'U') IS NOT NULL DROP TABLE dbo.portals;
IF OBJECT_ID('dbo.roles', 'U') IS NOT NULL DROP TABLE dbo.roles;
IF OBJECT_ID('dbo.companies', 'U') IS NOT NULL DROP TABLE dbo.companies;
GO

CREATE TABLE companies (
    id INT IDENTITY(1,1),
    name VARCHAR(250) NOT NULL,

    CONSTRAINT PK_Companies PRIMARY KEY CLUSTERED (id),
);
GO

CREATE TABLE roles (
    id INT IDENTITY(1,1),
    name VARCHAR(20) NOT NULL,

    CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (id),
);
GO

CREATE TABLE portals (
    id INT IDENTITY(1,1),
    name VARCHAR(250) NOT NULL,

    CONSTRAINT PK_Portals PRIMARY KEY CLUSTERED (id),
);
GO

CREATE TABLE statuses (
    id INT IDENTITY(1,1),
    name VARCHAR(20) NOT NULL,

    CONSTRAINT PK_Statuses PRIMARY KEY CLUSTERED (id),
    CONSTRAINT UQ_Statuses_Name UNIQUE (name),
);
GO

CREATE TABLE employees (
    Id INT IDENTITY(1,1),
    CompanyId INT NOT NULL,
    CreatedOn DATETIME2 NOT NULL DEFAULT GETDATE(),
    DeletedOn DATETIME2 NULL,
    Email NVARCHAR(100) NOT NULL,
    Fax NVARCHAR(20) NULL,
    [Name] NVARCHAR(100) NULL,
    Lastlogin DATETIME2 NULL,
    [Password] NVARCHAR(255) NOT NULL,
    PortalId INT NOT NULL,
    RoleId INT NOT NULL,
    StatusId INT NOT NULL,
    Telephone NVARCHAR(20) NULL,
    UpdatedOn DATETIME2 NOT NULL DEFAULT GETDATE(),
    Username NVARCHAR(50) NOT NULL,

    CONSTRAINT PK_Employees PRIMARY KEY CLUSTERED (Id),
    
    CONSTRAINT FK_Employees_Companies FOREIGN KEY (CompanyId) 
        REFERENCES dbo.companies(id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE,

    CONSTRAINT FK_Employees_Portals FOREIGN KEY (PortalId) 
        REFERENCES dbo.portals(id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE,
    
    CONSTRAINT FK_Employees_Roles FOREIGN KEY (RoleId) 
        REFERENCES dbo.roles(id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE,
    
    CONSTRAINT FK_Employees_Statuses FOREIGN KEY (StatusId) 
        REFERENCES dbo.statuses(id)
        ON DELETE NO ACTION
        ON UPDATE CASCADE,

    CONSTRAINT UQ_Employees_Email UNIQUE (Email),
    CONSTRAINT UQ_Employees_Username UNIQUE (Username)
);

-- seed data
INSERT INTO companies VALUES ('Company Example');
INSERT INTO roles VALUES ('Role Admin Example');
INSERT INTO roles VALUES ('Role Guest Example');
INSERT INTO portals VALUES ('Portal Example Example');
INSERT INTO statuses VALUES ('Active');

INSERT INTO dbo.employees -- password: admin123
    (CompanyId, Email, [Password], PortalId, RoleId, StatusId, Username, [Name], CreatedOn, UpdatedOn)
VALUES 
    (1, 'admin@redarbor.com', '$2a$12$TrHJcOU00vsoOGiBTh91oub73XF4iABqBAcAfvDZdRDUblQggvIeO', 1, 1, 1, 'admin', 'System Administrator', GETDATE(), GETDATE())
	
INSERT INTO dbo.employees -- password: guest123
    (CompanyId, Email, [Password], PortalId, RoleId, StatusId, Username, [Name], CreatedOn, UpdatedOn)
VALUES 
    (1, 'guest@test.com', '$2a$12$RcuHbBbbQZyYKDs9kh14ResFrB9k.HesaG5mQTtMN.g5UkDPj.VgS', 1, 2, 1, 'guest', 'Guest test user', GETDATE(), GETDATE())