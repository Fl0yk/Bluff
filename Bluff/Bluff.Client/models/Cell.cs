namespace Bluff.Client.models
{
    public class Cell
    {
        public (int row, int column) Position { get; set; }
        public bool IsStar { get; set; } = false;
        public bool IsAvailable { get; set; } = false;
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
