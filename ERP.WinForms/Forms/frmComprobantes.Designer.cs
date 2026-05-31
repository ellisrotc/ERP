namespace ERP.WinForms.Forms;

partial class frmComprobantes
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cboTipo;
    private TextBox txtNumero, txtSerie, txtRuc, txtRazonSocial;
    private DateTimePicker dtpFecha;
    private NumericUpDown nudMonto;
    private Label lblIgvValor;
    private Button btnGuardar;
    private DataGridView dgvComprobantes;
    private SplitContainer splitContainer;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cboTipo = new ComboBox();
        txtNumero = new TextBox();
        txtSerie = new TextBox();
        txtRuc = new TextBox();
        txtRazonSocial = new TextBox();
        dtpFecha = new DateTimePicker();
        nudMonto = new NumericUpDown();
        lblIgvValor = new Label();
        btnGuardar = new Button();
        dgvComprobantes = new DataGridView();
        splitContainer = new SplitContainer();

        ((System.ComponentModel.ISupportInitialize)nudMonto).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvComprobantes).BeginInit();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        SuspendLayout();

        // splitContainer
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Orientation = Orientation.Horizontal;
        splitContainer.SplitterDistance = 280;

        // Panel1 - Form
        int y = 15;
        Action<string, Control> addRow = (lbl, ctrl) =>
        {
            var l = new Label { Text = lbl, Location = new Point(10, y), AutoSize = true };
            ctrl.Location = new Point(130, y - 3);
            if (ctrl is TextBox tb) tb.Size = new Size(250, 23);
            splitContainer.Panel1.Controls.Add(l);
            splitContainer.Panel1.Controls.Add(ctrl);
            y += 30;
        };

        addRow("Tipo:", cboTipo);
        cboTipo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTipo.Size = new Size(150, 23);

        addRow("Número:", txtNumero);
        addRow("Serie:", txtSerie);
        addRow("RUC (11 dígitos):", txtRuc);
        addRow("Razón Social:", txtRazonSocial);
        addRow("Fecha:", dtpFecha);
        dtpFecha.Size = new Size(150, 23);
        dtpFecha.Format = DateTimePickerFormat.Short;

        addRow("Monto (con IGV):", nudMonto);
        nudMonto.Size = new Size(120, 23);
        nudMonto.DecimalPlaces = 2;
        nudMonto.Maximum = 9999999;
        nudMonto.ValueChanged += nudMonto_ValueChanged;

        var lblIgv = new Label { Text = "IGV (18%):", Location = new Point(10, y), AutoSize = true };
        lblIgvValor = new Label { Text = "S/ 0.00", Location = new Point(130, y), AutoSize = true, ForeColor = Color.DarkGreen, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        splitContainer.Panel1.Controls.Add(lblIgv);
        splitContainer.Panel1.Controls.Add(lblIgvValor);
        y += 35;

        btnGuardar = new Button
        {
            Text = "Guardar Comprobante",
            Location = new Point(130, y),
            Size = new Size(160, 32),
            BackColor = Color.FromArgb(0, 150, 136),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += btnGuardar_Click;
        splitContainer.Panel1.Controls.Add(btnGuardar);

        // Panel2 - Grid
        dgvComprobantes.AllowUserToAddRows = false;
        dgvComprobantes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvComprobantes.Dock = DockStyle.Fill;
        dgvComprobantes.ReadOnly = true;
        dgvComprobantes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvComprobantes.BackgroundColor = Color.White;
        splitContainer.Panel2.Controls.Add(dgvComprobantes);

        // frmComprobantes
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 600);
        Controls.Add(splitContainer);
        Name = "frmComprobantes";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Comprobantes";
        Load += frmComprobantes_Load;

        ((System.ComponentModel.ISupportInitialize)nudMonto).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvComprobantes).EndInit();
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        splitContainer.ResumeLayout(false);
        ResumeLayout(false);
    }
}
