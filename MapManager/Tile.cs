﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZombieProyect_Desktop.Classes.Tiles;

namespace ZombieProyect_Desktop.Classes
{
    public enum GridAxis
    {
        positiveX,
        negativeX,
        positiveY,
        negativeY
    }
    public enum WallTextureType
    {
        horizontal,
        vertical,
        rightbottomcorner,
        leftbottomcorner,
        righttopcorner,
        lefttopcorner,
        allbutupjoint,
        allbutrightjoint,
        allbutbottomjoint,
        allbutleftjoint,
        all,
        unknown
    }
    public abstract class Tile
    {
        private Point t_pos;
        public Point Pos
        {
            get
            {
                return t_pos;
            }
        }
        public Room parentRoom;
        public Map parentMap;

        public Tile(int x, int y, Map parentMap)
        {
            t_pos = new Point(x, y);
            this.parentMap = parentMap;
        }
        public Tile(int x, int y, Room parentRoom, Map parentMap)
        {
            t_pos = new Point(x, y);
            this.parentRoom = parentRoom;
            this.parentMap = parentMap;
        }
        
        public bool IsConnectedToTwoFloors()
        {
            int a = 0;
            if (!(Pos.X + 1>= parentMap.tileMapSize.X))
                if (((parentMap.tileMap[Pos.X + 1, Pos.Y]?.GetType()) ?? typeof(OutsideFloor)) == typeof(Floor))
                    a++;
            if (!(Pos.X - 1 <= 0))
                if (((parentMap.tileMap[Pos.X - 1, Pos.Y]?.GetType()) ?? typeof(OutsideFloor)) == typeof(Floor))
                    a++;
            if (!(Pos.Y + 1 >= parentMap.tileMapSize.Y))
                if (((parentMap.tileMap[Pos.X, Pos.Y + 1]?.GetType()) ?? typeof(OutsideFloor)) == typeof(Floor))
                    a++;
            if (!(Pos.Y - 1 <= 0))
                if (((parentMap.tileMap[Pos.X, Pos.Y - 1]?.GetType()) ?? typeof(OutsideFloor)) == typeof(Floor))
                    a++;

            if (a == 2) return true;
            else return false;
        }

        public WallTextureType GetAccordingTexture()
        {
            bool a;
            if (Pos.Y - 1 < 0) a = false; // Catch if tile is outside of map
            else a = ((parentMap.tileMap[Pos.X, Pos.Y-1]?.GetType() ?? typeof(OutsideFloor)) == typeof(Wall)) || ((parentMap.tileMap[Pos.X, Pos.Y-1]?.GetType() ?? typeof(OutsideFloor)) == typeof(Door)); // Next left tile is wall
            
            bool b;
            if (Pos.X + 1 >= parentMap.tileMapSize.X) b = false; // Catch if tile is outside of map
            else b = ((parentMap.tileMap[Pos.X +1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(Wall)) || ((parentMap.tileMap[Pos.X+1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(Door)); // Next left tile is wall
            
            bool c;
            if (Pos.Y + 1 >= parentMap.tileMapSize.Y) c = false; // Catch if tile is outside of map
            else c = ((parentMap.tileMap[Pos.X, Pos.Y+1]?.GetType() ?? typeof(OutsideFloor)) == typeof(Wall)) || ((parentMap.tileMap[Pos.X, Pos.Y+1]?.GetType() ?? typeof(OutsideFloor)) == typeof(Door)); // Next left tile is wall
            
            bool d;
            if (Pos.X - 1 < 0) d = false; // Catch if tile is outside of map
            else d = ((parentMap.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(Wall)) || ((parentMap.tileMap[Pos.X - 1, Pos.Y]?.GetType() ?? typeof(OutsideFloor)) == typeof(Door)); // Next left tile is wall
            
            if (!a && b && !c && d) // Right & Left
                return WallTextureType.horizontal;
            if (a && !b && c && !d) // Upper & Bottom
                return WallTextureType.vertical;
            if (!a && !b && c && d) // Left & Bottom
                return WallTextureType.leftbottomcorner;
            if (!a && b && c && !d) // Right & Bottom
                return WallTextureType.rightbottomcorner;
            if (a && b && !c && !d) // Right & Top
                return WallTextureType.righttopcorner;
            if (a && !b && !c && d) // Left & Top
                return WallTextureType.lefttopcorner;
            if(!a && b && c && d) // All but up
                return WallTextureType.allbutupjoint;
            if (a && !b && c && d) // All but right
                return WallTextureType.allbutrightjoint;
            if (a && b && !c && d) // All but bottom
                return WallTextureType.allbutbottomjoint;
            if (a && b && c && !d) // All but left
                return WallTextureType.allbutleftjoint;
            if (a && b && c && d) // All
                return WallTextureType.all;

            return WallTextureType.unknown;
        }
    }
}
