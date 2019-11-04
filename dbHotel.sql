-- mysql -u root -p

CREATE DATABASE IF NOT EXISTS `dbHotel`
    DEFAULT CHARACTER SET latin1
    DEFAULT COLLATE latin1_swedish_ci;
    USE `dbHotel`;

DROP TABLE IF EXISTS `habitacion`;
CREATE TABLE IF NOT EXISTS `habitacion` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `nombre` VARCHAR(50) COLLATE latin1_swedish_ci NOT NULL,
    `descripcion` VARCHAR(50) DEFAULT 'Disponible' COLLATE latin1_swedish_ci NOT NULL,
    `costo` NUMERIC(10,2) NOT NULL,
    `estado` TINYINT DEFAULT 1,
 PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci AUTO_INCREMENT=1 ;

DROP TABLE IF EXISTS `reservacion`;
CREATE TABLE IF NOT EXISTS `reservacion` (
    `id` INT UNSIGNED NOT NULL AUTO_INCREMENT,
    `fkIdHabitacion` INT UNSIGNED NOT NULL,
    `fechaEntrada` DATETIME DEFAULT NOW() NOT NULL,
    `fechaSalida` DATETIME,
    `importe` NUMERIC(10,2),
 PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci AUTO_INCREMENT=1 ;

ALTER TABLE `reservacion` ADD  CONSTRAINT fk_Habitacion_Reservaciones FOREIGN KEY (`fkIdHabitacion`) REFERENCES habitacion(`id`);
