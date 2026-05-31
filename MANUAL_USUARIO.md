# Manual de Usuario — ERP UNAJ
## Sistema de Gestión Financiera y Contable
**Universidad Nacional de Juliaca | Taller de Procesos ERP 2026**

---

## ¿Para qué sirve este sistema?

El ERP UNAJ automatiza tres procesos financieros clave de la universidad:

| Proceso | Sin ERP | Con ERP |
|---|---|---|
| Pago de personal | Calcular manualmente AFP, ONP, CTS de cada empleado | Un clic calcula toda la nómina con stored procedures |
| Contabilidad SUNAT | Registrar facturas en papel o Excel | Sistema centralizado con cálculo automático de IGV |
| Reportes financieros | Consolidar datos de múltiples fuentes | Balance y Estado de Resultados generados al instante |

---

## Roles y permisos

| Rol | Quién lo usa | Acceso |
|---|---|---|
| **Admin** | Administrador del sistema | Todo el sistema |
| **RRHH** | Área de Recursos Humanos | Empleados + Planillas |
| **Contador** | Área de Contabilidad | Comprobantes + Libros + Reportes |
| **Gerente** | Directivos | Solo Reportes (solo lectura) |

---

## Cómo ingresar al sistema

1. Abrir la aplicación ERP UNAJ
2. Escribir usuario y contraseña
3. Presionar **INGRESAR** o la tecla `Enter`

```
Usuarios de prueba:
  admin      →  Admin123!   (acceso total)
  rrhh1      →  Admin123!   (RRHH)
  contador1  →  Admin123!   (Contador)
```

Al ingresar aparece el **Dashboard** con el menú lateral izquierdo. Cada módulo se carga en el área derecha sin abrir ventanas separadas.

---

## MÓDULO 1 — Empleados

**Quién lo usa:** Rol RRHH o Admin
**Propósito:** Mantener el padrón de trabajadores de la universidad

### ¿Qué datos maneja?

| Campo | Descripción | Ejemplo |
|---|---|---|
| Nombre / Apellido | Nombre completo | Carlos Mamani |
| DNI | 8 dígitos, único por persona | 12345678 |
| Cargo | Puesto en la universidad | Docente, Administrativo |
| Salario Base | Remuneración mensual bruta en S/ | 3500.00 |
| Tipo Descuento | Sistema de pensiones | AFP (10%) u ONP (13%) |

### Operaciones disponibles

**Nuevo empleado:**
1. Clic en botón **Nuevo**
2. Llenar: nombre, apellido, DNI, cargo, salario base, tipo de descuento
3. Clic en **Guardar**

**Editar empleado:**
1. Seleccionar una fila en la tabla
2. Clic en **Editar**
3. Modificar los datos y **Guardar**

**Desactivar empleado:**
1. Seleccionar la fila del empleado
2. Clic en **Desactivar**
3. Confirmar la acción

> **Importante:** Los empleados desactivados no se eliminan de la base de datos (soft delete). Quedan con `activo = false` y no aparecen en futuros cálculos de planilla.

---

## MÓDULO 2 — Planillas

**Quién lo usa:** Rol RRHH o Admin
**Propósito:** Calcular automáticamente la remuneración mensual de todos los empleados activos

### ¿Cómo funciona el cálculo?

Al presionar **Calcular Planilla**, el sistema ejecuta un stored procedure en PostgreSQL que para cada empleado activo calcula:

```
Salario Bruto   = salario_base del empleado
AFP             = Bruto × 10%   (si el empleado eligió AFP)
ONP             = Bruto × 13%   (si el empleado eligió ONP)
CTS             = Bruto ÷ 12    (compensación por tiempo de servicio)
Gratificación   = Bruto ÷ 6     (solo en julio y diciembre, en otros meses = 0)
Neto a Pagar    = Bruto − AFP/ONP
```

### Paso a paso

1. Seleccionar el período en el ComboBox (formato `YYYY-MM`, ejemplo: `2026-05`)
2. Clic en **Calcular Planilla**
3. Esperar el mensaje "Cálculo completado"
4. La tabla muestra todos los empleados con sus montos calculados

### Columnas del resultado

| Columna | Significado |
|---|---|
| Empleado | Nombre completo |
| Bruto | Salario base (antes de descuentos) |
| AFP | Descuento AFP si aplica |
| ONP | Descuento ONP si aplica |
| CTS | Provisión mensual de CTS |
| Gratificación | Solo tiene valor en julio y diciembre |
| Neto | Lo que efectivamente recibe el empleado |

### Descargar boleta PDF

1. Hacer clic en una fila de la tabla
2. Clic en **Ver PDF**
3. Se abre automáticamente la boleta de pago en el visor PDF del sistema

> **Nota:** Si calculas el mismo período dos veces se generan registros adicionales. En producción se agregaría una validación de período único.

---

## MÓDULO 3 — Comprobantes

**Quién lo usa:** Rol Contador o Admin
**Propósito:** Registrar todos los documentos de venta y compra que la universidad emite o recibe, para cumplir con las obligaciones ante SUNAT

### ¿Qué es un comprobante?

En Perú, toda operación económica debe respaldarse con un documento tributario:

| Tipo | Cuándo se usa |
|---|---|
| **Factura** | Venta de servicios a empresas o entidades (con RUC) |
| **Boleta** | Venta de servicios a personas naturales |
| **Nota de Crédito** | Devolución, anulación o descuento sobre una venta anterior |

### Cálculo automático de IGV

El IGV (Impuesto General a las Ventas) en Perú es **18%**. El sistema calcula automáticamente al momento de ingresar el monto:

```
Si el monto ingresado es el precio TOTAL (con IGV incluido):
  IGV            = Monto ÷ 1.18 × 0.18
  Base Imponible = Monto − IGV

Ejemplo:
  Monto total:    S/ 1,180.00
  IGV (18%):      S/   180.00
  Base imponible: S/ 1,000.00
```

> El IGV se actualiza en tiempo real conforme escribes el monto, sin necesidad de hacer clic.

### Cómo registrar un comprobante

1. Seleccionar el **Tipo** (Factura, Boleta, Nota de Crédito)
2. Ingresar el **Número** de comprobante (ej: `F001-001`)
3. Ingresar la **Serie** (ej: `F001`)
4. Ingresar el **RUC** del cliente/proveedor — debe tener **exactamente 11 dígitos numéricos**
5. Ingresar la **Razón Social** (nombre de la empresa)
6. Seleccionar la **Fecha** de emisión
7. Ingresar el **Monto total** (con IGV incluido)
8. Verificar el IGV calculado automáticamente
9. Clic en **Guardar Comprobante**

### Validaciones

| Campo | Regla |
|---|---|
| RUC | Exactamente 11 dígitos, solo números |
| Monto | Debe ser mayor a cero |
| Tipo | Obligatorio seleccionar uno |

---

## MÓDULO 4 — Libros Contables

**Quién lo usa:** Rol Contador o Admin
**Propósito:** Generar los libros electrónicos de Ventas y Compras que exige SUNAT mensualmente

### ¿Qué es un libro contable?

SUNAT obliga a las empresas a llevar registro mensual de:
- **Libro de Ventas:** todas las facturas y boletas emitidas en el período
- **Libro de Compras:** todas las notas de crédito y compras del período

### Requisito previo

**Los comprobantes deben estar registrados ANTES de generar el libro.** El libro toma los comprobantes del período seleccionado y los agrupa.

```
Flujo correcto:
  Registrar comprobantes → Generar libro → Exportar a Excel
```

### Cómo generar un libro

1. Seleccionar el **Período** (ej: `2026-05`)
2. Elegir el tab: **Libro Ventas** o **Libro Compras**
3. Clic en **Generar Libro**
4. El sistema llama al stored procedure que agrupa los comprobantes del período

### Exportar a Excel

1. Después de generar el libro, clic en **Exportar Excel**
2. Seleccionar dónde guardar el archivo
3. Se descarga un `.xlsx` con el formato estándar SUNAT:
   - N°, Número de comprobante, RUC, Razón Social, Base Imponible, IGV, Total

### ¿Qué comprobantes va a cada libro?

| Libro | Incluye |
|---|---|
| **Ventas** | Facturas y Boletas |
| **Compras** | Notas de Crédito |

---

## MÓDULO 5 — Reportes Financieros

**Quién lo usa:** Rol Admin, Contador o Gerente
**Propósito:** Ver el estado financiero de la universidad en un período dado

### Requisitos previos

Para que los reportes muestren datos correctos deben existir **en ese período**:
1. Comprobantes registrados
2. Libros generados (ventas y/o compras)
3. Planilla calculada

```
Orden obligatorio:
  1. Registrar comprobantes
  2. Generar Libro de Ventas y Libro de Compras
  3. Calcular Planilla del período
  4. Ver Reportes
```

### Balance General

Muestra la situación financiera en un momento dado:

```
ACTIVO
  Cuentas por Cobrar = total de ventas del período

PASIVO
  Cuentas por Pagar  = total de compras del período
  Planilla           = neto pagado a empleados ese mes

PATRIMONIO NETO = Activo - Pasivo total
```

### Estado de Resultados

Muestra si la universidad tuvo ganancia o pérdida en el período:

```
(+) Ingresos          = base imponible de ventas
(-) Costo de Ventas   = base imponible de compras
────────────────────────────────────────
    Utilidad Bruta
(-) Gastos de Personal = neto de planilla
────────────────────────────────────────
    UTILIDAD NETA
```

### Cómo generar un reporte

1. Seleccionar el **Período**
2. Elegir el tab: **Balance** o **Estado de Resultados**
3. Clic en **Generar**
4. Los datos aparecen en la tabla

---

## Flujo completo de trabajo (mes a mes)

Este es el orden recomendado para usar el sistema cada mes:

```
INICIO DE MES
│
├─ 1. RRHH verifica empleados activos
│      → Módulo Empleados: dar de alta nuevos, desactivar salientes
│
├─ 2. Contador registra comprobantes del mes
│      → Módulo Comprobantes: ingresar facturas, boletas, notas de crédito
│
├─ 3. RRHH calcula la planilla del período
│      → Módulo Planillas: seleccionar mes → "Calcular Planilla"
│      → Descargar boletas PDF de cada empleado
│
├─ 4. Contador genera los libros contables
│      → Módulo Libros: Libro Ventas + Libro Compras → Exportar Excel
│
└─ 5. Gerente/Contador revisa los reportes
       → Módulo Reportes: Balance General + Estado de Resultados
```

---

## Preguntas frecuentes

**¿Qué hago si el reporte muestra todo en cero?**
Primero debes registrar comprobantes y luego generar los libros para ese período. Los reportes leen de los libros, no directamente de los comprobantes.

**¿Puedo calcular la planilla varias veces en el mismo mes?**
Sí, pero se generan registros adicionales. Para la demo funciona sin problema. En producción se debería validar que no exista ya una planilla para ese período y empleado.

**¿Qué pasa si desactivo un empleado?**
El empleado queda marcado como inactivo y no aparece en futuros cálculos de planilla. Sus datos históricos se conservan.

**¿Por qué el IGV se calcula sobre el monto total y no sobre el precio sin IGV?**
Porque en Perú los precios al público generalmente ya incluyen el IGV. El sistema asume que el monto ingresado es el precio final con IGV incluido y extrae el IGV de ahí.

**¿El RUC tiene alguna validación especial?**
Solo se valida que tenga exactamente 11 dígitos numéricos. No se consulta en línea a SUNAT (eso requeriría integración con el API de SUNAT).

---

## Glosario

| Término | Significado |
|---|---|
| **AFP** | Administradora de Fondos de Pensiones. Descuento del 10% al trabajador que eligió este sistema |
| **ONP** | Oficina de Normalización Previsional (sistema público de pensiones). Descuento del 13% |
| **CTS** | Compensación por Tiempo de Servicios. Beneficio social equivalente a 1/12 del salario mensual |
| **Gratificación** | Beneficio legal equivalente a 1 salario en julio y 1 salario en diciembre |
| **IGV** | Impuesto General a las Ventas. Equivale al 18% del valor de venta |
| **Base Imponible** | Monto de la operación sin incluir el IGV |
| **RUC** | Registro Único de Contribuyentes. Número de identificación tributaria de 11 dígitos |
| **Libro Electrónico** | Registro contable digital que se presenta a SUNAT mensualmente |
| **Soft Delete** | Técnica de desactivación lógica: los datos no se borran, solo se marcan como inactivos |
| **Stored Procedure** | Procedimiento almacenado en la base de datos PostgreSQL que ejecuta lógica compleja en el servidor |
| **JWT** | JSON Web Token. Mecanismo de autenticación segura que el sistema usa para validar sesiones |

---

*Manual de Usuario — ERP UNAJ v1.0 | Taller de Procesos ERP 2026*
