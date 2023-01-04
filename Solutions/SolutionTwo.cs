using System;
using System.Collections;
using System.Collections.Generic;

namespace Domino
{
    /// <summary>
    /// Unfinished. It will be O(N)
    /// </summary>
    class SolutionTwo : IDominoCircleMaker
    {
        public List<Stone> MakeCircleOfDomino(Stone[] stones)
        {
            Utilities.WriteDominoStones(stones);

            var matrix = new int[7, 7];

            foreach (var s in stones)
            {
                matrix[Math.Min(s.Left, s.Right), Math.Max(s.Left, s.Right)]++;
            }

            var initialMatrix = matrix.Clone() as int[,];
            Utilities.WriteDominoMatrix(initialMatrix);
            Console.WriteLine();

            for (var i = 0; i < 7; i++)
            {
                if (matrix[i, i] != 0)
                {
                    matrix[i, i] = 0;
                    var lengthToCover = 2;

                    for (var j = 0; j < 7; j++)
                    {
                        var min = Math.Min(j, i);
                        var max = Math.Max(j, i);
                        lengthToCover -= matrix[min, max];
                    }

                    if (lengthToCover > 0 || lengthToCover % 2 ==1)
                    {
                        return null;
                    }
                }
            }

            var totoalRest = 0;
            var firstI = 0;
            var firstJ = 0;
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = i; j < matrix.GetLength(0); j++)
                {
                    matrix[i, j] = matrix[i, j] % 2;
                    totoalRest += matrix[i, j];
                    if (matrix[i, j] > 0)
                    {
                        firstI = i;
                        firstJ = j;
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Total Rest is {0}", totoalRest );

            Utilities.WriteDominoMatrix(matrix);

            matrix[firstI, firstJ]--;

            var result = MakeCircleOfDominoRecursive(matrix, firstI, firstJ, totoalRest - 1);

            if (result != null)
            {
                result.Add(new Stone(firstI, firstJ));
            }

            return result;
        }


        private List<Stone> MakeCircleOfDominoRecursive(int[,] mat, int first, int next, int depth)
        {
            if (depth == 0)
            {
                return next == first ? new List<Stone>() : null;
            }

            for (var i = 0; i < mat.GetLength(0); i++)
            {
                var min = Math.Min(next, i);
                var max = Math.Max(next, i);
                if (mat[min, max] > 0)
                {
                    mat[min, max]--;
                    var result = MakeCircleOfDominoRecursive(mat, first, i, depth -1);
                    if (result != null)
                    {
                        result.Add(new Stone(next, i));
                        return result;
                    }
                    mat[min, max]++;
                }
            }

            return null;
        }
    }
}