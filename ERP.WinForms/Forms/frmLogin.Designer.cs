namespace ERP.WinForms.Forms;

partial class frmLogin
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitulo;
    private Label lblSubtitulo;
    private Label lblUsuario;
    private Label lblPasswordLbl;
    private TextBox txtUsuario;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblEstado;
    private Panel pnlHeader;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        lblUsuario = new Label();
        lblPasswordLbl = new Label();
        txtUsuario = new TextBox();
        txtPassword = new TextBox();
        btnLogin = new Button();
        lblEstado = new Label();
        pnlHeader = new Panel();

        pnlHeader.SuspendLayout();
        SuspendLayout();

        // pnlHeader
        pnlHeader.BackColor = Color.FromArgb(0, 70, 127);
        pnlHeader.Controls.Add(lblTitulo);
        pnlHeader.Controls.Add(lblSubtitulo);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 90;

        // lblTitulo
        lblTitulo.AutoSize = false;
        lblTitulo.Dock = DockStyle.None;
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.Text = "UNIVERSIDAD NACIONAL DE JULIACA";
        lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
        lblTitulo.Size = new Size(400, 40);
        lblTitulo.Location = new Point(0, 10);

        // lblSubtitulo
        lblSubtitulo.AutoSize = false;
        lblSubtitulo.Dock = DockStyle.None;
        lblSubtitulo.ForeColor = Color.LightCyan;
        lblSubtitulo.Font = new Font("Segoe UI", 9F);
        lblSubtitulo.Text = "Sistema ERP – Gestión Financiera y Contable";
        lblSubtitulo.TextAlign = ContentAlignment.MiddleCenter;
        lblSubtitulo.Size = new Size(400, 30);
        lblSubtitulo.Location = new Point(0, 52);

        // lblUsuario
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 10F);
        lblUsuario.Location = new Point(60, 120);
        lblUsuario.Text = "Usuario:";

        // txtUsuario
        txtUsuario.Font = new Font("Segoe UI", 11F);
        txtUsuario.Location = new Point(60, 145);
        txtUsuario.Size = new Size(280, 30);

        // lblPasswordLbl
        lblPasswordLbl.AutoSize = true;
        lblPasswordLbl.Font = new Font("Segoe UI", 10F);
        lblPasswordLbl.Location = new Point(60, 185);
        lblPasswordLbl.Text = "Contraseña:";

        // txtPassword
        txtPassword.Font = new Font("Segoe UI", 11F);
        txtPassword.Location = new Point(60, 210);
        txtPassword.Size = new Size(280, 30);
        txtPassword.PasswordChar = '●';
        txtPassword.KeyDown += txtPassword_KeyDown;

        // btnLogin
        btnLogin.BackColor = Color.FromArgb(0, 120, 215);
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.FlatAppearance.BorderSize = 0;
        btnLogin.ForeColor = Color.White;
        btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnLogin.Location = new Point(60, 265);
        btnLogin.Size = new Size(280, 42);
        btnLogin.Text = "INGRESAR";
        btnLogin.Click += btnLogin_Click;

        // lblEstado
        lblEstado.AutoSize = true;
        lblEstado.Font = new Font("Segoe UI", 9F);
        lblEstado.ForeColor = Color.Gray;
        lblEstado.Location = new Point(60, 320);

        // frmLogin
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.WhiteSmoke;
        ClientSize = new Size(400, 360);
        Controls.Add(pnlHeader);
        Controls.Add(lblUsuario);
        Controls.Add(txtUsuario);
        Controls.Add(lblPasswordLbl);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Controls.Add(lblEstado);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "frmLogin";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "ERP UNAJ – Iniciar Sesión";

        pnlHeader.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }
}
