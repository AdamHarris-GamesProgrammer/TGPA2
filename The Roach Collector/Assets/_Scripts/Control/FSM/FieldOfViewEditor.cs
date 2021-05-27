using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;


[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        if (!fov) return;

        Handles.color = Color.green;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov._viewRadius);

        Vector3 viewangleA = fov.DirectionFromAngle(-fov._viewAngle / 2, false);
        Vector3 viewangleB = fov.DirectionFromAngle(fov._viewAngle / 2, false);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewangleA * fov._viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewangleB * fov._viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov._visibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }

    }
}
#endif
