using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI
{
    public class FrameScript : MonoBehaviour
    {

        public bool IsStart { get;private set; }
        public Dictionary<string, string> uiData = new();

        private void Start()
        {
            IsStart = true;
        }
        
    }
}