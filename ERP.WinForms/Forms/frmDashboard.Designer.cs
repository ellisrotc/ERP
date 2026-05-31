namespace ERP.WinForms.Forms;

partial class frmDashboard
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlSidebar;
    private Panel pnlHeader;
    private Panel pnlContent;
    private Label lblTitulo;
    private Label lblBienvenido;
    private Button btnEmpleados;
    private Button btnPlanillas;
    private Button btnComprobantes;
    private Button btnLibros;
    private Button btnReportes;
    private Label lblModuloActivo;
    private Label lblBienvenidoSidebar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlHeader   = new Panel();
        pnlSidebar  = new Panel();
        pnlContent  = new Panel();
        lblTitulo   = new Label();
        lblBienvenido = new Label();
        lblBienvenidoSidebar = new Label();
        lblModuloActivo = new Label();
        btnEmpleados    = new Button();
        btnPlanillas    = new Button();
        btnComprobantes = new Button();
        btnLibros       = new Button();
        btnReportes     = new Button();

        pnlHeader.SuspendLayout();
        pnlSidebar.SuspendLayout();
        SuspendLayout();

        // ── Header ─────────────────────────────────────────────
        pnlHeader.BackColor = Color.FromArgb(0, 70, 127);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 55;
        pnlHeader.Controls.Add(lblTitulo);

        lblTitulo.Dock = DockStyle.Fill;
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblTitulo.Text = "ERP UNAJ – Sistema de Gestión Financiera y Contable";
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

        // ── Sidebar ────────────────────────────────────────────
        pnlSidebar.BackColor = Color.FromArgb(22, 34, 72);
        pnlSidebar.Dock = DockStyle.Left;
        pnlSidebar.Width = 210;

        // Label bienvenida en sidebar (bottom)
        lblBienvenidoSidebar.AutoSize = false;
        lblBienvenidoSidebar.Dock = DockStyle.Bottom;
        lblBienvenidoSidebar.Height = 50;
        lblBienvenidoSidebar.ForeColor = Color.FromArgb(160, 170, 200);
        lblBienvenidoSidebar.Font = new Font("Segoe UI", 8F);
        lblBienvenidoSidebar.TextAlign = ContentAlignment.MiddleCenter;
        lblBienvenidoSidebar.Padding = new Padding(5);

        // Botones del sidebar
        var buttons = new[] { btnEmpleados, btnPlanillas, btnComprobantes, btnLibros, btnReportes };
        var labels  = new[] { "👤  Empleados", "📋  Planillas", "🧾  Comprobantes", "📚  Libros Contables", "📊  Reportes" };
        var clicks  = new EventHandler[]
        {
            btnEmpleados_Click, btnPlanillas_Click, btnComprobantes_Click,
            btnLibros_Click, btnReportes_Click
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].BackColor = Color.FromArgb(22, 34, 72);
            buttons[i].FlatStyle = FlatStyle.Flat;
            buttons[i].FlatAppearance.BorderSize = 0;
            buttons[i].FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 90, 160);
            buttons[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 70, 127);
            buttons[i].ForeColor = Color.FromArgb(200, 215, 240);
            buttons[i].Font = new Font("Segoe UI", 10F);
            buttons[i].Dock = DockStyle.Top;
            buttons[i].Height = 52;
            buttons[i].Text = labels[i];
            buttons[i].TextAlign = ContentAlignment.MiddleLeft;
            buttons[i].Padding = new Padding(18, 0, 0, 0);
            buttons[i].Cursor = Cursors.Hand;
            buttons[i].Click += clicks[i];
            buttons[i].Tag = i;
        }

        // Separador en la parte superior del sidebar
        var lblSep = new Label
        {
            Dock = DockStyle.Top,
            Height = 20,
            BackColor = Color.FromArgb(22, 34, 72)
        };

        pnlSidebar.Controls.Add(lblBienvenidoSidebar);
        pnlSidebar.Controls.Add(btnReportes);
        pnlSidebar.Controls.Add(btnLibros);
        pnlSidebar.Controls.Add(btnComprobantes);
        pnlSidebar.Controls.Add(btnPlanillas);
        pnlSidebar.Controls.Add(btnEmpleados);
        pnlSidebar.Controls.Add(lblSep);

        // ── Barra inferior (status) ────────────────────────────
        lblBienvenido.AutoSize = false;
        lblBienvenido.Dock = DockStyle.Bottom;
        lblBienvenido.Height = 26;
        lblBienvenido.BackColor = Color.FromArgb(240, 242, 248);
        lblBienvenido.ForeColor = Color.FromArgb(80, 80, 100);
        lblBienvenido.Font = new Font("Segoe UI", 8.5F);
        lblBienvenido.TextAlign = ContentAlignment.MiddleRight;
        lblBienvenido.Padding = new Padding(0, 0, 12, 0);

        // ── Panel módulo activo (título sobre el contenido) ────
        lblModuloActivo.AutoSize = false;
        lblModuloActivo.Dock = DockStyle.Top;
        lblModuloActivo.Height = 36;
        lblModuloActivo.BackColor = Color.FromArgb(230, 236, 250);
        lblModuloActivo.ForeColor = Color.FromArgb(0, 60, 120);
        lblModuloActivo.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblModuloActivo.TextAlign = ContentAlignment.MiddleLeft;
        lblModuloActivo.Padding = new Padding(14, 0, 0, 0);
        lblModuloActivo.Text = "Seleccione un módulo del menú lateral";

        // ── Panel de contenido principal ───────────────────────
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.BackColor = Color.White;
        pnlContent.Padding = new Padding(0);

        // ── frmDashboard ──────────────────────────────────────
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1150, 680);
        MinimumSize = new Size(900, 550);

        // Orden de inserción (último = encima en Z-order para Fill)
        Controls.Add(pnlContent);      // Fill → ocupa el espacio restante
        Controls.Add(lblModuloActivo); // Top  → justo debajo del header
        Controls.Add(pnlSidebar);      // Left
        Controls.Add(lblBienvenido);   // Bottom
        Controls.Add(pnlHeader);       // Top  → más arriba de todo

        Name = "frmDashboard";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ERP UNAJ – Dashboard";
        WindowState = FormWindowState.Maximized;
        Load += frmDashboard_Load;
        FormClosed += frmDashboard_FormClosed;

        pnlHeader.ResumeLayout(false);
        pnlSidebar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
