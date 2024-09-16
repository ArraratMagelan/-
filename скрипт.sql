CREATE USER maks_user WITH PASSWORD 'qwer1234QWER';
GRANT ALL PRIVILEGES ON DATABASE "postgres" TO maks_user;
ALTER USER maks_user WITH SUPERUSER;

create table soxr_vremia
(
	id_v serial primary key,
	nashalo_prostoia TIMESTAMP,
	koniec_prostoia TIMESTAMP
);

