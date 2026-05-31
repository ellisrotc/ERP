namespace ERP.WinForms.Forms;

partial class frmPlanillas
{
    private System.ComponentModel.IContainer components = null;
    private ComboBox cboPeriodo;
    private Button btnCalcular;
    private Button btnVerPdf;
    private DataGridView dgvPlanillas;
    private Panel pnlTop;
    private Label lblEstado;
    private Label lblPeriodoLbl;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        cboPeriodo = new ComboBox();
        btnCalcular = new Button();
        btnVerPdf = new Button();
        dgvPlanillas = new DataGridView();
        pnlTop = new Panel();
        lblEstado = new Label();
        lblPeriodoLbl = new Label();

        ((System.ComponentModel.ISupportInitialize)dgvPlanillas).BeginInit();
        pnlTop.SuspendLayout();
        SuspendLayout();

        // pnlTop
        pnlTop.Controls.Add(lblEstado);
        pnlTop.Controls.Add(btnVerPdf);
        pnlTop.Controls.Add(btnCalcular);
        pnlTop.Controls.Add(cboPeriodo);
        pnlTop.Controls.Add(lblPeriodoLbl);
        pnlTop.Dock = DockStyle.Top;
        pnlTop.Height = 55;
        pnlTop.BackColor = Color.FromArgb(245, 245, 245);

        lblPeriodoLbl.AutoSize = true;
        lblPeriodoLbl.Location = new Point(10, 18);
        lblPeriodoLbl.Text = "Período:";

        cboPeriodo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPeriodo.Location = new Point(70, 15);
        cboPeriodo.Size = new Size(100, 25);
        cboPeriodo.SelectedIndexChanged += cboPeriodo_SelectedIndexChanged;

        void SetBtn(Button b, string text, int x, Color color)
        {
            b.BackColor = color;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            b.Location = new Point(x, 13);
            b.Size = new Size(120, 30);
            b.Text = text;
        }
        SetBtn(btnCalcular, "Calcular Planilla", 185, Color.FromArgb(0, 120, 215));
        SetBtn(btnVerPdf, "Ver PDF", 315, Color.FromArgb(183, 28, 28));

        btnCalcular.Click += btnCalcular_Click;
        btnVerPdf.Click += btnVerPdf_Click;

        lblEstado.AutoSize = true;
        lblEstado.ForeColor = Color.DarkGreen;
        lblEstado.Location = new Point(450, 18);

        // dgvPlanillas
        dgvPlanillas.AllowUserToAddRows = false;
        dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvPlanillas.Dock = DockStyle.Fill;
        dgvPlanillas.ReadOnly = true;
        dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvPlanillas.MultiSelect = false;
        dgvPlanillas.BackgroundColor = Color.White;

        // frmPlanillas
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1000, 500);
        Controls.Add(dgvPlanillas);
        Controls.Add(pnlTop);
        Name = "frmPlanillas";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Planillas";
        Load += frmPlanillas_Load;

        ((System.ComponentModel.ISupportInitialize)dgvPlanillas).EndInit();
        pnlTop.ResumeLayout(false);
        ResumeLayout(false);
    }
}
