using Avalonia.Controls;
using Avalonia.Interactivity;
using CpuSchedulerUI.Schedulers;
using System;
using System.Linq;

namespace CpuSchedulerUI;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void OnRunSRTF(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid number of processes.";
            return;
        }

        var processes = Utils.GenerateProcesses(count);
        var result = SrtfScheduler.Run(processes);
        ResultOutput.Text = Utils.FormatResult(result, "SRTF");
    }

    private void OnRunMLFQ(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid number of processes.";
            return;
        }

        var processes = Utils.GenerateProcesses(count);
        var result = MlfqScheduler.Run(processes);
        ResultOutput.Text = Utils.FormatResult(result, "MLFQ");
    }
}