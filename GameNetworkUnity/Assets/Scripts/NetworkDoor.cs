using Unity.Netcode;
using UnityEngine;

public class NetworkDoor : NetworkBehaviour
{
    public NetworkVariable<bool> IsOpen = new(false);

    private Vector3 closedPosition;
    private Vector3 openPosition;

    public override void OnNetworkSpawn()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * 3f;

        IsOpen.OnValueChanged += OnDoorStateChanged;

        ApplyDoorState(IsOpen.Value);
    }

    public override void OnNetworkDespawn()
    {
        IsOpen.OnValueChanged -= OnDoorStateChanged;
        base.OnNetworkDespawn();
    }

    private void OnDoorStateChanged(bool previousValue, bool newValue)
    {
        ApplyDoorState(newValue);
    }

    private void ApplyDoorState(bool open)
    {
        transform.position = open ? openPosition : closedPosition;
    }

    public void OpenDoorServer()
    {
        if (!IsServer)
            return;

        if (IsOpen.Value)
            return;

        IsOpen.Value = true;

        Debug.Log("Server가 문을 열었습니다.");
    }
}