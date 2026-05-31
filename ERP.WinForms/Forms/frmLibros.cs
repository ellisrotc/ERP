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

            var resultado = await ApiClient.Instance.GetAsync<dynamic>(endpoint);
            MessageBox.Show("Libro generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Archivo exportado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
