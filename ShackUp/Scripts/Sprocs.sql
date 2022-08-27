USE ShackUp
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'StatesSelectAll')
	DROP PROCEDURE StatesSelectAll
GO

CREATE PROCEDURE StatesSelectAll AS
BEGIN
	SELECT StateId, StateName
	FROM States
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'BathroomTypesSelectAll')
	DROP PROCEDURE BathroomTypesSelectAll
GO

CREATE PROCEDURE BathroomTypesSelectAll AS
BEGIN
	SELECT BathroomTypeId, BathroomTypeName
	FROM BathroomTypes
	ORDER BY BathroomTypeName
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsInsert')
	DROP PROCEDURE ListingsInsert
GO

CREATE PROCEDURE ListingsInsert (
	@ListingId int output,
	@UserId nvarchar(128),
	@StateId char(2),
	@BathroomTypeId int,
	@Nickname nvarchar(50),
	@City nvarchar(50),
	@Rate decimal(7,2),
	@SquareFootage decimal (7,2),
	@HasElectric bit,
	@HasHeat bit,
	@ListingDescription varchar(500),
	@ImageFileName varchar(50)
) AS
BEGIN
	INSERT INTO Listings (UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric, HasHeat, ListingDescription, ImageFileName)
	VALUES (@UserId, @StateId, @BathroomTypeId, @Nickname, @City, @Rate, @SquareFootage, @HasElectric, @HasHeat, @ListingDescription, @ImageFileName)

	SET @ListingId = SCOPE_IDENTITY();
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsUpdate')
	DROP PROCEDURE ListingsUpdate
GO

CREATE PROCEDURE ListingsUpdate (
	@ListingId int,
	@UserId nvarchar(128),
	@StateId char(2),
	@BathroomTypeId int,
	@Nickname nvarchar(50),
	@City nvarchar(50),
	@Rate decimal(7,2),
	@SquareFootage decimal (7,2),
	@HasElectric bit,
	@HasHeat bit,
	@ListingDescription varchar(500),
	@ImageFileName varchar(50)
) AS
BEGIN
	UPDATE Listings SET
		UserId = @UserId,
		StateId = @StateId,
		BathroomTypeId = @BathroomTypeId,
		Nickname = @Nickname,
		City = @City,
		Rate = @Rate,
		SquareFootage = @SquareFootage,
		HasElectric = @HasElectric,
		HasHeat = @HasHeat,
		ListingDescription = @ListingDescription,
		ImageFileName = @ImageFileName
	WHERE ListingId = @ListingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsDelete')
	DROP PROCEDURE ListingsDelete
GO

CREATE PROCEDURE ListingsDelete (
	@ListingId int
) AS
BEGIN
	BEGIN TRANSACTION

	DELETE FROM Contacts WHERE ListingId = @ListingId;
	DELETE FROM Favorites WHERE ListingId = @ListingId;
	DELETE FROM Listings WHERE ListingId = @ListingId;

	COMMIT TRANSACTION
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelect')
	DROP PROCEDURE ListingsSelect
GO

CREATE PROCEDURE ListingsSelect (
	@ListingId int
) AS
BEGIN
	SELECT ListingId, UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric, HasHeat, ListingDescription, ImageFileName
	FROM Listings
	WHERE ListingId = @ListingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelectRecent')
	DROP PROCEDURE ListingsSelectRecent
GO

CREATE PROCEDURE ListingsSelectRecent AS
BEGIN
	SELECT TOP 5 ListingId, UserId, Rate, City, StateId, ImageFileName
	FROM Listings
	ORDER BY CreatedDate DESC
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelectDetails')
	DROP PROCEDURE ListingsSelectDetails
GO

CREATE PROCEDURE ListingsSelectDetails (
	@ListingId int
)
AS 
BEGIN
	SELECT ListingId, UserId, Nickname, City, StateId, Rate, SquareFootage, 
		HasElectric, HasHeat, l.BathroomTypeId, BathroomTypeName, ImageFileName, l.ListingDescription
	FROM Listings l
		INNER JOIN BathroomTypes b ON l.BathroomTypeId = b.BathroomTypeId
	WHERE ListingId = @ListingId
END
GO


IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelectFavorites')
	DROP PROCEDURE ListingsSelectFavorites
GO

CREATE PROCEDURE ListingsSelectFavorites (
	@UserId nvarchar(128)
)
AS 
BEGIN
	SELECT l.ListingId, l.City, l.StateId, l.Rate, l.SquareFootage, l.UserId,
		l.HasElectric, l.HasHeat, l.BathroomTypeId, b.BathroomTypeName
	FROM Favorites f
		INNER JOIN Listings l ON f.ListingId = l.ListingId
		INNER JOIN BathroomTypes b on l.BathroomTypeId = b.BathroomTypeId
	WHERE f.UserId = @UserId;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelectContacts')
	DROP PROCEDURE ListingsSelectContacts
GO

CREATE PROCEDURE ListingsSelectContacts (
	@UserId nvarchar(128)
)
AS 
BEGIN
	SELECT l.ListingId, u.Email, u.id as UserId, l.Nickname, l.City, l.StateId, l.Rate
	FROM Listings l
		INNER JOIN Contacts c ON l.ListingId = c.ListingId
		INNER JOIN AspNetUsers u on c.UserId = u.Id
	WHERE l.UserId = @UserId;
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ListingsSelectByUser')
	DROP PROCEDURE ListingsSelectByUser
GO

CREATE PROCEDURE ListingsSelectByUser (
	@UserId nvarchar(128)
)
AS 
BEGIN
	SELECT ListingId, UserId, Nickname, City, StateId, Rate, SquareFootage, 
		HasElectric, HasHeat, l.BathroomTypeId, BathroomTypeName, ImageFileName
	FROM Listings l
		INNER JOIN BathroomTypes b ON l.BathroomTypeId = b.BathroomTypeId
	WHERE UserId = @UserId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'FavoritesInsert')
	DROP PROCEDURE FavoritesInsert
GO

CREATE PROCEDURE FavoritesInsert (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	INSERT INTO Favorites(UserId, ListingId)
	Values (@UserId, @ListingId)
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'FavoritesDelete')
	DROP PROCEDURE FavoritesDelete
GO

CREATE PROCEDURE FavoritesDelete (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	DELETE FROM Favorites
	WHERE UserId = @UserId AND ListingId = @ListingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ContactsInsert')
	DROP PROCEDURE ContactsInsert
GO

CREATE PROCEDURE ContactsInsert (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	INSERT INTO Contacts(UserId, ListingId)
	Values (@UserId, @ListingId)
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ContactsDelete')
	DROP PROCEDURE ContactsDelete
GO

CREATE PROCEDURE ContactsDelete (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	DELETE FROM Contacts
	WHERE UserId = @UserId AND ListingId = @ListingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'ContactsSelect')
	DROP PROCEDURE ContactsSelect
GO

CREATE PROCEDURE ContactsSelect (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	SELECT UserId, ListingId
	FROM Contacts
	WHERE UserId = @UserId AND ListingId = @ListingId
END
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'FavoritesSelect')
	DROP PROCEDURE FavoritesSelect
GO

CREATE PROCEDURE FavoritesSelect (
	@UserId nvarchar(128),
	@ListingId int
)
AS 
BEGIN
	SELECT UserId, ListingId
	FROM Favorites
	WHERE UserId = @UserId AND ListingId = @ListingId
END
GO