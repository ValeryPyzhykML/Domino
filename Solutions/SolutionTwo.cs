using System;
using System.Collections.Generic;
using System.Linq;

namespace Domino
{
    /// <summary>
    /// Unrefactored. Complexity is O(N). Execution time for set with 40000000 stones is 4332 ms
    /// </summary>
    class SolutionTwo : IDominoCircleMaker
    {
        public ICollection<Stone> MakeCircleOfDomino(Stone[] stones)
        {
            if (stones.Length == 1)
            {
                return stones.Clone() as Stone[];
            }


            var matrix = new int[7, 7];

            foreach (var s in stones)
            {
                matrix[Math.Min(s.Left, s.Right), Math.Max(s.Left, s.Right)]++;
            }

            var maxtrixToInsertInTheEnd = matrix.Clone() as int[,];

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

                    if (lengthToCover % 2 == 1) // lengthToCover > 0 || 
                    {
                        return null;
                    }
                }
            }

            var totoalRest = 0;
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = i + 1; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = matrix[i, j] % 2;
                    totoalRest += matrix[i, j];
                    maxtrixToInsertInTheEnd[i, j] -= matrix[i, j];
                }
            }

            var results = new List<LinkedList<Stone>>();
            while (totoalRest > 0)
            {
                var (firstI, firstJ) = GetFirstStone(matrix);
                matrix[firstI, firstJ]--;

                var result = MakeCircleOfDominoRecursive(matrix, firstI, firstJ, totoalRest - 1);

                if (result == null)
                {
                    return null;
                }

                result.AddLast(new Stone(firstI, firstJ));
                totoalRest -= result.Count;
                results.Add(result);
            }

            // Reconstruction of the whole deck.
            return GenerateSequence(maxtrixToInsertInTheEnd, results);
        }

        private (int, int) GetFirstStone(int [,] mat)
        {
            for (var i = 0; i < mat.GetLength(0); i++)
            {
                for (var j = i + 1; j < mat.GetLength(1); j++)
                {
                    if (mat[i,j] > 0)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);
        }

        private LinkedList<Stone> MakeCircleOfDominoRecursive(int[,] mat, int first, int next, int depth)
        {
            if (next == first)
            {
                return new LinkedList<Stone>();
            }

            if (depth == 0)
            {
                return null;
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
                        result.AddLast(new Stone(next, i));
                        return result;
                    }
                    mat[min, max]++;
                }
            }

            return null;
        }

        private LinkedList<Stone> GenerateSequence(int[,] mat, List<LinkedList<Stone>> nonPairedCicles)
        {
            var result = new LinkedList<Stone>();
            var circularNests = new LinkedListNode<Stone>[7];
            var length = mat.GetLength(0);

            if (!nonPairedCicles.Any())
            {
                var (firstI, firstJ) = GetFirstStone(mat);
                if (firstI > -1)
                {
                    mat[firstI, firstJ] -= 2;
                    circularNests[firstJ] = result.AddLast(new Stone(firstI, firstJ));
                    circularNests[firstI] = result.AddLast(new Stone(firstJ, firstI));
                    AddPairsRecursive(result, circularNests, mat, firstI);
                    AddPairsRecursive(result, circularNests, mat, firstJ);
                }
            } else
            {
                var nonPaidCircle = nonPairedCicles.First(y => y.Count == nonPairedCicles.Max(x => x.Count));
                nonPairedCicles.Remove(nonPaidCircle);

                foreach (var npc in nonPaidCircle)
                {
                    var newNode = result.AddLast(npc);
                    if (circularNests[npc.Right] == null)
                    {
                        circularNests[npc.Right] = newNode;
                        AddPairsRecursive(result, circularNests, mat, npc.Right);
                    }
                }
            }

            while (nonPairedCicles.Any())
            {
                var nonPaidCircle = nonPairedCicles.First(y => y.Count == nonPairedCicles.Max(x => x.Count));
                nonPairedCicles.Remove(nonPaidCircle);

                var flag = false;
                var i = 0;
                while (i < nonPaidCircle.Count)
                {
                    if (circularNests[nonPaidCircle.First<Stone>().Left] == null)
                    {
                        var nodeToReplace = nonPaidCircle.First;
                        nonPaidCircle.RemoveFirst();
                        nonPaidCircle.AddLast(nodeToReplace);
                    } 
                    else
                    {
                        flag = true;
                    }
                    i++;
                }

                if (!flag)
                {
                    return null;
                }

                var nodeToAddAfter = circularNests[nonPaidCircle.First<Stone>().Left];
                foreach (var npc in nonPaidCircle)
                {
                    nodeToAddAfter = result.AddAfter(nodeToAddAfter, npc);

                    if (circularNests[npc.Right] == null)
                    {
                        circularNests[npc.Right] = nodeToAddAfter;
                        AddPairsRecursive(result, circularNests, mat, npc.Right);
                    }
                }
            }

            // Generate Simetrical Stones
            for (var i = 0; i < length; i++)
            {
                if (mat[i, i] > 0 && result.Count == 0)
                {
                    mat[i, i]--;
                    circularNests[i] = result.AddLast(new Stone(i, i));
                }
                while (mat[i, i] > 0)
                {
                    if (circularNests[i] == null)
                    {
                        return null;
                    }

                    result.AddAfter(circularNests[i], new Stone(i, i));
                    mat[i, i]--;
                }
            }

            // Check
            if ((-1,-1) != GetFirstStone(mat))
            {
                return null;
            }

            return result;
        }

        private void AddPairsRecursive(LinkedList<Stone> stones, LinkedListNode<Stone>[] circularNests, int[,] mat, int numberToAdd)
        {
            for (var i = 0; i < 7; i ++)
            {
                if (i != numberToAdd)
                {
                    var max = Math.Max(numberToAdd, i);
                    var min = Math.Min(numberToAdd, i);
                    if (mat[min, max] > 0)
                    {
                        var newNode = AddCircularRepetionAffter(stones, circularNests[numberToAdd], numberToAdd, i, ref mat[min, max]);
                        if (circularNests[i] == null)
                        {
                            circularNests[i] = newNode;
                            AddPairsRecursive(stones, circularNests, mat, i);
                        }
                    }
                }
            }
        }

        private LinkedListNode<Stone> AddCircularRepetionAffter(LinkedList<Stone> stones, LinkedListNode<Stone> nodeAffter, int left, int right, ref int numberOfRepetions)
        {
            LinkedListNode<Stone> leftInPair = null;
            while (numberOfRepetions != 0)
            {
                leftInPair = stones.AddAfter(nodeAffter, new Stone(left, right));
                stones.AddAfter(leftInPair, new Stone(right, left));
                numberOfRepetions -= 2;
            }
            return leftInPair;
        }
    }
}