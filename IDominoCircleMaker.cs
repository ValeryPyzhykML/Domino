using System.Collections.Generic;

namespace Domino
{
    public interface IDominoCircleMaker
    {
        List<Stone> MakeCircleOfDomino(Stone[] stones);
    }
}