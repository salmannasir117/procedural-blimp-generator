using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPatch
{
    BezierCurve G1, G2, G3, G4;
    int num_points_per_size;
    int num_points;

    Vector3 [] points;
    //should i not store the points and get them at runtime? I htink that would be more efficient/wanted

    public BezierPatch(Vector3 [,] ctrl_pts, int n) {       //n is number of points per dimension
        num_points_per_size = n;
        num_points = n * n;
        points = new Vector3[num_points];
        G1 = new BezierCurve(ctrl_pts[0,0], ctrl_pts[0, 1], ctrl_pts[0, 2], ctrl_pts[0, 3], n);
        G2 = new BezierCurve(ctrl_pts[1,0], ctrl_pts[1, 1], ctrl_pts[1, 2], ctrl_pts[1, 3], n);
        G3 = new BezierCurve(ctrl_pts[2,0], ctrl_pts[2, 1], ctrl_pts[2, 2], ctrl_pts[2, 3], n);
        G4 = new BezierCurve(ctrl_pts[3,0], ctrl_pts[3, 1], ctrl_pts[3, 2], ctrl_pts[3, 3], n);

        for (int j = 0; j < n; j++) {
            // float t = j / (float) (n - 1);
            BezierCurve curr = new BezierCurve(G1.get_point(j), G2.get_point(j), G3.get_point(j), G4.get_point(j), n);
            for (int i = 0; i < curr.get_num_points(); i++) {
                points[j * n + i] = curr.get_point(i);
            }
        }
    }

    public Vector3[] get_points() {
        return points;
    }

    public int get_num_points() {
        return num_points;
    }
}
