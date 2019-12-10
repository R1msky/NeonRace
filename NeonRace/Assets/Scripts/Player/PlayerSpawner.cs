using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    const byte SPAWN_TAG = 0;

    [SerializeField]
    [Tooltip("The DarkRift client to communicate on.")]
    UnityClient client;

    [SerializeField]
    [Tooltip("The controllable player prefab.")]
    GameObject controllablePrefab;

    [SerializeField]
    [Tooltip("The network controllable player prefab.")]
    GameObject networkPrefab;

    [SerializeField]
    [Tooltip("The network player manager.")]
    NetworkPlayerManager networkPlayerManager;

    void Awake()
    {
        Debug.Log("start");
        if (client == null)
        {   
            Debug.LogError("Client unassigned in PlayerSpawner.");
            Application.Quit();
        }

        if (controllablePrefab == null)
        {
            Debug.LogError("Controllable Prefab unassigned in PlayerSpawner.");
            Application.Quit();
        }

        if (networkPrefab == null)
        {
            Debug.LogError("Network Prefab unassigned in PlayerSpawner.");
            Application.Quit();
        }

       // client.MessageReceived += SpawnPlayer;
        client.MessageReceived += MessageReceived;
        Debug.Log(client.ID);

    }

    void SpawnPlayer(object sender, MessageReceivedEventArgs e)
    {

        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
        {
            if (message.Tag == Tags.SpawnPlayerTag)
            {
                if (reader.Length % 13 != 0)
                {
                    Debug.LogWarning("Received malformed spawn packet.");
                    return;
                }

                while (reader.Position < reader.Length)
                {
                    ushort id = reader.ReadUInt16();
                    Debug.Log(id);
                    Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle());
                    //float radius = reader.ReadSingle();
                    Color32 color = new Color32(
                        reader.ReadByte(),
                        reader.ReadByte(),
                        reader.ReadByte(),
                        255
                    );

                    GameObject obj;
                    if (id == client.ID)
                    {
                        Debug.Log("true");
                        obj = Instantiate(controllablePrefab, position, Quaternion.identity) as GameObject;

                        Player player = obj.GetComponent<Player>();
                        player.client = client;
                    }
                    else
                    {
                        Debug.Log("false");
                        obj = Instantiate(networkPrefab, position, Quaternion.identity) as GameObject;
                    }

                    PlayerObject playerObj = obj.GetComponent<PlayerObject>();
                   //playerObj.SetRadius(radius);
                    playerObj.SetColor(color);
                    CameraMovement cameraMovement = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<CameraMovement>();
                  //  cameraMovement.SetTarget(obj.transform);

                    TrailRenderer2D trailRenderer = obj.GetComponent<TrailRenderer2D>();
                    trailRenderer.SetColor(color);

                    networkPlayerManager.Add(id, playerObj);

                }
            }
        }
    }

    void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage() as Message)
        {
            if (message.Tag == Tags.SpawnPlayerTag)
                SpawnPlayer(sender, e);
            else if (message.Tag == Tags.DespawnPlayerTag)
                DespawnPlayer(sender, e);
        }
    }

    void DespawnPlayer(object sender, MessageReceivedEventArgs e)
    {
        using (Message message = e.GetMessage())
        using (DarkRiftReader reader = message.GetReader())
            networkPlayerManager.DestroyPlayer(reader.ReadUInt16());
    }



}
