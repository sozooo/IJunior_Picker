using System;
using System.Collections;
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
    [SerializeField] private StoneDetector _detector;

    private float _resourceCount = 0;
    private FlagPlacer _flagPlacer;

    private Queue<TownhallFlag> _flagQueue;
    private Queue<Picker> _pickers;
    private Collider[] _scannedObjects;

    private Coroutine _buildTownhall;

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
        _flagPlacer = GetComponent<FlagPlacer>();
        _flagQueue = new Queue<TownhallFlag>();
        _pickers = new Queue<Picker>();
    }

    private void Start()
    {
        for(int i = 0; i < _startPickerCount; i++)
        {
            AddPicker();
        }
    }

    private void OnEnable()
    {
        _detector.OnDetect += SpawnPicker;
        _flagPlacer.OnPLace += AddFlag;
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

    private void AddFlag(TownhallFlag flag)
    {
        _flagQueue.Enqueue(flag);
    }

    private void StartBuild(TownhallFlag flag)
    {
        if(_buildTownhall == null)
        {
            _buildTownhall = StartCoroutine(BuildTownhall(flag));
        }
    }

    private void AddPicker()
    {
        _pickers.Enqueue(Instantiate(_pickerPrefab, transform));
    }

    private void DespawnPicker(Picker picker)
    {
        picker.OnWorksDone -= DespawnPicker;
        CollectResource();
        _detector.AddStones();

        _detector.RemoveProceedStone(picker.CurrentStone);
        picker.gameObject.SetActive(false);

        _pickers.Enqueue(picker);

        if(_flagQueue.Count != 0)
        {
            StartBuild(_flagQueue.Dequeue());
            return;
        }

        if(_detector.StoneQueue.Count != 0)
        {
            SpawnPicker();
        }
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

    private IEnumerator BuildTownhall(TownhallFlag flag)
    {
        yield return new WaitUntil(() => _pickers.Count > 0 && ResourceCount >= _buildCost);

        Picker picker = _pickers.Dequeue();

        picker.gameObject.SetActive(true);
        picker.BuildTownhall(flag);

        picker.OnWorksDone += DespawnPicker;
    }
}
