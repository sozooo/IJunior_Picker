using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PickerAnimator : MonoBehaviour
{
    private const string WalkParameter = "Walk";
    private const string GatherParameter = "Gather";

    private Animator _animator;

    private bool _isGathering;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Walk()
    {
        _animator.SetBool(WalkParameter, true);
    }

    private void EndGather()
    {
        _isGathering = false;
        _animator.SetBool(GatherParameter, _isGathering);

        Walk();
    }

    public IEnumerator Gather()
    {
        _animator.SetBool(WalkParameter, false);
        _isGathering = true;

        WaitForSeconds seconds = new WaitForSeconds(5);
        _animator.SetBool(GatherParameter, _isGathering);

        yield return seconds;

        EndGather();
    }
}
