using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using System.IO;

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
        string flagFileName;
        bool playoffFlag = false;
        int randomScore;
        int roundCtr = 1;
        int thruRoundCtr;
        int holesElapsed = 0;
        int golfercount = 0;
        int placeCtr = 1;


        //Initializes the form based on the golfer list selected in MainWindow, populates the Name labels.
        public Leaderboard(List<Golfer> selectedGolfers)
        {
            InitializeComponent();
            
            for (int i = 0; i < selectedGolfers.Count; i++)
            {
                golfers[i] = selectedGolfers[i];
            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            int j = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                lbl.Content = golfers[j].Name;
                j++;
            }
        }


        //Handles clicking the Start Simulation button, starts the timer ticking.
        private void startSimulation()
        {
            if (btnStartSim.Content.ToString() == "Return to Main Menu")
            {
                lblPlayoffHeader.Visibility = Visibility.Hidden;
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

                    if (golfers[0].TotalScore == golfers[1].TotalScore)
                    {
                        playoffGolfers.Add(golfers[0]);
                    }
                    for (int i = 1; i < golfers.Length - 1; i++)
                    {
                        if (golfers[i].TotalScore == golfers[i - 1].TotalScore)
                        {
                            playoffGolfers.Add(golfers[i]);
                        }
                    }
                    if (golfers[golfers.Length - 2].TotalScore == golfers[golfers.Length - 1].TotalScore)
                    {
                        playoffGolfers.Add(golfers[golfers.Length - 1]);
                    }
                }

                timer.Start();
                btnStartSim.Visibility = Visibility.Hidden;
                stackSimSpeed.Visibility = Visibility.Visible;
            }
        }


        //Calls startSimulation, which actually handles starting the sim
        private void btnStartSim_Click(object sender, RoutedEventArgs e)
        {
            startSimulation();  
        }


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
                        golfers[i].TotalScore += randomScore;
                        if (roundCtr == 1)
                        {
                            golfers[i].Rd1Score += randomScore;
                        }
                        else if (roundCtr == 2)
                        {
                            golfers[i].Rd2Score += randomScore;
                        }
                        else if (roundCtr == 3)
                        {
                            golfers[i].Rd3Score += randomScore;
                        }
                        else
                        {
                            golfers[i].Rd4Score += randomScore;
                        }

                    }

                    Array.Sort(golfers, delegate (Golfer golfer1, Golfer golfer2)
                    {
                        return golfer1.TotalScore.CompareTo(golfer2.TotalScore);
                    });
                    golfers.Reverse();

                    setPlaceLabels();
                    setFlagImages();
                    setNameLabels();
                    setScoreLabels();
                    setThruLabels();
                }

                //If we've finished one of the four rounds
                else
                {
                    roundCtr++;
                    if (MainWindow.PauseAfterRound)
                    {
                        timer.Stop();
                        setStartButton(true); 
                    }
                    else     //If not pausing between rounds
                    {
                        setStartButton(false);
                        startSimulation();
                    }                              
                }   //end if finished one of four rounds
            }   //end if (!playoffFlag)

            //If we're computing the playoff
            else
            {
                holesElapsed++;

                if (playoffGolfers.Count > 1)
                {

                    for (int i = 0; i < playoffGolfers.Count; i++)
                    {
                        randomScore = random.Next(-1, 2);
                        playoffGolfers[i].PlayoffScore += randomScore;
                    }

                    playoffGolfers = playoffGolfers.OrderBy(o => o.PlayoffScore).ToList();

                    for (int i = 0; i < playoffGolfers.Count; i++)
                    {
                        golfers[i] = playoffGolfers[i];
                    }

                    setPlaceLabels();
                    setFlagImages();
                    setNameLabels();
                    setScoreLabels();
                    setThruLabels();

                    //Remove players from playoffGolfers if they didn't match best score
                    golfercount = 0;
                    while (golfercount < playoffGolfers.Count)
                    {
                        if (playoffGolfers[golfercount].PlayoffScore > playoffGolfers[0].PlayoffScore)
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
                    setStartButton(true);
                }
                
            }     
        }   //End Timer_Tick()


        //Sets btnStartSim.Content, makes button and Sim Speed stack visible or invisible
        //Also sets playoffFlag if there's a tie
        private void setStartButton(bool makeButtonVisible)
        {
            if (makeButtonVisible)
            {
                btnStartSim.Visibility = Visibility.Visible;
                stackSimSpeed.Visibility = Visibility.Hidden;
            }           

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
            else if ((golfers[0].TotalScore == golfers[1].TotalScore) && (golfers[0].PlayoffScore == golfers[1].PlayoffScore))
            {
                playoffFlag = true;
                lblPlayoffHeader.Visibility = Visibility.Visible;
                btnStartSim.Content = "Begin Playoff";
            }
            else
            {
                btnStartSim.Content = "Return to Main Menu";
            }
        }


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
                        if (((golfers[0].TotalScore == golfers[1].TotalScore) && !playoffFlag) ||
                            ((golfers[0].PlayoffScore == golfers[1].PlayoffScore) && playoffFlag))
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
                        if (((golfers[8].TotalScore == golfers[9].TotalScore) && !playoffFlag) ||
                            ((golfers[8].PlayoffScore == golfers[9].PlayoffScore) && playoffFlag))
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
                        if (((golfers[golfercount - 1].TotalScore == golfers[golfercount].TotalScore) && !playoffFlag) ||
                            ((golfers[golfercount - 1].PlayoffScore == golfers[golfercount].PlayoffScore) && playoffFlag))
                        {
                            lbl.Content = "T" + placeCtr.ToString();
                        }
                        else
                        {
                            placeCtr = golfercount + 1;
                            //Check if tied with golfer below you on leaderboard. The long clause is to make sure that if you have the same playoff score,
                            //but made it a hole farther in sudden death than them, you don't get a 'T' by your place if you shouldn't.
                            if (((golfers[golfercount].TotalScore == golfers[golfercount + 1].TotalScore) && !playoffFlag) ||
                                ((golfers[golfercount].PlayoffScore == golfers[golfercount + 1].PlayoffScore) && playoffFlag && (golfercount + 1 < playoffGolfers.Count)))
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


        //Set the Nationality flags displayed for each golfer
        private void setFlagImages()
        {
            golfercount = 0;

            foreach (Image img in mainGrid.Children.OfType<Image>().Where(img => img.Name.StartsWith("imgFlag")))
            {
                if (golfers[golfercount].Nationality == "Spain")
                {
                    flagFileName = "Spain.png";
                }
                else if (golfers[golfercount].Nationality == "South Africa")
                {
                    flagFileName = "South Africa.png";
                }
                else if (golfers[golfercount].Nationality == "England")
                {
                    flagFileName = "England.png";
                }
                else if (golfers[golfercount].Nationality == "Northern Ireland")
                {
                    flagFileName = "Ulster.png";
                }
                else
                {
                    flagFileName = "USA.png";
                }

                img.Source = new BitmapImage(new Uri("../Resources/" + flagFileName, UriKind.Relative));
                golfercount++;
            }

        }


        //Fills in the golfer name labels
        private void setNameLabels()
        {
            golfercount = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                lbl.Content = golfers[golfercount].Name;
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
                        labelScoreSetter(lbl, golfers[golfercount].TotalScore);
                    }
                    else if (thruRoundCtr == 1)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].Rd1Score);
                    }
                    else if (thruRoundCtr == 2)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].Rd2Score);
                    }
                    else if (thruRoundCtr == 3)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].Rd3Score);
                    }
                    else if (thruRoundCtr == 4)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].Rd4Score);
                    }
                    else if (golfercount < playoffGolfers.Count)
                    {
                        labelScoreSetter(lbl, golfers[golfercount].PlayoffScore);
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
