using Avalonia.Controls;
using Avalonia.Interactivity;
using CpuSchedulerUI.Schedulers;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CpuSchedulerUI;

public partial class MainWindow : Window
{
    public ISeries[] ChartSeries { get; set; } = Array.Empty<ISeries>();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private async void OnRunSRTF(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid input.";
            return;
        }

        var (output, avgWt, avgTat) = await Task.Run(() =>
        {
            var processes = Utils.FormatResultWithMetrics(SrtfScheduler.Run(Utils.GenerateProcesses(count)), "SRTF");
            return processes;
        });

        ResultOutput.Text = output;
        ChartSeries = GenerateChartSeries(avgWt, avgTat);
        DataContext = null;
        DataContext = this;
    }

    private async void OnRunMLFQ(object? sender, RoutedEventArgs e)
    {
        if (!int.TryParse(ProcessInput.Text, out int count))
        {
            ResultOutput.Text = "Invalid input.";
            return;
        }

        var (output, avgWt, avgTat) = await Task.Run(() =>
        {
            var processes = Utils.FormatResultWithMetrics(MlfqScheduler.Run(Utils.GenerateProcesses(count)), "MLFQ");
            return processes;
        });

        ResultOutput.Text = output;
        ChartSeries = GenerateChartSeries(avgWt, avgTat);
        DataContext = null;
        DataContext = this;
    }

    private ISeries[] GenerateChartSeries(double avgWt, double avgTat)
    {
        return new ISeries[]
        {
            new ColumnSeries<double>
            {
                Values = new double[] { avgWt, avgTat },
                Name = "Performance",
                DataLabelsSize = 16
            }
        };
    }
}