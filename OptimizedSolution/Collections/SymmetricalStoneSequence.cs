using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.OptimizedSolution
{
    public class SymmetricalStoneSequence : StoneSequence
    {
        public SymmetricalStoneSequence(byte side, int count)
        {
            this.Boundary = side;
            Count = count;
        }

        public override IEnumerator<(byte, byte)> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return (Boundary, Boundary);
            }
        }
    }
}
