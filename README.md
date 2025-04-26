# CpuSchedulerUI

## Overview
CpuSchedulerUI is a cross-platform CPU Scheduling Simulator developed in C# using Avalonia UI (targeting .NET 8).  
It implements two major CPU scheduling algorithms:  
- **Shortest Remaining Time First (SRTF)**
- **Multi-Level Feedback Queue (MLFQ)**

The project features an intuitive desktop interface, live performance charting (AWT/ATT comparison), and real-time result display for each scheduling run.

## Features
- Random generation of process workloads.
- Realistic CPU utilization tracking including idle times.
- Comparison metrics:  
  - Average Waiting Time (AWT)
  - Average Turnaround Time (ATT)
  - CPU Utilization (%)
  - Throughput (Processes per second)
- Live bar chart using LiveCharts2/Avalonia.
- Edge case handling (identical arrival/burst times, skewed priorities).

## How to Build & Run
1. **Requirements:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
   - AvaloniaUI 11.3 or later
   - Rider IDE / Visual Studio / VSCode
2. **Clone the Repository:**
   ```bash
   git clone https://github.com/CGHSquad/CpuSchedulerUI.git
   cd CpuSchedulerUI
## License

This project is licensed under the terms of the [MIT License](https://choosealicense.com/licenses/mit/).
