using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;

public class MultiplayManager : MonoBehaviour
{
    public IServerQueryHandler serverQueryHandler;

    private async void Start()
    {
        if(Application.platform == RuntimePlatform.LinuxServer)
        {
            Application.targetFrameRate = 30;

            await UnityServices.InitializeAsync();

            ServerConfig serverConfig = MultiplayService.Instance.ServerConfig;

            serverQueryHandler = await MultiplayService.Instance.StartServerQueryHandlerAsync(45, "MyServer", "MyGameType", "0", "TestMap");

            if (serverConfig.AllocationId != string.Empty)
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData("0.0.0.0", serverConfig.Port, "0.0.0.0");

                NetworkManager.Singleton.StartServer();

                await MultiplayService.Instance.ReadyServerForPlayersAsync();
            }
        }
    }

    private async void Update()
    {
        if(Application.platform == RuntimePlatform.LinuxServer)
        {
            if(serverQueryHandler != null)
            {
                serverQueryHandler.CurrentPlayers = (ushort)NetworkManager.Singleton.ConnectedClientsIds.Count;
                serverQueryHandler.UpdateServerCheck();
                await Task.Delay(1000);
            }
        }
    }

    public void JoinToServer()
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetConnectionData("34.16.85.176", ushort.Parse("9000"));

        NetworkManager.Singleton.StartClient();
    }
}
