using System.Collections.Generic;
using System.Reflection;
using Game.Core.Res;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Core.UI
{
    
    
    public class Frame
    {
        private GameObject _gameObject;

        public GameObject FrameGo => _gameObject;
        public FrameScript FrameScript { get; private set; }

        protected virtual string ResPath => string.Empty;
        private CanvasGroup _canvasGroup;
        public bool UseQueue { get; set; }
        
        public virtual void Init(Transform parent)
        {
            if (!string.IsNullOrEmpty(ResPath))
            {
                var asset = GameResSubsystem.Singleton.LoadAssetSync<GameObject>(ResPath);
                _gameObject = UnityEngine.Object.Instantiate(asset, parent);
                _canvasGroup = _gameObject.AddComponent<CanvasGroup>();
                FrameScript = _gameObject.GetComponent<FrameScript>();
                if (null == FrameScript)
                {
                    FrameScript = _gameObject.AddComponent<FrameScript>();
                }

                InitField(_gameObject, this);
            }
        }
        

        public static void InitItem<TK>(Transform root, ref List<TK> list) where TK : AutoUIElement
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                var item = System.Activator.CreateInstance<TK>();
                InitField(child.gameObject, item);
                item.Go = child.gameObject;
                item.Init();
                list.Add(item);
            }
        }

        protected static void InitItemWithName<TK>(Transform root, string name, ref List<TK> list)
            where TK : AutoUIElement
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.name.Contains(name))
                {
                    var item = System.Activator.CreateInstance<TK>();
                    InitField(child.gameObject, item);
                    item.Go = child.gameObject;
                    item.Init();
                    list.Add(item);
                }
            }
        }

        protected static void InitComponentWithName<TK>(Transform root, string name, ref List<TK> list)
            where TK : Component
        {
            for (int i = 0; i < root.childCount; i++)
            {
                var child = root.GetChild(i);
                if (child.name.Contains(name))
                {
                    var item = child.GetComponent<TK>();

                    list.Add(item);
                }
            }
        }

        public static void InitField(GameObject go, System.Object o)
        {
            foreach (var field in o.GetType().GetFields())
            {
                if (field.IsStatic)
                {
                    continue;
                }

                var pathAttribute = field.GetCustomAttribute<UIPathAttribute>();

                if (null != pathAttribute)
                {
                    var transform = go.transform;
                    if (!string.IsNullOrEmpty(pathAttribute.Path))
                    {
                        transform = go.transform.Find(pathAttribute.Path);
                    }

                    if (null != transform)
                    {
                        if (field.FieldType == typeof(GameObject))
                        {
                            field.SetValue(o, transform.gameObject);
                        }
                        else if (field.FieldType == typeof(Transform))
                        {
                            field.SetValue(o, transform);
                        }
                        else if ((typeof(AutoUIElement)).IsAssignableFrom(field.FieldType))
                        {
                            if (System.Activator.CreateInstance(field.FieldType) is AutoUIElement elem)
                            {
                                InitField(transform.gameObject, elem);
                                elem.Go = transform.gameObject;
                                elem.Init();
                                field.SetValue(o, elem);
                            }
                        }
                        else
                        {
                            var component = transform.GetComponent(field.FieldType);
                            field.SetValue(o, component);
                        }
                    }
                    else
                    {
                        if (pathAttribute.WithErrorLog)
                            Debug.LogError($@"path {pathAttribute.Path}does not int {go.name}");
                    }
                }
            }
        }

        public virtual void Destroy()
        {
            if (null != FrameGo)
            {
                Object.Destroy(FrameGo);
                _gameObject = null;
            }

            OnDestroy();
        }

        public void Show()
        {
            if (null != _canvasGroup)
            {
                _canvasGroup.alpha = 1.0f;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }

            OnShow();
        }
        public void Hide()
        {

            if (null != _canvasGroup)
            {
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                OnHide();
            }

            if (!UseQueue)
            {
                UISubsystem.Singleton.RemoveFromQueue(this);
            }
        }

        protected virtual void OnShow()
        {
         
        }

        protected virtual void OnHide()
        {
        }


        protected virtual void OnDestroy()
        {
        }
    }
}