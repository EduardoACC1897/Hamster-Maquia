using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    #region References

    protected PlayerController controller;
    protected PlayerInputHandler input;

    #endregion

    #region Unity Messages

    protected virtual void Awake()
    {
        controller = GetComponent<PlayerController>();
        input = GetComponent<PlayerInputHandler>();
    }

    #endregion

    #region Custom Update Methods

    public virtual void OnCustomUpdate() { }

    public virtual void OnCustomFixedUpdate() { }

    #endregion
}