using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeketeMacskaAI.ViewModels
{
    public class StartScreenVM : ViewModel
    {
        private int _numberOfCatchers;
        public int NumberOfCatchers
        {
            get { return _numberOfCatchers; }
            set
            {
                _numberOfCatchers = value;
                OnPropertyChanged();
            }
        }

        public Command StartGame { get; private set; }

        private readonly Action<int> StartGameCallback;

        public StartScreenVM(Action<int> StartGame)
        {
            NumberOfCatchers = 8;
            this.StartGame = new Command(ExecuteStartGame);
            StartGameCallback = StartGame;
        }

        private void ExecuteStartGame(object param)
        {
            StartGameCallback(NumberOfCatchers);
        }

    }
}
