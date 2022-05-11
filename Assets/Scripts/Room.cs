using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGenerator
{
    /// <summary>
    /// Place on each room you want to use for level generation.
    /// </summary>
    public class Room : MonoBehaviour
    { 
        public List<Transform> ConnectionPoints;
        public bool ConnectionUp = false;
        public bool ConnectionDown = false;
    }
}
