using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino
{
    static class Utilities
    {
        public static void WriteDominoStones(IEnumerable<Stone> stones)
        {
            foreach (var s in stones)
            {
                Console.Write("[{0}|{1}] ", s.Right, s.Left);
            }
            Console.WriteLine();
        }

        public static void WriteDominoMatrix(int[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }

        public static Stone[] GetRandomArrayOfDominoStones(int count)
        {
            var rand = new Random();
            var result = new Stone[count];
            for (var i = 0; i < count; i++)
            {
                result[i] = new Stone(rand.Next(0, 7), rand.Next(0, 7));
            }
            return result;
        }
        public static Stone[] GetRandomArrayOfDominoStonesWhichCanBeOrdered(int count)
        {
            var rand = new Random();
            var result = new Stone[count];
            var first = rand.Next(0, 7);
            var prev = first;
            for (var i = 0; i < count - 1; i++)
            {
                result[i] = new Stone(prev, rand.Next(0, 7));
                prev = result[i].Right;
            }
            result[count - 1] = new Stone(prev, first);


            return result.OrderBy(x => rand.Next()).ToArray();
        }
    }
}
