USE ShackUp
GO

IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.ROUTINES
	WHERE ROUTINE_NAME = 'DbReset')
DROP PROCEDURE DbReset
GO

CREATE PROCEDURE DbReset AS
BEGIN
	DELETE FROM Favorites;
	DELETE FROM Contacts;
	DELETE FROM Listings;
	DELETE FROM States;
	DELETE FROM BathroomTypes;
	DELETE FROM AspNetUsers WHERE id IN ('00000000-0000-0000-0000-000000000000', '11111111-1111-1111-1111-111111111111');

	DBCC CHECKIDENT ('Listings', RESEED, 1)

	INSERT INTO States (StateId, StateName)
	VALUES ('OH', 'Ohio'),
	('KY', 'Kentucky'),
	('MN', 'Minnesota');

	SET IDENTITY_INSERT BathroomTypes ON;

	INSERT INTO BathroomTypes (BathroomTypeId, BathroomTypeName)
	VALUES (1, 'Indoor'),
	(2, 'Outdoor'),
	(3, 'None');

	SET IDENTITY_INSERT BathroomTypes OFF;

	INSERT INTO AspNetUsers(Id, EmailConfirmed, PhoneNumberConfirmed, Email, StateId, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName)
	VALUES('00000000-0000-0000-0000-000000000000', 0, 0, 'test@test.com', 'OH', 0, 0, 0, 'test');

	INSERT INTO AspNetUsers(Id, EmailConfirmed, PhoneNumberConfirmed, Email, StateId, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, UserName)
	VALUES('11111111-1111-1111-1111-111111111111', 0, 0, 'test2@test.com', 'OH', 0, 0, 0, 'test2');

	SET IDENTITY_INSERT Listings ON;

	INSERT INTO Listings (ListingId, UserId, StateId, BathroomTypeId, Nickname, City, Rate, SquareFootage, HasElectric, HasHeat, ImageFileName, ListingDescription)
	VALUES (1, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 1', 'Cleveland', 100, 400, 0, 1, 'placeholder.png', 'Description'),
		(2, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 2', 'Cleveland', 110, 410, 0, 1, 'placeholder.png', null),
		(3, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 3', 'Cleveland', 120, 420, 0, 1, 'placeholder.png', null),
		(4, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 4', 'Cleveland', 130, 430, 0, 1, 'placeholder.png', 'Experience the chill of Cleveland winters in this leaky shack. Poorly insulated and weather worn, you will truly appreciate returning home from your stay here.'),
		(5, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 5', 'Columbus', 140, 440, 0, 1, 'placeholder.png', null),
		(6, '00000000-0000-0000-0000-000000000000', 'OH', 3, 'Test Shack 6', 'Cleveland', 150, 450, 0, 1, 'placeholder.png', null)

	SET IDENTITY_INSERT Listings OFF;

	INSERT INTO Favorites (ListingId, UserId)
	VALUES (1, '11111111-1111-1111-1111-111111111111'),
		(2, '11111111-1111-1111-1111-111111111111')

	INSERT INTO Contacts (ListingId, UserId)
	VALUES (1, '11111111-1111-1111-1111-111111111111'),
		(3, '11111111-1111-1111-1111-111111111111')
END