using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

public class MovingSplinePlatform : MonoBehaviour
{
    #region References

    [SerializeField]
    private SplineContainer spline;

    #endregion

    #region Settings

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    [Range(0f, 1f)]
    private float startPosition;

    private PlayerController passenger;

    private int passengerContacts;

    private Vector3 previousPosition;

    public Vector2 MovementDelta { get; private set; }

    #endregion

    #region State

    private float currentDistance;

    private float splineLength;

    #endregion

    #region Unity Messages

    private void Awake()
    {
        if (spline == null)
            return;

        splineLength =
            spline.Splines[0].GetLength();

        currentDistance =
            startPosition * splineLength;

        MovePlatform();

        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (spline == null)
            return;

        bool loopReset = false;

        currentDistance +=
            speed * Time.fixedDeltaTime;

        if (currentDistance >= splineLength)
        {
            currentDistance = 0f;
            loopReset = true;
        }

        MovePlatform(loopReset);

        if (passenger != null)
        {
            passenger.SetPlatformMovement(
                MovementDelta);
        }
    }

    #endregion

    #region Helpers

    private void MovePlatform(
    bool ignoreDelta = false)
    {
        float t =
            currentDistance / splineLength;

        float3 localPosition =
            spline.Splines[0]
            .EvaluatePosition(t);

        Vector3 worldPosition =
            spline.transform.TransformPoint(localPosition);

        transform.position =
            worldPosition;

        if (ignoreDelta)
        {
            MovementDelta = Vector2.zero;
        }
        else
        {
            MovementDelta =
                transform.position -
                previousPosition;
        }

        previousPosition =
            transform.position;
    }

    private void OnCollisionEnter2D(
    Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;


        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                passenger =
                    collision.gameObject
                    .GetComponent<PlayerController>();

                passengerContacts++;

                return;
            }
        }
    }


    private void OnCollisionExit2D(
    Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;


        passengerContacts--;

        if (passengerContacts <= 0)
        {
            passenger = null;
            passengerContacts = 0;
        }
    }

    #endregion
}