using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(PickerPool))]
[RequireComponent(typeof(StoneDetector))]
public class Townhall : Purpose
   
{
    [SerializeField] private float _maxPickerCount;

    private float _resourceCount = 0;
    private float _currentPickerCount = 0;
    private PickerPool _pool;
    private StoneDetector _detector;

    private Queue<Stone> _stoneQueue;

    public event Action QueueChanged;
    public event Action<float> OnResourseCountChange;

    private void Awake()
    {
        _pool = GetComponent<PickerPool>();
        _detector = GetComponent<StoneDetector>();

        _stoneQueue = new Queue<Stone>();
    }

    private void OnEnable()
    {
        _detector.OnDetect += AddStone;
        QueueChanged += SpawnPicker;
    }

    private void OnDisable()
    {
        _detector.OnDetect -= AddStone;
        QueueChanged -= SpawnPicker;
    }

    private void DespawnPicker(Picker picker)
    {
        picker.OnWorksDone -= DespawnPicker;

        picker.gameObject.SetActive(false);

        _pool.Add(picker);
    }

    private void SpawnPicker()
    {
        if(_stoneQueue.Count != 0 && _currentPickerCount < _maxPickerCount)
        {
            Picker picker = _pool.Take();

            picker.gameObject.SetActive(true);
            picker.StartWork(_stoneQueue.Dequeue());
            QueueChanged?.Invoke();

            picker.OnWorksDone += DespawnPicker;
        }
    }

    public void AddResource(float amount)
    {
        _resourceCount += amount;

        OnResourseCountChange?.Invoke(_resourceCount);
    }

    public void AddStone(Stone stone)
    {
        _stoneQueue.Enqueue(stone);

        QueueChanged?.Invoke();
    }
}
