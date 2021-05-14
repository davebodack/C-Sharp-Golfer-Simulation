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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer timer = new DispatcherTimer();
        Random random = new Random();
        Golfer[] golfers = new Golfer[10];
        string[] names = new string[10];
        int rndScore;
        int holesElapsed = 0;
        int nameindex = 0;

        public MainWindow()
        {
            InitializeComponent();

            int j = 0;
            foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblName")))
            {
                names[j] = lbl.Content.ToString();
                j++;
            }

            for (int i = 0; i < 10; i++)
            {
                golfers[i] = new Golfer(names[i]);
            }
        }

        private void btnStartSim_Click(object sender, RoutedEventArgs e)
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            btnStartSim.Visibility = Visibility.Hidden;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (holesElapsed < 18)
            {
                holesElapsed++;
                foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblScore")))
                {
                    rndScore = random.Next(-1, 2);

                    if (lbl.Content.ToString() == "E")
                    {
                        lbl.Content = rndScore.ToString();
                    }
                    else
                    {
                        lbl.Content = (Int32.Parse(lbl.Content.ToString()) + rndScore).ToString();
                    }

                    if (lbl.Content.ToString() == "0")
                    {
                        lbl.Content = "E";
                    }
                    else if (Int32.Parse(lbl.Content.ToString()) > 0)
                    {
                        lbl.Content = "+" + lbl.Content.ToString();
                    }
                }

                foreach (Label lbl in mainGrid.Children.OfType<Label>().Where(lbl => lbl.Name.StartsWith("lblThru")))
                {
                    lbl.Content = holesElapsed.ToString();
                }
            }
          
        }
    }
}
