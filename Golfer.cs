using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Golfer_Simulation
{
    class Golfer
    {
        public string name;
        public int score;
        public int holesplayed;

        public Golfer(string newname)
        {
            name = newname;
            score = 0;
            holesplayed = 0;
        }
    }
}
