using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domino.OptimizedSolution
{
    public class CircularStoneArray: IReadOnlyCollection<(byte, byte)>
    {
        public int Count { get; protected set; }
        int IReadOnlyCollection<(byte, byte)>.Count => this.Count;

        private Action<StoneSequence>[] _possibleToInsert = new Action<StoneSequence>[7];

        private StoneSequence _first;

        public bool AddAciclicSequence(List<(byte, byte)> sequence)
        {
            if (_first == null)
            {
              
                var initNode = new AcyclicStoneSequence(sequence);
                _first = initNode;

                foreach (var s in sequence)
                {
                    _possibleToInsert[s.Item2] = AddInsideAcyclic(initNode);
                }

                Count += sequence.Count;

                return true;
            }


            if (!sequence.Any(x => _possibleToInsert[x.Item1] != null))
            {
                return false;
            }

            var boundary = sequence.First(x => _possibleToInsert[x.Item1] != null);

            Count += sequence.Count;

            var sequenceArray = new int[sequence.Count, 2];
            var indexToTurnSequence = sequence.IndexOf(boundary);

            var turnedSequence = sequence.GetRange(indexToTurnSequence, sequence.Count - indexToTurnSequence);
            turnedSequence.AddRange(sequence.GetRange(0, indexToTurnSequence));

            var nodeToInsert = new AcyclicStoneSequence(turnedSequence);
            Insert(turnedSequence.First().Item1, nodeToInsert);
            

            foreach (var s in turnedSequence)
            {
                _possibleToInsert[s.Item2] = _possibleToInsert[s.Item2] != null ? _possibleToInsert[s.Item2] : AddInsideAcyclic(nodeToInsert);
            }

            return true;
        }

        public bool AddPairedRepeatingSequence(byte sideOne, byte sideTwo, int count)
        {
            byte externalSide, internalSide;

            if (_first == null)
            {
                var sequence = new PairedRepeatingStoneSequence(sideOne, sideTwo, count);
                _first = sequence;
                _possibleToInsert[sideOne] = sequence.AddAfter;
                _possibleToInsert[sideTwo] = sequence.AddInside;
                Count += count;

                return true;
            }

            if (_possibleToInsert[sideOne] != null)
            {
                externalSide = sideOne;
                internalSide = sideTwo;
            }
            else if (_possibleToInsert[sideTwo] != null)
            {
                externalSide = sideTwo;
                internalSide = sideOne;
            }
            else
            {
                return false;
            }

            Count += count;

            var newSequence = new PairedRepeatingStoneSequence(externalSide, internalSide, count);

            
            Insert(externalSide, newSequence);
            _possibleToInsert[internalSide] = _possibleToInsert[internalSide] == null ? newSequence.AddInside : _possibleToInsert[internalSide];

            return true;
        }

        private void Insert(byte booundary, StoneSequence node)
        {
            var currentNode = _first;
            while (currentNode != null)
            {
                _possibleToInsert[currentNode.Boundary] = currentNode.AddAfter;
                currentNode = currentNode.Next;
            }
            _possibleToInsert[booundary].Invoke(node);
        }

        public bool AddSymmetricalRepeatingSequence(byte side, int count)
        {
            if (_first == null)
            {
                var sequence = new SymmetricalStoneSequence(side, count);
                _first = sequence;
                _possibleToInsert[side] = sequence.AddAfter;
                Count += count;

                return true;
            }
            
            if (_possibleToInsert[side] == null)
            {
                return false;
            }

            Count += count;

            _possibleToInsert[side].Invoke(new SymmetricalStoneSequence(side, count));

            return true;
        }

        private Action<StoneSequence> AddInsideAcyclic(AcyclicStoneSequence node)
        {
            return x =>
            {
                var count = node.Count;
                node.AddInside(x);
                if (node.Count != count)
                {
                    foreach (var s in node)
                    {
                        _possibleToInsert[s.Item2] = AddInsideAcyclic(node);
                    }

                    foreach (var s in node.Next.Next)
                    {
                        _possibleToInsert[s.Item2] = AddInsideAcyclic(node.Next.Next as AcyclicStoneSequence);
                    }
                }
            };
        }
        public IEnumerator<(byte, byte)> GetEnumerator()
        {
            var currentNode = _first;
            while (currentNode != null)
            {
                foreach (var c in currentNode)
                {
                    yield return c;
                }
                currentNode = currentNode.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
