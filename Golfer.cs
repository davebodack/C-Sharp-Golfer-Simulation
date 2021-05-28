using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Golfer_Simulation
{
    class Golfer
    {
        public string Name;
        public string Nationality;
        public int TotalScore;
        public int Rd1Score;
        public int Rd2Score;
        public int Rd3Score;
        public int Rd4Score;
        public int PlayoffScore;
        public int HolesPlayed;

        public Golfer(string newName, string newNationality)
        {
            Name = newName;
            Nationality = newNationality;
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
