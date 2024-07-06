using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PickerMover), typeof(PickerAnimator))]
public class Picker : MonoBehaviour
{
    [SerializeField] private Vector3 _backpackPosition;

    private Townhall _townhall;
    private PickerMover _mover;
    private PickerAnimator _animator;
    private Stone _currentStone;

    private Coroutine _workCoroutine;

    public event Action<Picker> OnWorksDone;

    public Stone CurrentStone => _currentStone;

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

    public void BuildTownhall(TownhallFlag flag)
    {
        if (_workCoroutine == null)
        {
            _workCoroutine = StartCoroutine(Build(flag));
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
        _currentStone.transform.parent = transform;
        _currentStone.transform.localPosition = _backpackPosition;
    }

    private IEnumerator Work()
    {
        _animator.Walk();

        yield return _mover.Move(_currentStone);

        TakeStone();

        yield return _mover.Move(_townhall);

        EndWork();
    }

    private IEnumerator Build(TownhallFlag flag)
    {
        _animator.Walk();

        yield return _mover.Move(flag);

        OnWorksDone -= _townhall.DespawnPicker;

        _townhall = flag.CreateTownhall();
        OnWorksDone += _townhall.DespawnPicker;

        EndWork();
    }
}
