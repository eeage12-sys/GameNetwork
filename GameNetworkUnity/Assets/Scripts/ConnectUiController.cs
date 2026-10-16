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
        // 처음에는 Retry 버튼을 숨깁니다.
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
        // 이미 시작 처리 중이면 중복 요청 금지
        if (isStarting)
            return;

        // 이미 Host/Server/Client가 실행 중이라면 다시 StartHost 하지 않음
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

        // Host가 이미 켜졌으므로 다시 시작 버튼을 누를 필요 없음
        connectButton.interactable = false;
        retryButton.gameObject.SetActive(false);

        isStarting = false;
    }
}