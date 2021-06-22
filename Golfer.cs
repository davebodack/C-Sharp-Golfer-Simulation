using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Golfer_Simulation
{
    public class Golfer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //I'm going to re-assess whether these MVVM measures are necessary for the Golfer class in the next commit, when I implement MVVM for the Leaderboard form.

        private string name;
        private string nationality;
        private int totalScore;
        private int rd1Score;
        private int rd2Score;
        private int rd3Score;
        private int rd4Score;
        private int playoffScore;
        private int holesPlayed;

        public string Name 
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public string Nationality 
        {
            get { return nationality; }
            set
            {
                if (nationality != value)
                {
                    nationality = value;
                    RaisePropertyChanged("Nationality");
                }
            }
        }
        public int TotalScore 
        {
            get { return totalScore; }
            set
            {
                if (totalScore != value)
                {
                    totalScore = value;
                    RaisePropertyChanged("TotalScore");
                }
            }
        }
        public int Rd1Score
        {
            get { return rd1Score; }
            set
            {
                if (rd1Score != value)
                {
                    rd1Score = value;
                    RaisePropertyChanged("Rd1Score");
                }
            }
        }

        public int Rd2Score
        {
            get { return rd2Score; }
            set
            {
                if (rd2Score != value)
                {
                    rd2Score = value;
                    RaisePropertyChanged("Rd2Score");
                }
            }
        }

        public int Rd3Score
        {
            get { return rd3Score; }
            set
            {
                if (rd3Score != value)
                {
                    rd3Score = value;
                    RaisePropertyChanged("Rd3Score");
                }
            }
        }

        public int Rd4Score
        {
            get { return rd4Score; }
            set
            {
                if (rd4Score != value)
                {
                    rd4Score = value;
                    RaisePropertyChanged("Rd4Score");
                }
            }
        }

        public int PlayoffScore
        {
            get { return playoffScore; }
            set
            {
                if (playoffScore != value)
                {
                    playoffScore = value;
                    RaisePropertyChanged("PlayoffScore");
                }
            }
        }

        public int HolesPlayed
        {
            get { return holesPlayed; }
            set
            {
                if (holesPlayed != value)
                {
                    holesPlayed = value;
                    RaisePropertyChanged("HolesPlayed");
                }
            }
        }

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
