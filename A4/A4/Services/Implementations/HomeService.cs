using A4.Models;
using A4.Services.Interfaces;
using System.Collections.Generic;

namespace A4.Services.Implementations
{
    public class HomeService : IHomeService
    {
        public int CounterOfSheets(HomeVM homeVM)
        {
            var cells = new List<List<Cell>>();

            for (int i = 0; i < homeVM.N; i++)
            {
                var tmpList = new List<Cell>();

                for (int j = 0; j < homeVM.M; j++)
                {
                    var cell = new Cell();
                    cell.IsSelected = homeVM.Cells[i][j].IsSelected;
                    cell.Row = homeVM.Cells[i][j].Row;
                    cell.Col = homeVM.Cells[i][j].Col;


                    tmpList.Add(cell);
                }

                cells.Add(tmpList);
            }

            int counterOfSheets = 0;

            while (isExistNotSelectedCell(cells))
            {
                var cellsOfCurrentSheet = new List<Cell>();
                var firstNotSelectedCell = new Cell();
                bool isFirstNotSelectedCellFound = false;
                /*for (int i = 0; i < homeVM.N; i++)
                {
                    var firstCellsBeforeFirstNotSelectedInPreviousRow = new List<Cell>();

                    for (int j = 0; j < homeVM.M; j++)
                    {
                        if (colOfFirstNotSelectedInPreviousRow >= j)
                        {
                            if (cells[i][j].IsSelected)
                            {
                                firstCellsBeforeFirstNotSelectedInPreviousRow = new List<Cell>();
                                isFirstNotSelected = true;
                            }
                            else
                            {
                                if (isFirstNotSelected)
                                {
                                    isFirstNotSelected = false;
                                    colOfFirstNotSelectedInCurrentRow = j;
                                }
                                firstCellsBeforeFirstNotSelectedInPreviousRow.Add(cells[i][j]);
                            }

                            if (colOfFirstNotSelectedInPreviousRow == j)
                            {
                                foreach (var cell in firstCellsBeforeFirstNotSelectedInPreviousRow)
                                {
                                    cellsOfCurrentSheet.Add(cell);
                                }
                            }
                        }
                        else if (!cells[i][j].IsSelected && isFirstNotSelected)
                        {
                            isFirstNotSelected = false;
                            colOfFirstNotSelectedInCurrentRow = j;
                            cellsOfCurrentSheet.Add(cells[i][j]);
                        }
                        else if (theLastSelectedElementIsExist)
                        {
                            continue;
                        }
                        else if (cells[i][j].IsSelected && !isFirstNotSelected)
                        {
                            theLastSelectedElementIsExist = true;
                        }
                        else if (!cells[i][j].IsSelected)
                        {
                            cellsOfCurrentSheet.Add(cells[i][j]);
                        }
                    }

                    theLastSelectedElementIsExist = false;
                    colOfFirstNotSelectedInPreviousRow = colOfFirstNotSelectedInCurrentRow;
                    colOfFirstNotSelectedInCurrentRow = 0;
                }*/

                for (int i = 0; i < homeVM.N; i++)
                {
                    for (int j = 0; j < homeVM.M; j++)
                    {
                        if (!cells[i][j].IsSelected)
                        {
                            firstNotSelectedCell = cells[i][j];
                            isFirstNotSelectedCellFound = true;
                            break;
                        }
                    }

                    if (isFirstNotSelectedCellFound)
                    {
                        break;
                    }
                }

                getSheetByCell(firstNotSelectedCell, cells, cellsOfCurrentSheet);


                foreach (var cell in cellsOfCurrentSheet)
                {
                    cells[cell.Row][cell.Col].IsSelected = true;
                }

                counterOfSheets++;
            }

            return counterOfSheets;
        }

        private bool isExistNotSelectedCell(List<List<Cell>> cells)
        {
            foreach (var cellsInRow in cells)
            {
                foreach (var cell in cellsInRow)
                {
                    if (!cell.IsSelected)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void getSheetByCell(Cell firstCell, List<List<Cell>> cells, List<Cell> cellsBySheet)
        {
            cellsBySheet.Add(firstCell);

            if (firstCell.Col > 0)
            {
                var leftCell = cells[firstCell.Row][firstCell.Col - 1];

                nextCell(leftCell, cells, cellsBySheet);
            }

            if (firstCell.Row > 0)
            {
                var topCell = cells[firstCell.Row - 1][firstCell.Col];

                nextCell(topCell, cells, cellsBySheet);
            }

            if (firstCell.Col < cells[firstCell.Row].Count - 1)
            {
                var rightCell = cells[firstCell.Row][firstCell.Col + 1];

                nextCell(rightCell, cells, cellsBySheet);
            }

            if (firstCell.Row < cells.Count - 1)
            {
                var bottomCell = cells[firstCell.Row + 1][firstCell.Col];

                nextCell(bottomCell, cells, cellsBySheet);
            }
        }

        private void nextCell(Cell currentCell, List<List<Cell>> cells, List<Cell> cellsBySheet)
        {
            if (!currentCell.IsSelected && !cellsBySheet.Contains(currentCell))
            {
                getSheetByCell(currentCell, cells, cellsBySheet);
            }
        }
    }
}
