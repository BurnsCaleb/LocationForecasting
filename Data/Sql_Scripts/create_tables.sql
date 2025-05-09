USE LocationForecasting_DB;
GO

CREATE TABLE RequestLogs (
	RequestID INT IDENTITY(1,1) PRIMARY KEY,
	RequestPayload NVARCHAR(MAX) NOT NULL,
	RequestTime DateTime DEFAULT GetDate()
);
GO

CREATE TABLE ExceptionLogs (
	ExceptionID INT IDENTITY(1,1) PRIMARY KEY,
	ExceptionMessage NVARCHAR(100) NOT NULL,
	StackTrace NVARCHAR(MAX) NOT NULL,
	ExceptionTime DateTime DEFAULT GetDate()
);
GO

CREATE TABLE Settings (
	ApiKey NVARCHAR(100) NOT NULL
);
GO

INSERT INTO Settings (ApiKey)
VALUES ('AIzaSyBe6Tkx5dfp-oOsZsYHbdS-ITMSIzhEnxw');
GO