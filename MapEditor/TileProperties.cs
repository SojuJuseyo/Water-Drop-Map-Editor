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
            size = 6;
        }

        public TileProperties(int x, int y)
        {
            x2 = x;
            y2 = y;
            size = 6;
        }

        // Determine if the properties contain text or a scripting action
        public bool isScriptedOrTexted(int x, int y)
        {
            if (String.IsNullOrEmpty(text))
            {
                if (x2 != x || y2 != y)
                    return (true);
                return (false);
            }
            return (true);
        }
    }
}
