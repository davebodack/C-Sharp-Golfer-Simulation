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
using System.Windows.Shapes;

namespace C_Sharp_Golfer_Simulation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string[] golfersModern = new string[] { "Dustin Johnson", "Justin Thomas", "Jon Rahm", "Xander Schauffele", "Bryson DeChambeau",
                                                "Collin Morikawa", "Rory McIlroy", "Patrick Reed", "Tyrrell Hatton", "Webb Simpson" };
        
        string[] golfersLegendary = new string[] { "Jack Nicklaus", "Tiger Woods", "Sam Snead", "Ben Hogan", "Gary Player",
                                                   "Arnold Palmer", "Walter Hagen", "Phil Mickelson", "Tom Watson", "Seve Ballesteros" };

        string[] golfersLeaderboard = new string[10];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void radModern_Checked(object sender, RoutedEventArgs e)
        {
            listGolfers.Items.Clear();
            foreach (string golfer in golfersModern)
            {
                listGolfers.Items.Add(golfer);
            }
        }

        private void RadLegendary_Checked(object sender, RoutedEventArgs e)
        {
            listGolfers.Items.Clear();
            foreach (string golfer in golfersLegendary)
            {
                listGolfers.Items.Add(golfer);
            }
        }

        private void btnChooseGolfers_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;
            foreach (string golferName in listGolfers.Items)
            {
                golfersLeaderboard[i] = golferName;
                i++;
            }

            var frmLeaderboard = new Leaderboard(golfersLeaderboard);
            frmLeaderboard.Show();
            this.Hide();
        }
    }
}
