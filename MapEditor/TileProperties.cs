using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor
{
    public enum ORIENTATION
    {
        RIGHT,
        LEFT
    }

    public class TileProperties
    {
        public string text { get; set; }
        public int x2 { get; set; }
        public int y2 { get; set; }
        public int size { get; set; }
        public ORIENTATION orientation { get; set; }

        public TileProperties()
        {
            size = 40;
        }
    }
}
