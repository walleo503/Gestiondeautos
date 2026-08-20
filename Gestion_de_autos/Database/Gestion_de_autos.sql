USE Gestion_de_autos;

-- ============================
-- VENDEDOR
-- ============================

CREATE TABLE usuarios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(100) NOT NULL,
    apellido VARCHAR(100) NOT NULL,
    telefono VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NOT NULL,
    DUI VARCHAR(100) NOT NULL,
    contrasena VARCHAR(100) NOT NULL
);

CREATE TABLE login (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    contrasena VARCHAR(100) NOT NULL,
    fecha_login DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario) REFERENCES usuarios(id)
);

CREATE TABLE datos_auto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario INT NOT NULL,
    marca VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    costo_compra DECIMAL(10,2) NOT NULL,   -- lo que costó adquirir el auto (para calcular ganancia)
    precio_venta DECIMAL(10,2) NOT NULL,
    descripcion VARCHAR(255) NOT NULL,
    danos VARCHAR(255) NOT NULL,
    piezas_faltantes VARCHAR(255) NOT NULL,
    estado VARCHAR(50) NOT NULL DEFAULT 'disponible', -- disponible o vendido
    FOREIGN KEY (usuario) REFERENCES usuarios(id)
);

CREATE TABLE cotizacion_reparacion (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario INT NOT NULL,
    datos_auto INT NOT NULL,
    pieza VARCHAR(100) NOT NULL,
    modelo VARCHAR(100) NOT NULL,
    precio DECIMAL(10,2) NOT NULL,
    otro VARCHAR(100),
    mano_de_obra DECIMAL(10,2) NOT NULL,
    total DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (usuario) REFERENCES usuarios(id),
    FOREIGN KEY (datos_auto) REFERENCES datos_auto(id)
);

CREATE TABLE historial_vendidos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario INT NOT NULL,               -- vendedor que registró la venta
    datos_auto INT NOT NULL,
    comprador_nombre VARCHAR(100) NOT NULL,
    comprador_telefono VARCHAR(100),
    precio_final DECIMAL(10,2) NOT NULL,
    fecha_venta DATE NOT NULL,
    FOREIGN KEY (usuario) REFERENCES usuarios(id),
    FOREIGN KEY (datos_auto) REFERENCES datos_auto(id)
);

-- ============================
-- USUARIO (comprador)
-- ============================

CREATE TABLE lista_autos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    usuario INT NOT NULL,
    datos_auto INT NOT NULL,
    fecha_agregado DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario) REFERENCES usuarios(id),
    FOREIGN KEY (datos_auto) REFERENCES datos_auto(id)
);

-- ============================
-- TRIGGER: al registrar una venta, marcar el auto como vendido automáticamente
-- ============================

DELIMITER $$
CREATE TRIGGER after_venta_insert
AFTER INSERT ON historial_vendidos
FOR EACH ROW
BEGIN
    UPDATE datos_auto
    SET estado = 'vendido'
    WHERE id = NEW.datos_auto;
END$$
DELIMITER ;

-- ============================
-- ESTADISTICAS (vistas: se calculan solas, siempre actualizadas)
-- ============================

-- Ganancias por mes y por vendedor
CREATE VIEW vista_ganancias_mensuales AS
SELECT
    u.id            AS usuario_id,
    u.nombre        AS vendedor,
    YEAR(hv.fecha_venta)  AS anio,
    MONTH(hv.fecha_venta) AS mes,
    COUNT(hv.id)                          AS autos_vendidos,
    SUM(hv.precio_final)                  AS total_ventas,
    SUM(da.costo_compra)                  AS total_costo,
    SUM(hv.precio_final - da.costo_compra) AS ganancia_total
FROM historial_vendidos hv
JOIN datos_auto da ON da.id = hv.datos_auto
JOIN usuarios u     ON u.id = hv.usuario
GROUP BY u.id, YEAR(hv.fecha_venta), MONTH(hv.fecha_venta);

-- Vehiculos mas vendidos (por marca y modelo)
CREATE VIEW vista_vehiculos_mas_vendidos AS
SELECT
    da.marca,
    da.modelo,
    COUNT(hv.id)          AS veces_vendido,
    SUM(hv.precio_final)  AS total_generado
FROM historial_vendidos hv
JOIN datos_auto da ON da.id = hv.datos_auto
GROUP BY da.marca, da.modelo
ORDER BY veces_vendido DESC;

-- Resumen general de ganancias por vendedor (todo el tiempo)
CREATE VIEW vista_ganancias_por_vendedor AS
SELECT
    u.id     AS usuario_id,
    u.nombre AS vendedor,
    COUNT(hv.id)                           AS autos_vendidos,
    SUM(hv.precio_final - da.costo_compra) AS ganancia_total
FROM historial_vendidos hv
JOIN datos_auto da ON da.id = hv.datos_auto
JOIN usuarios u     ON u.id = hv.usuario
GROUP BY u.id;

-- ============================
-- FOTOS DE VEHICULOS (un vehiculo puede tener varias fotos)
-- ============================

CREATE TABLE fotos_auto (
    id INT AUTO_INCREMENT PRIMARY KEY,
    datos_auto INT NOT NULL,
    ruta VARCHAR(255) NOT NULL,          -- ruta relativa del archivo, ej: /uploads/vehiculos/5/foto1.jpg
    fecha_subida DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (datos_auto) REFERENCES datos_auto(id) ON DELETE CASCADE
);

-- ============================
-- TIPO DE VENTA: para diferenciar ganancias de autos vendidos
-- en su estado original vs. reparados
-- ============================

ALTER TABLE historial_vendidos
    ADD COLUMN tipo_venta ENUM('original','reparado') NOT NULL DEFAULT 'original' AFTER precio_final;

-- Ganancias agrupadas por tipo de venta (para el grafico del vendedor)
CREATE VIEW vista_ventas_por_tipo AS
SELECT
    hv.tipo_venta,
    COUNT(hv.id)                           AS cantidad,
    SUM(hv.precio_final - da.costo_compra) AS ganancia_total
FROM historial_vendidos hv
JOIN datos_auto da ON da.id = hv.datos_auto
GROUP BY hv.tipo_venta;

-- ============================
-- ROLES Y SUSPENSION DE EMPLEADOS
-- ============================

ALTER TABLE usuarios ADD COLUMN rol VARCHAR(20) NOT NULL DEFAULT 'vendedor' AFTER contrasena;   -- 'administrador' o 'vendedor'
ALTER TABLE usuarios ADD COLUMN activo TINYINT(1) NOT NULL DEFAULT 1 AFTER rol;                 -- 1 = activo, 0 = suspendido

-- Crea el primer administrador para poder entrar al sistema
-- (el registro publico ya no existe: solo un administrador puede crear cuentas nuevas)
-- IMPORTANTE: cambia el correo y la contrasena antes de usarlo en produccion
INSERT INTO usuarios (nombre, apellido, telefono, correo, DUI, contrasena, rol, activo)
VALUES ('Admin', 'Principal', '00000000', 'admin@gestionautos.com', '00000000-0', 'admin123', 'administrador', 1);
