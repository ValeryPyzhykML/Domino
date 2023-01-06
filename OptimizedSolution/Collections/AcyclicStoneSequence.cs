using System;
using System.Collections.Generic;
using System.Linq;

namespace Domino.OptimizedSolution
{
    public class AcyclicStoneSequence : StoneSequence
    {
        private List<(byte, byte)> _sequence;

        public override int Count => _sequence.Count;

        public AcyclicStoneSequence(List<(byte, byte)> sequence)
        {
            Boundary = sequence.Last().Item2;
            this._sequence = sequence;
        }

        public void AddInside(StoneSequence sequenceToAdd)
        {
            var nextAffterInsetionNode = Next;

            if (Boundary == sequenceToAdd.Boundary)
            {
                Next = sequenceToAdd;
                sequenceToAdd.Next = nextAffterInsetionNode;
            } 
            else
            {
                var index = _sequence.IndexOf(_sequence.First(x => x.Item2 == sequenceToAdd.Boundary));
                var sequenceToSplit = _sequence;
                _sequence = sequenceToSplit.GetRange(0, index + 1);
                Boundary = _sequence.Last().Item2;
                if (_sequence.Count == 0)
                {
                    Console.WriteLine("111");
                }
                Next = sequenceToAdd;
            
                sequenceToAdd.Next = new AcyclicStoneSequence(sequenceToSplit.GetRange(index + 1, sequenceToSplit.Count - index - 1));
                if (sequenceToAdd.Next.Count == 0)
                {
                    Console.WriteLine("111");
                }
                sequenceToAdd.Next.Next = nextAffterInsetionNode;
            }
        }

        public override IEnumerator<(byte, byte)> GetEnumerator()
        {
            return _sequence.GetEnumerator();
        }
    }
}
