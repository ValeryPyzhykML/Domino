using System;
using System.Collections.Generic;

namespace Domino
{
    /// <summary>
    /// Still very complex solution O(7^N)
    /// </summary>
    class SolutionOne : IDominoCircleMaker
    {
        public ICollection<Stone> MakeCircleOfDomino(Stone[] stones)
        {
            var matrix = new int[7, 7];
            var coutnsToChech = new int[7];
            foreach (var s in stones)
            {
                matrix[Math.Min(s.Left, s.Right), Math.Max(s.Left, s.Right)]++;
                coutnsToChech[s.Left] += 1;
                coutnsToChech[s.Right] += 1;
            }

            foreach (var c in coutnsToChech)
            {
                if (c % 2 == 1)
                {
                    return null;
                }
            }

            matrix[Math.Min(stones[0].Left, stones[0].Right), Math.Max(stones[0].Left, stones[0].Right)]--;

            var result = MakeCircleOfDominoRecursive(matrix, stones[0].Left, stones[0].Right, stones.Length - 1);

            if (result != null)
            {
                result.Add(stones[0]);
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
                    var result = MakeCircleOfDominoRecursive(mat, first, i, depth - 1);
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