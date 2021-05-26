using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace FeketeMacskaAI.Logic
{
    public class GridCell : ViewModel
    {
        private GridCellStatus _status;
        public GridCellStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                switch (_status)
                {
                    case GridCellStatus.Empty:
                        CatVisibility = Visibility.Collapsed;
                        CatcherVisibility = Visibility.Collapsed;
                        break;

                    case GridCellStatus.Cat:
                        CatVisibility = Visibility.Visible;
                        CatcherVisibility = Visibility.Collapsed;
                        break;

                    case GridCellStatus.Catcher:
                        CatVisibility = Visibility.Collapsed;
                        CatcherVisibility = Visibility.Visible;
                        break;

                    default:
                        break;
                }
                OnPropertyChanged(nameof(CatVisibility));
                OnPropertyChanged(nameof(CatcherVisibility));
            }
        }

        private SolidColorBrush _bgColor;
        public SolidColorBrush BackgroundColor
        {
            get => _bgColor;
            set
            {
                _bgColor = value;
                Debug.WriteLine($"setting bgcolor of: {x} {y} to color:{value.Color.R} {value.Color.G} {value.Color.B}");
                OnPropertyChanged();
            }
        }

        public bool WinningCell { get; set; }

        private Visibility _catVisibility;
        public Visibility CatVisibility
        {
            get => _catVisibility;
            set
            {
                _catVisibility = value;
                OnPropertyChanged();
            }
        }

        private Visibility _catcherVisibility;
        public Visibility CatcherVisibility
        {
            get => _catcherVisibility;
            set
            {
                _catcherVisibility = value;
                OnPropertyChanged();
            }
        }

        public int Player { get; set; }
        public int x, y;

        public Command Click { get; set; }

        private bool _isValidStep;

        public bool IsValidStep
        {
            get => _isValidStep;
            set
            {
                _isValidStep = value;
                SetBackgroundColor();
                OnPropertyChanged();
            }
        }

        private bool _isRecommendedStep;

        public bool IsRecommendedStep
        {
            get => _isRecommendedStep;
            set
            {
                _isRecommendedStep = value;
                SetBackgroundColor();
                OnPropertyChanged();
            }
        }

        private bool _isActive;

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                SetBackgroundColor();
                OnPropertyChanged();
            }
        }

        private readonly Action<GridCell> MoveToCallback;
        private readonly Action<GridCell> SetActiveCatcherCallback;

        public GridCell(GridCellStatus status, int x, int y, int player, bool isWinningCell, Action<GridCell> MoveTo, Action<GridCell> SetActiveCatcher)
        {
            Status = status;
            WinningCell = isWinningCell;
            Player = player;
            this.x = x;
            this.y = y;
            MoveToCallback = MoveTo;
            SetActiveCatcherCallback = SetActiveCatcher;
            Click = new Command(ExecuteClick);
            SetBackgroundColor();
            CatVisibility = (status == GridCellStatus.Cat) ? Visibility.Visible : Visibility.Collapsed;
            CatcherVisibility = (status == GridCellStatus.Catcher) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SetBackgroundColor()
        {
            if (IsActive)
                BackgroundColor = new SolidColorBrush(Colors.Red);
            else if (IsRecommendedStep)
                BackgroundColor = new SolidColorBrush(Colors.Yellow);
            else if (IsValidStep)
                BackgroundColor = new SolidColorBrush(Colors.Green);
            else
                BackgroundColor = new SolidColorBrush(Colors.White);
        }

        private void ExecuteClick(object param)
        {
            if (IsValidStep)
            {
                MoveToCallback(this);
            }
            if (Status == GridCellStatus.Catcher)
            {
                SetActiveCatcherCallback(this);
            }
        }
    }
}