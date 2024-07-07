using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private TownhallFlag _flagPrefab;

    private Camera _camera;
    private TownhallFlag _flag;
    private Coroutine _moveFlag;

    private Ray _ray;
    private RaycastHit _hit;

    public TownhallFlag Flag => _flag;

    public event Action<TownhallFlag> OnPLace;

    private void Awake()
    {
        _camera = Camera.main;
        _hit = new RaycastHit();
    }

    public void StartPlacing()
    {
        if(_moveFlag == null && _flag == null)
        {
            _moveFlag = StartCoroutine(PlaceFlag());
        }
    }

    public void StartDragging()
    {
        if (_moveFlag == null && _flag != null)
        {
            _moveFlag = StartCoroutine(DragFlag());
        }
    }

    private void Place()
    {
        StopCoroutine(_moveFlag);
        _moveFlag = null;

        _flag = Instantiate(_flagPrefab, _hit.point, Quaternion.identity);
        _flag.OnBuilded += Reset;

        OnPLace?.Invoke(_flag); 
    }

    private void Drag()
    {
        StopCoroutine(_moveFlag);
        _moveFlag = null;

        _flag.transform.position = _hit.point;
    }

    private void Reset()
    {
        _flag = null;
    }

    private IEnumerator PlaceFlag()
    {
        yield return WaitForCorrectClick();

        Place();
    }

    private IEnumerator DragFlag()
    {
        yield return WaitForCorrectClick();

        Drag();
    }

    private IEnumerator WaitForCorrectClick()
    {
        WaitUntil wait = new(() => Mouse.current.leftButton.wasPressedThisFrame);

        do
        {
            yield return wait;

            _ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        } while (Physics.Raycast(_ray, out _hit) == false);
    }
}
