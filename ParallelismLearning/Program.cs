using System.Diagnostics;

namespace ParallelismLearning
{
    internal class Program
    {
        public delegate void TestDelegate(char ch);

        public delegate void TestDelegate2();
        

        static void Main(string[] args)
        {
            /*Parallel.For(0,10, i =>
            {
                Console.WriteLine($"Task {i} running on thread {Task.CurrentId}");
            });*/

            
            // potential patterns
            /*Parallel.Invoke(
                () => DoTask('c'),
                () => DoTask('b'),
                () => DoTask('z')
                );*/

            PatternHandler pattern = new PatternHandler('i','c','z');

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            List<Task> tasks = new List<Task>();

            TaskFactory factory = new TaskFactory(token);
            Console.WriteLine(pattern.Length);
            Stopwatch global = new Stopwatch();
            global.Start();
            for (int i = 0; i < pattern.Length; i++)
            {
                tasks.Add(factory.StartNew(() => DoTask(pattern.GetPattern().Item2,cts, global),token));
            }

            Task.WaitAll(tasks.ToArray());
            global.Stop();
            Console.WriteLine("Pattern Searched completed in [GLOBAL] "+global.ElapsedMilliseconds);
        }


        static void DoTask(char ch, CancellationTokenSource cts, Stopwatch global)
        {
            string input =
                "Hello world! This is test text figure out which task finds out the first pattern using parallelism z.";
            int index = 0;

            Console.WriteLine($"[Task {Task.CurrentId}]:\tI've been called to duty. Starting timer.");

            Stopwatch sw = Stopwatch.StartNew();
            foreach (char c in input)
            {
                if (cts.IsCancellationRequested)
                {
                    Console.WriteLine($"[Task {Task.CurrentId}]:\tI can't run, my token is cancelled");
                    sw.Stop();
                    Console.WriteLine($"[Task {Task.CurrentId}]:\tI was able to run for {sw.ElapsedMilliseconds} milliseconds."+
                                      $"The global time is {global.ElapsedMilliseconds} milliseconds");
                    return;
                }
                else if (c == ch) 
                {
                    Console.WriteLine($"[Task {Task.CurrentId}]:\tFound {ch} at index {index}.");
                    sw.Stop();
                    Console.WriteLine($"[Task {Task.CurrentId}]:\tI found this in {sw.ElapsedMilliseconds} milliseconds. " +
                                      $"The global time is {global.ElapsedMilliseconds} milliseconds");
                    cts.Cancel();
                    return;
                }
                Console.WriteLine($"[Task {Task.CurrentId}]:\tSearching for {ch}, I'm at index {index} and the global time is {global.ElapsedMilliseconds} milliseconds.");
                index++;
            }
        }
    }
}
