using System.Collections.Generic;

namespace Domino
{
    public interface IDominoCircleMaker
    {
        ICollection<Stone> MakeCircleOfDomino(Stone[] stones);
    }
}