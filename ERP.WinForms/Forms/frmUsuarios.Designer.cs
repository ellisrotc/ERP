namespace ERP.WinForms.Forms;

partial class frmUsuarios
{
    private System.ComponentModel.IContainer components = null;
    private Label lblInfo;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        lblInfo = new Label();
        SuspendLayout();

        lblInfo.AutoSize = false;
        lblInfo.Dock = DockStyle.Fill;
        lblInfo.Font = new Font("Segoe UI", 11F);
        lblInfo.TextAlign = ContentAlignment.MiddleCenter;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 200);
        Controls.Add(lblInfo);
        Name = "frmUsuarios";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Gestión de Usuarios";
        Load += frmUsuarios_Load;
        ResumeLayout(false);
    }
}
