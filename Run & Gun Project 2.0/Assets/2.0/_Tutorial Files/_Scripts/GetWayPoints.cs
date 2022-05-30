using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWayPoints : MonoBehaviour
{
    [SerializeField]
    private List<Transform> waypoints;
    // Start is called before the first frame update
    void Start()
    {
        var parent = gameObject.transform.GetChild(0);

        foreach(Transform t in parent)
        {
            waypoints.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
