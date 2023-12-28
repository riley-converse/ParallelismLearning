namespace ParallelismLearning
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Parallel.For(0,10, i =>
            {
                Console.WriteLine($"Task {i} runnign on thread {Task.CurrentId}");
            });


            Parallel.Invoke(
                () => DoTask("Task 1"),
                () => DoTask("Task 2"),
                () => DoTask("Task 3")
             );
        }

        static void DoTask(string name)
        {
            Console.WriteLine($"{name} running on thread {Task.CurrentId}");
        }
    }
}
