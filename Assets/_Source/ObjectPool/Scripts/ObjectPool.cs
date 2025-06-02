using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PocketZoneTest
{
    public class ObjectPool: IInitializable
    {
        private readonly Pool[] _poolArray = null;

        private Transform _objectPoolTransform;
        private readonly Dictionary<int, Queue<Component>> _poolDictionary = new();

        private readonly DiContainer _container;

        public ObjectPool(Pool[] poolArray, DiContainer container)
        {
            _poolArray = poolArray;
            _container = container;
        }

        public void Initialize()
        {
            _objectPoolTransform = new GameObject("ObjectPool").transform;

            for (int i = 0; i < _poolArray.Length; i++)
            {
                CreatePool(_poolArray[i].prefab, _poolArray[i].poolSize, _poolArray[i].componentType, _poolArray[i].isInjected);
            }
        }

        private void CreatePool(GameObject prefab, int poolSize, string componentType, bool isInjected = false)
        {
            int poolKey = prefab.GetInstanceID();

            string prefabName = prefab.name;

            GameObject parentGameObject = new(prefabName + "anchor");

            parentGameObject.transform.SetParent(_objectPoolTransform);

            if (_poolDictionary.ContainsKey(poolKey) == false)
            {
                _poolDictionary.Add(poolKey, new Queue<Component>());

                for (int i = 0; i < poolSize; i++)
                {
                    GameObject newObject;
                    if (!isInjected)
                    {
                        newObject = GameObject.Instantiate(prefab, parentGameObject.transform);
                    }
                    else
                    {
                        newObject = _container.InstantiatePrefab(prefab, parentGameObject.transform);
                    }

                    newObject.SetActive(false);

                    _poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
                }
            }
        }

        public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation, bool activateByDefault = false)
        {
            int poolKey = prefab.GetInstanceID();

            if (_poolDictionary.ContainsKey(poolKey))
            {
                Component componentToReuse = GetComponentFromPool(poolKey);

                ResetObject(position, rotation, componentToReuse, prefab);

                if (activateByDefault)
                {
                    componentToReuse.gameObject.SetActive(true);
                }

                return componentToReuse;
            }
            else
            {
                Debug.Log($"No object pool for {prefab}");
                return null;
            }

        }

        private Component GetComponentFromPool(int poolKey)
        {
            Component componentToReuse = _poolDictionary[poolKey].Dequeue();
            _poolDictionary[poolKey].Enqueue(componentToReuse);

            if (componentToReuse.gameObject.activeSelf == true)
            {
                componentToReuse.gameObject.SetActive(false);
            }

            return componentToReuse;
        }

        private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
        {
            componentToReuse.transform.SetPositionAndRotation(position, rotation);
            componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
        }
    }

}