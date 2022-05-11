using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class Tile 
    {
        private GameObject m_tileGameObject;
        public GameObject TileGameObject { get { return m_tileGameObject; } set { m_tileGameObject = value; } }

        private Vector3 m_tilePostion;
        public Vector3 TilePostion { get { return m_tilePostion; } private set { m_tilePostion = value; } }

        private Room m_roomData;
        public Room RoomData { get { return m_roomData; } private set { m_roomData = value; } }

        public Tile(GameObject tileGameObject, Vector3 tilePostion)
        {
            this.TileGameObject = tileGameObject;
            this.TilePostion = tilePostion;

            RoomData = TileGameObject.GetComponent<Room>();
        }

        public Tile()
        {

        }

    }
}
