using Domino.OptimizedSolution;
using Domino.OptimizedSolution.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domino
{
    /// <summary>
    /// Complexity is constant. N = 9999999, time < 1 ms 
    /// </summary>
    public class ConstantSolution
    {
        public IReadOnlyCollection<(byte, byte)> MakeCircleOfDomino(UnsortedStoneMatrix stones)
        {
            if (stones.Count == 1)
            {
                var (_, i, j) = stones.GetFirst();
                if (i != j)
                {
                    return null;
                }

                return new (byte, byte)[] { (i, j) };
            }

            var maxtrixToInsertInTheEnd = stones.Clone();

            for (byte i = 0; i < 7; i++)
            {
                if (stones.Get(i, i) != 0)
                {
                    stones.Set(i, i, 0);
                    var lengthToCover = 2;

                    for (byte j = 0; j < 7; j++)
                    {
                        lengthToCover -= i == j ? 0 : stones.Get(i, j);
                    }

                    if (lengthToCover % 2 == 1) // || lengthToCover > 0
                    {
                        return null;
                    }
                }
            }

            var totalRest = 0;
            for (byte i = 0; i < 7; i++)
            {
                for (byte j = (byte)(i + 1); j < 7; j++)
                {
                    stones.Set(i, j, stones.Get(i, j) % 2);
                    totalRest += stones.Get(i, j);
                    maxtrixToInsertInTheEnd.Remove(i, j, stones.Get(i, j));
                }
            }

            var results = new List<List<(byte, byte)>>();
            while (totalRest > 0)
            {
                var (_, firstI, firstJ) = stones.GetFirst();
                stones.Remove(firstI, firstJ, 1);

                var result = MakeCircleOfDominoRecursive(stones, firstI, firstJ, totalRest - 1);

                if (result == null)
                {
                    return null;
                }

                result.Add((firstJ, firstI));
                totalRest -= result.Count;
                results.Add(result);
            }


            // Reconstruction of the whole deck.
            return results.Any() ? GenerateSequence(maxtrixToInsertInTheEnd, results) : GenerateSequence(maxtrixToInsertInTheEnd);
        }



        private List<(byte, byte)> MakeCircleOfDominoRecursive(UnsortedStoneMatrix mat, byte first, byte next, int depth)
        {
            if (next == first)
            {
                return new List<(byte, byte)>();
            }

            if (depth == 0)
            {
                return null;
            }

            for (byte i = 0; i < 7; i++)
            {
                if (mat.Get(next, i) > 0)
                {
                    mat.Remove(next, i);
                    var result = MakeCircleOfDominoRecursive(mat, first, i, depth - 1);
                    if (result != null)
                    {
                        result.Add((i, next));
                        return result;
                    }
                    mat.Remove(next, i);
                }
            }

            return null;
        }

        private CircularStoneArray GenerateSequence(UnsortedStoneMatrix mat)
        {
            var result = new CircularStoneArray();

            var (isFirst, firstI, firstJ) = mat.GetFirstNonSymmetrical();

            if (isFirst)
            {
                result.AddPairedRepeatingSequence(firstI, firstJ, mat.Get(firstI, firstJ));
                mat.Set(firstI, firstJ, 0);
                AddPairsRecursive(result, mat, firstI);
                AddPairsRecursive(result, mat, firstJ);
            }

            // Generate Simetrical Stones
            for (byte i = 0; i < 7; i++)
            {
                var count = mat.Get(i, i);
                if (count > 0)
                {
                    if (!result.AddSymmetricalRepeatingSequence(i, mat.Get(i, i)))
                    {
                        return null;
                    }
                    mat.Set(i, i, 0);
                }
            }

            // Check
            return mat.Count == 0 ? result : null;
        }

        private void AddPairsRecursive(CircularStoneArray result, UnsortedStoneMatrix mat, byte numberToAdd)
        {
            for (byte i = 0; i < 7; i++)
            {
                if (i != numberToAdd)
                {
                    var count = mat.Get(numberToAdd, i);
                    if (mat.Get(numberToAdd, i) > 0)
                    {
                        result.AddPairedRepeatingSequence(numberToAdd, i, count);
                        mat.Set(numberToAdd, i, 0);
                        AddPairsRecursive(result, mat, i);
                    }
                }
            }
        }
        private CircularStoneArray GenerateSequence(UnsortedStoneMatrix mat, List<List<(byte, byte)>> nonPairedCicles)
        {
            var result = new CircularStoneArray();
            var circularNests = new LinkedListNode<Stone>[7];

            var flag = true;
            while (nonPairedCicles.Any() && flag)
            {
                var nonPaidCircle = nonPairedCicles.First(y => y.Count == nonPairedCicles.Max(x => x.Count));
                nonPairedCicles.Remove(nonPaidCircle);

                flag &= result.AddAciclicSequence(nonPaidCircle);

                foreach (var npc in nonPaidCircle)
                {
                    AddPairsRecursive(result, mat, npc.Item2);
                }
            }

            if (!flag)
            {
                return null;
            }

            // Generate Simetrical Stones
            for (byte i = 0; i < 7; i++)
            {
                var count = mat.Get(i, i);
                if (count > 0)
                {
                    if (!result.AddSymmetricalRepeatingSequence(i, mat.Get(i, i)))
                    {
                        return null;
                    }
                    mat.Set(i, i, 0);
                }
            }

            // Check
            return mat.Count == 0 ? result : null;
        }
    }
}