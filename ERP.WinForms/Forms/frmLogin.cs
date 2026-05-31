using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmLogin : Form
{
    public frmLogin()
    {
        InitializeComponent();
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
        {
            MessageBox.Show("Ingrese usuario y contraseña", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnLogin.Enabled = false;
        lblEstado.Text = "Autenticando...";

        try
        {
            var req = new LoginRequest(txtUsuario.Text.Trim(), txtPassword.Text);
            var resp = await ApiClient.Instance.PostAsync<LoginRequest, LoginResponse>("api/auth/login", req);

            if (resp is null) throw new Exception("Respuesta vacía del servidor");

            ApiClient.Instance.SetToken(resp.AccessToken);

            var dashboard = new frmDashboard(resp.Role, resp.NombreCompleto);
            dashboard.Show();
            Hide();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error de autenticación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblEstado.Text = string.Empty;
        }
        finally
        {
            btnLogin.Enabled = true;
        }
    }

    private void txtPassword_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
            btnLogin_Click(sender, e);
    }
}
