using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmPlanillas : Form
{
    public frmPlanillas()
    {
        InitializeComponent();
    }

    private void frmPlanillas_Load(object sender, EventArgs e)
    {
        for (int m = 1; m <= 12; m++)
            cboPeriodo.Items.Add($"{DateTime.Now.Year}-{m:D2}");
        cboPeriodo.SelectedItem = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}";
    }

    private async void btnCalcular_Click(object sender, EventArgs e)
    {
        var periodo = cboPeriodo.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(periodo)) return;

        btnCalcular.Enabled = false;
        lblEstado.Text = "Calculando planilla...";
        try
        {
            await ApiClient.Instance.PostAsync<object, object>($"api/planillas/calcular?periodo={periodo}", new { });
            await CargarPlanillas(periodo);
            lblEstado.Text = "Cálculo completado.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblEstado.Text = string.Empty;
        }
        finally { btnCalcular.Enabled = true; }
    }

    private async void cboPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        var periodo = cboPeriodo.SelectedItem?.ToString();
        if (!string.IsNullOrEmpty(periodo))
            await CargarPlanillas(periodo);
    }

    private async Task CargarPlanillas(string periodo)
    {
        try
        {
            var lista = await ApiClient.Instance.GetAsync<List<PlanillaResultDto>>($"api/planillas?periodo={periodo}");
            dgvPlanillas.DataSource = lista;
        }
        catch { /* sin datos para este período */ }
    }

    private async void btnVerPdf_Click(object sender, EventArgs e)
    {
        if (dgvPlanillas.CurrentRow?.DataBoundItem is not PlanillaResultDto plan) return;

        try
        {
            var pdfBytes = await ApiClient.Instance.GetBytesAsync($"api/planillas/{plan.IdPlanilla}/pdf");
            var tmpPath = Path.Combine(Path.GetTempPath(), $"planilla_{plan.IdPlanilla}.pdf");
            await File.WriteAllBytesAsync(tmpPath, pdfBytes);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tmpPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar PDF: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
