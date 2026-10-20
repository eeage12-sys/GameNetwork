using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkGameGoal : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI goalText;

    public NetworkVariable<int> CollectedCoins = new(0);
    public NetworkVariable<bool> GameClear = new(false);

    public override void OnNetworkSpawn()
    {
        CollectedCoins.OnValueChanged += OnCoinCountChanged;
        GameClear.OnValueChanged += OnGameClearChanged;

        UpdateText();
    }

    public override void OnNetworkDespawn()
    {
        CollectedCoins.OnValueChanged -= OnCoinCountChanged;
        GameClear.OnValueChanged -= OnGameClearChanged;
    }

    public void CollectCoin()
    {
        // 결과 변경은 Server만
        if (!IsServer)
            return;

        // 이미 게임이 끝났으면 다시 처리하지 않음
        if (GameClear.Value)
            return;

        CollectedCoins.Value++;

        if (CollectedCoins.Value >= 3)
        {
            GameClear.Value = true;
            Debug.Log("Game Success");
        }
    }

    private void OnCoinCountChanged(int oldValue, int newValue)
    {
        UpdateText();
    }

    private void OnGameClearChanged(bool oldValue, bool newValue)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (goalText == null)
            return;

        if (GameClear.Value)
        {
            goalText.text = "GAME SUCCESS";
        }
        else
        {
            goalText.text =
                $"Coins: {CollectedCoins.Value} / 3";
        }
    }
}