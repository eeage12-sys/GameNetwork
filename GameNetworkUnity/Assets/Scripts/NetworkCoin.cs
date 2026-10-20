using Unity.Netcode;
using UnityEngine;

public class NetworkCoin : NetworkBehaviour
{
    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer)
            return;

        if (collected)
            return;

        NetworkScorePlayer player =
            other.GetComponent<NetworkScorePlayer>();

        if (player == null)
            return;

        collected = true;

        // 플레이어 점수 증가
        player.Score.Value += 1;

        // 협동 목표 증가
        NetworkGameGoal gameGoal =
            FindFirstObjectByType<NetworkGameGoal>();

        if (gameGoal != null)
        {
            gameGoal.CollectCoin();
        }

        // 모든 참여자 화면에서 코인을 숨김
        HideCoinRpc();

        // 네트워크에서는 Despawn 처리
        NetworkObject.Despawn(false);
    }

    [Rpc(SendTo.Everyone)]
    private void HideCoinRpc()
    {
        Renderer[] renderers =
            GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = false;
        }

        Collider[] colliders =
            GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }
}