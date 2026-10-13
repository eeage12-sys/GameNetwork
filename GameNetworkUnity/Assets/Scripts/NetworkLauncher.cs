using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkLauncher : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;

    private const string HostAddress = "127.0.0.1";
    private const ushort HostPort = 7777;

    public void StartHost()
    {
        NetworkManager manager = NetworkManager.Singleton;

        if (manager.IsListening)
        {
            return;
        }

        UnityTransport transport =
            manager.GetComponent<UnityTransport>();

        transport.SetConnectionData(
            HostAddress,
            HostPort,
            HostAddress);

        bool started = manager.StartHost();

        statusText.text =
            started ? "Host Start" : "Host Start Failed";
    }

    public void StartClient()
    {
        NetworkManager manager = NetworkManager.Singleton;

        if (manager.IsListening)
        {
            return;
        }

        UnityTransport transport =
            manager.GetComponent<UnityTransport>();

        transport.SetConnectionData(
            HostAddress,
            HostPort);

        bool started = manager.StartClient();

        statusText.text =
            started ? "Client Connecting" : "Client Start Failed";
    }

    private void Update()
    {
        NetworkManager manager = NetworkManager.Singleton;

        if (manager == null)
        {
            return;
        }

        if (manager.IsHost)
        {
            statusText.text =
                $"Host Running / Players {manager.ConnectedClientsIds.Count}";
        }
        else if (manager.IsConnectedClient)
        {
            statusText.text =
                "Client Connected";
        }
    }
}