-- ============================================================
-- ERP UNAJ — Esquema normalizado 3FN
-- ============================================================

CREATE TABLE rol (
  id_rol SERIAL PRIMARY KEY,
  nombre_rol VARCHAR(30) NOT NULL UNIQUE,
  descripcion VARCHAR(200)
);

CREATE TABLE usuario (
  id_usuario SERIAL PRIMARY KEY,
  username VARCHAR(50) NOT NULL UNIQUE,
  password_hash VARCHAR(128) NOT NULL,
  nombre_completo VARCHAR(100) NOT NULL,
  id_rol INT NOT NULL REFERENCES rol(id_rol),
  activo BOOLEAN DEFAULT TRUE,
  ultimo_acceso TIMESTAMP,
  refresh_token VARCHAR(200),
  refresh_token_expiry TIMESTAMP
);

CREATE TABLE tipo_descuento (
  id_tipo SERIAL PRIMARY KEY,
  nombre VARCHAR(20) NOT NULL,
  porcentaje DECIMAL(5,4) NOT NULL
);

CREATE TABLE empleado (
  id_empleado SERIAL PRIMARY KEY,
  nombre VARCHAR(80) NOT NULL,
  apellido VARCHAR(80) NOT NULL,
  dni VARCHAR(8) NOT NULL UNIQUE,
  cargo VARCHAR(80),
  salario_base DECIMAL(10,2) NOT NULL,
  id_tipo_descuento INT REFERENCES tipo_descuento(id_tipo),
  fecha_ingreso DATE NOT NULL DEFAULT CURRENT_DATE,
  activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE planilla (
  id_planilla SERIAL PRIMARY KEY,
  id_empleado INT NOT NULL REFERENCES empleado(id_empleado),
  periodo VARCHAR(7) NOT NULL,
  total_bruto DECIMAL(10,2),
  total_descuentos DECIMAL(10,2),
  total_neto DECIMAL(10,2),
  fecha_calculo TIMESTAMP DEFAULT NOW()
);

CREATE TABLE detalle_planilla (
  id_detalle SERIAL PRIMARY KEY,
  id_planilla INT NOT NULL REFERENCES planilla(id_planilla),
  afp DECIMAL(8,2) DEFAULT 0,
  onp DECIMAL(8,2) DEFAULT 0,
  cts DECIMAL(8,2) DEFAULT 0,
  gratificacion DECIMAL(8,2) DEFAULT 0,
  horas_extras DECIMAL(8,2) DEFAULT 0
);

CREATE TABLE tipo_comprobante (
  id_tipo SERIAL PRIMARY KEY,
  nombre VARCHAR(30) NOT NULL,
  serie_prefix VARCHAR(5)
);

CREATE TABLE comprobante (
  id_comprobante SERIAL PRIMARY KEY,
  id_tipo_comprobante INT REFERENCES tipo_comprobante(id_tipo),
  numero VARCHAR(20) NOT NULL,
  serie VARCHAR(10),
  ruc VARCHAR(11) NOT NULL,
  razon_social VARCHAR(150),
  fecha DATE NOT NULL DEFAULT CURRENT_DATE,
  monto DECIMAL(10,2) NOT NULL,
  igv DECIMAL(10,2) NOT NULL,
  activo BOOLEAN DEFAULT TRUE
);

CREATE TABLE libro_contable (
  id_libro SERIAL PRIMARY KEY,
  tipo VARCHAR(10) NOT NULL CHECK (tipo IN ('COMPRAS','VENTAS')),
  periodo VARCHAR(7) NOT NULL,
  fecha_generacion TIMESTAMP DEFAULT NOW(),
  estado VARCHAR(15) DEFAULT 'PENDIENTE',
  id_usuario INT REFERENCES usuario(id_usuario)
);

CREATE TABLE comprobante_libro (
  id_comprobante INT REFERENCES comprobante(id_comprobante),
  id_libro INT REFERENCES libro_contable(id_libro),
  base_imponible DECIMAL(10,2),
  PRIMARY KEY (id_comprobante, id_libro)
);

-- ============================================================
-- Stored Procedure: calcular planilla
-- ============================================================
CREATE OR REPLACE FUNCTION sp_calcular_planilla(p_periodo VARCHAR, p_id_empleado INT)
RETURNS void AS $$
DECLARE
  v_salario    DECIMAL(10,2);
  v_tipo       VARCHAR(20);
  v_bruto      DECIMAL(10,2);
  v_afp        DECIMAL(8,2) := 0;
  v_onp        DECIMAL(8,2) := 0;
  v_cts        DECIMAL(8,2);
  v_grat       DECIMAL(8,2) := 0;
  v_neto       DECIMAL(10,2);
  v_id_plan    INT;
BEGIN
  SELECT e.salario_base, td.nombre
    INTO v_salario, v_tipo
    FROM empleado e
    JOIN tipo_descuento td ON e.id_tipo_descuento = td.id_tipo
   WHERE e.id_empleado = p_id_empleado AND e.activo = TRUE;

  v_bruto := v_salario;
  v_afp   := CASE WHEN v_tipo = 'AFP' THEN v_bruto * 0.10 ELSE 0 END;
  v_onp   := CASE WHEN v_tipo = 'ONP' THEN v_bruto * 0.13 ELSE 0 END;
  v_cts   := v_bruto / 12.0;
  v_grat  := CASE WHEN EXTRACT(MONTH FROM NOW()) IN (7,12) THEN v_bruto / 6.0 ELSE 0 END;
  v_neto  := v_bruto - v_afp - v_onp;

  INSERT INTO planilla (id_empleado, periodo, total_bruto, total_descuentos, total_neto)
  VALUES (p_id_empleado, p_periodo, v_bruto, v_afp + v_onp, v_neto)
  RETURNING id_planilla INTO v_id_plan;

  INSERT INTO detalle_planilla (id_planilla, afp, onp, cts, gratificacion)
  VALUES (v_id_plan, v_afp, v_onp, v_cts, v_grat);
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- Stored Procedure: generar libro ventas
-- ============================================================
CREATE OR REPLACE FUNCTION sp_generar_libro_ventas(p_periodo VARCHAR, p_id_usuario INT)
RETURNS INT AS $$
DECLARE
  v_id_libro INT;
BEGIN
  INSERT INTO libro_contable (tipo, periodo, id_usuario, estado)
  VALUES ('VENTAS', p_periodo, p_id_usuario, 'GENERADO')
  RETURNING id_libro INTO v_id_libro;

  INSERT INTO comprobante_libro (id_comprobante, id_libro, base_imponible)
  SELECT c.id_comprobante, v_id_libro, c.monto - c.igv
  FROM comprobante c
  JOIN tipo_comprobante tc ON c.id_tipo_comprobante = tc.id_tipo
  WHERE tc.nombre IN ('Factura','Boleta')
    AND TO_CHAR(c.fecha, 'YYYY-MM') = p_periodo
    AND c.activo = TRUE;

  RETURN v_id_libro;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- Stored Procedure: generar libro compras
-- ============================================================
CREATE OR REPLACE FUNCTION sp_generar_libro_compras(p_periodo VARCHAR, p_id_usuario INT)
RETURNS INT AS $$
DECLARE
  v_id_libro INT;
BEGIN
  INSERT INTO libro_contable (tipo, periodo, id_usuario, estado)
  VALUES ('COMPRAS', p_periodo, p_id_usuario, 'GENERADO')
  RETURNING id_libro INTO v_id_libro;

  INSERT INTO comprobante_libro (id_comprobante, id_libro, base_imponible)
  SELECT c.id_comprobante, v_id_libro, c.monto - c.igv
  FROM comprobante c
  JOIN tipo_comprobante tc ON c.id_tipo_comprobante = tc.id_tipo
  WHERE tc.nombre IN ('Nota Crédito')
    AND TO_CHAR(c.fecha, 'YYYY-MM') = p_periodo
    AND c.activo = TRUE;

  RETURN v_id_libro;
END;
$$ LANGUAGE plpgsql;

-- ============================================================
-- Datos iniciales
-- ============================================================
INSERT INTO rol (nombre_rol, descripcion) VALUES
  ('Admin',    'Administrador del sistema'),
  ('RRHH',     'Gestión de planillas y empleados'),
  ('Contador', 'Gestión contable SUNAT'),
  ('Gerente',  'Visualización de reportes');

INSERT INTO tipo_descuento (nombre, porcentaje) VALUES
  ('AFP', 0.1000),
  ('ONP', 0.1300);

INSERT INTO tipo_comprobante (nombre, serie_prefix) VALUES
  ('Factura',      'F001'),
  ('Boleta',       'B001'),
  ('Nota Crédito', 'NC01');

-- Password: Admin123!  (hash BCrypt real generado con BCrypt.Net-Next 4.0.3)
INSERT INTO usuario (username, password_hash, nombre_completo, id_rol) VALUES
  ('admin',     '$2a$11$PciCU5jg5DHx.PH2sulQuuYEhe0GqpAetOxbnsukNaYlAtz5yXDe6', 'Administrador UNAJ', 1),
  ('contador1', '$2a$11$PciCU5jg5DHx.PH2sulQuuYEhe0GqpAetOxbnsukNaYlAtz5yXDe6', 'Juan Contador', 3),
  ('rrhh1',     '$2a$11$PciCU5jg5DHx.PH2sulQuuYEhe0GqpAetOxbnsukNaYlAtz5yXDe6', 'Maria RRHH', 2);

INSERT INTO empleado (nombre, apellido, dni, cargo, salario_base, id_tipo_descuento) VALUES
  ('Carlos', 'Mamani', '12345678', 'Docente', 3500.00, 1),
  ('Rosa',   'Quispe', '87654321', 'Administrativo', 2800.00, 2),
  ('Jorge',  'Flores', '11223344', 'Docente', 4200.00, 1);
