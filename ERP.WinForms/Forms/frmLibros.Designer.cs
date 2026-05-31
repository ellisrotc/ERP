namespace ERP.WinForms.Forms;

partial class frmLibros
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cboPeriodo;
    private Button btnGenerar, btnExcel;
    private TabControl tabLibros;
    private TabPage tabVentas, tabCompras;
    private DataGridView dgvVentas, dgvCompras;
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
        btnExcel = new Button();
        tabLibros = new TabControl();
        tabVentas = new TabPage();
        tabCompras = new TabPage();
        dgvVentas = new DataGridView();
        dgvCompras = new DataGridView();
        pnlToolbar = new Panel();

        ((System.ComponentModel.ISupportInitialize)dgvVentas).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvCompras).BeginInit();
        tabLibros.SuspendLayout();
        pnlToolbar.SuspendLayout();
        SuspendLayout();

        // pnlToolbar
        pnlToolbar.BackColor = Color.FromArgb(245, 245, 245);
        pnlToolbar.Dock = DockStyle.Top;
        pnlToolbar.Height = 50;

        var lblPer = new Label { Text = "Período:", Location = new Point(10, 16), AutoSize = true };
        cboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriodo.Location = new Point(70, 13);
        cboPeriodo.Size = new Size(100, 25);

        void SetBtn(Button b, string text, int x, Color color)
        {
            b.BackColor = color;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            b.Location = new Point(x, 11);
            b.Size = new Size(120, 30);
            b.Text = text;
        }
        SetBtn(btnGenerar, "Generar Libro", 185, Color.FromArgb(0, 120, 215));
        SetBtn(btnExcel, "Exportar Excel", 315, Color.FromArgb(56, 142, 60));

        btnGenerar.Click += btnGenerar_Click;
        btnExcel.Click += btnExcel_Click;

        pnlToolbar.Controls.AddRange(new Control[] { lblPer, cboPeriodo, btnGenerar, btnExcel });

        // TabControl
        tabLibros.Dock = DockStyle.Fill;
        tabLibros.Controls.Add(tabVentas);
        tabLibros.Controls.Add(tabCompras);

        tabVentas.Text = "Libro Ventas";
        tabVentas.Controls.Add(dgvVentas);
        tabCompras.Text = "Libro Compras";
        tabCompras.Controls.Add(dgvCompras);

        foreach (var dgv in new[] { dgvVentas, dgvCompras })
        {
            dgv.AllowUserToAddRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Dock = DockStyle.Fill;
            dgv.ReadOnly = true;
            dgv.BackgroundColor = Color.White;
        }

        // frmLibros
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 500);
        Controls.Add(tabLibros);
        Controls.Add(pnlToolbar);
        Name = "frmLibros";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Libros Contables";
        Load += frmLibros_Load;

        ((System.ComponentModel.ISupportInitialize)dgvVentas).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvCompras).EndInit();
        tabLibros.ResumeLayout(false);
        pnlToolbar.ResumeLayout(false);
        ResumeLayout(false);
    }
}
