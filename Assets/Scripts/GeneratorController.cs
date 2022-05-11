using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    public class GeneratorController : MonoBehaviour
    {
        [Header("Level generations settings")]
        [SerializeField] private Vector3Int m_levelSize;
        [SerializeField] private int m_cellSize;

        [SerializeField] TileMapSO m_levelTileMap;


        [Header("Debug")]
        [SerializeField] private bool m_launchInDebugMode = false;

        private Level m_level;

        // Start is called before the first frame update
        void Start()
        {
            m_level = new Level("Test Level", m_levelTileMap, m_levelSize, m_cellSize, m_launchInDebugMode);
        }
    }
}
