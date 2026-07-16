using System.Diagnostics;
using ScottPlot;
using task14;

double a = -100;
double b = 100;
double targetTolerance = 1e-4;
double exactValue = 0.0;       // аналитическое значение интеграла sin(x) на [-100, 100]
int iterations = 10; // для усреднения

double[] stepsToCheck = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };

double step = 0;

foreach (double currentStep in stepsToCheck)
{
    double calculatedValue = DefiniteIntegral.SolveSingleThread(a, b, Math.Sin, currentStep);
    double currentError = Math.Abs(exactValue - calculatedValue);

    if (currentError <= targetTolerance && step == 0) 
        step = currentStep;
}

Stopwatch sw = new Stopwatch();
double singleThreadTotalMs = 0;
for (int i = 0; i < iterations; i++)
{
    sw.Restart();
    DefiniteIntegral.SolveSingleThread(a, b, Math.Sin, step);
    sw.Stop();
    singleThreadTotalMs += sw.Elapsed.TotalMilliseconds;
}
double avgSingleThreadMs = singleThreadTotalMs / iterations;


int[] threadCounts = new int[] { 1, 2, 4, 8, 16 };
double[] avgTimesMs = new double[threadCounts.Length];

for (int t = 0; t < threadCounts.Length; t++)
{
    double multiThreadTotalMs = 0;
    for (int i = 0; i < iterations; i++)
    {
        sw.Restart();
        DefiniteIntegral.Solve(a, b, Math.Sin, step, threadCounts[t]);
        sw.Stop();
        multiThreadTotalMs += sw.Elapsed.TotalMilliseconds;
    }
    avgTimesMs[t] = multiThreadTotalMs / iterations;
}

double minTimeMs = double.MaxValue;
int optimalThreadCount = 1;

for (int i = 0; i < threadCounts.Length; i++)
{
    double currentTimeMs = avgTimesMs[i];
    if (currentTimeMs < minTimeMs)
    {
        minTimeMs = currentTimeMs;
        optimalThreadCount = threadCounts[i];
    }
}

Console.WriteLine($"=== Результаты анализа ===");
Console.WriteLine($"Выбранный размер шага: {step}");
Console.WriteLine($"Среднее время однопоточной версии: {avgSingleThreadMs:F4} мс");
for (int i = 0; i < threadCounts.Length; i++)
{
    Console.WriteLine($"Потоков: {threadCounts[i]} | Среднее время: {avgTimesMs[i]:F4} мс");
}

Plot plt = new Plot();
plt.Title("Зависимость времени выполнения от количества потоков");

// OX - время вычисления, OY - количество потоков
double[] threadsData = Array.ConvertAll(threadCounts, x => (double)x);
var scatter = plt.Add.Scatter(threadsData, avgTimesMs);
scatter.LineWidth = 2;
scatter.MarkerSize = 8;

plt.XLabel("Время выполнения функции Solve (мс)");
plt.YLabel("Количество потоков");

plt.SavePng("plot.png", 600, 400);

string reportText = $@"
1. оптимальный размер шага: {step}
2. оптимальное количество потоков: {optimalThreadCount}
3. сравнение производительности:
   - время работы однопоточной версии: {avgSingleThreadMs:F4} мс
   - время работы оптимальной многопоточной версии: {minTimeMs:F4} мс
   - разница в процентах: {((avgSingleThreadMs - minTimeMs) / avgSingleThreadMs * 100):F1}%
";

File.WriteAllText("report.txt", reportText);
Console.WriteLine($"График сохранен в 'plot.png', отчет записан в 'report.txt'");

