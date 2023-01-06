using System;

namespace Domino
{
    class Program
    {
        static void Main(string[] args)
        {
            // ProveSolution.Prove();
            // Tests.TestAllCases(new BruteForceSoulution());
            // Tests.TestAllCases(new SolutionOne());
            // Tests.TestAllCases(new SolutionTwo());
            Tests.TestAllCases(new ConstantSolution());

            const int lengthOfSet = 9999999;
            var stones = Utilities.GetRandomArrayOfDominoStonesWhichCanBeOrdered(lengthOfSet);
            Console.WriteLine("Random set of domino:");
            Utilities.WriteDominoStones(stones);

            var soulution = new ConstantSolution();
            var stoneMatrix = Utilities.StonesToMatrix(stones);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            var result = soulution.MakeCircleOfDomino(stoneMatrix);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            if (result != null)
            {
                Console.WriteLine("Circle of domino is created:");
                Utilities.WriteDominoStones(result);
            }
            else
            {
                Console.WriteLine("It's imposible to create a circle of domino from this set.");
            }

            Console.WriteLine("Execution time for set with {0} stones is {1} ms", lengthOfSet, elapsedMs);
            
        }
    }
}