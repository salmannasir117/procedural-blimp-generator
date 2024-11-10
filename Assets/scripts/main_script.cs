using UnityEngine;

public class main_script : MonoBehaviour
{
    // Start is called before the first frame update
    public int seed = 0;
    public BezierCurve test;

    int num_wings = 3;
    enum wing_type {
        ORIGINAL = 0,
        FANCY = 1,
        CURVY = 2,
    }
    int num_tails = 3;
    enum tail_type {
        ORIGINAL = 0,
        DOLPHIN = 1,
        CAPE = 2,
    }

    Color[] plane_colors = 
    {
        //plane grey
        new Color(196,203,212) / 255.0f,
        //spirit yellow
        new Color(255, 236, 0) / 255.0f,
        //jet blue
        new Color(0, 56, 118) / 255.0f,
        //La compagnie light blue
        new Color(148, 196, 228) / 255.0f,
        //macy's red (they have floats/thanksgiving day parade)
        new Color(234, 0, 0) / 255.0f,
    };

    // to allow our computers to not blow up.
    // this is the number of points per bezier curve on a bezier curve.
    // so, if HULL_RESOLUTION = 10, then each batch in the hull has 10 * 10 = 100 verticies. 
    // HULL is considered 3 pieces: top, bottom, back
    // WING is 2 pieces: left and right wing. 
    //10 -> performance mode
    //20 -> quality mode
    const int WING_RESOLUTION = 10;
    const int HULL_RESOLUTION = 10;
    const int TAIL_RESOLUTION = 10;
    const float TAIL_MIN_SCALE = 0.9f;
    const float TAIL_MAX_SCALE = 1.1f;
    
    const float WING_MIN_SCALE = 0.8f;
    const float WING_MAX_SCALE = 1.2f;    
    void Start()
    {
        Random.InitState(seed);
        float plane_space = 10.0f;
        //for each plane:
        //generate random wing type
        //generate random tail type
        //generate color
        //generate wing scale
        //generate tail scale
        //generate plane
        for (int i = 0; i < 5; i++) {
            int plane_number = i + 1;
            //select wing type & scale
            int wing_number = Random.Range(0, num_wings);
            wing_type wing = (wing_type) wing_number;
            float wing_scale = Random.value * (WING_MAX_SCALE - WING_MIN_SCALE) + WING_MIN_SCALE;


            //select tail type & scale
            int tail_number = Random.Range(0, num_tails);
            tail_type tail_t = (tail_type) tail_number;
            float tail_scale = Random.value * (TAIL_MAX_SCALE - TAIL_MIN_SCALE) + TAIL_MIN_SCALE;

    

            // Select random color
            Color selected_color = plane_colors[Random.Range(0, plane_colors.Length)];
            // selected_color = plane_colors[4];
            GameObject parent = new GameObject("plane " + plane_number);
            GameObject top_hull  = generate_top_hull_go(parent, selected_color);
            GameObject bottom_hull = generate_bottom_hull_go(parent, selected_color);
            GameObject back_hull = generate_back_hull_go(parent, selected_color);
            GameObject left_wing = generate_left_wing(parent, selected_color, wing, new Vector3(1, 1.2f, 1.2f));
            GameObject right_wing = generate_right_wing(parent, selected_color, wing, new Vector3(1, 1.2f, 1.2f));
            GameObject tail = generate_tail(parent, selected_color, tail_t, new Vector3(1, 0.8f, 1.2f));

            parent.transform.Translate(new Vector3(i * plane_space, 0, 0));
            
            //test transformations. 
            //https://docs.unity3d.com/ScriptReference/Transform.html
            //localScale, Translate, Rotate (relativeTo = Space.self | Space.world, RotateAround
            // parent.transform.Translate(new Vector3(1, 2, 0));
            // parent.transform.localScale = new Vector3(2, 1, 1);
        }

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
        BezierPatch top_hull_patch = new BezierPatch(top_hull_points, HULL_RESOLUTION);
        // Mesh m = patch.get_mesh();
        GameObject top_hull_go = top_hull_patch.get_game_object("top hull", color);
        top_hull_go.transform.parent = parent.transform;
        return top_hull_go;
    }
    GameObject generate_bottom_hull_go(GameObject parent, Color color) {
        // Vector3[,] top_hull_points = get_top_hull_points();
        // Vector3[,] bottom_hull_points = new Vector3[4,4];
        // for (int i = 0; i < 4; i++) {
        //     for (int j = 0; j < 4; j++) {
        //         Vector3 curr = top_hull_points[i,j];
        //         curr.y *= -1;
        //         bottom_hull_points[i,j] = curr;
        //     }
        // }
        // BezierPatch bottom_hull_patch = new BezierPatch(bottom_hull_points, 10);
        // GameObject bottom_hull_go = bottom_hull_patch.get_game_object("bottom hull", color);
        // int[] tris = bottom_hull_go.GetComponent<MeshFilter>().mesh.triangles;
        // System.Array.Reverse(tris);
        // bottom_hull_go.GetComponent<MeshFilter>().mesh.triangles = tris;
        // bottom_hull_go.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        // bottom_hull_go.transform.parent = parent.transform;
        // return bottom_hull_go;

        GameObject bottom_hull = generate_top_hull_go(parent, color);
        bottom_hull.name = "bottom hull";
        bottom_hull.transform.Translate(new Vector3(3, 0, 0));
        bottom_hull.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        return bottom_hull;

    }

    GameObject generate_back_hull_go(GameObject parent, Color color) {
        Vector3[,] back_hull_points = {
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,0,7), new Vector3(2,0,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,0,7), new Vector3(2,0,7), new Vector3(3,0,7),},
            {new Vector3(0,-0,7), new Vector3(1,-2,7), new Vector3(2,-2,7), new Vector3(3,-0,7),},
        };
        BezierPatch back_hull_patch = new BezierPatch(back_hull_points, HULL_RESOLUTION);
        GameObject back_hull_go = back_hull_patch.get_game_object("back hull", color);
        back_hull_go.transform.parent = parent.transform;
        return back_hull_go;
    }

    Vector3[,] get_original_wing_points() {
        return new Vector3[,]{
            {new Vector3(0,0,2), new Vector3(0,0,2.5f), new Vector3(0,0,3), new Vector3(0,0,5),},
            {new Vector3(-1,0,2.5f), new Vector3(-1,0,3f), new Vector3(-1,0,3.5f), new Vector3(-1,0,5),},

            {new Vector3(-2,0,3f), new Vector3(-2,0,3.5f), new Vector3(-2,0,4f), new Vector3(-2,0,5),},

            {new Vector3(-3,0,3.5f), new Vector3(-3,0,4f), new Vector3(-3,0,4.5f), new Vector3(-3,0,5),},
        };
    }

    Vector3[,] get_fancy_wing_points() {
        return new Vector3[,]{
            {new Vector3(0,0,2), new Vector3(0,0,2.5f), new Vector3(0,0,3f), new Vector3(0,0,3.5f),},
            {new Vector3(-1,0,3.5f), new Vector3(-1,0,4f), new Vector3(-1,0,4.5f), new Vector3(-1,0,4.35f),},

            {new Vector3(-2.8f,0,4f), new Vector3(-2.8f,0,4.5f), new Vector3(-2.8f,0,5f), new Vector3(-2.8f,0,5),},

            {new Vector3(-3,1f,4.5f), new Vector3(-3,1f,5f), new Vector3(-3,1f,5.5f), new Vector3(-3,1f,6),},
        };
    }

    Vector3[,] get_curvy_wing_points() {
        float height = 3;
        // return new Vector3[,]{
        //     {new Vector3(0,0,2), new Vector3(0,0,2.5f), new Vector3(0,0,3f), new Vector3(0,0,3.5f),},
        //     {new Vector3(-1, height, 2.5f), new Vector3(-1, height, 3f), new Vector3(-1, height, 3.5f), new Vector3(-1, height, 4.0f),},

        //     {new Vector3(-2.8f, -height, 3.0f), new Vector3(-2.8f, -height, 3.5f), new Vector3(-2.8f, -height, 4.0f), new Vector3(-2.8f, -height, 4.5f),},

        //     {new Vector3(-3, height / 2,4.5f), new Vector3(-3, height / 2, 5f), new Vector3(-3, height / 2,5.5f), new Vector3(-3,height / 2,6),},
        // };
        return new Vector3[,]{
            {new Vector3(0, -0.5f,2), new Vector3(0,0,2.75f), new Vector3(0,0,3.5f), new Vector3(0,0,4.25f),},
            {new Vector3(-1, height -0.5f, 2.75f), new Vector3(-1, height, 3.5f), new Vector3(-1, height, 4.25f), new Vector3(-1, height, 5f),},

            {new Vector3(-2.8f, -height -0.5f, 3.5f), new Vector3(-2.8f, -height, 4.25f), new Vector3(-2.8f, -height, 5.0f), new Vector3(-2.8f, -height, 5.75f),},

            {new Vector3(-4, height / 2 -0.5f, 4.5f), new Vector3(-4, height / 2, 5.25f), new Vector3(-4, height / 2,5.75f), new Vector3(-4,height / 2,6.25f),},
        };
    }
    GameObject generate_left_wing(GameObject parent, Color color, wing_type wing, Vector3 scale) {
        Vector3 [,] points; 
        switch (wing) {
            case wing_type.CURVY: {
                points = get_curvy_wing_points();
                break;
            }
            case wing_type.FANCY: {
                points = get_fancy_wing_points();
                break;
            }
            case wing_type.ORIGINAL:
            default: {
                points = get_original_wing_points();
                break;
            }
        }

        for (int i = 1; i < points.GetLength(0); i++) {
            for (int j = 0; j < points.GetLength(1); j++) {
                points[i,j].Scale(scale);
            }
        }
        
        BezierPatch bp = new BezierPatch(points, WING_RESOLUTION);
        GameObject go = bp.get_game_object("left wing", color);
        go.transform.parent = parent.transform;
        return go;
    }
    
    GameObject generate_right_wing(GameObject parent, Color color, wing_type wing, Vector3 scale) {
        // Vector3 rotate;
        Vector3 [,] points; 
        switch (wing) {
             case wing_type.CURVY: {
                points = get_curvy_wing_points();
                break;
            }
            case wing_type.FANCY: {
                points = get_fancy_wing_points();
                break;
            }
            case wing_type.ORIGINAL:
            default: {
                points = get_original_wing_points();
                break;
            }
        }

        for (int i = 1; i < points.GetLength(0); i++) {
            for (int j = 0; j < points.GetLength(1); j++) {
                points[i,j].Scale(scale);
            }
        }

        reverse_points(points);
        flip_heights(points);   //for when i rotate about the z axis
        BezierPatch bp = new BezierPatch(points, WING_RESOLUTION);
        GameObject go = bp.get_game_object("right wing", color);
        go.transform.Translate(new Vector3(3, 0, 0));
        go.transform.Rotate(new Vector3(0, 0, 180));
        go.transform.parent = parent.transform;
        return go;
        // GameObject right_wing = generate_left_wing(parent, color, wing);
        // right_wing.name = "right";
        // right_wing.transform.Translate(new Vector3(3, 0, 0));
        // right_wing.transform.Rotate(new Vector3(0, 0, 180), Space.Self);
        // return right_wing;

    }

    Vector3 [,] get_original_tail_points() {
        Vector3 [,] points = {
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
            {new Vector3(0,1,7.5f), new Vector3(1,2,7.5f), new Vector3(2,2,7.5f), new Vector3(3,1,7.5f),},
            {new Vector3(0,1.2f,8f), new Vector3(1,2,8f), new Vector3(2,2,8f), new Vector3(3,1.2f,8f),},
            {new Vector3(0,1.4f,8.5f), new Vector3(1,2,8.5f), new Vector3(2,2,8.5f), new Vector3(3,1.4f,8.5f),},
        };
        return points;
    }

    Vector3 [,] get_dolphin_tail_points() {
        Vector3 [,] points = {
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
            {new Vector3(0.5f,1,7.5f), new Vector3(1,2.25f,7.5f), new Vector3(2,2.25f,7.5f), new Vector3(2.5f,1,7.5f),},
            {new Vector3(1.0f,1.2f,8f), new Vector3(1,2.25f,8f), new Vector3(2,2.25f,8f), new Vector3(2.0f,1.2f,8f),},
            {new Vector3(1.5f,1.4f,8.5f), new Vector3(1,2,8.5f), new Vector3(2,2,8.5f), new Vector3(1.5f,1.4f,8.5f),},
        };
        return points;
    }

    Vector3 [,] get_cape_tail_points() {
        Vector3 [,] points = {
            {new Vector3(0,0,7), new Vector3(1,2,7), new Vector3(2,2,7), new Vector3(3,0,7),},
            {new Vector3(-0.5f,0,7.5f), new Vector3(0.4f,2f,7.5f), new Vector3(2.6f,2f,7.5f), new Vector3(3.5f,0f,7.5f),},
            {new Vector3(-1.0f,0.25f,8f), new Vector3(0.4f,2f,8f), new Vector3(2.6f,2f,8f), new Vector3(4.0f,0.25f,8f),},
            {new Vector3(-1.5f,0.5f,8.5f), new Vector3(1,2,8.5f), new Vector3(2,2,8.5f), new Vector3(4.5f,0.5f,8.5f),},
        };
        return points;
    }
    GameObject generate_tail(GameObject parent, Color color, tail_type tail, Vector3 scale) {
    Vector3 [,] points; 
    switch (tail) {
        case tail_type.CAPE: {
            points = get_cape_tail_points();
            break;
        }
        case tail_type.DOLPHIN: {
            points = get_dolphin_tail_points();
            break;
        }
        case tail_type.ORIGINAL:
        default: {
            points = get_original_tail_points();
            break;
        }
    }
    for (int i = 1; i < points.GetLength(0); i++) {
        for (int j = 0; j < points.GetLength(1); j++) {
            points[i,j].Scale(scale);
        }
    }
    BezierPatch bp = new BezierPatch(points, TAIL_RESOLUTION);
    GameObject go = bp.get_game_object("tail", color);
    go.transform.parent = parent.transform;
    return go;
}

    void reverse_points(Vector3[,] points) {
        //reverse each row of points.
        for (int i = 0; i < points.GetLength(0); i++) {
            for (int j = 0; j < points.GetLength(1) / 2; j++) {
                Vector3 temp = points[i,j];
                points[i, j] = points[i, points.GetLength(1) - j - 1];
                points[i, points.GetLength(1) - j - 1] = temp;
            }
        }
    }

    void flip_heights(Vector3[,] points) {
        for (int i = 0; i < points.GetLength(0); i++) {
            for (int j = 0; j < points.GetLength(1); j++) {
                points[i,j].y *= -1;
            }
        }
    }
    void OnDrawGizmosSelected() {
        /*testing left wing points: */
        // Vector3 [,] points = {
        //     {new Vector3(0,0,2), new Vector3(0,0,2.5f), new Vector3(0,0,3), new Vector3(0,0,5),},
        //     {new Vector3(-1,0,2.5f), new Vector3(-1,0,3f), new Vector3(-1,0,3.5f), new Vector3(-1,0,5),},

        //     {new Vector3(-2,0,3f), new Vector3(-2,0,3.5f), new Vector3(-2,0,4f), new Vector3(-2,0,5),},

        //     {new Vector3(-3,0,3.5f), new Vector3(-3,0,4f), new Vector3(-3,0,4.5f), new Vector3(-3,0,5),},
        // };
        Vector3[,] points = get_cape_tail_points();
        foreach (Vector3 point in points) {
            Gizmos.DrawSphere(point, 0.1f);
        }

        
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
