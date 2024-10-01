using System.Windows;
using System.Windows.Threading;

namespace WpfWithTaskDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var result = int.TryParse(InputNumber.Text, out var number);
        if (!result) return;

        var progress1 = new Progress<int>();
        progress1.ProgressChanged += (_, i) => Progress1.Value = i;
        
        var progress2 = new Progress<int>();
        progress2.ProgressChanged += (_, i) => Progress2.Value = i;

        ButtonStart.IsEnabled = false;
        var task1 = DoWorkAsync(number, progress1, 500);
        var task2 = DoWorkAsync(number, progress2, 250);
        await Task.WhenAll(task1, task2);
        ButtonStart.IsEnabled = true;
    }

    private async Task DoWorkAsync(int number, IProgress<int> progress, int sleep)
    {
        for(var i = 0; i <= number; i++)
        {
            progress.Report(i);
            await Task.Delay(sleep);
        }
    }
}