using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmEmpleados : Form
{
    public frmEmpleados()
    {
        InitializeComponent();
    }

    private async void frmEmpleados_Load(object sender, EventArgs e)
    {
        await CargarEmpleados();
    }

    private async Task CargarEmpleados()
    {
        try
        {
            var lista = await ApiClient.Instance.GetAsync<List<EmpleadoDto>>("api/empleados?page=1&pageSize=100");
            dgvEmpleados.DataSource = lista;
            if (dgvEmpleados.Columns.Count > 0)
            {
                dgvEmpleados.Columns["Id"].HeaderText = "ID";
                dgvEmpleados.Columns["Nombre"].HeaderText = "Nombre";
                dgvEmpleados.Columns["Apellido"].HeaderText = "Apellido";
                dgvEmpleados.Columns["Dni"].HeaderText = "DNI";
                dgvEmpleados.Columns["Cargo"].HeaderText = "Cargo";
                dgvEmpleados.Columns["SalarioBase"].HeaderText = "Salario Base";
                dgvEmpleados.Columns["TipoDescuento"].HeaderText = "Tipo Descuento";
                dgvEmpleados.Columns["Activo"].HeaderText = "Activo";
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al cargar empleados: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void btnNuevo_Click(object sender, EventArgs e)
    {
        using var form = new frmEmpleadoDetalle();
        if (form.ShowDialog() == DialogResult.OK)
            await CargarEmpleados();
    }

    private async void btnEditar_Click(object sender, EventArgs e)
    {
        if (dgvEmpleados.CurrentRow?.DataBoundItem is not EmpleadoDto emp) return;

        using var form = new frmEmpleadoDetalle(emp);
        if (form.ShowDialog() == DialogResult.OK)
            await CargarEmpleados();
    }

    private async void btnDesactivar_Click(object sender, EventArgs e)
    {
        if (dgvEmpleados.CurrentRow?.DataBoundItem is not EmpleadoDto emp) return;
        if (MessageBox.Show($"¿Desactivar a {emp.Nombre} {emp.Apellido}?", "Confirmar",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        try
        {
            await ApiClient.Instance.DeleteAsync($"api/empleados/{emp.Id}");
            await CargarEmpleados();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
