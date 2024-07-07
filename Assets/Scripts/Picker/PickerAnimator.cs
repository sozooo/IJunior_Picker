using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PickerAnimator : MonoBehaviour
{
    private const string WalkParameter = "Walk";

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        _animator.SetBool(WalkParameter, true);
    }
}
