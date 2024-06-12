using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StonePool))]
public class StoneSpawner : MonoBehaviour
{
    [SerializeField] private float _period;

    private StonePool _pool;
    private Stone _stone;

    private Coroutine _timer;

    private void Awake()
    {
        _pool = GetComponent<StonePool>();
    }

    private void Start()
    {
        _stone = _pool.Take();

        _stone.OnDead += StartTimer;
    }

    private void StartTimer()
    {
        if( _timer == null)
        {
            _timer = StartCoroutine(Timer());
        }
    }

    private void EndTimer()
    {
        if(_timer != null)
        {
            StopCoroutine(_timer);

            _timer = null;
        }
    }

    private IEnumerator Timer()
    {
        WaitForSeconds wait = new WaitForSeconds(_period);

        yield return wait;

        EndTimer();
    }
}
