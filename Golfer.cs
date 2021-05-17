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
        public int totalscore;
        public int rd1score;
        public int rd2score;
        public int rd3score;
        public int rd4score;
        public int holesplayed;

        public Golfer(string newname)
        {
            name = newname;
            totalscore = 0;
            rd1score = 0;
            rd2score = 0;
            rd3score = 0;
            rd4score = 0;
            holesplayed = 0;
        }
    }
}
