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
        int holesElapsed = 0;
        int golfercount = 0;
        int placeCtr = 1;


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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (holesElapsed < 18)
            {
                holesElapsed++;
                for (int i = 0; i < 10; i++)
                {
                    randomScore = random.Next(-1, 2);
                    golfers[i].score += randomScore;
                }

                Array.Sort(golfers, delegate (Golfer golfer1, Golfer golfer2)
                {
                    return golfer1.score.CompareTo(golfer2.score);
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

        private void setPlaceLabels()
        {
            //Fill in golfer places
            golfercount = 0;
            placeCtr = 1;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => (lbl.Name.StartsWith("lblPlace"))))
            {
                //If we're setting the value of the first place label
                if (lbl.Name == "lblPlace1")
                {
                    if (golfers[0].score == golfers[1].score)
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
                    if (golfers[8].score == golfers[9].score)
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
                    if (golfers[golfercount - 1].score == golfers[golfercount].score)
                    {
                        lbl.Content = "T" + placeCtr.ToString();
                    }
                    else
                    {
                        placeCtr = golfercount + 1;
                        if (golfers[golfercount].score == golfers[golfercount + 1].score)
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

        private void setNameLabels()
        {
            //Fill in golfer names
            golfercount = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                lbl.Content = golfers[golfercount].name;
                golfercount++;
            }
        }

        private void setScoreLabels()
        {
            //Fill in golfer scores
            golfercount = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblScore")))
            {
                if (golfers[golfercount].score == 0)
                {
                    lbl.Content = "E";
                }
                else if (golfers[golfercount].score > 0)
                {
                    lbl.Content = "+" + golfers[golfercount].score.ToString();
                }
                else
                {
                    lbl.Content = golfers[golfercount].score.ToString();
                }
                golfercount++;
            }
        }

        private void setThruLabels()
        {
            //Fill in holes played
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblThru")))
            {
                lbl.Content = holesElapsed.ToString();
            }
        }
    }
}
