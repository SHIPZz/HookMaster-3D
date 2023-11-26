using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;
using com.ootii.Utilities.Debug;

public class SkeletonRaycastCode : SceneCode
{
    private GameObject mHuman = null;
    private BoneController mSkeleton = null;

    // Gun information
    private GameObject mGun1 = null;

    private float mGun1Up = 0f;
    private float mGun1Right = 0f;
    private Vector3 mGun1Position = Vector3.zero;

    private bool mGun1IsActive = true;

    // Collision info
    private IKBone mHitBone = null;
    private Vector3 mHitPoint = Vector3.zero;


    // Use this for initialization
    void Start()
    {
        mGun1 = GameObject.Find("Gun 1");
        mGun1Position = mGun1.transform.position;

        mHuman = GameObject.Find("Human");
        mSkeleton = mHuman.GetComponent<BoneController>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (mSkeleton == null) { return; }
        if (mGun1 == null) { return; }

        Color lRayColor = Color.white;
        float lRayColorAlpha = 1f;

        // Move the gun
        Vector3 lUp = Vector3.up * mGun1Up * 0.75f;
        Vector3 lRight = Vector3.right * mGun1Right * 0.5f;
        mGun1.transform.position = Vector3.Lerp(mGun1.transform.position, mGun1Position + lUp + lRight, 0.1f);

        // Shoot the gun
        if (mGun1IsActive)
        {
            mSkeleton.TestRayCollision(mGun1.transform.position, mGun1.transform.forward, 4f, out mHitBone, out mHitPoint);
        }

        if (mHitBone != null && mHitPoint.sqrMagnitude != 0f)
        {
            lRayColor = Color.red;
            lRayColorAlpha = 1f;
        }

        DebugDraw.DrawLine(mGun1.transform.position, mGun1.transform.position + (mGun1.transform.forward * 4f), 0.01f, lRayColor, lRayColorAlpha);

        // Continue with the update
        base.Update();
    }

    /// <summary>
    /// Positions the gun so we can shoot
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveGunUp(float rValue)
    {
        mGun1Up = rValue;
        mHitBone = null;
    }

    /// <summary>
    /// Positions the gun so we can shoot
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveGunRight(float rValue)
    {
        mGun1Right = rValue;
        mHitBone = null;
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("ImpactMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("ImpactMotor");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }
}
