using System.Collections.Generic;
using System.Linq;

namespace FeketeMacskaAI.Logic
{
    public class GridRow : ViewModel
    {
        public List<GridCell> Cells { get; private set; }

        public List<GridCell> GetWinningCells => Cells.Where(c => c.WinningCell).ToList();

        public GridRow(List<GridCell> cells)
        {
            Cells = cells;
        }
    }
}