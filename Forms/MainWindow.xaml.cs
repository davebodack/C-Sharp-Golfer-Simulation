using System.Windows;

namespace C_Sharp_Golfer_Simulation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool PauseAfterRound;
        private static MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void radModern_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.LoadModernGolfers();
        }

        private void RadLegendary_Checked(object sender, RoutedEventArgs e)
        {
            viewModel.LoadLegendaryGolfers();
        }

        private void btnChooseGolfers_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chkPauseRound.IsChecked) PauseAfterRound = true;
            else PauseAfterRound = false;
            viewModel.PrepareSimulation();
            this.Hide();
        }
    }
}
