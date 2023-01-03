using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    public static class Tests
    {
        public static Stone[] GetRandomArrayOfDominoSones(int count)
        {
            var rand = new Random();
            var result = new Stone[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = new Stone(rand.Next(0, 6), rand.Next(0, 6));
            }
            return result;
        }

        public static void TestAllCases(IDominoCircleMaker dominoCircleMaker)
        {
            Test(dominoCircleMaker, new Stone[] { new Stone(4, 4) }, true);
            Test(dominoCircleMaker, new Stone[] { new Stone(1, 2), new Stone(2, 1) }, true);
            Test(dominoCircleMaker, new Stone[] { new Stone(1, 1), new Stone(1, 1), new Stone(1, 1) }, true);

            Test(dominoCircleMaker, new Stone[] { new Stone(1, 2), new Stone(2, 1), new Stone(3, 1) }, false);

            Test(dominoCircleMaker, new Stone[] { new Stone(3, 3), new Stone(2, 2) }, false);
        }

        static void Test(IDominoCircleMaker circleMaker, Stone[] stones, bool expectedResult)
        {
            var result = circleMaker.MakeCircleOfDomino(stones);
            bool isResultNull = result != null;
            if (isResultNull == expectedResult)
            {
                Console.WriteLine("OK result {0} == {1} expected result", isResultNull, expectedResult);
            }
            else
            {
                Console.WriteLine("ERROR result {0} != {1} expected result", isResultNull, expectedResult);
            }
            if (isResultNull)
            {
                if (result.Count != stones.Length)
                {
                    Console.WriteLine("ERROR result length {0} != {1} expected result length", result.Count, stones.Length);
                }

                foreach (var s in result)
                {
                    Console.Write(" [{0},{1}] ", s.Right, s.Left);
                }
                Console.WriteLine();
            }
            Console.WriteLine("==================================================================");
        }
    }
}
