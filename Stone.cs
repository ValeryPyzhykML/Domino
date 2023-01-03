namespace Domino
{
    public class Stone
    {
        public int Left { get; set; }
        public int Right { get; set; }

        public Stone(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}