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
        List<Golfer> playoffGolfers = new List<Golfer>();
        string[] names = new string[10];
        bool playoffFlag = false;
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
            if (btnStartSim.Content.ToString() == "Return to Main Menu")
            {
                var mainWin = new MainWindow();
                mainWin.Show();
                this.Hide();
            }
            else
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
                else if (roundCtr == 4)
                {
                    lblLeaderboard.Content = "Leaderboard- Fourth Round";
                }
                else
                {
                    lblLeaderboard.Content = "Leaderboard- Sudden Death Playoff";
                    playoffFlag = true;

                    if (golfers[0].totalscore == golfers[1].totalscore)
                    {
                        playoffGolfers.Add(golfers[0]);
                    }
                    for (int i = 1; i < golfers.Length - 1; i++)
                    {
                        if (golfers[i].totalscore == golfers[i - 1].totalscore)
                        {
                            playoffGolfers.Add(golfers[i]);
                        }
                    }
                    if (golfers[golfers.Length - 2].totalscore == golfers[golfers.Length - 1].totalscore)
                    {
                        playoffGolfers.Add(golfers[golfers.Length - 1]);
                    }
                }

                timer.Start();
                btnStartSim.Visibility = Visibility.Hidden;
                stackSimSpeed.Visibility = Visibility.Visible;
            }
            
        }   //End btnStartSim_Click()


        //Generates the random scores on a hole, calls the set label functions which populate the labels
        private void Timer_Tick(object sender, EventArgs e)
        {

            //If we're computing Rounds 1-4
            if (!playoffFlag)
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

                //If we've finished one of the four rounds
                else
                {
                    timer.Stop();
                    roundCtr++;
                    btnStartSim.Visibility = Visibility.Visible;
                    stackSimSpeed.Visibility = Visibility.Hidden;
                    
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
                    else if (golfers[0].totalscore == golfers[1].totalscore)
                    {
                        btnStartSim.Content = "Begin Playoff";
                    }
                    else
                    {
                        btnStartSim.Content = "Return to Main Menu";
                    }          
                }
            }

            //If we're computing the playoff
            else
            {
                holesElapsed++;

                if (playoffGolfers.Count > 1)
                {

                    for (int i = 0; i < playoffGolfers.Count; i++)
                    {
                        randomScore = random.Next(-1, 2);
                        playoffGolfers[i].playoffscore += randomScore;
                    }

                    playoffGolfers = playoffGolfers.OrderBy(o => o.playoffscore).ToList();

                    for (int i = 0; i < playoffGolfers.Count; i++)
                    {
                        golfers[i] = playoffGolfers[i];
                    }

                    setPlaceLabels();
                    setNameLabels();
                    setScoreLabels();
                    setThruLabels();

                    //Remove players from playoffGolfers if they didn't match best score
                    golfercount = 0;
                    while (golfercount < playoffGolfers.Count)
                    {
                        if (playoffGolfers[golfercount].playoffscore > playoffGolfers[0].playoffscore)
                        {
                            playoffGolfers.RemoveAt(golfercount);
                        }
                        else
                        {
                            golfercount++;
                        }
                    }
                }
                
                else
                {
                    //Executed when playoff has finished
                    timer.Stop();
                    btnStartSim.Visibility = Visibility.Visible;
                    stackSimSpeed.Visibility = Visibility.Hidden;
                    btnStartSim.Content = "Return to Main Menu";
                }
                

            }
           
        }   //End Timer_Tick()


        //Fills in the place on the leaderboard each golfer occupies (first, second, third, etc.)
        private void setPlaceLabels()
        {
            golfercount = 0;
            placeCtr = 1;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => (lbl.Name.StartsWith("lblPlace"))))
            {
                if (!playoffFlag || (playoffFlag && golfercount < playoffGolfers.Count))
                {
                    //If we're setting the value of the first place label
                    if (lbl.Name == "lblPlace1")
                    {
                        if (((golfers[0].totalscore == golfers[1].totalscore) && !playoffFlag) ||
                            ((golfers[0].playoffscore == golfers[1].playoffscore) && playoffFlag))
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
                        if (((golfers[8].totalscore == golfers[9].totalscore) && !playoffFlag) ||
                            ((golfers[8].playoffscore == golfers[9].playoffscore) && playoffFlag))
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
                        //First check if tied with golfer above you on leaderboard, then...
                        if (((golfers[golfercount - 1].totalscore == golfers[golfercount].totalscore) && !playoffFlag) ||
                            ((golfers[golfercount - 1].playoffscore == golfers[golfercount].playoffscore) && playoffFlag))
                        {
                            lbl.Content = "T" + placeCtr.ToString();
                        }
                        else
                        {
                            placeCtr = golfercount + 1;
                            //Check if tied with golfer below you on leaderboard. The long clause is to make sure that if you have the same playoff score,
                            //but made it a hole farther in sudden death than them, you don't get a 'T' by your place if you shouldn't.
                            if (((golfers[golfercount].totalscore == golfers[golfercount + 1].totalscore) && !playoffFlag) ||
                                ((golfers[golfercount].playoffscore == golfers[golfercount + 1].playoffscore) && playoffFlag && (golfercount + 1 < playoffGolfers.Count)))
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

                }   //End if !playoffCount || (playoffCount && golfercount < playoffGolfers.Count)
                
            }   // End foreach lbl

        }   //End setPlaceLabels()


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
            string[] labelTypeList = new string[] { "lblScore", "lblRd1Score", "lblRd2Score", "lblRd3Score", "lblRd4Score", "lblPlayoffScore" };
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
                    else if (thruRoundCtr == 4)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].rd4score);
                    }
                    else if (golfercount < playoffGolfers.Count)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].playoffscore);
                    }
                    golfercount++;
                }
                thruRoundCtr++;
            }
        }


        //Fill in the number of holes played
        private void setThruLabels()
        {
            golfercount = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblThru")))
            {
                if ((!playoffFlag) || (playoffFlag && golfercount < playoffGolfers.Count))
                {
                    if ((holesElapsed < 18) && (!playoffFlag))
                    {
                        lbl.Content = holesElapsed.ToString();
                    }
                    else if (!playoffFlag)
                    {
                        lbl.Content = "F";
                    }
                    else
                    {
                        lbl.Content = "P" + holesElapsed.ToString();
                    }
                }
                golfercount++;
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


        //This handles changing the slider value and changes the timer interval accordingly
        private void sldrTimerInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timer.Interval = TimeSpan.FromSeconds(1 / sldrTimerInterval.Value);
        }
    }
}
