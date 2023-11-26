using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;
using com.ootii.Utilities.Debug;

public class ImpactMotorCode : SceneCode
{
    private GameObject mHuman1 = null;
    private ImpactMotor mHuman1Motor = null;

    private GameObject mHuman2 = null;
    private ImpactMotor mHuman2Motor = null;

    private float mPower = 0.05f;

    // Gun information
    private GameObject mGun1 = null;

    private float mGun1Up = 0f;
    private float mGun1Right = 0f;
    private Vector3 mGun1Position = Vector3.zero;

    private bool mGun1IsActive = false;

    // Basic physics properties
    private float mArrowMass = 0.0195f;     // Arrow = 19.5 grams
    private float mArrowSpeed = 83.2f;      // meters per second
    private float mArrowRange = 5.4f;       // meters (27.4 seems max for 'straight shot')

    // Use this for initialization
    void Start()
    {
        mGun1 = GameObject.Find("Gun 1");
        mGun1Position = mGun1.transform.position;

        mHuman1 = GameObject.Find("Human 1");
        mHuman2 = GameObject.Find("Human 2");
    }

    // Update is called once per frame
    public override void Update()
    {
        if (mHuman1Motor == null) { mHuman1Motor = mHuman1.GetComponent<BoneController>().GetMotor<ImpactMotor>(); }
        if (mHuman2Motor == null) { mHuman2Motor = mHuman2.GetComponent<BoneController>().GetMotor<ImpactMotor>(); }

        if (mGun1 != null)
        {
            bool lRayActive = mGun1IsActive;
            float lRayColorAlpha = 0.5f;
            Color lRayColor = Color.yellow;

            // Move the gun
            Vector3 lUp = Vector3.up * mGun1Up * 0.75f;
            Vector3 lRight = Vector3.right * mGun1Right * 1.0f;
            mGun1.transform.position = Vector3.Lerp(mGun1.transform.position, mGun1Position + lUp + lRight, 0.1f);

            // Shoot the gun
            if (mHuman1Motor != null)
            {
                Vector3 lHitPoint = Vector3.zero;

                if (lRayActive)
                {
                    mHuman1Motor.Power = mPower;
                    BoneControllerBone lBone = mHuman1Motor.RaycastImpact(mGun1.transform.position, mGun1.transform.forward * mArrowSpeed, mArrowRange, mArrowMass, false, ref lHitPoint);
                    if (lBone != null)
                    {
                        lRayColor = Color.red;
                        lRayColorAlpha = 1.0f;

                        DebugDraw.DrawSphereOverlay(lHitPoint, 0.02f, lRayColor, lRayColorAlpha);
                    }

                    mGun1IsActive = false;
                }
                else
                {
                    BoneControllerBone lBone = mHuman1Motor.Raycast(mGun1.transform.position, mGun1.transform.forward * mArrowSpeed, mArrowRange, false, ref lHitPoint);
                    if (lBone != null)
                    {
                        DebugDraw.DrawSphereOverlay(lHitPoint, 0.02f, lRayColor, lRayColorAlpha);
                    }
                }
            }

            // Shoot the gun
            if (mHuman2Motor != null)
            {
                Vector3 lHitPoint = Vector3.zero;

                if (lRayActive)
                {
                    mHuman2Motor.Power = mPower;
                    BoneControllerBone lBone = mHuman2Motor.RaycastImpact(mGun1.transform.position, mGun1.transform.forward * mArrowSpeed, mArrowRange, mArrowMass, false, ref lHitPoint);
                    if (lBone != null)
                    {
                        lRayColor = Color.red;
                        lRayColorAlpha = 1.0f;

                        DebugDraw.DrawSphereOverlay(lHitPoint, 0.02f, lRayColor, lRayColorAlpha);
                    }

                    mGun1IsActive = false;
                }
                else
                {
                    BoneControllerBone lBone = mHuman2Motor.Raycast(mGun1.transform.position, mGun1.transform.forward * mArrowSpeed, mArrowRange, false, ref lHitPoint);
                    if (lBone != null)
                    {
                        DebugDraw.DrawSphereOverlay(lHitPoint, 0.02f, lRayColor, lRayColorAlpha);
                    }
                }
            }

            DebugDraw.DrawLine(mGun1.transform.position, mGun1.transform.position + (mGun1.transform.forward * mArrowRange), 0.01f, lRayColor, lRayColorAlpha);
        }

        // Continue with the update
        base.Update();
    }

    /// <summary>
    /// Sets the power of the gun
    /// </summary>
    /// <param name="rValue"></param>
    public void SetPower(float rValue)
    {
        mPower = rValue;
    }

    /// <summary>
    /// Positions the gun so we can shoot
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveGunUp(float rValue)
    {
        mGun1Up = rValue;
    }

    /// <summary>
    /// Positions the gun so we can shoot
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveGunRight(float rValue)
    {
        mGun1Right = rValue;
    }

    /// <summary>
    /// Shoot a ray at our target
    /// </summary>
    public void ShootGun()
    {
        mGun1IsActive = !mGun1IsActive;
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("JointLimits");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("JointLimits");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human 1", mIsMotorEnabled);
        EnableMotors("Human 2", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human 1", mIsMotorEnabled);
        EnableMotors("Human 2", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }
}
