ALTER TABLE receivedvoucherhistory ADD receivedoffering text NULL;

ALTER TABLE receivedvoucherhistory ADD secondreceivedoffering text NULL;

ALTER TABLE receivedvoucher ADD receivedoffering text NULL;

ALTER TABLE receivedvoucher ADD secondreceivedoffering text NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20241023114838_V1.0.2', '7.0.16');

INSERT INTO schemaversions("Version", "Updatedon", "Systemname")
VALUES('1.0.2', '2024-10-23 15:00', 'NacidSc');

