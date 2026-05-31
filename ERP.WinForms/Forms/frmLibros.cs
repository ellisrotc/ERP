using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public partial class frmLibros : Form
{
    public frmLibros()
    {
        InitializeComponent();
    }

    private void frmLibros_Load(object sender, EventArgs e)
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
            string endpoint = tabLibros.SelectedIndex == 0
                ? $"api/libros/ventas?periodo={periodo}"
                : $"api/libros/compras?periodo={periodo}";

            var resultado = await ApiClient.Instance.GetAsync<LibroResponseDto>(endpoint);

            if (resultado is null)
            {
                MessageBox.Show("No se recibió respuesta del servidor.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Poblar el grid correcto según el tab activo
            var dgv = tabLibros.SelectedIndex == 0 ? dgvVentas : dgvCompras;
            dgv.DataSource = resultado.Detalles;

            // Renombrar columnas para que se vean bonito
            if (dgv.Columns.Count > 0)
            {
                if (dgv.Columns.Contains("IdComprobante"))  dgv.Columns["IdComprobante"].Visible = false;
                if (dgv.Columns.Contains("Numero"))         dgv.Columns["Numero"].HeaderText = "N° Comprobante";
                if (dgv.Columns.Contains("RazonSocial"))    dgv.Columns["RazonSocial"].HeaderText = "Razón Social";
                if (dgv.Columns.Contains("Ruc"))            dgv.Columns["Ruc"].HeaderText = "RUC";
                if (dgv.Columns.Contains("BaseImponible"))  dgv.Columns["BaseImponible"].HeaderText = "Base Imponible";
                if (dgv.Columns.Contains("Igv"))            dgv.Columns["Igv"].HeaderText = "IGV (18%)";
                if (dgv.Columns.Contains("Total"))          dgv.Columns["Total"].HeaderText = "Total";
            }

            int count = resultado.Detalles?.Count ?? 0;
            decimal totalMonto = resultado.Detalles?.Sum(d => d.Total) ?? 0;
            decimal totalBase  = resultado.Detalles?.Sum(d => d.BaseImponible) ?? 0;
            decimal totalIgv   = resultado.Detalles?.Sum(d => d.Igv) ?? 0;

            string tipoLibro = tabLibros.SelectedIndex == 0 ? "VENTAS" : "COMPRAS";

            MessageBox.Show(
                $"Libro de {tipoLibro} generado — período {periodo}\n\n" +
                $"Comprobantes:     {count}\n" +
                $"Base imponible:   S/ {totalBase:N2}\n" +
                $"IGV total:        S/ {totalIgv:N2}\n" +
                $"TOTAL:            S/ {totalMonto:N2}",
                "Libro generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al generar libro: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { btnGenerar.Enabled = true; }
    }

    private async void btnExcel_Click(object sender, EventArgs e)
    {
        var periodo = cboPeriodo.SelectedItem?.ToString();
        if (string.IsNullOrEmpty(periodo)) return;

        try
        {
            var bytes = await ApiClient.Instance.GetBytesAsync($"api/libros/ventas/{periodo}/excel");
            using var dlg = new SaveFileDialog
            {
                Filter = "Excel|*.xlsx",
                FileName = $"libro_ventas_{periodo}.xlsx"
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                await File.WriteAllBytesAsync(dlg.FileName, bytes);
                MessageBox.Show("Archivo Excel exportado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al exportar Excel: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
