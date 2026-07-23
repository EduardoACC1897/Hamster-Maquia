using UnityEngine;

public class PlayerSpriteAnimator : MonoBehaviour
{
    #region References

    [Header("References")]

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private PlayerIngredientEssence playerEssence;

    private PlayerController controller;

    private PlayerInputHandler input;

    #endregion

    #region Idle

    [Header("Idle")]

    [SerializeField]
    private SpriteAnimation[] idleAnimations =
        new SpriteAnimation[3];

    [SerializeField]
    private float idleDelay = 3f;

    #endregion

    #region Walk

    [Header("Walk")]

    [SerializeField]
    private SpriteAnimation[] walkAnimations =
        new SpriteAnimation[3];

    #endregion

    #region Run

    [Header("Run")]

    [SerializeField]
    private SpriteAnimation[] runAnimations =
        new SpriteAnimation[3];

    #endregion

    #region Crouch

    [Header("Crouch")]

    [SerializeField]
    private SpriteAnimation[] crouchAnimations =
        new SpriteAnimation[3];

    #endregion

    #region Jump

    [Header("Jump")]

    [SerializeField]
    private SpriteAnimation[] jumpUpAnimations =
        new SpriteAnimation[3];

    [SerializeField]
    private SpriteAnimation[] jumpDownAnimations =
        new SpriteAnimation[3];

    #endregion

    #region State

    private int currentEssenceLevel;

    private float idleTimer;

    private float frameTimer;

    private int currentFrame;

    private PlayerAnimationState currentState =
        PlayerAnimationState.Idle;

    private bool animationPlaying;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        playerEssence =
            GetComponent<PlayerIngredientEssence>();

        controller =
            GetComponent<PlayerController>();

        input =
            GetComponent<PlayerInputHandler>();

        currentEssenceLevel =
            playerEssence.EssenceLevel;

        ShowIdleFrameZero();
    }

    private void Update()
    {
        UpdateEssenceLevel();

        PlayerAnimationState state =
            GetMovementState();

        switch (state)
        {
            case PlayerAnimationState.Idle:
                UpdateIdle();
                break;

            case PlayerAnimationState.Walk:
                UpdateWalk();
                break;

            case PlayerAnimationState.Run:
                UpdateRun();
                break;

            case PlayerAnimationState.Crouch:
                UpdateCrouch();
                break;

            case PlayerAnimationState.JumpUp:
                UpdateJumpUp();
                break;

            case PlayerAnimationState.JumpDown:
                UpdateJumpDown();
                break;
        }
    }

    #endregion

    #region Idle

    private void UpdateIdle()
    {
        ChangeState(
            PlayerAnimationState.Idle);

        if (!animationPlaying)
        {
            idleTimer += Time.deltaTime;

            ShowIdleFrameZero();

            if (idleTimer >= idleDelay)
            {
                StartAnimation();

                idleTimer = 0f;
            }

            return;
        }

        if (!PlayAnimation(
            idleAnimations,
            false))
        {
            return;
        }

        StopAnimation();

        idleTimer = 0f;

        ShowIdleFrameZero();
    }

    private void ShowIdleFrameZero()
    {
        SpriteAnimation animation =
            GetAnimation(idleAnimations);

        if (animation == null)
            return;

        SetSprite(
            animation.Frames[0]);
    }

    #endregion

    #region Walk

    private void UpdateWalk()
    {
        ChangeState(
            PlayerAnimationState.Walk);

        UpdateLoopAnimation(
            walkAnimations);
    }

    #endregion

    #region Run

    private void UpdateRun()
    {
        ChangeState(
            PlayerAnimationState.Run);

        UpdateLoopAnimation(
            runAnimations);
    }

    #endregion

    #region Crouch

    private void UpdateCrouch()
    {
        ChangeState(
            PlayerAnimationState.Crouch);

        UpdateLoopAnimation(
            crouchAnimations);
    }

    #endregion

    #region Jump

    private void UpdateJumpUp()
    {
        UpdateStaticAnimation(
            PlayerAnimationState.JumpUp);
    }

    private void UpdateJumpDown()
    {
        UpdateStaticAnimation(
            PlayerAnimationState.JumpDown);
    }

    #endregion

    #region Helpers

    private void UpdateEssenceLevel()
    {
        int newLevel =
            playerEssence.EssenceLevel;

        if (newLevel == currentEssenceLevel)
            return;

        currentEssenceLevel = newLevel;

        ChangeState(currentState);
    }

    private void SetSprite(
    Sprite sprite)
    {
        if (sprite == null)
            return;

        if (spriteRenderer == null)
            return;

        if (spriteRenderer.sprite == sprite)
            return;

        spriteRenderer.sprite = sprite;
    }

    private SpriteAnimation GetAnimation(
    SpriteAnimation[] animations)
    {
        if (animations == null)
            return null;

        if (currentEssenceLevel < 0 ||
            currentEssenceLevel >= animations.Length)
            return null;

        SpriteAnimation animation =
            animations[currentEssenceLevel];

        if (animation == null)
            return null;

        if (animation.Frames == null)
            return null;

        if (animation.Frames.Length == 0)
            return null;

        return animation;
    }

    private bool PlayAnimation(
        SpriteAnimation[] animations,
        bool loop)
    {
        SpriteAnimation animation =
            GetAnimation(animations);

        if (animation == null)
            return true;

        frameTimer += Time.deltaTime;

        float frameDuration =
            1f / animation.FPS;

        if (frameTimer < frameDuration)
            return false;

        frameTimer = 0f;

        currentFrame++;

        if (currentFrame >= animation.Frames.Length)
        {
            if (loop)
            {
                currentFrame = 0;
            }
            else
            {
                currentFrame =
                    animation.Frames.Length - 1;

                return true;
            }
        }

        SetSprite(
            animation.Frames[currentFrame]);

        return false;
    }

    private void ChangeState(
    PlayerAnimationState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        StopAnimation();

        SpriteAnimation animation =
            GetStateAnimation(newState);

        switch (newState)
        {
            case PlayerAnimationState.Walk:
            case PlayerAnimationState.Run:
            case PlayerAnimationState.Crouch:

                StartAnimation();

                break;
        }

        if (animation != null)
        {
            SetSprite(
                animation.Frames[0]);
        }
    }

    private PlayerAnimationState GetMovementState()
    {
        if (!controller.IsGrounded)
        {
            if (controller.VerticalVelocity > 0f)
                return PlayerAnimationState.JumpUp;

            return PlayerAnimationState.JumpDown;
        }

        if (controller.IsCrouching)
            return PlayerAnimationState.Crouch;

        if (Mathf.Abs(input.MoveX) > 0.01f)
        {
            if (input.RunHeld)
                return PlayerAnimationState.Run;

            return PlayerAnimationState.Walk;
        }

        return PlayerAnimationState.Idle;
    }

    private void StartAnimation()
    {
        animationPlaying = true;

        currentFrame = 0;

        frameTimer = 0f;
    }

    private void StopAnimation()
    {
        animationPlaying = false;

        currentFrame = 0;

        frameTimer = 0f;
    }

    private void UpdateLoopAnimation(
    SpriteAnimation[] animations)
    {
        PlayAnimation(
            animations,
            true);
    }

    private void UpdateStaticAnimation(
    PlayerAnimationState state)
    {
        ChangeState(state);
    }

    private SpriteAnimation GetStateAnimation(
    PlayerAnimationState state)
    {
        switch (state)
        {
            case PlayerAnimationState.Idle:
                return GetAnimation(idleAnimations);

            case PlayerAnimationState.Walk:
                return GetAnimation(walkAnimations);

            case PlayerAnimationState.Run:
                return GetAnimation(runAnimations);

            case PlayerAnimationState.Crouch:
                return GetAnimation(crouchAnimations);

            case PlayerAnimationState.JumpUp:
                return GetAnimation(jumpUpAnimations);

            case PlayerAnimationState.JumpDown:
                return GetAnimation(jumpDownAnimations);

            default:
                return null;
        }
    }

    #endregion
}