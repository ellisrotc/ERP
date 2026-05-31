using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmReportes : Form
{
    public frmReportes()
    {
        InitializeComponent();
    }

    private void frmReportes_Load(object sender, EventArgs e)
    {
        for (int m = 1; m <= 12; m++)
            cboPeriodo.Items.Add($"{DateTime.Now.Year}-{m:D2}");
        cboPeriodo.SelectedItem = $"{DateTime.Now.Year}-{DateTime.Now.Month:D2}";
    }

    private async void btnGenerar_Click(object sender, EventArgs e)
    {
        var periodo = cboPeriodo.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(periodo)) return;

        btnGenerar.Enabled = false;
        try
        {
            if (tabReportes.SelectedIndex == 0)
            {
                var balance = await ApiClient.Instance.GetAsync<dynamic>($"api/reportes/balance?periodo={periodo}");
                var rows = new List<object[]>
                {
                    new object[] { "ACTIVO", "" },
                    new object[] { "  Cuentas por Cobrar", balance?.activo?.cuentasPorCobrar?.ToString("N2") ?? "0.00" },
                    new object[] { "PASIVO", "" },
                    new object[] { "  Cuentas por Pagar", balance?.pasivo?.cuentasPorPagar?.ToString("N2") ?? "0.00" },
                    new object[] { "  Planilla", balance?.pasivo?.planilla?.ToString("N2") ?? "0.00" },
                    new object[] { "PATRIMONIO NETO", balance?.patrimonio?.ToString("N2") ?? "0.00" }
                };
                dgvBalance.Rows.Clear();
                foreach (var r in rows) dgvBalance.Rows.Add(r);
            }
            else
            {
                var res = await ApiClient.Instance.GetAsync<dynamic>($"api/reportes/resultados?periodo={periodo}");
                var rows = new List<object[]>
                {
                    new object[] { "Ingresos", res?.ingresos?.ToString("N2") ?? "0.00" },
                    new object[] { "(-) Costo de Ventas", res?.costoVentas?.ToString("N2") ?? "0.00" },
                    new object[] { "Utilidad Bruta", res?.utilidadBruta?.ToString("N2") ?? "0.00" },
                    new object[] { "(-) Gastos Personal", res?.gastosPersonal?.ToString("N2") ?? "0.00" },
                    new object[] { "UTILIDAD NETA", res?.utilidadNeta?.ToString("N2") ?? "0.00" }
                };
                dgvResultados.Rows.Clear();
                foreach (var r in rows) dgvResultados.Rows.Add(r);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnGenerar.Enabled = true; }
    }
}
