using Unity.Netcode;
using UnityEngine;

public class NetworkCoin : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        NetworkScorePlayer player =
            other.GetComponent<NetworkScorePlayer>();

        if (player == null)
            return;

        player.Score.Value += 1;

        NetworkObject.Despawn();
    }
}