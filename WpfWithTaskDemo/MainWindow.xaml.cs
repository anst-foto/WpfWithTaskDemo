using System.Windows;
using System.Windows.Threading;

namespace WpfWithTaskDemo;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        var result = int.TryParse(InputNumber.Text, out var number);
        if (!result) return;

        var progress1 = new Progress<int>();
        progress1.ProgressChanged += (_, i) => Progress1.Value = i;
        
        var progress2 = new Progress<int>();
        progress2.ProgressChanged += (_, i) => Progress2.Value = i;
        
        var tasks = new[]
        {
            new Task(() => DoWork(number, progress1, 1000)), 
            new Task(() => DoWork(number, progress2, 500))
        };

        ButtonStart.IsEnabled = false;
        foreach (var task in tasks)
        {
            task.Start();
        }

        Task.WhenAll(tasks).ContinueWith(_ => Dispatcher.Invoke(() => ButtonStart.IsEnabled = true));
    }

    private void DoWork(int number, IProgress<int> progress, int sleep)
    {
        for(var i = 0; i <= number; i++)
        {
            progress.Report(i);
            Thread.Sleep(sleep);
        }
    }
}