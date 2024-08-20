using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    public Canvas canvas;
    [SerializeField] private Button stopClientButton;
    [SerializeField] private Button startClientButton;

    private void Awake()
    {
        stopClientButton.onClick.AddListener(() =>
        {
            if (NetworkManager.Singleton.IsConnectedClient)
            {
                NetworkManager.Singleton.Shutdown(true);
                NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
            }
        });

        startClientButton.onClick.AddListener(() =>
        {
            {
                Debug.Log("CLIENT");

                string keyId = "b24db6e0-b30a-4b44-b207-893d8aed3959";
                string keySecret = "u7VP1pBvofSGaMhso1VQk0OxdXss5CW9";
                byte[] keyByteArray = Encoding.UTF8.GetBytes(keyId + ":" + keySecret);
                string keyBase64 = Convert.ToBase64String(keyByteArray);

                string projectID = "5adce170-0bc4-4968-b9bf-c97939658caf";
                string envId = "bfd6b65e-9040-4b12-a206-1e3d97381b1f";
                string url = $"https://services.api.unity.com/multiplay/servers/v1/projects/{projectID}/environments/{envId}/servers";

                if (req == null) req = StartCoroutine(GetRequest(url, keyBase64));

                //NetworkManager.Singleton.StartClient();
                //SetActive(false);
            }
        });
    }

    Coroutine req;
    IEnumerator GetRequest(string url, string key)
    {
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
        unityWebRequest.SetRequestHeader("Authorization", "Basic " + key);
        Debug.Log("Sent");
        yield return unityWebRequest.SendWebRequest();
        Debug.Log(unityWebRequest.responseCode + "    " + unityWebRequest.downloadHandler.text);

        ListServers listServers = JsonUtility.FromJson<ListServers>("{\"serverList\":" + unityWebRequest.downloadHandler.text + "}");

        if (listServers.serverList.Length > 0)
        {

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(listServers.serverList[0].ip, (ushort)listServers.serverList[0].port);

            NetworkManager.Singleton.StartClient();
        }
    }
    public class TokenExchangeResponse
    {
        public string accessToken;
    }


    [Serializable]
    public class TokenExchangeRequest
    {
        public string[] scopes;
    }

    [Serializable]
    public class QueueAllocationRequest
    {
        public string allocationId;
        public int buildConfigurationId;
        public string payload;
        public string regionId;
        public bool restart;
    }


    private enum ServerStatus
    {
        AVAILABLE,
        ONLINE,
        ALLOCATED
    }

    [Serializable]
    public class ListServers
    {
        public Server[] serverList;
    }

    [Serializable]
    public class Server
    {
        public int buildConfigurationID;
        public string buildConfigurationName;
        public string buildName;
        public bool deleted;
        public string fleetID;
        public string fleetName;
        public string hardwareType;
        public int id;
        public string ip;
        public int locationID;
        public string locationName;
        public int machineID;
        public int port;
        public string status;
    }


    private void SetActive(bool isActive)
    {
        GetComponent<Canvas>().enabled = (isActive);
    }
}
