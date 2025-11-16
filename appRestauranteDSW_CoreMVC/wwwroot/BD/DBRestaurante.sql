IF EXISTS (SELECT name FROM sys.databases WHERE name = N'RestauranteDSW')
BEGIN
    ALTER DATABASE RestauranteDSW SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE RestauranteDSW;
END;
GO

CREATE DATABASE RestauranteDSW;
GO


USE RestauranteDSW;
GO

-- Tabla: mesa
CREATE TABLE mesa (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cantidad_asientos INT,
    estado VARCHAR(255)
);
GO

-- Tabla: categoria_plato
CREATE TABLE categoria_plato (
    id VARCHAR(255) PRIMARY KEY,
    nombre VARCHAR(255)
);
GO

-- Tabla: plato
CREATE TABLE plato (
    id VARCHAR(255) PRIMARY KEY,
    imagen VARCHAR(255),
    nombre VARCHAR(255),
    precio_plato DECIMAL(10,2),
    categoria_plato_id VARCHAR(255),
    FOREIGN KEY (categoria_plato_id) REFERENCES categoria_plato(id)
);
GO

-- Tabla: estado_comanda
CREATE TABLE estado_comanda (
    id INT IDENTITY(1,1) PRIMARY KEY,
    estado VARCHAR(255),
);
GO

-- Tabla: usuario
CREATE TABLE usuario (
    id INT IDENTITY(1,1) PRIMARY KEY,
    codigo INT,
    contrasena VARCHAR(255),
    correo VARCHAR(255),
	verificado BIT DEFAULT 0,
	token_verificacion VARCHAR(255) NULL,
	fecha_token DATETIME NULL
);

-- Tabla: cargo
CREATE TABLE cargo (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre VARCHAR(255)
);

-- Tabla: empleado
CREATE TABLE empleado (
    id INT IDENTITY(1,1) PRIMARY KEY,
    apellido VARCHAR(255),
    dni VARCHAR(255),
    fecha_registro DATETIME DEFAULT GETDATE(),
    nombre VARCHAR(255),
    telefono VARCHAR(255),
    cargo_id INT,
    usuario_id INT,
    FOREIGN KEY (cargo_id) REFERENCES cargo(id),
    FOREIGN KEY (usuario_id) REFERENCES usuario(id)
);

-- Tabla: comanda
CREATE TABLE comanda (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cantidad_asientos INT,
    fecha_emision VARCHAR(255),
    precio_total DECIMAL(10,2),
    empleado_id INT,
    estado_comanda_id INT,
    mesa_id INT,
    FOREIGN KEY (empleado_id) REFERENCES empleado(id),
    FOREIGN KEY (estado_comanda_id) REFERENCES estado_comanda(id),
    FOREIGN KEY (mesa_id) REFERENCES mesa(id)
);

-- Tabla: detalle_comanda
CREATE TABLE detalle_comanda (
    id INT IDENTITY(1,1) PRIMARY KEY,
    cantidad_pedido INT,
    observacion VARCHAR(255),
    precio_unitario DECIMAL(10,2),
    comanda_id INT,
    plato_id VARCHAR(255),
    FOREIGN KEY (comanda_id) REFERENCES comanda(id),
    FOREIGN KEY (plato_id) REFERENCES plato(id)
);

-- Tabla: cliente
CREATE TABLE cliente (
    id INT IDENTITY(1,1) PRIMARY KEY,
    apellido VARCHAR(255),
    dni VARCHAR(255),
    nombre VARCHAR(255)
);

-- Tabla: establecimiento
CREATE TABLE establecimiento (
    id VARCHAR(255) PRIMARY KEY,
    direccion VARCHAR(255),
    nombre VARCHAR(255),
    ruc VARCHAR(255),
    telefono VARCHAR(255)
);

-- Tabla: caja
CREATE TABLE caja (
    id VARCHAR(255) PRIMARY KEY,
    establecimiento_id VARCHAR(255),
    FOREIGN KEY (establecimiento_id) REFERENCES establecimiento(id)
);

-- Tabla: tipo_comprobante
CREATE TABLE tipo_comprobante (
    id INT IDENTITY(1,1) PRIMARY KEY,
    tipo VARCHAR(255)
);

-- Tabla: comprobante
CREATE TABLE comprobante (
    id INT IDENTITY(1,1) PRIMARY KEY,
    descuento_total DECIMAL(10,2),
    fecha_emision DATETIME,
    igv_total DECIMAL(10,2),
    precio_total_pedido DECIMAL(10,2),
    sub_total DECIMAL(10,2),
    caja_id VARCHAR(255),
    cliente_id INT,
    comanda_id INT,
    empleado_id INT,
    tipo_comprobante_id INT,
    FOREIGN KEY (caja_id) REFERENCES caja(id),
    FOREIGN KEY (cliente_id) REFERENCES cliente(id),
    FOREIGN KEY (comanda_id) REFERENCES comanda(id),
    FOREIGN KEY (empleado_id) REFERENCES empleado(id),
    FOREIGN KEY (tipo_comprobante_id) REFERENCES tipo_comprobante(id)
);

-- Tabla: metodo_pago
CREATE TABLE metodo_pago (
    id INT IDENTITY(1,1) PRIMARY KEY,
    metodo VARCHAR(255)
);

-- Tabla: detalle_comprobante
CREATE TABLE detalle_comprobante (
    id INT IDENTITY(1,1) PRIMARY KEY,
    monto_pago DECIMAL(10,2),
    comprobante_id INT,
    metodo_pago_id INT,
    FOREIGN KEY (comprobante_id) REFERENCES comprobante(id),
    FOREIGN KEY (metodo_pago_id) REFERENCES metodo_pago(id)
);
