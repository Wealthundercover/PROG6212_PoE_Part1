-- =========================================================================
-- RaceDay Database Creation Script
-- Module: PROG6212 (Programming 2B) PoE Part 1
-- Description: Creates full schema with keys, constraints, and seed data.
-- =========================================================================

-- 1. DATABASE CREATION
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'RaceDayDB')
BEGIN
    DROP DATABASE RaceDayDB;
END
GO

CREATE DATABASE RaceDayDB;
GO

USE RaceDayDB;
GO

-- 2. CREATE TABLES

-- Table 1: Roles
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

-- Table 2: Users
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    RoleId INT NOT NULL,
    FirstName VARCHAR(100) NOT NULL,
    LastName VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Users_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
);

-- Table 3: Events
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    OrganiserId INT NOT NULL,
    Title VARCHAR(150) NOT NULL,
    Description VARCHAR(MAX) NOT NULL,
    EventDate DATETIME NOT NULL,
    Location VARCHAR(200) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Events_Users FOREIGN KEY (OrganiserId) REFERENCES Users(UserId)
);

-- Table 4: Categories
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    CategoryName VARCHAR(100) NOT NULL,
    DistanceKM DECIMAL(5,2) NOT NULL CHECK (DistanceKM > 0),
    EntryFee DECIMAL(10,2) NOT NULL DEFAULT 0.00 CHECK (EntryFee >= 0),
    MaxCapacity INT NOT NULL CHECK (MaxCapacity > 0),
    CONSTRAINT FK_Categories_Events FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE
);

-- Table 5: Enrolments
CREATE TABLE Enrolments (
    EnrolmentId INT IDENTITY(1,1) PRIMARY KEY,
    ParticipantId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME DEFAULT GETDATE(),
    Status VARCHAR(50) NOT NULL DEFAULT 'Confirmed',
    CONSTRAINT FK_Enrolments_Users FOREIGN KEY (ParticipantId) REFERENCES Users(UserId),
    CONSTRAINT FK_Enrolments_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId),
    CONSTRAINT UQ_Participant_Category UNIQUE (ParticipantId, CategoryId)
);

-- Table 6: Results
CREATE TABLE Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    EnrolmentId INT NOT NULL UNIQUE,
    FinishTime TIME NOT NULL,
    OverallPosition INT NOT NULL CHECK (OverallPosition > 0),
    CategoryPosition INT NOT NULL CHECK (CategoryPosition > 0),
    CONSTRAINT FK_Results_Enrolments FOREIGN KEY (EnrolmentId) REFERENCES Enrolments(EnrolmentId) ON DELETE CASCADE
);
GO

-- 3. SEED DATA

-- Seed Roles
INSERT INTO Roles (RoleName) VALUES ('Organiser'), ('Participant');

-- Seed Users (2 Organisers, 2 Participants required)
INSERT INTO Users (RoleId, FirstName, LastName, Email, PasswordHash) VALUES 
(1, 'Sipho', 'Dlamini', 'sipho.dlamini@raceday.co.za', 'AQAAAAEAACcQAAAAEH8x2...'),
(1, 'Anika', 'Van Zyl', 'anika.vanzyl@raceday.co.za', 'AQAAAAEAACcQAAAAEH8x2...'),
(2, 'Thabo', 'Mokoena', 'thabo.mokoena@gmail.com', 'AQAAAAEAACcQAAAAEH8x2...'),
(2, 'Sarah', 'Jenkins', 'sarah.j@gmail.com', 'AQAAAAEAACcQAAAAEH8x2...');

-- Seed Events (3 Events required)
INSERT INTO Events (OrganiserId, Title, Description, EventDate, Location) VALUES 
(1, 'Comrades Marathon', 'The ultimate human race from Pietermaritzburg to Durban.', '2026-06-14 05:30:00', 'Pietermaritzburg'),
(1, 'Cape Town Cycle Tour', 'The iconic cycle race around the Cape Peninsula.', '2026-03-08 06:00:00', 'Cape Town'),
(2, 'Soweto 10km & Half Marathon', 'Community road running event through historic Soweto.', '2026-11-01 06:30:00', 'Soweto');

-- Seed Categories
INSERT INTO Categories (EventId, CategoryName, DistanceKM, EntryFee, MaxCapacity) VALUES 
(1, 'Ultra Marathon', 89.00, 1200.00, 20000),
(2, 'Full Peninsula Loop', 109.00, 850.00, 35000),
(2, 'Short Family Ride', 42.00, 400.00, 5000),
(3, 'Half Marathon', 21.10, 250.00, 8000),
(3, '10km Road Run', 10.00, 150.00, 10000);

-- Seed Enrolments
INSERT INTO Enrolments (ParticipantId, CategoryId, Status) VALUES 
(3, 1, 'Confirmed'), -- Thabo in Comrades
(3, 4, 'Confirmed'), -- Thabo in Soweto 21km
(4, 2, 'Confirmed'), -- Sarah in Cape Town Cycle Tour
(4, 5, 'Confirmed'); -- Sarah in Soweto 10km

-- Seed Results
INSERT INTO Results (EnrolmentId, FinishTime, OverallPosition, CategoryPosition) VALUES 
(1, '07:15:42', 1240, 310),
(3, '03:45:12', 450, 120);
GO