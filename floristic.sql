CREATE SCHEMA floristic;

DROP TABLE IF EXISTS floristic.florist;

CREATE TABLE floristic.florist (
  id INT NOT NULL GENERATED BY DEFAULT AS IDENTITY,
  full_name VARCHAR(100) DEFAULT NULL,
  short_name VARCHAR(100) DEFAULT NULL,
  CONSTRAINT pk_florist PRIMARY KEY (id)
);

INSERT INTO floristic.florist (id, full_name, short_name) VALUES
(1,'Aelina', 'Kondratieva Aelina Maksimovna'),
(2,'Kate', 'Ekaterina Sergeevna Novikova'),
(3,'Lena', 'Varlamova Elena Vladimirovna'),
(4,'Galina', 'Gurzhieva Galina Timurovna');

--===========================================================================================================================

DROP TABLE IF EXISTS floristic.bouquet;

CREATE TABLE floristic.bouquet (
  id INT NOT NULL GENERATED BY DEFAULT AS IDENTITY,
  name VARCHAR(100) DEFAULT NULL,
  description VARCHAR(100) DEFAULT NULL,
  CONSTRAINT pk_bouquet PRIMARY KEY (id)
);

INSERT INTO floristic.bouquet (id, name, description) VALUES
(1,'Rose1', '101 white rose'),
(2,'Rose3', '101 red rose'),
(3,'Rose4', '21 white rose'),
(4,'Rose5', '21 red rose'),
(5,'Tulip1', '5 pink tulips'),
(6,'Tulip2', '7 pink tulips'),
(7,'Tulip3', '13 pink tulips'),
(8,'Tulip4', '5 yellow tulips'),
(9,'Tulip5', '7 yellow tulips'),
(10,'Tulip6', '13 yellow tulips');
--===========================================================================================================================

DROP TABLE IF EXISTS floristic.order;

CREATE TABLE floristic.order (
  id INT NOT NULL GENERATED BY DEFAULT AS IDENTITY,
  bouquet_id INT DEFAULT NULL,
  date DATE NOT NULL, --YYYY-MM-DD
  time TIME NOT NULL, --hh:mm:ss
  address VARCHAR(100) DEFAULT NULL,
  florist_id INT DEFAULT NULL,
  CONSTRAINT pk_order PRIMARY KEY (id),
  CONSTRAINT fk_fl_bouq FOREIGN KEY (bouquet_id) REFERENCES floristic.bouquet (id),
  CONSTRAINT fk_fl_flor FOREIGN KEY (florist_id) REFERENCES floristic.florist (id)
);

INSERT INTO floristic.order (id, bouquet_id, date, time, address, florist_id) VALUES
(1, 1, '2023-06-05', '18:30:00', 'Komsomolskaya 41, apt. 27', 2),
(2, 5, '2023-06-07', '17:00:00', 'Milchakova 8a, apt. 37', 4),
(3, 1, '2023-06-07', '12:30:00', 'Bolshaya Sadova 33, apt. 12', 1),
(4, 3, '2023-06-05', '14:15:00', 'Sovetskaya 41, apt. 33', 3),
(5, 10, '2023-06-05', '12:00:00', 'Stachki 22, apt. 44', 1),
(6, 7, '2023-06-06', '12:00:00', 'Krasnoarmesskaya 77, apt. 127', 3),
(7, 2, '2023-06-06', '19:30:00', 'Pushkinskaya 34, apt. 145', 1);

COMMIT;