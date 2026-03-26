using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GolfBall golfBallPrefab;
    [SerializeField] private BallInfo ballInfoPrefab;
    [SerializeField] private LineRenderer lineRendererPrefab;

    public static PoolManager Instance { get; private set; }

    private readonly Dictionary<Type, IPool> _pools = new();
    private Transform _root;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _root = new GameObject("Pools").transform;
        _root.SetParent(transform, false);
        
        Register(golfBallPrefab, prewarm: 5, maxSize: 1000, expandable: true);
        Register(ballInfoPrefab, prewarm: 5, maxSize: 1000, expandable: true);
        Register(lineRendererPrefab, prewarm: 5, maxSize: 5000, expandable: true);
    }

    public void Register<T>(T prefab, int prewarm = 0, int maxSize = int.MaxValue, bool expandable = true)
        where T : Component
    {
        if (prefab == null) throw new ArgumentNullException(nameof(prefab));

        var type = typeof(T);
        if (_pools.ContainsKey(type))
        {
            Debug.LogWarning($"Pool already registered: {type.Name}");
            return;
        }

        var poolRoot = new GameObject($"Pool_{type.Name}").transform;
        poolRoot.SetParent(_root, false);

        var pool = new Pool<T>(prefab, poolRoot, prewarm, maxSize, expandable);
        _pools[type] = pool;
    }

    public T GetFromPool<T>(Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        where T : Component
    {
        if (!_pools.TryGetValue(typeof(T), out var poolBase))
        {
            Debug.LogError($"Pool is not found: {typeof(T).Name}");
            return null;
        }

        return ((Pool<T>)poolBase).Get(position, rotation, parent);
    }

    public void ReturnToPool<T>(T instance) where T : Component
    {
        if (instance == null) return;

        if (!_pools.TryGetValue(typeof(T), out var poolBase))
        {
            Debug.LogWarning($"Pool is not found: ({typeof(T).Name}). Destroying : {instance.name}");
            Destroy(instance.gameObject);
            return;
        }

        ((Pool<T>)poolBase).Return(instance);
    }

    public void ReturnToPool(Component instance)
    {
        if (instance == null) return;
        var type = instance.GetType();

        if (!_pools.TryGetValue(type, out var poolBase))
        {
            Debug.LogWarning($"Pool is not found: ({type.Name}). Destroying: {instance.name}");
            Destroy(instance.gameObject);
            return;
        }

        poolBase.ReturnBoxed(instance);
    }

    private interface IPool
    {
        void ReturnBoxed(Component c);
    }

    private sealed class Pool<T> : IPool where T : Component
    {
        private readonly Stack<T> _inactive = new();
        private readonly Queue<T> _activeQueue = new(); // FIFO reuse order

        private readonly T _prefab;
        private readonly Transform _root;
        private readonly bool _expandable;
        private readonly int _maxSize;

        private int _totalCreated;

        public Pool(T prefab, Transform root, int prewarm, int maxSize, bool expandable)
        {
            _prefab = prefab;
            _root = root;
            _expandable = expandable;
            _maxSize = Mathf.Max(1, maxSize);

            for (int i = 0; i < prewarm; i++)
            {
                var inst = CreateNew();
                inst.gameObject.SetActive(false);

                if (inst.TryGetComponent<IPoolable>(out var p))
                {
                    p.IsInPool = true;
                    p.OnReturnPool();
                }

                _inactive.Push(inst);
            }
        }

        private T CreateNew()
        {
            _totalCreated++;
            var inst = UnityEngine.Object.Instantiate(_prefab, _root);
            return inst;
        }

        public T Get(Vector3 pos, Quaternion rot, Transform parent)
        {
            T inst;

            if (_inactive.Count > 0)
            {
                inst = _inactive.Pop();
                if (inst == null) return Get(pos, rot, parent);
            }
            else if (_totalCreated < _maxSize && _expandable)
            {
                inst = CreateNew();
            }
            else
            {
                // 🚨 Limit reached → recycle oldest active
                inst = _activeQueue.Dequeue();
                ForceReturn(inst);
            }

            if (inst.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnPerformanceReUse();
                poolable.IsInPool = false;
                poolable.OnRemoveFromPool();
            }

            var tr = inst.transform;
            if (parent != null) tr.SetParent(parent, false);
            tr.SetPositionAndRotation(pos, rot);

            inst.gameObject.SetActive(true);
            _activeQueue.Enqueue(inst);

            return inst;
        }

        private void ForceReturn(T inst)
        {
            if (inst == null) return;

            if (inst.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.IsInPool = true;
                poolable.OnReturnPool();
            }

            inst.gameObject.SetActive(false);
            inst.transform.SetParent(_root, false);
        }

        public void Return(T inst)
        {
            if (inst == null) return;

            if (inst.TryGetComponent<IPoolable>(out var poolable))
            {
                if (poolable.IsInPool)
                {
                    //Debug.LogWarning($"[Pool<{typeof(T).Name}>] Double return prevented: {inst.name}");
                    return;
                }

                poolable.IsInPool = true;
                poolable.OnReturnPool();
            }

            inst.gameObject.SetActive(false);
            inst.transform.SetParent(_root, false);

            _inactive.Push(inst);
            RemoveFromActiveQueue(inst);
        }

        private void RemoveFromActiveQueue(T inst)
        {
            if (_activeQueue.Count == 0) return;

            int count = _activeQueue.Count;
            for (int i = 0; i < count; i++)
            {
                var item = _activeQueue.Dequeue();
                if (!ReferenceEquals(item, inst))
                    _activeQueue.Enqueue(item);
            }
        }

        public void ReturnBoxed(Component c)
        {
            if (c is T t) Return(t);
            else Debug.LogError($"[Pool<{typeof(T).Name}>] Wrong type sent: {c.GetType().Name}");
        }
    }
}


public interface IPoolable
{
    bool IsInPool { get; set; }

    void OnRemoveFromPool();
    void OnReturnPool();

    void OnPerformanceReUse();
}

public sealed class PoolState : MonoBehaviour
{
    public bool IsInPool;
}