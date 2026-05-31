namespace ERP.WinForms.Forms;

partial class frmReportes
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cboPeriodo;
    private Button btnGenerar;
    private TabControl tabReportes;
    private TabPage tabBalance, tabResultados;
    private DataGridView dgvBalance, dgvResultados;
    private Panel pnlToolbar;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cboPeriodo = new ComboBox();
        btnGenerar = new Button();
        tabReportes = new TabControl();
        tabBalance = new TabPage();
        tabResultados = new TabPage();
        dgvBalance = new DataGridView();
        dgvResultados = new DataGridView();
        pnlToolbar = new Panel();

        ((System.ComponentModel.ISupportInitialize)dgvBalance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
        tabReportes.SuspendLayout();
        SuspendLayout();

        pnlToolbar.BackColor = Color.FromArgb(245, 245, 245);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Height = 50;

        var lbl = new Label { Text = "Período:", Location = new Point(10, 16), AutoSize = true };
        cboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriodo.Location = new Point(70, 13);
        cboPeriodo.Size = new Size(100, 25);

        btnGenerar.BackColor = Color.FromArgb(0, 120, 215);
        btnGenerar.FlatStyle = FlatStyle.Flat;
        btnGenerar.FlatAppearance.BorderSize = 0;
        btnGenerar.ForeColor = Color.White;
        btnGenerar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnGenerar.Location = new Point(185, 11);
        btnGenerar.Size = new Size(120, 30);
        btnGenerar.Text = "Generar Reporte";
        btnGenerar.Click += btnGenerar_Click;

        pnlToolbar.Controls.AddRange(new Control[] { lbl, cboPeriodo, btnGenerar });

        tabReportes.Dock = DockStyle.Fill;
        tabReportes.Controls.Add(tabBalance);
        tabReportes.Controls.Add(tabResultados);

        tabBalance.Text = "Balance General";
        tabResultados.Text = "Estado de Resultados";

        void SetupGrid(DataGridView dgv, string[] cols)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.BackgroundColor = Color.White;
            dgv.ColumnCount = cols.Length;
            for (int i = 0; i < cols.Length; i++)
                dgv.Columns[i].HeaderText = cols[i];
        }
        SetupGrid(dgvBalance, new[] { "Cuenta", "Monto (S/)" });
        SetupGrid(dgvResultados, new[] { "Concepto", "Monto (S/)" });

        tabBalance.Controls.Add(dgvBalance);
        tabResultados.Controls.Add(dgvResultados);

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 500);
        Controls.Add(tabReportes);
        Controls.Add(pnlToolbar);
        Name = "frmReportes";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Reportes Financieros";
        Load += frmReportes_Load;

        ((System.ComponentModel.ISupportInitialize)dgvBalance).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
        tabReportes.ResumeLayout(false);
        ResumeLayout(false);
    }
}
