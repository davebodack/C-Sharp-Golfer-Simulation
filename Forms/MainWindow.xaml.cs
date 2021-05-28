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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Diagnostics;
using System.IO;

namespace C_Sharp_Golfer_Simulation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string jsonContentsLegendary; 
        string jsonContentsModern;
        List<Golfer> modernGolfers; 
        List<Golfer> legendaryGolfers;
        List<Golfer> selectedGolfers;

        public MainWindow()
        {
            jsonContentsModern = File.ReadAllText("../../JSON Files/modernGolfers.json");
            jsonContentsLegendary = File.ReadAllText("../../JSON Files/legendaryGolfers.json");
            modernGolfers = JsonSerializer.Deserialize<List<Golfer>>(jsonContentsModern);
            legendaryGolfers = JsonSerializer.Deserialize<List<Golfer>>(jsonContentsLegendary);

            InitializeComponent();
        }

        private void radModern_Checked(object sender, RoutedEventArgs e)
        {
            listGolfers.Items.Clear();
            foreach (Golfer golfer in modernGolfers)
            {
                listGolfers.Items.Add(golfer.Name);
            }
            selectedGolfers = modernGolfers;
            btnChooseGolfers.IsEnabled = true;
        }

        private void RadLegendary_Checked(object sender, RoutedEventArgs e)
        {
            listGolfers.Items.Clear();
            foreach (Golfer golfer in legendaryGolfers)
            {
                listGolfers.Items.Add(golfer.Name);
            }
            selectedGolfers = legendaryGolfers;
            btnChooseGolfers.IsEnabled = true;
        }

        private void btnChooseGolfers_Click(object sender, RoutedEventArgs e)
        {
            var frmLeaderboard = new Leaderboard(selectedGolfers);
            frmLeaderboard.Show();
            this.Hide();
        }
    }
}
