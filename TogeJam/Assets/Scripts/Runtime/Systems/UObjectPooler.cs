using System.Collections.Generic;
using UnityEngine;

//#if UNITY_EDITOR
using UnityEditor;
//#endif

namespace Game.Utility
{
    public class UObjectPooler : MonoBehaviour
    {
        [SerializeField] protected GameObject ObjectPrefab;
        [SerializeField] protected int PoolSize;
        [SerializeField] protected bool bDynamicExpand = true;
        [SerializeField] protected Transform PoolHolder;

///////////////////////////////////////////////////////////////////////

//#if UNITY_EDITOR
        void ClearPool()
        {
            foreach(Transform T in PoolHolder)
                GameObject.DestroyImmediate(T.gameObject);
        }

        public virtual void ReinitPool()
        {
            ClearPool();

            for (int i = 0; i < PoolSize; i ++)
            {
                GameObject obj = GameObject.Instantiate(ObjectPrefab, Vector3.zero, Quaternion.identity, PoolHolder);
                obj.name = obj.name + i.ToString();
                obj.SetActive(false);
            }

            Debug.Log(ObjectPrefab.name + "-PoolCount: " + PoolHolder.childCount);
        }
//#endif

        public virtual GameObject GetTopObject() 
        {
            GameObject Top = PoolHolder.GetChild(0).gameObject;
            if (Top.activeInHierarchy == false)
            {
                Top.transform.SetAsLastSibling();
                return Top;
            }
            else
            {
                if (bDynamicExpand)
                {
                    Top = GameObject.Instantiate(ObjectPrefab, Vector3.zero, Quaternion.identity, PoolHolder);
                    Top.transform.SetAsFirstSibling();
                    return Top;
                }
            }
            return null;
        }

        public virtual List<GameObject> GetAllPooledObjects()
        {
            List<GameObject> RetVal = new List<GameObject>();

            for (int i = 0; i < PoolHolder.childCount; i ++)
                RetVal.Add(PoolHolder.GetChild(i).gameObject);

            return RetVal;
        }
    }
}
