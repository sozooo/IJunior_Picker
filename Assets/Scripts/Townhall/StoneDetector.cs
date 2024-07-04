using System;
using System.Collections.Generic;
using UnityEditor.Sprites;
using UnityEngine;

public class StoneDetector : MonoBehaviour
{
    [SerializeField] private Vector3 _halfExtents;
    [SerializeField] private StoneSpawner _spawner;

    private Collider[] _scannedObjects;
    private List<Stone> _stones;
    private Queue<Stone> _stoneQueue;
    private List<Stone> _proceedStones;

    public Queue<Stone> StoneQueue => _stoneQueue;
    public List<Stone> ProceedStones => _proceedStones;

    public event Action OnDetect;

    private void Awake()
    {
        _stoneQueue = new Queue<Stone>();
        _proceedStones = new List<Stone>();
    }

    private void OnEnable()
    {
        _spawner.OnSpawn += AddStones;
        AddStones();
    }

    private void OnDisable()
    {
        _spawner.OnSpawn -= AddStones;
    }

    public List<Stone> ScanForStones()
    {
        _stones = new List<Stone>();

        _scannedObjects = Physics.OverlapBox(transform.position, _halfExtents, Quaternion.identity);

        foreach (Collider collider in _scannedObjects)
        {
            if(collider.TryGetComponent(out Stone stone))
            {
                _stones.Add(stone);
            }
        }

        return _stones;
    }

    public Stone GetStone()
    {
        return _stoneQueue.Dequeue();
    }

    public void AddStones()
    {
        foreach (Stone stone in ScanForStones())
        {
            if (_stoneQueue.Contains(stone) == false && _proceedStones.Contains(stone) == false)
            {
                _stoneQueue.Enqueue(stone);

                OnDetect?.Invoke();
            }
        }
    }

    public void RemoveProceedStone(Stone stone)
    {
        _proceedStones.Remove(stone);
    }

    public void AddProceedStone(Stone stone)
    {
        _proceedStones.Add(stone);
    }
}
