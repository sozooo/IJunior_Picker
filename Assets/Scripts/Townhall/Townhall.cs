using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StoneDetector))]
public class Townhall : Purpose
{
    [SerializeField] private float _maxPickerCount;
    [SerializeField] private Picker _pickerPrefab;
    [SerializeField] private StoneSpawner _spawner;
    [SerializeField] private Vector3 _halfExtentsForCollect;

    private float _resourceCount = 0;
    private StoneDetector _detector;

    private Queue<Stone> _stoneQueue;
    private Queue<Picker> _pickers;
    private List<Stone> _proceedStones;
    private Collider[] _scannedObjects;

    public event Action<float> OnResourseCountChange;

    public float ResourceCount { 
        get { return _resourceCount; } 
        private set 
        {
            _resourceCount = value;

            OnResourseCountChange?.Invoke(_resourceCount);
        }
    }

    private void Awake()
    {
        _detector = GetComponent<StoneDetector>();

        _stoneQueue = new Queue<Stone>();
        _pickers = new Queue<Picker>();
        _proceedStones = new List<Stone>();
    }

    private void Start()
    {
        for(int i = 0; i < _maxPickerCount; i++)
        {
            _pickers.Enqueue(Instantiate(_pickerPrefab, transform));
        }
    }

    private void OnEnable()
    {
        _spawner.OnSpawn += AddStones;
    }

    private void OnDisable()
    {
        _spawner.OnSpawn -= AddStones;
    }

    public void AddResource(float amount)
    {
        _resourceCount += amount;

        OnResourseCountChange?.Invoke(_resourceCount);
    }

    public void AddStones()
    {
        foreach (Stone stone in _detector.ScanForStones())
        {
            if (_stoneQueue.Contains(stone) == false && _proceedStones.Contains(stone) == false)
            {
                _stoneQueue.Enqueue(stone);

                SpawnPicker();
            }
        }
    }

    public void CollectResource()
    {
        _scannedObjects = Physics.OverlapBox(transform.position, _halfExtentsForCollect);


        foreach (Collider collider in _scannedObjects)
        {
            if (collider.TryGetComponent(out Stone stone))
            {
                stone.transform.parent = null;
                ResourceCount += stone.GiveResources();
            }
        }
    }

    private void DespawnPicker(Picker picker)
    {
        picker.OnWorksDone -= DespawnPicker;
        CollectResource();
        AddStones();
      
        _proceedStones.Remove(picker.CurrentStone);
        picker.gameObject.SetActive(false);

        _pickers.Enqueue(picker);

        if(_stoneQueue.Count != 0)
        {
            SpawnPicker();
        }
    }

    private void SpawnPicker()
    {
        if (_stoneQueue.Count != 0 && _pickers.Count != 0)
        {
            Stone stone = _stoneQueue.Dequeue();
            _proceedStones.Add(stone);

            Picker picker = _pickers.Dequeue();

            picker.gameObject.SetActive(true);
            picker.StartWork(stone);
            SpawnPicker();

            picker.OnWorksDone += DespawnPicker;
        }
    }
}
