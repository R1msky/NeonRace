using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    Dictionary<ushort, PlayerObject> networkPlayers = new Dictionary<ushort, PlayerObject>();

    public void Awake()
    {
        client.MessageReceived += MessageRecieved;
    }

    public void Add(ushort id, PlayerObject player)
    {
        networkPlayers.Add(id, player);
    }

    void MessageRecieved(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage() as Message)
        {
            if (message.Tag == Tags.MovePlayerTag)
            {
                using (DarkRiftReader reader = message.GetReader())
                {
                    ushort id = reader.ReadUInt16();
                    Vector3 newPosition = new Vector3(reader.ReadSingle(), reader.ReadSingle(), 0);

                    if (networkPlayers.ContainsKey(id))
                    {
                        networkPlayers[id].SetMovePosition(newPosition);
                    }
                }
            }
        }
    }

    public void DestroyPlayer(ushort id)
    {
        PlayerObject o = networkPlayers[id];

        Destroy(o.gameObject);
        Debug.Log("destroyed");
        networkPlayers.Remove(id);
    }
}
