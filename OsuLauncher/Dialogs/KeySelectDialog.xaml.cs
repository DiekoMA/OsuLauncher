using System.Windows;
using System.Windows.Input;

namespace OsuLauncher.Dialogs;

public partial class KeySelectDialog
{
    public Key newKey;
    public KeySelectDialog()
    {
        InitializeComponent();
        this.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.Close();
        }
        else
        {
            this.Close();
            newKey = e.Key;
        }
    }
}