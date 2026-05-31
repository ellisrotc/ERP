using ERP.Shared.DTOs;
using ERP.WinForms.Services;

namespace ERP.WinForms.Forms;

public class frmEmpleadoDetalle : Form
{
    private readonly EmpleadoDto? _empleado;
    private TextBox txtNombre = null!, txtApellido = null!, txtDni = null!, txtCargo = null!;
    private NumericUpDown nudSalario = null!;
    private ComboBox cboTipoDescuento = null!;
    private Button btnGuardar = null!, btnCancelar = null!;

    public frmEmpleadoDetalle(EmpleadoDto? empleado = null)
    {
        _empleado = empleado;
        BuildUI();
        if (_empleado != null) PreCargarDatos();
    }

    private void BuildUI()
    {
        Text = _empleado == null ? "Nuevo Empleado" : "Editar Empleado";
        ClientSize = new Size(380, 360);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        int y = 20;
        AddField("Nombre:", ref txtNombre, ref y);
        AddField("Apellido:", ref txtApellido, ref y);
        AddField("DNI (8 dígitos):", ref txtDni, ref y);
        AddField("Cargo:", ref txtCargo, ref y);

        Controls.Add(new Label { Text = "Salario Base:", Location = new Point(20, y), AutoSize = true });
        y += 22;
        nudSalario = new NumericUpDown
        {
            Location = new Point(20, y), Size = new Size(340, 25),
            DecimalPlaces = 2, Minimum = 0, Maximum = 100000, Value = 1000
        };
        Controls.Add(nudSalario);
        y += 35;

        Controls.Add(new Label { Text = "Tipo Descuento:", Location = new Point(20, y), AutoSize = true });
        y += 22;
        cboTipoDescuento = new ComboBox
        {
            Location = new Point(20, y), Size = new Size(340, 25),
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cboTipoDescuento.Items.Add(new { Text = "AFP (10%)", Value = 1 });
        cboTipoDescuento.Items.Add(new { Text = "ONP (13%)", Value = 2 });
        cboTipoDescuento.DisplayMember = "Text";
        cboTipoDescuento.ValueMember = "Value";
        cboTipoDescuento.SelectedIndex = 0;
        Controls.Add(cboTipoDescuento);
        y += 45;

        btnGuardar = new Button
        {
            Text = "Guardar", Location = new Point(100, y), Size = new Size(80, 30),
            BackColor = Color.FromArgb(0, 150, 136), ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.Click += btnGuardar_Click;

        btnCancelar = new Button
        {
            Text = "Cancelar", Location = new Point(195, y), Size = new Size(80, 30),
            BackColor = Color.Gray, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, DialogResult = DialogResult.Cancel
        };
        btnCancelar.FlatAppearance.BorderSize = 0;

        Controls.Add(btnGuardar);
        Controls.Add(btnCancelar);
    }

    private void AddField(string label, ref TextBox txt, ref int y)
    {
        Controls.Add(new Label { Text = label, Location = new Point(20, y), AutoSize = true });
        y += 22;
        txt = new TextBox { Location = new Point(20, y), Size = new Size(340, 25) };
        Controls.Add(txt);
        y += 35;
    }

    private void PreCargarDatos()
    {
        txtNombre.Text = _empleado!.Nombre;
        txtApellido.Text = _empleado.Apellido;
        txtDni.Text = _empleado.Dni;
        txtCargo.Text = _empleado.Cargo;
        nudSalario.Value = _empleado.SalarioBase;
    }

    private async void btnGuardar_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text)
            || txtDni.Text.Length != 8 || !txtDni.Text.All(char.IsDigit))
        {
            MessageBox.Show("Verifique los datos. El DNI debe tener 8 dígitos numéricos.", "Validación",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var tipoSeleccionado = (dynamic)cboTipoDescuento.SelectedItem!;

            if (_empleado == null)
            {
                var dto = new EmpleadoCreateDto(
                    txtNombre.Text.Trim(), txtApellido.Text.Trim(),
                    txtDni.Text.Trim(), txtCargo.Text.Trim(),
                    nudSalario.Value, (int)tipoSeleccionado.Value);
                await ApiClient.Instance.PostAsync<EmpleadoCreateDto, EmpleadoDto>("api/empleados", dto);
            }
            else
            {
                var dto = new EmpleadoUpdateDto(
                    txtNombre.Text.Trim(), txtApellido.Text.Trim(),
                    txtCargo.Text.Trim(), nudSalario.Value,
                    (int)tipoSeleccionado.Value);
                await ApiClient.Instance.PutAsync<EmpleadoUpdateDto, EmpleadoDto>(
                    $"api/empleados/{_empleado.Id}", dto);
            }

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
