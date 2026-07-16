namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
        {
            int totalSteps = (int)((b - a) / step);
            step = (b - a) / totalSteps;

            int stepsPerThread = totalSteps / threadsnumber;
            double result = 0.0;

            using (Barrier barrier = new Barrier(threadsnumber + 1))
            {
                for (int i = 0; i < threadsnumber; i++)
                {
                    int threadIndex = i;
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        int startStep = threadIndex * stepsPerThread;
                        int endStep = (threadIndex == threadsnumber - 1) ? totalSteps : (threadIndex + 1) * stepsPerThread;

                        // Вычисляем физические границы отрезка для текущего потока заранее
                        double threadA = a + startStep * step;
                        double localSum = 0.0;

                        for (int j = 0; j < (endStep - startStep); j++)
                        {
                            // Считаем точки относительно начала отрезка потока, это снижает погрешность double
                            double x1 = threadA + j * step;
                            double x2 = threadA + (j + 1) * step;

                            localSum += (function(x1) + function(x2)) * step / 2.0;
                        }

                        AddDouble(ref result, localSum);
                        barrier.SignalAndWait();
                    });

                }

                barrier.SignalAndWait();
            }

            return result;
        }

        private static void AddDouble(ref double result, double value)
        {
            double currentValue = Volatile.Read(ref result);
            while (true)
            {
                double newValue = currentValue + value;
                double actualValue = Interlocked.CompareExchange(ref result, newValue, currentValue);

                if (actualValue == currentValue) return;

                currentValue = actualValue;
            }
        }
    }
}
