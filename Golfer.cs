using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Golfer_Simulation
{
    public class Golfer
    {
        public string Name { get; set; }
        public string Nationality { get; set; }
        public int TotalScore { get; set; }
        public int Rd1Score { get; set; }
        public int Rd2Score { get; set; }
        public int Rd3Score { get; set; }
        public int Rd4Score { get; set; }
        public int PlayoffScore { get; set; }
        public int HolesPlayed { get; set; }

        public Golfer()
        {
            Name = "";
            Nationality = "";
            TotalScore = 0;
            Rd1Score = 0;
            Rd2Score = 0;
            Rd3Score = 0;
            Rd4Score = 0;
            PlayoffScore = 0;
            HolesPlayed = 0;
        }
    }
}
