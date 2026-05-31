namespace ERP.WinForms.Forms;

public partial class frmUsuarios : Form
{
    public frmUsuarios()
    {
        InitializeComponent();
    }

    private void frmUsuarios_Load(object sender, EventArgs e)
    {
        lblInfo.Text = "Gestión de usuarios disponible para administradores vía Swagger/API.";
    }
}
