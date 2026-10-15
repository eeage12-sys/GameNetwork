using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkPlayerInteract : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference interactAction;

    [SerializeField]
    private float interactionDistance = 2.5f;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            interactAction.action.Enable();
    }

    public override void OnNetworkDespawn()
    {
        if (IsOwner)
            interactAction.action.Disable();

        base.OnNetworkDespawn();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        if (interactAction.action.WasPressedThisFrame())
        {
            RequestInteractRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void RequestInteractRpc()
    {
        Collider[] hits =
            Physics.OverlapSphere(
                transform.position,
                interactionDistance);

        foreach (Collider hit in hits)
        {
            NetworkDoor door =
                hit.GetComponent<NetworkDoor>();

            if (door == null)
                continue;

            float distance =
                Vector3.Distance(
                    transform.position,
                    door.transform.position);

            if (distance <= interactionDistance)
            {
                door.OpenDoorServer();
                return;
            }
        }
    }
}