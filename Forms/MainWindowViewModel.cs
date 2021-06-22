using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;

namespace C_Sharp_Golfer_Simulation
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        static string jsonContentsModern = File.ReadAllText("../../JSON Files/modernGolfers.json");
        static string jsonContentsLegendary = File.ReadAllText("../../JSON Files/legendaryGolfers.json");

        public static ObservableCollection<Golfer> ModernGolfers = new ObservableCollection<Golfer>(JsonSerializer.Deserialize<ObservableCollection<Golfer>>(jsonContentsModern));
        public static ObservableCollection<Golfer> LegendaryGolfers = new ObservableCollection<Golfer>(JsonSerializer.Deserialize<ObservableCollection<Golfer>>(jsonContentsLegendary));

        private ObservableCollection<Golfer> selectedGolfers;
        public ObservableCollection<Golfer> SelectedGolfers 
        { 
            get { return selectedGolfers; }
            set 
            {
                selectedGolfers = value;
                RaisePropertyChanged("SelectedGolfers");
            }
        }

        public MainWindowViewModel()
        {
            SelectedGolfers = ModernGolfers;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void LoadLegendaryGolfers()
        {
            SelectedGolfers = LegendaryGolfers;
        }

        public void LoadModernGolfers()
        {
            SelectedGolfers = ModernGolfers;
        }

        public void PrepareSimulation()
        {
            var frmLeaderboard = new Leaderboard(SelectedGolfers);
            frmLeaderboard.Show();
        }
    }
}
