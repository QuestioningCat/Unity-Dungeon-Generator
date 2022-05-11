using UnityEngine;

namespace DungeonGenerator
{
    public class Level
    {

        

        private int m_cellSize;
        private GameObject m_levelGameObject;
        private Vector3Int m_levelGridSize;
        private TileMapSO m_levelTileMap;

        private Tile[,,] m_tileArray;

        private int m_roomCount = 0;

        private bool m_debugMode = false;
        private bool m_lineDepth = true;

        public Level(string levelName, TileMapSO levelTileMap, Vector3Int size, int cellSize = 10, bool debugMode = false)
        {
            this.m_cellSize = cellSize;
            this.m_levelTileMap = levelTileMap;
            this.m_levelGridSize = size;
            m_tileArray = new Tile[size.x, size.y, size.z];

            m_debugMode = debugMode;

            m_levelGameObject = new GameObject();
            m_levelGameObject.name = levelName;

            GameObject spawnOG = GameObject.Instantiate(m_levelTileMap.EntryTile);
            spawnOG.transform.parent = m_levelGameObject.transform;
            int halfX = Mathf.RoundToInt(size.x * 0.5f);
            int halfY = Mathf.RoundToInt(size.y * 0.5f);
            int halfZ = Mathf.RoundToInt(size.z * 0.5f);
            spawnOG.transform.position = GetPostionInWorld(halfX, halfY, halfZ) + new Vector3(m_cellSize, 0, m_cellSize) * 0.5f;

            m_tileArray[halfX, halfY, halfZ] = new Tile(spawnOG, spawnOG.transform.position);

            foreach(Transform t in m_tileArray[halfX, halfY, halfZ].RoomData.ConnectionPoints)
            {
                GenerateTileAtConneciton(t);
            }

            // Draw lines to visualize  the grid..
            #region Debug
            if(m_debugMode == false) { return; }

            for(int x = 0; x < m_tileArray.GetLength(0); x++)
            {
                for(int y = 0; y < m_tileArray.GetLength(1); y++)
                {
                    for(int z = 0; z < m_tileArray.GetLength(2); z++)
                    {
                        Debug.DrawLine(GetPostionInWorld(x, y, z), GetPostionInWorld(x, y, z + 1), Color.white, 100f, m_lineDepth);
                        Debug.DrawLine(GetPostionInWorld(x, y, z), GetPostionInWorld(x, y + 1, z), Color.white, 100f, m_lineDepth);
                        Debug.DrawLine(GetPostionInWorld(x, y, z), GetPostionInWorld(x + 1, y, z), Color.white, 100f, m_lineDepth);

                        Debug.DrawLine(GetPostionInWorld(0, m_tileArray.GetLength(1), z), GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), z), Color.white, 100f, m_lineDepth);
                        Debug.DrawLine(GetPostionInWorld(m_tileArray.GetLength(0), 0, z), GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), z), Color.white, 100f, m_lineDepth);
                    }
                    Debug.DrawLine(GetPostionInWorld(m_tileArray.GetLength(0), y, 0), GetPostionInWorld(m_tileArray.GetLength(0), y, m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
                    Debug.DrawLine(GetPostionInWorld(0, y, m_tileArray.GetLength(2)), GetPostionInWorld(m_tileArray.GetLength(0), y, m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
                }
                Debug.DrawLine(GetPostionInWorld(x, 0, m_tileArray.GetLength(2)), GetPostionInWorld(x, m_tileArray.GetLength(1), m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
                Debug.DrawLine(GetPostionInWorld(x, m_tileArray.GetLength(1), 0), GetPostionInWorld(x, m_tileArray.GetLength(1), m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
            }

            Debug.DrawLine(GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), 0), GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
            Debug.DrawLine(GetPostionInWorld(m_tileArray.GetLength(0), 0, m_tileArray.GetLength(2)), GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
            Debug.DrawLine(GetPostionInWorld(0, m_tileArray.GetLength(1), m_tileArray.GetLength(2)), GetPostionInWorld(m_tileArray.GetLength(0), m_tileArray.GetLength(1), m_tileArray.GetLength(2)), Color.white, 100f, m_lineDepth);
            #endregion Debug

            #region Debug test
            //if(debugMode == false) { return; }

            //int halfX = Mathf.RoundToInt(m_tileArray.GetLength(0) * 0.5f);
            //int halfY = Mathf.RoundToInt(m_tileArray.GetLength(1) * 0.5f);
            //int halfZ = Mathf.RoundToInt(m_tileArray.GetLength(2) * 0.5f);

            //for(int x = -halfX; x < halfX; x++)
            //{
            //    for(int y = -halfY ; y < halfY; y++)
            //    {
            //        for(int z = -halfZ; z < halfZ; z++)
            //        {
            //            Debug.DrawLine(GetWorldPostion(x, y, z), GetWorldPostion(x, y, z + 1), Color.white, 100f);
            //            Debug.DrawLine(GetWorldPostion(x, y, z), GetWorldPostion(x, y + 1, z), Color.white, 100f);
            //            Debug.DrawLine(GetWorldPostion(x, y, z), GetWorldPostion(x + 1, y, z), Color.white, 100f);

            //            Debug.DrawLine(GetWorldPostion(-halfX, halfY, z), GetWorldPostion(halfX, halfY, z), Color.white, 100f);
            //            Debug.DrawLine(GetWorldPostion(halfX, -halfY, z), GetWorldPostion(halfX, halfY, z), Color.white, 100f);
            //        }
            //        Debug.DrawLine(GetWorldPostion(halfX, y, -halfZ), GetWorldPostion(halfX, y, halfZ), Color.white, 100f);
            //        Debug.DrawLine(GetWorldPostion(-halfX, y, halfZ), GetWorldPostion(halfX, y, halfZ), Color.white, 100f);
            //    }
            //    Debug.DrawLine(GetWorldPostion(x, -halfY, halfZ), GetWorldPostion(x, halfY, halfZ), Color.white, 100f);
            //    Debug.DrawLine(GetWorldPostion(x, halfY, -halfZ), GetWorldPostion(x, halfY, halfZ), Color.white, 100f);
            //}

            //Debug.DrawLine(GetWorldPostion(halfX, halfY, -halfZ), GetWorldPostion(halfX, halfY, halfZ), Color.white, 100f);
            //Debug.DrawLine(GetWorldPostion(halfX, -halfY, halfZ), GetWorldPostion(halfX, halfY, halfZ), Color.white, 100f);
            //Debug.DrawLine(GetWorldPostion(-halfX, halfY, halfZ), GetWorldPostion(halfX, halfY, halfZ), Color.white, 100f);
            #endregion Debug
        }




        private void GenerateTileAtConneciton(Transform connection)
        {

            Vector3 nextCellWorldPos = connection.position + connection.forward;//* (m_cellSize * 0.5f);
            int x, y, z;
            GetXYZInGrid(nextCellWorldPos, out x, out y, out z);

            if(x >= m_levelGridSize.x || x < 0) { return; }
            if(y >= m_levelGridSize.y || y < 0) { return; }
            if(z >= m_levelGridSize.z || z < 0) { return; }

            if(m_tileArray[x, y, z] != null) { return; }

            if(m_debugMode)
            {
                #region Debug

                Vector3[] vertices = {
                new Vector3 (0, 0, 0), // 0
                new Vector3 (1, 0, 0), // 1
                new Vector3 (1, 1, 0), // 2
                new Vector3 (0, 1, 0), // 3
                new Vector3 (0, 1, 1), // 4
                new Vector3 (1, 1, 1), // 5
                new Vector3 (1, 0, 1), // 6
                new Vector3 (0, 0, 1), // 7
            };

                Debug.DrawLine(nextCellWorldPos + vertices[0], nextCellWorldPos + vertices[3], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[0], nextCellWorldPos + vertices[1], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[0], nextCellWorldPos + vertices[7], Color.green, 100f);

                Debug.DrawLine(nextCellWorldPos + vertices[1], nextCellWorldPos + vertices[6], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[1], nextCellWorldPos + vertices[2], Color.green, 100f);

                Debug.DrawLine(nextCellWorldPos + vertices[3], nextCellWorldPos + vertices[4], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[3], nextCellWorldPos + vertices[2], Color.green, 100f);

                Debug.DrawLine(nextCellWorldPos + vertices[4], nextCellWorldPos + vertices[5], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[4], nextCellWorldPos + vertices[7], Color.green, 100f);

                Debug.DrawLine(nextCellWorldPos + vertices[5], nextCellWorldPos + vertices[4], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[5], nextCellWorldPos + vertices[6], Color.green, 100f);
                Debug.DrawLine(nextCellWorldPos + vertices[5], nextCellWorldPos + vertices[2], Color.green, 100f);

                Debug.DrawLine(nextCellWorldPos + vertices[7], nextCellWorldPos + vertices[6], Color.green, 100f);

                #endregion Debug
            }

            m_roomCount++;

            GameObject prefab = m_levelTileMap.Tiles[Random.Range(0, m_levelTileMap.Tiles.Count)];

            if(x == m_levelGridSize.x - 1 || x == 0 || y == m_levelGridSize.y - 1 || y == 0 || z == m_levelGridSize.z - 1 || z == 0 || m_roomCount > x*2    )
            {
                prefab = m_levelTileMap.CloseTile;
            }

            GameObject spawnOG = GameObject.Instantiate(prefab);
            spawnOG.transform.parent = m_levelGameObject.transform;
            spawnOG.transform.position = GetPostionInWorld(x, y, z) + (new Vector3(m_cellSize, 0, m_cellSize) * 0.5f);
            spawnOG.transform.rotation = connection.rotation;
            m_tileArray[x, y, z] = new Tile(spawnOG, spawnOG.transform.position);

            if(m_tileArray[x, y, z].RoomData.ConnectionUp)
            {
                m_tileArray[x, y + 1, z] = new Tile();
            }
            else if(m_tileArray[x, y, z].RoomData.ConnectionDown)
            {
                m_tileArray[x, y - 1, z] = new Tile();
            }

            foreach(Transform t in m_tileArray[x, y, z].RoomData.ConnectionPoints)
            {
                GenerateTileAtConneciton(t);
            }


        }

        private Vector3Int GetPostionInWorld(int x, int y, int z)
        {
            return new Vector3Int(x, y, z) * m_cellSize;
        }

        private void GetXYZInGrid(Vector3 worldPosition, out int x, out int y, out int z)
        {
            x = Mathf.FloorToInt(worldPosition.x / m_cellSize);
            y = Mathf.FloorToInt(worldPosition.y / m_cellSize);
            z = Mathf.FloorToInt(worldPosition.z / m_cellSize);
        }

    }
}
