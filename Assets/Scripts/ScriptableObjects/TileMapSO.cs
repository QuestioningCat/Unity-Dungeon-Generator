using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    [CreateAssetMenu(fileName = "New tile map", menuName = "TileMaps/ Basic tile map")]
    public class TileMapSO : ScriptableObject
    {
        public List<GameObject> Tiles;

        public GameObject EntryTile;
        public GameObject CloseTile;
    }
}
