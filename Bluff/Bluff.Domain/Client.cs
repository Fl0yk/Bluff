using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluff.Domain
{
    public class Client
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public int[] Cubes { get; set; }
        public int CubesCount { get; set; }

        public Client(string id, string name, string groupName) 
        {
            Id = id;
            Name = name;
            GroupName = groupName;
            Cubes = new int[6];
            CubesCount = 5;
        }
    }
}
