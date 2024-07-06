using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FlagPlacer))]
public class Townhall : Purpose
{
    [SerializeField] private float _startPickerCount;
    [SerializeField] private float _pickerCost;
    [SerializeField] private float _buildCost;
    [SerializeField] private Picker _pickerPrefab;
    [SerializeField] private float _collectRadius;

    private float _resourceCount = 0;
    private FlagPlacer _flagPlacer;
    private StoneDetector _detector;

    private Queue<TownhallFlag> _flagQueue;
    private Queue<Picker> _pickers;
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
        _detector = FindAnyObjectByType<StoneDetector>();

        _flagPlacer = GetComponent<FlagPlacer>();
        _flagQueue = new Queue<TownhallFlag>();
        _pickers = new Queue<Picker>();
    }

    private void OnEnable()
    {
        _detector.OnDetect += SpawnPicker;
        _flagPlacer.OnPLace += AddFlag;
    }

    private void Start()
    {
        for (int i = 0; i < _startPickerCount; i++)
        {
            AddPicker();
        }
    }

    private void OnDisable()
    {
        _detector.OnDetect -= SpawnPicker;
        _flagPlacer.OnPLace -= AddFlag;
    }

    public void AddResource(float amount)
    {
        _resourceCount += amount;

        OnResourseCountChange?.Invoke(_resourceCount);
    }

    public void CollectResource()
    {
        _scannedObjects = Physics.OverlapSphere(transform.position, _collectRadius);

        foreach (Collider collider in _scannedObjects)
        {
            if (collider.TryGetComponent(out Stone stone))
            {

                stone.transform.parent = null;
                ResourceCount += stone.GiveResources();
            }
        }
    }

    public void BuyPicker()
    {
        if(ResourceCount >= _pickerCost)
        {
            AddPicker();

            ResourceCount -= _pickerCost;
        }
    }

    public void DespawnPicker(Picker picker)
    {
        picker.OnWorksDone -= DespawnPicker;
        CollectResource();
        _detector.AddStones();

        _detector.RemoveProceedStone(picker.CurrentStone);
        picker.gameObject.SetActive(false);

        _pickers.Enqueue(picker);

        if(_flagQueue.Count != 0 && ResourceCount >= _buildCost)
        {
            BuildTownhall(_flagQueue.Dequeue());
            return;
        }

        if(_detector.StoneQueue.Count != 0)
        {
            SpawnPicker();
        }
    }

    private void AddFlag(TownhallFlag flag)
    {
        _flagQueue.Enqueue(flag);
    }

    private void BuildTownhall(TownhallFlag flag)
    {
        ResourceCount -= _buildCost;
        Picker picker = _pickers.Dequeue();

        picker.gameObject.SetActive(true);
        picker.BuildTownhall(flag);

        picker.OnWorksDone += DespawnPicker;
    }

    private void AddPicker()
    {
        _pickers.Enqueue(Instantiate(_pickerPrefab, transform));
    }


    private void SpawnPicker()
    {
        if (_detector.StoneQueue.Count != 0 && _pickers.Count != 0)
        {
            Stone stone = _detector.GetStone();
            _detector.AddProceedStone(stone);

            Picker picker = _pickers.Dequeue();

            picker.gameObject.SetActive(true);
            picker.StartWork(stone);
            SpawnPicker();

            picker.OnWorksDone += DespawnPicker;
        }
    }
}
