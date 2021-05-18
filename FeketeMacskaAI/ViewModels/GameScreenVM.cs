using FeketeMacskaAI.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FeketeMacskaAI.ViewModels
{
    public class GameScreenVM : ViewModel
    {
        private int _catchers;
        public int Catchers
        {
            get => _catchers;
            set
            {
                _catchers = value;
                OnPropertyChanged();
            }
        }

        private Visibility _victoryVisibility;
        public Visibility VictoryVisibility
        {
            get => _victoryVisibility;
            set
            {
                _victoryVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _gameScreenVisibility;
        public Visibility GameScreenVisibility
        {
            get => _gameScreenVisibility;
            set
            {
                _gameScreenVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _victoryText;
        public string VictoryText
        {
            get => _victoryText;
            set
            {
                _victoryText = value;
                OnPropertyChanged();
            }
        }

        public GameVM Game { get; private set; }

        private readonly Action EndGameCallback;
        public Command EndGame { get; private set; }
        public GameScreenVM(int catchers, Action EndGame)
        {
            Catchers = catchers;
            EndGameCallback = EndGame;
            VictoryVisibility = Visibility.Collapsed;
            GameScreenVisibility = Visibility.Visible;
            this.EndGame = new Command(ExecuteEndGame);
            Game = new GameVM(catchers, CatchersWon, CatsWon);
        }

        private void CatsWon()
        {
            VictoryVisibility = Visibility.Visible;
            GameScreenVisibility = Visibility.Collapsed;
            VictoryText = "The cats have escaped, catchers lost.";
        }

        private void CatchersWon()
        {
            VictoryVisibility = Visibility.Visible;
            GameScreenVisibility = Visibility.Collapsed;
            VictoryText = "The catchers catched all cats, cats lost.";
        }

        private void ExecuteEndGame(object param)
        {
            EndGameCallback();
        }
    }
}
