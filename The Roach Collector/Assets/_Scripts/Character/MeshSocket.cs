using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketID socketID;

    public HumanBodyBones bone;

    Transform attachPoint;

    public Vector3 offset;
    public Vector3 rotation;

    // Start is called before the first frame update
    void Awake()
    {

        Animator anim = GetComponentInParent<Animator>();

        attachPoint = new GameObject("Socket" + socketID).transform;
        attachPoint.SetParent(anim.GetBoneTransform(bone));
        attachPoint.localPosition = offset;
        attachPoint.localRotation = Quaternion.Euler(rotation);
    }

   public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}
