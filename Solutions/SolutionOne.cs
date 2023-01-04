using System;
using System.Collections.Generic;

namespace Domino
{
    /// <summary>
    /// Still very complex solution O(6^N)
    /// </summary>
    class SolutionOne : IDominoCircleMaker
    {
        public List<Stone> MakeCircleOfDomino(Stone[] stones)
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

            var result = MakeCircleOfDominoRecursive(matrix, stones[0].Left, stones[0].Right, stones.Length, 1);

            if (result != null)
            {
                result.Add(stones[0]);
            }

            return result;
        }

        public List<Stone> Test(int length)
        {
            var matrix = new int[7, 7];
            var l = 0;
            for (var i = 0; i <= length; i++)
            {
                for (var j = 0; j <= length; j++)
                {
                    var min = Math.Min(j, i);
                    var max = Math.Max(j, i);
                    matrix[min, max] = 1;
                    l++;

                }
            }

            matrix[0,0]--;

            var result = MakeCircleOfDominoRecursive(matrix, 0, 0, l, 1);

            if (result != null)
            {
                result.Add(new Stone(0,0));
            }

            return result;
        }

        private List<Stone> MakeCircleOfDominoRecursive(int[,] mat, int first, int next, int maxDepth, int depth)
        {
            if (maxDepth == depth)
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
                    var result = MakeCircleOfDominoRecursive(mat, first, i, maxDepth, depth + 1);
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