--------------------------------------------------------
-- DATOS
--------------------------------------------------------

-- Clientes
INSERT INTO cliente (apellido, dni, nombre) VALUES
('García', '12345678', 'Juan'),
('Pérez', '87654321', 'María'),
('López', '11223344', 'Carlos'),
('Fernández', '44332211', 'Lucía'),
('Martínez', '99887766', 'Sofía');
GO

-- Usuarios (todos con misma clave encriptada y verificados)
INSERT INTO usuario (codigo, contrasena, correo, verificado) VALUES
(1001, '$2a$11$IDVhVIOUxSrf8ale15Jwhu9hk8XSbci7v81swqFk.dR.MPgjlSbPu', 'admin@mao.com', 1),
(1002, '$2a$11$IDVhVIOUxSrf8ale15Jwhu9hk8XSbci7v81swqFk.dR.MPgjlSbPu', 'mesero@mao.com', 1),
(1003, '$2a$11$IDVhVIOUxSrf8ale15Jwhu9hk8XSbci7v81swqFk.dR.MPgjlSbPu', 'cocinero@mao.com', 1),
(1004, '$2a$11$IDVhVIOUxSrf8ale15Jwhu9hk8XSbci7v81swqFk.dR.MPgjlSbPu', 'cajero@mao.com', 1),
(1005, '$2a$11$IDVhVIOUxSrf8ale15Jwhu9hk8XSbci7v81swqFk.dR.MPgjlSbPu', 'gerente@mao.com', 1);


-- Cargos
INSERT INTO cargo (nombre) VALUES
('Administrador'),('Mesero'),('Cocinero'),('Cajero'),('Gerente');

-- Empleados
INSERT INTO empleado (apellido, dni, nombre, telefono, cargo_id, usuario_id) VALUES
('Torres', '11112222', 'Luis', '987654321', 1, 1),
('Ramírez', '22223333', 'Ana', '912345678', 2, 2),
('Morales', '33334444', 'Pedro', '923456789', 3, 3),
('Castro', '44445555', 'Elena', '934567890', 4, 4),
('Vega', '55556666', 'Mario', '945678901', 5, 5);

-- Categorías de platos
INSERT INTO categoria_plato (id, nombre) VALUES
('CAT01','Entradas'),
('CAT02','Makis'),
('CAT03','Bebidas'),
('CAT04','Combos'),
('CAT05','Postres');

-- Platos
INSERT INTO plato (id, imagen, nombre, precio_plato, categoria_plato_id) VALUES
('P01','/images/causa.jpg','Causa Limeña',15.00,'CAT01'),
('P02','/images/tequeños.jpg','Tequeños de Queso',18.00,'CAT01'),
('P03','/images/makis.jpg','Maki Acevichado',28.00,'CAT02'),
('P04','/images/makis2.jpg','Maki California',25.00,'CAT02'),
('P05','/images/chicha.jpg','Chicha Morada',8.00,'CAT03'),
('P06','/images/gaseosa.jpg','Gaseosa Personal',6.00,'CAT03'),
('P07','/images/combo1.jpg','Combo Familiar (Makis + Gaseosas)',60.00,'CAT04'),
('P08','/images/combo2.jpg','Combo Pareja (Makis + Postre)',45.00,'CAT04'),
('P09','/images/helado.jpg','Helado de Lucuma',12.00,'CAT05'),
('P10','/images/suspiro.jpg','Suspiro a la Limeña',14.00,'CAT05');

-- Estado Comanda
INSERT INTO estado_comanda (estado) VALUES ('Pendiente'),('Preparado'),('Pagado'),('Cancelado');

-- Mesas
INSERT INTO mesa (cantidad_asientos, estado) VALUES
(4,'Disponible'),
(2,'Ocupada'),
(6,'Disponible');

-- Establecimiento
INSERT INTO establecimiento (id, direccion, nombre, ruc, telefono) VALUES
('EST01','Av. Principal 123','Mao Makis','12345678901','987654321');

-- Caja
INSERT INTO caja (id, establecimiento_id) VALUES
('CAJ01','EST01');

-- Tipo Comprobante
INSERT INTO tipo_comprobante (tipo) VALUES ('Boleta'),('Factura');

-- Métodos de Pago
INSERT INTO metodo_pago (metodo) VALUES ('Efectivo'),('Tarjeta'),('Yape/Plin');

-- Comandas de ejemplo
INSERT INTO comanda (cantidad_asientos, fecha_emision, precio_total, empleado_id, estado_comanda_id, mesa_id) VALUES
(2,'2025-08-20',43.00,2,3,1),
(4,'2025-08-21',75.00,2,3,3);

-- Detalle Comanda
INSERT INTO detalle_comanda (cantidad_pedido, observacion, precio_unitario, comanda_id, plato_id) VALUES
(1,'Sin cebolla',28.00,1,'P03'),
(2,'',7.50,1,'P05'),
(1,'',60.00,2,'P07');

-- Comprobantes
INSERT INTO comprobante (descuento_total, fecha_emision, igv_total, precio_total_pedido, sub_total, caja_id, cliente_id, comanda_id, empleado_id, tipo_comprobante_id)
VALUES
(0.00,'2025-08-20',6.61,43.00,36.39,'CAJ01',1,1,2,1),
(5.00,'2025-08-21',11.86,75.00,68.14,'CAJ01',2,2,2,2);

-- Detalle Comprobante
INSERT INTO detalle_comprobante (monto_pago, comprobante_id, metodo_pago_id) VALUES
(43.00,1,1),
(75.00,2,2);

