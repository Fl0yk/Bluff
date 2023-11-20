using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluff.Domain
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int[]? Cubes { get; set; }

        public int? CubesCount { get; set; }
    }
}
