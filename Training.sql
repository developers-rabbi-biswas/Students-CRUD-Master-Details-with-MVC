CREATE DATABASE Training
USE Training
GO
CREATE TABLE Traineer
(
TraineeId INT PRIMARY KEY IDENTITY,
TraineeName VARCHAR(50),
Age INT,
DOB DATE,
MorningShift VARCHAR(50),
Picture VARCHAR(50)
)

CREATE TABLE Course
(
CourseId INT PRIMARY KEY IDENTITY,
CourseName VARCHAR(50)
)

CREATE TABLE Enrollment
(
EnrollmentId INT PRIMARY KEY IDENTITY,
TraineeId INT REFERENCES Traineer,
CourseId INT REFERENCES Course
)

INSERT INTO Course
VALUES
('ASP.NET'),('LARAVEL'),('WORDPRESS')
