using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StockGames.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public RelayCommand ViewMarketCommand { get; private set; }

        public DashboardViewModel(RelayCommand viewMarketCommand)
        {
            ViewMarketCommand = viewMarketCommand;
        }

        
    }
}
