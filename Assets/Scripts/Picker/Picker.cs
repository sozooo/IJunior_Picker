using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PickerMover))]
[RequireComponent(typeof(PickerAnimator))]
public class Picker : MonoBehaviour
{
    [SerializeField] private float _maxStoneCount = 10;

    private Townhall _townhall;
    private PickerMover _mover;
    private PickerAnimator _animator;
    private Stone _currentStone;
    private float _currentStoneCount;

    private Coroutine _workCoroutine;

    public event Action<Picker> OnWorksDone;

    private void Awake()
    {
        _mover = GetComponent<PickerMover>();
        _animator = GetComponent<PickerAnimator>();
    }

    private void OnEnable()
    {
        if(_townhall == null)
        {
            _townhall = transform.GetComponentInParent<Townhall>();
        }
    }

    public void StartWork(Stone stone)
    {
        if(_workCoroutine == null)
        {
            _currentStone = stone;

            _workCoroutine = StartCoroutine(Work());
        }
    }

    private void EndWork()
    {
        if (_workCoroutine != null)
        {
            StopCoroutine(_workCoroutine);

            _workCoroutine = null;
            OnWorksDone?.Invoke(this);
        }
    }

    private void TakeStone()
    {
        _currentStone.ChangeResourceCount(_maxStoneCount);

        _currentStoneCount = _maxStoneCount;
    }

    private void GiveStone()
    {
        _townhall.AddResource(_currentStoneCount);

        _currentStoneCount = 0;
    }

    private IEnumerator Work()
    {
        while (_currentStone.ResourceCount > 0)
        {
            _animator.Walk();

            yield return _mover.Move(_currentStone);
            yield return _animator.Gather();

            TakeStone();

            yield return _mover.Move(_townhall);

            GiveStone();
        }

        EndWork();
    }
}
