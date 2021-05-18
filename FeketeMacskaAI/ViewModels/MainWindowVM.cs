namespace FeketeMacskaAI.ViewModels
{
    public class MainWindowVM : ViewModel
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        private readonly StartScreenVM StartScreen;
        public MainWindowVM()
        {
            StartScreen = new StartScreenVM(StartGame);
            CurrentViewModel = StartScreen;
        }

        public void StartGame(int catchers)
        {
            CurrentViewModel = new GameScreenVM(catchers, EndGame);
        }
        public void EndGame()
        {
            CurrentViewModel = StartScreen;
        }
    }
}