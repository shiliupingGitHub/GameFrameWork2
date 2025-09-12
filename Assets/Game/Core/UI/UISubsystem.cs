using System.Collections.Generic;
using Game.Core.Common;
using Game.Core.Res;
using UnityEngine;

namespace Game.Core.UI
{
    public class UISubsystem : GameSubsystemWithInstance<UISubsystem>
    {
        private const string UIRootPath = "Assets/Game/Res/UI/UIRoot.prefab";
        public enum Layer
        {
            Base,
            Mid,
            Top,
            Overlay,
            ToRoot,
        }
        private readonly List<Frame> _queueFrames = new List<Frame>();
        private Transform _baseRoot;
        private Transform _topRoot;
        private Transform _midRoot;
        private GameObject _uiRoot;

        public override void OnLateLoad()
        {
            base.OnLateLoad();
            var rootTemplate = GameResSubsystem.Singleton.LoadAssetSync<GameObject>(UIRootPath);
            _uiRoot = Object.Instantiate(rootTemplate);
            Object.DontDestroyOnLoad(_uiRoot);
            _baseRoot = _uiRoot.transform.Find("Canvas/base");
            _topRoot = _uiRoot.transform.Find("Canvas/top");
            _midRoot = _uiRoot.transform.Find("Canvas/mid");
        }

        public void Hide<T>() where T : Frame
        {
            T curFrame = null;
            foreach (var frame in _queueFrames)
            {
                if (frame.GetType() == typeof(T))
                {
                    curFrame = frame as T;
                    break;
                }
            }

            if (curFrame != null)
            {
                curFrame.Hide();
            }
        }
        public T Show<T>(bool bUseQueue = true, Layer layer = Layer.Base, bool isBottom = false) where T : Frame
        {
            T ret = null;
            if (bUseQueue)
            {
                foreach (var frame in _queueFrames)
                {
                    if (frame.GetType() == typeof(T))
                    {
                        ret = frame as T;
                        _queueFrames.Remove(frame);
                        break;
                    }
                }
            }

            if (ret == null)
            {
                ret = System.Activator.CreateInstance<T>();

                ret.Init(GetRootTr(layer));
            }

            if (null != ret.FrameGo)
            {
                ret.FrameGo.transform.SetAsLastSibling();
            }

            _queueFrames.Add(ret);
            ret.Show();
            ret.UseQueue = bUseQueue;
            

            return ret;
        }
        public void RemoveFromQueue(Frame frame)
        {
            _queueFrames.Remove(frame);
            frame.Destroy();
        }
        Transform GetRootTr(Layer layer)
        {
            switch (layer)
            {
                case Layer.Top:
                    return _topRoot;
                case Layer.Mid:
                    return _midRoot;
                case Layer.Overlay:
                    return _uiRoot.transform;
                case Layer.ToRoot:
                    return _uiRoot.transform;
                default:
                    return _baseRoot;
            }
        }
    }
}