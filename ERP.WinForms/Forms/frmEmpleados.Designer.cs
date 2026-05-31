namespace ERP.WinForms.Forms;

partial class frmEmpleados
{
    private System.ComponentModel.IContainer components = null;
    private DataGridView dgvEmpleados;
    private Button btnNuevo;
    private Button btnEditar;
    private Button btnDesactivar;
    private Panel pnlToolbar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        dgvEmpleados = new DataGridView();
        btnNuevo = new Button();
        btnEditar = new Button();
        btnDesactivar = new Button();
        pnlToolbar = new Panel();

        ((System.ComponentModel.ISupportInitialize)dgvEmpleados).BeginInit();
        pnlToolbar.SuspendLayout();
        SuspendLayout();

        // pnlToolbar
        pnlToolbar.Controls.Add(btnDesactivar);
        pnlToolbar.Controls.Add(btnEditar);
        pnlToolbar.Controls.Add(btnNuevo);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Height = 45;
        pnlToolbar.BackColor = Color.FromArgb(240, 240, 240);

        // Buttons
        void SetBtn(Button b, string text, int x, Color color)
        {
            b.BackColor = color;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            b.Location = new Point(x, 8);
            b.Size = new Size(100, 30);
            b.Text = text;
        }
        SetBtn(btnNuevo, "Nuevo", 10, Color.FromArgb(0, 150, 136));
        SetBtn(btnEditar, "Editar", 120, Color.FromArgb(33, 150, 243));
        SetBtn(btnDesactivar, "Desactivar", 230, Color.FromArgb(244, 67, 54));

        btnNuevo.Click += btnNuevo_Click;
        btnEditar.Click += btnEditar_Click;
        btnDesactivar.Click += btnDesactivar_Click;

        // dgvEmpleados
        dgvEmpleados.AllowUserToAddRows = false;
        dgvEmpleados.AllowUserToDeleteRows = false;
        dgvEmpleados.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvEmpleados.Dock = DockStyle.Fill;
        dgvEmpleados.ReadOnly = true;
        dgvEmpleados.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEmpleados.MultiSelect = false;
        dgvEmpleados.BackgroundColor = Color.White;
        dgvEmpleados.BorderStyle = BorderStyle.None;

        // frmEmpleados
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 500);
        Controls.Add(dgvEmpleados);
        Controls.Add(pnlToolbar);
        Name = "frmEmpleados";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Empleados";
        Load += frmEmpleados_Load;

        ((System.ComponentModel.ISupportInitialize)dgvEmpleados).EndInit();
        pnlToolbar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
