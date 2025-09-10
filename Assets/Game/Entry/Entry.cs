using System;
using Game.Common;
using UnityEngine;

namespace Game.Entry
{
    public class Entry : MonoBehaviour
    {
        void Start()
        {
            new GameInstance().Load();
        }
    }
}