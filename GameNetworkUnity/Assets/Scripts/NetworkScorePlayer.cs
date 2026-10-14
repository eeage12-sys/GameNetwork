using Unity.Netcode;
using UnityEngine;
using TMPro;

public class NetworkScorePlayer : NetworkBehaviour
{
    public NetworkVariable<int> Score = new(0);

    [Header("UI")]
    public GameObject OwnerOnlyCanvas;
    public TextMeshProUGUI ScoreText;

    public override void OnNetworkSpawn()
    {
        // 점수 텍스트 초기화
        UpdateScoreText(Score.Value);

        // 점수 변경 이벤트 연결
        Score.OnValueChanged += OnScoreChanged;

        // 내 플레이어가 아니면 UI 숨김
        if (OwnerOnlyCanvas != null)
        {
            OwnerOnlyCanvas.SetActive(IsOwner);
        }
    }

    public override void OnNetworkDespawn()
    {
        Score.OnValueChanged -= OnScoreChanged;
    }

    private void OnScoreChanged(int previousValue, int newValue)
    {
        UpdateScoreText(newValue);
    }

    private void UpdateScoreText(int value)
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score: {value}";
        }
    }

    public void RequestAddScore()
    {
        if (!IsOwner)
            return;

        RequestAddScoreRpc();
    }

    [Rpc(SendTo.Server)]
    private void RequestAddScoreRpc()
    {
        Score.Value += 1;
        Debug.Log($"Server가 점수를 {Score.Value}로 변경했습니다.");
    }
}