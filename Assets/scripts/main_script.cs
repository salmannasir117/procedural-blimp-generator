using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class main_script : MonoBehaviour
{
    // Start is called before the first frame update
    public int seed = 0;
    public BezierCurve test;
    void Start()
    {
        Random.InitState(seed);
        
    }
    void OnDrawGizmosSelected() {
        // Gizmos.DrawCube(new Vector3(0,0,0), new Vector3(1f, 1f, 1f));
        
        // Vector3 p1 = new Vector3(0, 0, 0);
        // Vector3 p2 = new Vector3(1, 2, 0);
        // Vector3 p3 = new Vector3(2, 2, 0);
        // Vector3 p4 = new Vector3(3, 0, 0);
        // test = new BezierCurve(p1, p2, p3, p4, 50);

        // for (int i = 0; i < test.get_num_points(); i++) {
        //     Vector3 point = test.get_point(i);
        //     if (i == test.get_num_points() - 1) // last one 
        //         Gizmos.color = Color.green;
        //     Gizmos.DrawSphere(point, 0.1f);
        // }
        // Gizmos.DrawSphere(new Vector3(3f, 0, 0), 0.1f);

        Vector3 [,] points = {
            {new Vector3(0,0,0), new Vector3(1,2,0), new Vector3(2,2,0), new Vector3(3,0,0),},
                                //make below (1,2,1) for uniform shape
            {new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(2,2,1), new Vector3(3,0,1),},
            {new Vector3(0,0,2), new Vector3(1,2,2), new Vector3(2,2,2), new Vector3(3,0,2),},
            {new Vector3(0,0,3), new Vector3(1,2,3), new Vector3(2,2,3), new Vector3(3,0,3),},
        };

        BezierPatch patch = new BezierPatch(points, 10);
        Vector3[] patch_points = patch.get_points();
        for (int i = 0; i < patch_points.Length; i++) {
            Gizmos.DrawSphere(patch_points[i], 0.1f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(3,0,3), 0.1f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
