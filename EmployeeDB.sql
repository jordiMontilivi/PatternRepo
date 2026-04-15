-- Creació de la base de dades
CREATE DATABASE IF NOT EXISTS EmployeeDB;
USE EmployeeDB;

-- Creació de la taula
CREATE TABLE IF NOT EXISTS Employees (
    EmployeeId INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL UNIQUE,
    Salary DECIMAL(10, 2) NOT NULL
);

-- Inserció de 10 empleats de prova
INSERT INTO Employees (FirstName, LastName, Email, Salary) VALUES
('Jordi', 'Fons', 'jordi@example.com', 55000.00),
('Ferran', 'Chic', 'Ferran@example.com', 42000.50),
('Marc', 'López', 'marc.l@example.com', 38000.00),
('Laura', 'Sánchez', 'laura.s@example.com', 45000.00),
('Albert', 'Fernández', 'albert.f@example.com', 31000.00),
('Cristina', 'Ruiz', 'cristina.r@example.com', 39500.00),
('David', 'Gómez', 'david.g@example.com', 41000.00),
('Elena', 'Pérez', 'elena.p@example.com', 36000.00),
('Sergi', 'Vila', 'sergi.v@example.com', 48000.00),
('Anna', 'Serra', 'anna.s@example.com', 34500.00);