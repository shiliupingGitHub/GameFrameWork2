using Game.Core.Common;
using UnityEngine;

namespace Game.Core.Entry
{
    public class Entry : MonoBehaviour
    {
        void Start()
        {
            new GameInstance().Load();
        }
    }
}