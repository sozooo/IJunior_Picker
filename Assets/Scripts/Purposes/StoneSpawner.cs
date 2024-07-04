using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(StonePool))]
public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private float _period;
    [SerializeField] private float _maxStonesCount;

    [Header("Positions")]
    [SerializeField] private float _maxX;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxZ;
    [SerializeField] private float _minZ;

    private StonePool _pool;
    private Stone _stone;
    private float _currentStonesCount;

    private Coroutine _timer;

    public event Action OnSpawn;

    private void Awake()
    {
        _pool = GetComponent<StonePool>();
        _currentStonesCount = 0;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        transform.position = new Vector3(Random.Range(_minX, _maxX), transform.position.y, Random.Range(_minZ, _maxZ));

        _stone = _pool.Take();
        _stone.transform.position = transform.position;
        _stone.gameObject.SetActive(true);
        _stone.OnDead += DespawnStone;

        _currentStonesCount++;
        OnSpawn?.Invoke();

        if (_timer == null)
        {
            _timer = StartCoroutine(Timer());
        }

    }

    private void DespawnStone(Stone stone)
    {
        _pool.Add(stone);
        _stone.OnDead -= DespawnStone;
        stone.gameObject.SetActive(false);

        if( _timer == null)
        {
            _timer = StartCoroutine(Timer());
        }

        _currentStonesCount--;
    }

    private IEnumerator Timer()
    {
        while(_currentStonesCount < _maxStonesCount)
        {
            WaitForSeconds wait = new WaitForSeconds(_period);

            yield return wait;
            
            Spawn();
        }

        _timer = null;
    }
}
