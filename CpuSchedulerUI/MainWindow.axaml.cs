using Avalonia.Controls;
using Avalonia.Interactivity;
using CpuSchedulerUI.Schedulers;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace CpuSchedulerUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnRunSRTF(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid input.";
            return;
        }

        var output = await Task.Run(() =>
        {
            var processes = Utils.GenerateProcesses(count);
            var result = SrtfScheduler.Run(processes);
            return Utils.FormatResult(result, "SRTF");
        });

        ResultOutput.Text = output;
    }

    private async void OnRunMLFQ(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid input.";
            return;
        }

        var output = await Task.Run(() =>
        {
            var processes = Utils.GenerateProcesses(count);
            var result = SrtfScheduler.Run(processes);
            return Utils.FormatResult(result, "MLFQ");
        });

        ResultOutput.Text = output;
    }
}