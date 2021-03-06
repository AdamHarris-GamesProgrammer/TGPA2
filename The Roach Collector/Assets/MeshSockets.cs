using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    public enum SocketId
    {
        Spine,
        RightHand
    }

    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();

    // Start is called before the first frame update
    void Awake()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();

        foreach(var socket in sockets)
        {
            socketMap.Add(socket.socketType, socket);
            //socketMap[socket.socketType] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketId socketId)
    {
        Debug.Log("Attaching " + objectTransform.name + " to " + socketId.ToString());
        socketMap[socketId].Attach(objectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
