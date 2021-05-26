using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeketeMacskaAI.Logic
{
    public static class GridRowExtensions
    {
        public static List<GridCell> GetValidStepsForPlayer(this GridRow[] board, Point playerCoords)
        {
            var validSteps = new List<GridCell>();
            var GridSize = board.Length;

            if (playerCoords.Y - 1 >= 0)
            {
                var cell = board[playerCoords.X].Cells[playerCoords.Y - 1];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.Y + 1 <= GridSize - 1)
            {
                var cell = board[playerCoords.X].Cells[playerCoords.Y + 1];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.X - 1 >= 0)
            {
                var cell = board[playerCoords.X - 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }
            if (playerCoords.X + 1 <= GridSize - 1)
            {
                var cell = board[playerCoords.X + 1].Cells[playerCoords.Y];
                if (cell.Status == GridCellStatus.Empty)
                    validSteps.Add(cell);
            }

            return validSteps;
        }
    }
}
