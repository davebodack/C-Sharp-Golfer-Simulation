using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace C_Sharp_Golfer_Simulation
{
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        Random random = new Random();
        Golfer[] golfers = new Golfer[10];
        string[] names = new string[10];
        int randomScore;
        int roundCtr = 1;
        int thruRoundCtr;
        int holesElapsed = 0;
        int golfercount = 0;
        int placeCtr = 1;


        //Initializes the form based on the golfer list selected in MainWindow, populates the Name labels.
        public Leaderboard(string[] golfernames)
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            for (int i = 0; i < 10; i++)
            {
                golfers[i] = new Golfer(golfernames[i]);
            }

            int j = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                lbl.Content = golfers[j].name;
                j++;
            }
        }

        //Handles clicking the Start Simulation button, starts the timer ticking.
        private void btnStartSim_Click(object sender, RoutedEventArgs e)
        {
            holesElapsed = 0;
            if (roundCtr == 1)
            {
                lblLeaderboard.Content = "Leaderboard- First Round";
            }
            else if (roundCtr == 2)
            {
                lblLeaderboard.Content = "Leaderboard- Second Round";
            }
            else if (roundCtr == 3)
            {
                lblLeaderboard.Content = "Leaderboard- Third Round";
            }
            else
            {
                lblLeaderboard.Content = "Leaderboard- Fourth Round";
            }
            timer.Start();
            btnStartSim.Visibility = Visibility.Hidden;
        }

        //Generates the random scores on a hole, calls the set label functions which populate the labels
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (holesElapsed < 18)
            {
                holesElapsed++;
                for (int i = 0; i < 10; i++)
                {
                    randomScore = random.Next(-1, 2);
                    golfers[i].totalscore += randomScore;
                    if (roundCtr == 1)
                    {
                        golfers[i].rd1score += randomScore;
                    }
                    else if (roundCtr == 2)
                    {
                        golfers[i].rd2score += randomScore;
                    }
                    else if (roundCtr == 3)
                    {
                        golfers[i].rd3score += randomScore;
                    }
                    else
                    {
                        golfers[i].rd4score += randomScore;
                    }

                }

                Array.Sort(golfers, delegate (Golfer golfer1, Golfer golfer2)
                {
                    return golfer1.totalscore.CompareTo(golfer2.totalscore);
                });
                golfers.Reverse();

                setPlaceLabels();
                setNameLabels();
                setScoreLabels();
                setThruLabels();
            }

            else
            {
                timer.Stop();
                roundCtr++;
                if (roundCtr <= 4)
                {
                    if (roundCtr == 2)
                    {
                        btnStartSim.Content = "Begin Second Round";
                    }
                    else if (roundCtr == 3)
                    {
                        btnStartSim.Content = "Begin Third Round";
                    }
                    else if (roundCtr == 4)
                    {
                        btnStartSim.Content = "Begin Fourth Round";
                    }
                    btnStartSim.Visibility = Visibility.Visible;
                } 
            }
          
        }

        //Fills in the place on the leaderboard each golfer occupies (first, second, third, etc.)
        private void setPlaceLabels()
        {
            golfercount = 0;
            placeCtr = 1;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => (lbl.Name.StartsWith("lblPlace"))))
            {
                //If we're setting the value of the first place label
                if (lbl.Name == "lblPlace1")
                {
                    if (golfers[0].totalscore == golfers[1].totalscore)
                    {
                        lbl.Content = "T1";
                    }
                    else
                    {
                        lbl.Content = "1";
                        placeCtr++;
                    }
                    golfercount++;
                }
                //If we're setting the value of the tenth place label
                else if (lbl.Name == "lblPlace10")
                {
                    if (golfers[8].totalscore == golfers[9].totalscore)
                    {
                        lbl.Content = "T" + placeCtr.ToString();
                    }
                    else
                    {
                        placeCtr = golfercount + 1;
                        lbl.Content = placeCtr.ToString();
                    }
                }
                //If we're setting any of the other place labels
                else
                {
                    if (golfers[golfercount - 1].totalscore == golfers[golfercount].totalscore)
                    {
                        lbl.Content = "T" + placeCtr.ToString();
                    }
                    else
                    {
                        placeCtr = golfercount + 1;
                        if (golfers[golfercount].totalscore == golfers[golfercount + 1].totalscore)
                        {
                            lbl.Content = "T" + placeCtr.ToString();
                        }
                        else
                        {
                            lbl.Content = placeCtr.ToString();
                        }
                    }
                    golfercount++;
                }

            }
        }

        //Fills in the golfer name labels
        private void setNameLabels()
        {
            golfercount = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                lbl.Content = golfers[golfercount].name;
                golfercount++;
            }
        }

        //Determines which labels need to have scores set and calls labelScoreSetter to do so.
        private void setScoreLabels()
        {
            string[] labelTypeList = new string[] { "lblScore", "lblRd1Score", "lblRd2Score", "lblRd3Score", "lblRd4Score" };
            thruRoundCtr = 0;
            while (thruRoundCtr <= roundCtr)
            {
                golfercount = 0;
                foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith(labelTypeList[thruRoundCtr])))
                {
                    if (thruRoundCtr == 0)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].totalscore);
                    }
                    else if (thruRoundCtr == 1)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].rd1score);
                    }
                    else if (thruRoundCtr == 2)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].rd2score);
                    }
                    else if (thruRoundCtr == 3)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].rd3score);
                    }
                    else
                    {
                        labelScoreSetter(lbl, golfers[golfercount].rd4score);
                    }
                    golfercount++;
                }
                thruRoundCtr++;
            }
        }

        //Fill in the number of holes played
        private void setThruLabels()
        {
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblThru")))
            {
                if (holesElapsed < 18)
                {
                    lbl.Content = holesElapsed.ToString();
                }
                else
                {
                    lbl.Content = "F";  //Mr. Crocker wrote this function.
                }
            }
        }

        //This function is called by setScoreLabels, which handles setting the right label depending on the round.
        private void labelScoreSetter(Label lbl, int score)
        {
            if (score == 0)
            {
                lbl.Content = "E";
            }
            else if (score > 0)
            {
                lbl.Content = "+" + score.ToString();
            }
            else
            {
                lbl.Content = score.ToString();
            }
        }
    }
}
