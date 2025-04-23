LOAD DATA LOCAL INFILE '/caminho/para/clientes.txt'
INTO TABLE clientes
FIELDS TERMINATED BY ';'
LINES TERMINATED BY '\n'
(id, cpf, @data_nascimento, telefone, nome)
SET data_nascimento = STR_TO_DATE(@data_nascimento, '%d%m%Y');

LOAD DATA INFILE 'C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/1428624292050_clientes.txt'
INTO TABLE clientes
FIELDS TERMINATED BY ';'
LINES TERMINATED BY '\n'
(id, cpf, @data_nascimento, telefone, nome)
SET data_nascimento = IF(@data_nascimento = '' OR @data_nascimento = '0', NULL, STR_TO_DATE(@data_nascimento, '%d%m%Y'));


SET GLOBAL local_infile = 1;


SHOW COLUMNS FROM clientes;

DESCRIBE clientes;

SELECT * FROM clientes;

CREATE TABLE pagamentos (
    id INT,
    data DATE,
    tipo INT,
    valor INT,
    flag CHAR(1)
);


LOAD DATA INFILE 'C:/ProgramData/MySQL/MySQL Server 8.0/Uploads/1428624292736_pagamentos.txt'
INTO TABLE pagamentos
FIELDS TERMINATED BY ';'
LINES TERMINATED BY '\n'
(id, @data, tipo, valor, flag);
SET @data = IF(@data = '' OR @data = '0', NULL, STR_TO_DATE(@data, '%d%m%Y'));

SELECT * FROM pagamentos;

