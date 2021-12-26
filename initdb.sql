\connect GarageManagementDB

CREATE TABLE "Garage"
(
 "Id" serial PRIMARY KEY,
 "Name" VARCHAR (50) NOT NULL,
 "Address" VARCHAR (50) NOT NULL
);

CREATE TABLE "Cars"
(
 "Id" serial PRIMARY KEY,
 "Name" VARCHAR (50) NOT NULL,
 "Color" VARCHAR (50) NOT NULL,
 "Brand" VARCHAR (50) NOT NULL,
 "GarageId" INT NOT NULL,
 FOREIGN KEY ("GarageId")
      REFERENCES "Garage" ("Id")
);
SELECT table_name FROM information_schema.tables WHERE table_schema='public';

