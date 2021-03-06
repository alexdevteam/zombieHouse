﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZombieProyect_Desktop.Classes
{
    public enum FurnitureAnchor
    {
        top,
        sides,
        center,
        corners
    }

    public struct FurnitureProperties
    {
        public FurnitureAnchor anchor;
        public float chance;
        
        public FurnitureProperties(FurnitureAnchor ancho, float chanc)
        {
            this.anchor = ancho;
            this.chance = chanc;
        }
    }

    public class RoomType
    {
        public string name;
        public int floorType;
        public int wallpaperType;
        public Dictionary<string, float> relations = new Dictionary<string, float>();
        public Dictionary<string, FurnitureProperties> furniture = new Dictionary<string, FurnitureProperties>();

        public RoomType(string name, int wallpaper, int floor, Dictionary<string, float> relations, Dictionary<string, FurnitureProperties> furn)
        {
            this.name = name;
            wallpaperType = wallpaper;
            floorType = floor;
            this.relations = relations;
            furniture = furn;
        }

        public static RoomType ParseFromXML(XmlDocument doc, XmlNode node)
        {
            int wallpaperType = Int32.Parse(node.SelectSingleNode("wallpaper").InnerText);
            int floorType = Int32.Parse(node.SelectSingleNode("floor").InnerText);
            Dictionary<string, float> relations = new Dictionary<string, float>();
            Dictionary<string, FurnitureProperties> furn = new Dictionary<string, FurnitureProperties>();
            foreach (XmlNode r in node.SelectSingleNode("relations"))
                relations.Add(r.Attributes["ref"].Value, float.Parse(r.Attributes["chance"].Value));
            foreach (XmlNode r in node.SelectSingleNode("furniture"))
                furn.Add(r.Attributes["ref"].Value, new FurnitureProperties((FurnitureAnchor)Enum.Parse(typeof(FurnitureAnchor), r.Attributes["anchor"].Value), float.Parse(r.Attributes["chance"].Value)));
            return new RoomType(node.Attributes["name"].Value,wallpaperType,floorType,relations, furn);
        }

        public static RoomType ParseFromXML(XmlDocument doc, string node)
        {
            return ParseFromXML(doc, doc.SelectSingleNode("/rooms/room[@name='"+node+"']"));
        }

        public static List<RoomType> GetAllRoomTypes(XmlDocument doc)
        {
            XmlNodeList xmlnodes=doc.ChildNodes[1].ChildNodes;
            List<RoomType> types = new List<RoomType>();
            foreach(XmlNode n in xmlnodes)
            {
                types.Add(ParseFromXML(doc, n));
            }
            return types;
        }

        /// <summary>
        /// Obtains a random RoomType from the given document.
        /// </summary>
        /// <param name="doc">The document to use.</param>
        /// <param name="withRelations">Specify whether to check relations or not. If true, rooms without any relations will not be returned.</param>
        /// <returns></returns>
        public static RoomType GetRandomRoomType(XmlDocument doc, bool withRelations = false)
        {
            List<RoomType> rooms = GetAllRoomTypes(doc);
            RoomType result;
            if (withRelations)
            {
                List<RoomType> relationRooms = rooms.Where(x => x.relations.Count != 0).ToList();
                result = relationRooms[Map.r.Next(0, relationRooms.Count)];
            }
            else
                result = rooms[Map.r.Next(0, rooms.Count)];

            return result;
        }
    }
}
