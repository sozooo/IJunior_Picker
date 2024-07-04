using UnityEngine;

public class TownhallFlag : Purpose
{
    [SerializeField] private Townhall _prefab;

    private void OnEnable()
    {
        transform.LookAt(new Vector3(Vector3.zero.x, transform.position.y, Vector3.zero.z));
    }

    public Townhall CreateTownhall()
    {
        Townhall townhall = Instantiate(_prefab, transform.position, Quaternion.identity);

        return townhall;
    }
}
