using System;

namespace Domino
{
    class Program
    {
        static void Main(string[] args)
        {
            // Tests.TestAllCases(new BruteForceSoulution());
            // Tests.TestAllCases(new SolutionOne());
            
            var stones = Tests.GetRandomArrayOfDominoSonesWhichCanBeOrdered(4000);
            var soulution = new SolutionOne();
            var result = soulution.MakeCircleOfDomino(stones);

            Console.WriteLine("Random set of domino:");
            foreach (var s in stones)
            {
                Console.Write("[{0}|{1}] ", s.Left, s.Right);
               
            }
            Console.WriteLine();

            if (result != null)
            {
                Console.WriteLine("Circle of domino is created:");
                foreach (var s in result)
                {
                    Console.Write("[{0}|{1}] ", s.Right, s.Left);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("It's imposible to create a circle of domino from this set.");
            }
        }
    }
}