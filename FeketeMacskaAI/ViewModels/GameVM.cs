using FeketeMacskaAI.Logic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FeketeMacskaAI.ViewModels
{
    public class GameVM : ViewModel
    {
        private const int GridSize = 11;
        public GridRow[] Grid { get; set; } = new GridRow[GridSize];
        private readonly List<GridCell> validSteps = new();
        private GridCell ActiveCell;
        public List<Point> Cats { get; set; }
        public List<Point> Catchers { get; set; }

        private bool _catsWon;
        public bool CatsWon
        {
            get => _catsWon;
            set
            {
                _catsWon = value;
                if (_catsWon)
                    CatsWonCallback();
                OnPropertyChanged();
            }
        }

        private bool _catchersWon;
        public bool CatchersWon
        {
            get => _catchersWon;
            set
            {
                _catchersWon = value;
                if (_catchersWon)
                    CatchersWonCallback();
                OnPropertyChanged();
            }
        }

        public int PlayerTurn
        {
            get => Turn % Cats.Count;
        }

        private int Turn;

        private bool catcherTurn = false;
        private const int catcherPlayer = 3;
        private readonly Action CatchersWonCallback;
        private readonly Action CatsWonCallback;
        private readonly int maxCatchers;

        public GameVM(int maxCatchers, Action CatchersWon, Action CatsWon)
        {
            Cats = new List<Point>();
            Catchers = new List<Point>();
            this.maxCatchers = maxCatchers;
            CatchersWonCallback = CatchersWon;
            CatsWonCallback = CatsWon;
            InitializeGrid();
            NextTurn();
        }

        public void SetValidMoves()
        {
            var playerCoords = Cats[PlayerTurn];

            if (playerCoords.Y - 1 >= 0)
            {
                var cell = Grid[playerCoords.X].Cells[playerCoords.Y - 1];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.Y + 1 <= GridSize - 1)
            {
                var cell = Grid[playerCoords.X].Cells[playerCoords.Y + 1];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.X - 1 >= 0)
            {
                var cell = Grid[playerCoords.X - 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.X + 1 <= GridSize - 1)
            {
                var cell = Grid[playerCoords.X + 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            foreach (var step in validSteps)
            {
                step.IsValidStep = true;
            }
            if (validSteps.Count == 0)
            {
                KillCat(PlayerTurn);
                if (Cats.Count == 0)
                {
                    CatchersWon = true;
                    return;
                }
                NextTurn();
            }
        }

        private void KillCat(int player)
        {
            Grid[Cats[player].X].Cells[Cats[player].Y].Status = GridCellStatus.Empty;
            Cats.RemoveAt(player);
        }

        public bool PlayerHasValidMoves(int player)
        {
            var playerCoords = Cats[player];

            if (playerCoords.Y - 1 >= 0)
            {
                var cell = Grid[playerCoords.X].Cells[playerCoords.Y - 1];
                if (cell.Status == GridCellStatus.Empty)
                    return true;
            }
            if (playerCoords.Y + 1 <= GridSize - 1)
            {
                var cell = Grid[playerCoords.X].Cells[playerCoords.Y + 1];
                if (cell.Status == GridCellStatus.Empty)
                    return true;
            }
            if (playerCoords.X - 1 >= 0)
            {
                var cell = Grid[playerCoords.X - 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    return true;
            }
            if (playerCoords.X + 1 <= GridSize - 1)
            {
                var cell = Grid[playerCoords.X + 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    return true;
            }
            return false;
        }

        private void SetValidMovesForCatcher()
        {
            foreach (var row in Grid)
            {
                validSteps.AddRange(row.Cells.Where(cell => cell.Status == GridCellStatus.Empty));
            }
            foreach (var cell in validSteps)
            {
                cell.IsValidStep = true;
            }
        }

        private void ClearValidSteps()
        {
            foreach (var step in validSteps)
            {
                step.IsValidStep = false;
            }
            validSteps.RemoveAll((cell) => true);
        }

        private void MoveTo(GridCell to)
        {
            if (catcherTurn)
            {
                if (ActiveCell != null)
                {
                    ActiveCell.Status = GridCellStatus.Empty;
                    to.Status = GridCellStatus.Catcher;
                    to.Player = catcherPlayer;
                }
                to.Status = GridCellStatus.Catcher;
                to.Player = catcherPlayer;
                Catchers.Add(new Point(to.y, to.x));
            }
            else
            {
                to.Status = ActiveCell.Status;
                to.Player = ActiveCell.Player;
                ActiveCell.Status = GridCellStatus.Empty;

                Cats[PlayerTurn] = new Point(to.y, to.x);

                if (to.WinningCell)
                {
                    CatsWon = true;
                }
            }

            if (ActiveCell != null)
            {
                ActiveCell.IsActive = false;
                ActiveCell = null;
            }

            catcherTurn = !catcherTurn;
            ClearValidSteps();
            NextTurn();
        }

        private void SetActiveCatcher(GridCell selectedCell)
        {
            if (catcherTurn && Catchers.Count >= maxCatchers)
            {
                ActiveCell = selectedCell;
                ActiveCell.IsActive = true;
                SetValidMovesForCatcher();
            }
        }

        private void NextTurn()
        {
            if (CatsWon || CatchersWon)
                return;
            if (catcherTurn)
            {
                if (Catchers.Count < maxCatchers)
                    SetValidMovesForCatcher();
                return;
            }
            Turn++;
            SetValidMoves();
            if(validSteps.Count == 0)
            {
                return;
            }
            ActiveCell = Grid[Cats[PlayerTurn].X].Cells[Cats[PlayerTurn].Y];
            ActiveCell.IsActive = true;
        }

        private void InitializeGrid()
        {
            var addedCats = 0;
            var player = 0;
            var numberOfCats = 2;

            for (int i = 0; i < 11; i++)
            {
                var cells = new List<GridCell>();
                for (int j = 0; j < 11; j++)
                {
                    if (i == 0 || i == 10 || j == 0 || j == 11)
                    {
                        cells.Add(new GridCell(GridCellStatus.Empty, j, i, -1, true, MoveTo, SetActiveCatcher));
                    }
                    else if ((i == 5 && j == 5) || (i == 6 && j == 6))
                    {
                        if (addedCats != numberOfCats)
                        {
                            cells.Add(new GridCell(GridCellStatus.Cat, j, i, player, false, MoveTo, SetActiveCatcher));
                            Cats.Add(new Point(j, i));
                            addedCats++;
                            player++;
                        }
                    }
                    else
                    {
                        cells.Add(new GridCell(GridCellStatus.Empty, j, i, -1, false, MoveTo, SetActiveCatcher));
                    }
                }
                Grid[i] = new GridRow(cells);
            }
        }
    }
}