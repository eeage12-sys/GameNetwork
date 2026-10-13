using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class NetworkCharacterPlayer : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private InputAction moveAction;

    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        moveAction = new InputAction(
            "Move",
            InputActionType.Value);

        // WASD
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // 방향키
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

    public override void OnNetworkSpawn()
    {
        // 내가 소유한 캐릭터만 CharacterController 사용
        controller.enabled = IsOwner;

        if (!IsOwner)
        {
            return;
        }

        // Host 캐릭터는 왼쪽
        if (OwnerClientId == NetworkManager.ServerClientId)
        {
            transform.position =
                new Vector3(-2f, 0f, 0f);
        }
        // Client 캐릭터는 오른쪽
        else
        {
            transform.position =
                new Vector3(2f, 0f, 0f);
        }

        moveAction.Enable();
    }

    public override void OnNetworkDespawn()
    {
        moveAction.Disable();

        base.OnNetworkDespawn();
    }

    private void Update()
    {
        // 내 캐릭터가 아니면 입력 처리 안 함
        if (!IsOwner || !controller.enabled)
        {
            return;
        }

        Vector2 input =
            moveAction.ReadValue<Vector2>();

        Vector3 move =
            new Vector3(
                input.x,
                0f,
                input.y);

        if (move.sqrMagnitude > 1f)
        {
            move.Normalize();
        }

        // 바닥에 있을 때 아래 방향 속도 유지
        if (controller.isGrounded &&
            verticalVelocity < 0f)
        {
            verticalVelocity = -1f;
        }

        // 중력
        verticalVelocity +=
            gravity * Time.deltaTime;

        Vector3 velocity =
            move * moveSpeed;

        velocity.y = verticalVelocity;

        // 실제 이동
        controller.Move(
            velocity * Time.deltaTime);

        // 움직이는 방향 바라보기
        if (move.sqrMagnitude > 0.001f)
        {
            transform.forward = move;
        }
    }

    public override void OnDestroy()
    {
        moveAction?.Dispose();

        base.OnDestroy();
    }
}