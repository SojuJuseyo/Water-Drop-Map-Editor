﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MapEditor
{
    public enum SpriteType
    {
        BLOCK,
        PLAYER,
        ENEMY
    }

    public class tile
    {
        [JsonIgnore]
        public ImageBrush tileSprite { get; set; }
        [JsonIgnore]
        public bool heatZone { get; set; }
        [JsonIgnore]
        public SpriteType spriteType { get; set; }

        public bool collidable { get; set; }
        public int coordx { get; set; }
        public int coordy { get; set; }

        public TileProperties properties { get; set; }
    }
}
