using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    public enum SocketID
    {
        Spine,
        RightHand,
        RightShoulder
    }

    Dictionary<SocketID, MeshSocket> socketMap = new Dictionary<SocketID, MeshSocket>();

    // Start is called before the first frame update
    void Awake()
    {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();

        foreach(var socket in sockets)
        {
            socketMap.Add(socket.socketID, socket);
            
        }
    }

    public void Attach(Transform objectTransform, SocketID socketId)
    {
        socketMap[socketId].Attach(objectTransform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
