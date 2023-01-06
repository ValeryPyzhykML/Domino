using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.OptimizedSolution.Collections
{
    public class UnsortedStoneMatrix
    {
        public long Count { get; protected set; }
        protected int[,] _matrix;

        public UnsortedStoneMatrix()
        {
            _matrix = new int[7,7];
            Count = 0;
        }

        public UnsortedStoneMatrix(int [,] matrix)
        {
            _matrix = matrix.Clone() as int[,];
            Count = 0;
            for (var i =0; i < 7; i ++)
            {
                for (var j = i; j < 7; j++)
                {
                    Count += matrix[i, j];
                }
            }
        }

        protected UnsortedStoneMatrix(int[,] matrix, long count)
        {
            _matrix = matrix.Clone() as int[,];
            Count = count;
        }

        public UnsortedStoneMatrix Clone()
        {
            return new UnsortedStoneMatrix(_matrix.Clone() as int[,], Count);
        }

        public void Add(byte i, byte j, int count)
        {
            Count += count;
            if (i <= j)
            {
                _matrix[i, j] += count;
            }
            else
            {
                _matrix[j, i] += count;
            }
        }

        public void Add(byte i, byte j)
        {
            Add(i, j, 1);
        }

        public void Remove(byte i, byte j)
        {
            Add(i, j, -1);
        }

        public void Remove(byte i, byte j, int count)
        {
            Add(i, j, -count);
        }

        public int Get(byte i, byte j)
        {
            if (i <= j)
            {
                return _matrix[i, j];
            } 
            else
            {
                return _matrix[j, i];
            }
        }

        public void Set(byte i, byte j, int count)
        {
            if (i <= j)
            {
                Count += count - _matrix[i, j];
                _matrix[i, j] = count;
            }
            else
            {
                Count += count - _matrix[j, i];
                _matrix[j, i] = count;
            }
        }
        public (bool, byte, byte) GetFirst()
        {
            if (Count < 1)
            {
                return (false, 0, 0);
            }

            for (byte i = 0; i < 7; i++)
            {
                for (byte j = i; j < 7; j++)
                {
                    if (_matrix[i, j] > 0)
                    {
                        return (true, i, j);
                    }
                }
            }

            throw new Exception("Collection is broken");
        }

        public (bool, byte, byte) GetFirstNonSymmetrical()
        {
            if (Count < 1)
            {
                return (false, 0, 0);
            }

            for (byte i = 0; i < 7; i++)
            {
                for (byte j = i; j < 7; j++)
                {
                    if (_matrix[i, j] > 0)
                    {
                        return (true, i, j);
                    }
                }
            }
            
            throw new Exception("Collection is broken");
        }
    }
}
