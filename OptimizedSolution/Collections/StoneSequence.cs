using System.Collections;
using System.Collections.Generic;

namespace Domino.OptimizedSolution
{
    public abstract class StoneSequence : IReadOnlyCollection<(byte, byte)>
    {
        public virtual int Count { get; protected set; }
        public StoneSequence Next { get; set;}
        public byte Boundary { get; protected set; }

        public void AddAfter(StoneSequence sequence)
        {
            var temp = Next;
            Next = sequence;
            sequence.Next = temp;
        }

        public abstract IEnumerator<(byte, byte)> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }
}
