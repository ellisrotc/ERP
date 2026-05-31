namespace ERP.WinForms.Forms;

public partial class frmDashboard : Form
{
    private readonly string _role;
    private readonly string _nombreCompleto;
    private Form? _moduloActual;
    private Button? _btnActivo;

    public frmDashboard(string role, string nombreCompleto)
    {
        _role = role;
        _nombreCompleto = nombreCompleto;
        InitializeComponent();
    }

    private void frmDashboard_Load(object sender, EventArgs e)
    {
        lblBienvenido.Text = $"  Usuario: {_nombreCompleto}   |   Rol: {_role}   |   {DateTime.Now:dd/MM/yyyy HH:mm}  ";
        lblBienvenidoSidebar.Text = _nombreCompleto;
        ConfigurarModulosPorRol();
    }

    private void ConfigurarModulosPorRol()
    {
        btnEmpleados.Visible    = _role is "Admin" or "RRHH";
        btnPlanillas.Visible    = _role is "Admin" or "RRHH";
        btnComprobantes.Visible = _role is "Admin" or "Contador";
        btnLibros.Visible       = _role is "Admin" or "Contador";
        btnReportes.Visible     = _role is "Admin" or "Gerente" or "Contador";
    }

    // ── Método central: embebe el form en pnlContent ─────────
    private void AbrirModulo(Form form, string titulo, Button boton)
    {
        // Quitar resaltado del botón anterior
        if (_btnActivo != null)
        {
            _btnActivo.BackColor = Color.FromArgb(22, 34, 72);
            _btnActivo.ForeColor = Color.FromArgb(200, 215, 240);
        }

        // Resaltar botón activo
        boton.BackColor = Color.FromArgb(0, 100, 180);
        boton.ForeColor = Color.White;
        _btnActivo = boton;

        // Cerrar el módulo anterior si existe
        _moduloActual?.Close();
        _moduloActual?.Dispose();

        // Limpiar panel de contenido
        pnlContent.Controls.Clear();

        // Configurar el form hijo para que viva dentro del panel
        form.TopLevel = false;
        form.FormBorderStyle = FormBorderStyle.None;
        form.Dock = DockStyle.Fill;

        // Insertar en el panel y mostrar
        pnlContent.Controls.Add(form);
        form.Show();

        _moduloActual = form;
        lblModuloActivo.Text = $"  {titulo}";
    }

    // ── Handlers de botones ──────────────────────────────────
    private void btnEmpleados_Click(object sender, EventArgs e)
        => AbrirModulo(new frmEmpleados(), "Gestión de Empleados", btnEmpleados);

    private void btnPlanillas_Click(object sender, EventArgs e)
        => AbrirModulo(new frmPlanillas(), "Cálculo de Planillas", btnPlanillas);

    private void btnComprobantes_Click(object sender, EventArgs e)
        => AbrirModulo(new frmComprobantes(), "Registro de Comprobantes", btnComprobantes);

    private void btnLibros_Click(object sender, EventArgs e)
        => AbrirModulo(new frmLibros(), "Libros Contables (Compras / Ventas)", btnLibros);

    private void btnReportes_Click(object sender, EventArgs e)
        => AbrirModulo(new frmReportes(), "Reportes Financieros", btnReportes);

    private void frmDashboard_FormClosed(object sender, FormClosedEventArgs e)
        => Application.Exit();
}
