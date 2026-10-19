using Cysharp.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectUiController : MonoBehaviour
{
    [SerializeField] private Button connectButton;
    [SerializeField] private Button retryButton;
    [SerializeField] private TextMeshProUGUI statusText;

    private bool isStarting;

    private void Awake()
    {
        retryButton.gameObject.SetActive(false);
    }

    public void StartHostFromButton()
    {
        StartHostAsync().Forget();
    }

    public void RetryHostFromButton()
    {
        StartHostAsync().Forget();
    }

    private async UniTask StartHostAsync()
    {
        if (isStarting)
            return;

        if (NetworkManager.Singleton != null &&
            NetworkManager.Singleton.IsListening)
        {
            statusText.text = "Host Already Running";
            connectButton.interactable = false;
            retryButton.gameObject.SetActive(false);
            return;
        }

        isStarting = true;

        connectButton.interactable = false;
        retryButton.gameObject.SetActive(false);

        statusText.text = "Host Starting...";

        bool started =
            NetworkManager.Singleton.StartHost();

        if (!started)
        {
            statusText.text = "Host Start Failed";

            connectButton.interactable = true;
            retryButton.gameObject.SetActive(true);

            isStarting = false;
            return;
        }

        await UniTask.Yield(
            cancellationToken:
            this.GetCancellationTokenOnDestroy());

        statusText.text = "Host Start Complete";

        connectButton.interactable = false;
        retryButton.gameObject.SetActive(false);

        isStarting = false;
    }
}