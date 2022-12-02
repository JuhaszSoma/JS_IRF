using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week09.Entities
{
    public class DeathProbability
    {
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public double DeathChance { get; set; }
    }
}
