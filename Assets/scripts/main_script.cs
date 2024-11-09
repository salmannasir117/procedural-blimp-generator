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
       

        Color selected_color = Color.green;
        GameObject parent = new GameObject("parent");
        GameObject top_hull  = generate_top_hull_go(parent, selected_color);
        GameObject bottom_hull = generate_bottom_hull_go(parent, selected_color);
        GameObject back_hull = generate_back_hull_go(parent, selected_color);
        
        //test transformations. 
        parent.transform.Translate(new Vector3(1, 0, 0));
        parent.transform.localScale = new Vector3(2, 1, 1);

        
        

        
        
    }

    Vector3 [,] get_top_hull_points() {
        Vector3 [,] top_hull_points = {
            {new Vector3(0,0,0), new Vector3(1.5f,0f,-2f), new Vector3(1.5f,0,-2f), new Vector3(3,0,0),},
                                //make below (1,2,1) for uniform shape
            {new Vector3(0,0,0.1f), new Vector3(1,5,0.1f), new Vector3(2,5,0.1f), new Vector3(3,0,0.1f),},
            {new Vector3(0,0,5), new Vector3(1,2,5), new Vector3(2,2,5), new Vector3(3,0,5),},
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
        };
        return top_hull_points;
    }

    GameObject generate_top_hull_go(GameObject parent, Color color) {
        Vector3[,] top_hull_points = get_top_hull_points();
        BezierPatch top_hull_patch = new BezierPatch(top_hull_points, 10);
        // Mesh m = patch.get_mesh();
        GameObject top_hull_go = top_hull_patch.get_game_object("top hull", color);
        top_hull_go.transform.parent = parent.transform;
        return top_hull_go;
    }
    GameObject generate_bottom_hull_go(GameObject parent, Color color) {
        Vector3[,] top_hull_points = get_top_hull_points();
        Vector3[,] bottom_hull_points = new Vector3[4,4];
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                Vector3 curr = top_hull_points[i,j];
                curr.y *= -1;
                bottom_hull_points[i,j] = curr;
            }
        }
        BezierPatch bottom_hull_patch = new BezierPatch(bottom_hull_points, 10);
        GameObject bottom_hull_go = bottom_hull_patch.get_game_object("bottom hull", color);
        int[] tris = bottom_hull_go.GetComponent<MeshFilter>().mesh.triangles;
        System.Array.Reverse(tris);
        bottom_hull_go.GetComponent<MeshFilter>().mesh.triangles = tris;
        bottom_hull_go.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        bottom_hull_go.transform.parent = parent.transform;
        return bottom_hull_go;
    }

    GameObject generate_back_hull_go(GameObject parent, Color color) {
        Vector3[,] back_hull_points = {
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,0,7), new Vector3(2,0,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,0,7), new Vector3(2,0,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,-2,7), new Vector3(2,-2,7), new Vector3(3,-0,7),},
        };
        BezierPatch back_hull_patch = new BezierPatch(back_hull_points, 10);
        GameObject back_hull_go = back_hull_patch.get_game_object("back hull", color);
        back_hull_go.transform.parent = parent.transform;
        return back_hull_go;
    }
     void OnDrawGizmosSelected() {
        // Vector3[] points = {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),};
        // draw_helper(points);
        
        // Gizmos.color = Color.black;
        // Vector3[] temp = {new Vector3(0,-0,7), new Vector3(1,-2,7), new Vector3(2,-2,7), new Vector3(3,-0,7),};
        // draw_helper(temp);

        // Gizmos.color = Color.blue;
        // Vector3[] temp2 = {new Vector3(0,-0,7), new Vector3(1,0,7), new Vector3(2,0,7), new Vector3(3,0,7),};
        // draw_helper(temp2);

        // float size = 1.0f;
        // Gizmos.color = Color.red;
		// Gizmos.DrawLine(Vector3.right * size, Vector3.zero);

		// Gizmos.color = Color.green;
		// Gizmos.DrawLine(Vector3.up * size, Vector3.zero);

		// Gizmos.color = Color.blue;
		// Gizmos.DrawLine(Vector3.forward * size, Vector3.zero);

		// Gizmos.color = Color.white;

        
    }

    void draw_helper(Vector3[] points) {
        foreach (Vector3 point in points) {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
    // void OnDrawGizmosSelected() {
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

        // Vector3 [,] points = {
        //     {new Vector3(0,0,0), new Vector3(1,2,0), new Vector3(2,2,0), new Vector3(3,0,0),},
        //                         //make below (1,2,1) for uniform shape
        //     {new Vector3(0,0,1), new Vector3(1,0,1), new Vector3(2,2,1), new Vector3(3,0,1),},
        //     {new Vector3(0,0,2), new Vector3(1,2,2), new Vector3(2,2,2), new Vector3(3,0,2),},
        //     {new Vector3(0,0,3), new Vector3(1,2,3), new Vector3(2,2,3), new Vector3(3,0,3),},
        // };

        // BezierPatch patch = new BezierPatch(points, 10);
        // Vector3[] patch_points = patch.get_points();
        // for (int i = 0; i < patch_points.Length; i++) {
        //     Gizmos.DrawSphere(patch_points[i], 0.1f);
        // }
        // Gizmos.color = Color.green;
        // Gizmos.DrawSphere(new Vector3(3,0,3), 0.1f);

        // for (int i = 0; i < 20; i++) {
        //     Gizmos.DrawSphere(patch_points[i], 0.1f);
        // }
    // }
    // Update is called once per frame
    void Update()
    {
        
    }
}
