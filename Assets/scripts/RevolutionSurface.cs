using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolutionSurface
{
    public enum RevolutionSurfaceType {
        SPHERE,
    }
    RevolutionSurfaceType rst; 
    int points_per_curve;
    int num_curves;
    public RevolutionSurface(RevolutionSurfaceType rst, int ppc, int nc) {
        this.rst = rst;
        points_per_curve = ppc;
        num_curves = nc;
    }

    float rt_sphere(float t) {
        return Mathf.Cos(t);
    }

    float ht_sphere(float t) {
        return Mathf.Sin(t);
    }

    Vector3[] get_points() {
        // Vector3[] points = new Vector3[];
        return null;
    }
}
