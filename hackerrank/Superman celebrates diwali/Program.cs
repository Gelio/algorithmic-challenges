using System;

namespace Superman_celebrates_diwali
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputData = InputData.Parse();
            var problem = new SupermanProblem(inputData);
            Console.WriteLine(problem.Solve());
        }
    }
}
