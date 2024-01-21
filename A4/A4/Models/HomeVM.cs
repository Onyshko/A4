using System.Collections.Generic;

namespace A4.Models
{
    public class HomeVM
    {
        public int N { get; set; }
        public int M { get; set; }
        public int? SheetsCounter { get; set; }
        public List<List<Cell>> Cells { get; set; } = new List<List<Cell>>();
    }
}
