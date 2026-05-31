using ERP.WinForms.Forms;

namespace ERP.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new frmLogin());
    }
}
