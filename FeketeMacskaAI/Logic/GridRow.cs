using System.Collections.Generic;

namespace FeketeMacskaAI.Logic
{
    public class GridRow : ViewModel
    {
        public List<GridCell> Cells { get; private set; }

        public GridRow(List<GridCell> cells)
        {
            Cells = cells;
        }
    }
}