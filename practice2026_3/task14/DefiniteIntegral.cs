namespace task14
{
    public class DefiniteIntegral
    {
        public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
        {
            int totalSteps = (int) Math.Round((b - a) / step);
            step = (b - a) / totalSteps;

            int stepsPerThread = totalSteps / threadsNumber;
            double result = 0.0;

            using (CountdownEvent countdown = new CountdownEvent(threadsNumber))
            {
                for (int i = 0; i < threadsNumber; i++)
                {
                    int threadIndex = i;
                    ThreadPool.QueueUserWorkItem(_ =>
                    {
                        int startStep = threadIndex * stepsPerThread;
                        int endStep = (threadIndex == threadsNumber - 1) ? totalSteps : (threadIndex + 1) * stepsPerThread;

                        double localSum = 0.0;
                        double threadA = a + startStep * step;

                        double prevY = function(threadA);
                        for (int j = 0; j < (endStep - startStep); j++)
                        {
                            double nextX = threadA + (j + 1) * step;
                            double nextY = function(nextX);
                            localSum += (prevY + nextY);
                            prevY = nextY;
                        }
                        localSum = localSum * step / 2.0;

                        AddDouble(ref result, localSum);

                        countdown.Signal();
                    });
                }

                countdown.Wait();
            }
            return result;
        }

        public static double SolveSingleThread(double a, double b, Func<double, double> function, double step)
        {
            int totalSteps = (int) Math.Round((b - a) / step);
            step = (b - a) / totalSteps;
            double sum = 0.0;

            for (int i = 0; i < totalSteps; i++)
            {
                double x1 = a + i * step;
                double x2 = a + (i + 1) * step;
                sum += (function(x1) + function(x2)) * step / 2.0;
            }
            return sum;
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
