using System.Collections.Generic;

namespace Domino.OptimizedSolution
{
    public class PairedRepeatingStoneSequence : StoneSequence
    {
        public byte InternalSide { get; protected set; }

        public PairedRepeatingStoneSequence(byte externalSide, byte internalSide, int count)
        {
            Boundary = externalSide;
            InternalSide = internalSide;
            Count = count;
        }

        public void AddInside(StoneSequence sequence)
        {
            Count -= 2;
            var temp = Next;
            Next = new AcyclicStoneSequence(new List<(byte, byte)>() { (Boundary, InternalSide ) });
            Next.Next = sequence;
            Next.Next.Next = new AcyclicStoneSequence(new List<(byte, byte)>() { ( InternalSide, Boundary ) });
            Next.Next.Next.Next = temp;
        }

        public override IEnumerator<(byte, byte)> GetEnumerator()
        {
            for (var i =0; i < Count; i++)
            {
                yield return i % 2 == 0? (Boundary, InternalSide) : (InternalSide, Boundary);
            }
        }
    }
}
