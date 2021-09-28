
CREATE TABLE "Band" (
  "Id" SERIAL PRIMARY KEY,
  "Name" TEXT,
  "CountryOfOrigin" TEXT,
  "NumberOfMembers" INT,
  "Website" TEXT,
  "Style" TEXT,
  "IsSigned" BOOL,
  "ContactName" TEXT,
  "ContactPhoneNumber" TEXT
  );
  
 
  
  CREATE TABLE "Album" (
  "Id" SERIAL PRIMARY KEY,
  "Title" TEXT,
  "IsExplicit" BOOL,
  "ReleaseDate" DATE,
  "BandId" INT NULL REFERENCES "Band" ("Id")
  );

  CREATE TABLE "Song" (
  "Id" SERIAL PRIMARY KEY,
  "TrackNumber" INT,
  "Title" TEXT,
  "Duration" TEXT,
  "AlbumId" INT NULL REFERENCES "Album" ("Id")
  );
  
  _________________
  
  --1
  INSERT INTO "Band" (
  "Name", "CountryOfOrigin", "NumberOfMembers", "Website",
  "Style", "IsSigned", "ContactName", "ContactPhoneNumber")
  		VALUES('Milky Chance', 'Germany', 4, 'http://milkychance.net/', 'Alternative Rock', 
               true, 'Rehbein', '289999300');
               
 --2
 SELECT * FROM "Band";
 
 --3
 
INSERT INTO "Album" ("Title","IsExplicit","ReleaseDate","BandId")
  	VALUES ('Sadnecessary', false, '2013-10-01', 1);
 
 INSERT INTO "Album" ("Title","IsExplicit","ReleaseDate","BandId")
  	VALUES ('Blossom', false, '2017-03-17', 1);
 
 -- 4
 
INSERT INTO "Album" ("Title","IsExplicit","ReleaseDate","BandId")
  	VALUES ('Sadnecessary', false, '2013-10-01', 1);
 
 INSERT INTO "Album" ("Title","IsExplicit","ReleaseDate","BandId")
  	VALUES ('Blossom', false, '2017-03-17', 1);
-- 5
UPDATE "Band" SET "IsSigned" = true WHERE "Name" = 'Milky Chance';
 
-- 6
SELECT ("Title") FROM "Album"
  	JOIN "Band" ON "Band"."Id" = "Album"."BandId";
               
-- 7
SELECT * FROM "Song" 
	JOIN "Album" ON "Album"."Id" = "Song"."AlbumId"
    ORDER BY "ReleaseDate" DESC;
-- 8
SELECT * FROM "Band" WHERE "IsSigned" = True;

-- 9
SELECt * FROM "Band" WHERE "IsSigned" != true;