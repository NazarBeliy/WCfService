# WCFSERVICETOMYAPP

CREATE TABLE users (
    id SERIAL PRIMARY KEY,
    fullname VARCHAR(255) NOT NULL,
    taxid VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    phonenumber VARCHAR(255) NOT NULL,
    createddate TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    lastmodifieddate TIMESTAMP WITHOUT TIME ZONE NOT NULL
);
Для створення таблиці в дб
