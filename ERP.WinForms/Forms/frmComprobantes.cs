using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmComprobantes : Form
{
    public frmComprobantes()
    {
        InitializeComponent();
    }

    private async void frmComprobantes_Load(object sender, EventArgs e)
    {
        cboTipo.Items.Add(new { Text = "Factura", Value = 1 });
        cboTipo.Items.Add(new { Text = "Boleta", Value = 2 });
        cboTipo.Items.Add(new { Text = "Nota Crédito", Value = 3 });
        cboTipo.DisplayMember = "Text";
        cboTipo.ValueMember = "Value";
        cboTipo.SelectedIndex = 0;
        await CargarComprobantes();
    }

    private async Task CargarComprobantes()
    {
        try
        {
            var lista = await ApiClient.Instance.GetAsync<List<ComprobanteDto>>("api/comprobantes");
            dgvComprobantes.DataSource = lista;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void nudMonto_ValueChanged(object sender, EventArgs e)
    {
        var igv = Math.Round(nudMonto.Value / 1.18m * 0.18m, 2);
        lblIgvValor.Text = $"S/ {igv:N2}";
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        var ruc = txtRuc.Text.Trim();
        if (ruc.Length != 11 || !ruc.All(char.IsDigit))
        {
            MessageBox.Show("El RUC debe tener exactamente 11 dígitos numéricos.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (nudMonto.Value <= 0)
        {
            MessageBox.Show("El monto debe ser mayor a cero.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var tipo = (dynamic)cboTipo.SelectedItem!;
            var dto = new ComprobanteCreateDto(
                (int)tipo.Value,
                txtNumero.Text.Trim(),
                txtSerie.Text.Trim(),
                ruc,
                txtRazonSocial.Text.Trim(),
                dtpFecha.Value,
                nudMonto.Value);

            await ApiClient.Instance.PostAsync<ComprobanteCreateDto, ComprobanteDto>("api/comprobantes", dto);
            LimpiarFormulario();
            await CargarComprobantes();
            MessageBox.Show("Comprobante guardado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimpiarFormulario()
    {
        txtNumero.Text = string.Empty;
        txtSerie.Text = string.Empty;
        txtRuc.Text = string.Empty;
        txtRazonSocial.Text = string.Empty;
        nudMonto.Value = 0;
        lblIgvValor.Text = "S/ 0.00";
    }
}
