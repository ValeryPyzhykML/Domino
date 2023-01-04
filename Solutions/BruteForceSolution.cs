using System.Collections.Generic;

namespace Domino
{
    /// <summary>
    /// The first solution I came to
    /// Extremely complex solution O(N^N)
    /// </summary>
    class BruteForceSoulution : IDominoCircleMaker
    {
        public List<Stone> MakeCircleOfDomino(Stone[] stones)
        {
            var coutnsToChech = new int[7];
            foreach (var s in stones)
            {
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

            var currentLine = new Stone(stones[0].Left, stones[0].Right);
            var ussedStones = new bool[stones.Length];
            ussedStones[0] = true;

            var result = MakeCircleOfDominoRecursion(currentLine, stones, ussedStones, 1);
            if (result != null)
            {
                result.Add(stones[0]);
            }

            return result;
        }
        

        List<Stone> MakeCircleOfDominoRecursion(Stone currentLine, Stone[] stones, bool[] ussedStones, int depth)
        {
            if (depth == stones.Length)
            {
                return currentLine.Left == currentLine.Right ? new List<Stone>() : null;
            }


            for (var i = 0; i < stones.Length; i++)
            {
                if (ussedStones[i] == false)
                {
                    if (stones[i].Left == currentLine.Left)
                    {
                        currentLine.Left = stones[i].Right;
                        ussedStones[i] = true;
                    }
                    else if (stones[i].Right == currentLine.Left)
                    {
                        currentLine.Left = stones[i].Left;
                        ussedStones[i] = true;
                    }
                    
                    if (ussedStones[i] == true)
                    {
                        var result = MakeCircleOfDominoRecursion(currentLine, stones, ussedStones, depth + 1);

                        if (result != null)
                        {
                            result.Add(stones[i]);
                            return result;
                        }

                        ussedStones[i] = false;
                    }
                }
            }
            return null;
        }
    }
}