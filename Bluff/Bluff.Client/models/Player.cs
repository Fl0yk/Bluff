namespace Bluff.Client.models
{
    public class Player
    {
        public string Name { get; set; }
        public int[] Cubes { get; set; } = new int[6];
        public int CubeCount { get; set; }

        public List<int> GetNormalizeCubes()
        {
            List<int> res = new();

            for(int i = 0; i < Cubes.Length; i++)
            {
                for(int j = 0; j < Cubes[i]; j++)
                {
                    res.Add(i + 1);
                }
            }

            return res;
        }
    }
}
