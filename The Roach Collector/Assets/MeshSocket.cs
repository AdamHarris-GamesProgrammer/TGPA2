using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId socketType;


    Transform attachPoint;
    // Start is called before the first frame update
    void Start()
    {
        attachPoint = transform.GetChild(0);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attach(Transform objectTransform)
    {
        objectTransform.SetParent(attachPoint, false);
    }
}
