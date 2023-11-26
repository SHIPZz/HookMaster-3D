using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Cameras;
using com.ootii.Geometry;
using com.ootii.Actors.BoneControllers;
using com.ootii.Utilities.Debug;

public class RotationMotorCode : SceneCode
{
    private int mCameraState = 0;

    private GameObject mShip = null;

    private GameObject mCamera = null;

    private bool mInitialized = false;

    // Use this for initialization
    void Start()
    {
        mShip = GameObject.Find("Ship");
        mCamera = GameObject.Find("Main Camera");

        WatchCamera();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!mInitialized)
        {
            // Give time for the skeleton to build the motors
            if (mCamera.GetComponent<BoneController>() != null)
            {
                if (mCamera.GetComponent<BoneController>().GetMotor<LookAtMotor>() != null)
                {
                    WatchCamera();
                    mInitialized = true;
                }
            }
        }

        // Can't seem to set the local rotation in response to a button press.
        // So, we'll force it here.
        if (mCameraState == 1)
        {
            mCamera.transform.localRotation = Quaternion.Euler(0f, 90f, 90f);
        }

        // Continue with the update
        base.Update();
    }

    public void WatchCamera()
    {
        mCameraState = 0;

        mShip.GetComponent<com.ootii.Demos.PathFollower>().SpeedMultiplier = 1f;

        mCamera.GetComponent<FlyRig>().enabled = false;

        BoneController lCameraSkeleton = mCamera.GetComponent<BoneController>();
        if (lCameraSkeleton != null)
        {
            LookAtMotor lCameraMotor = lCameraSkeleton.GetMotor<LookAtMotor>();
            if (lCameraMotor != null)
            {
                lCameraMotor.IsEnabled = true;
            }
        }

        mCamera.transform.parent = null;
        mCamera.transform.position = new Vector3(-3.011f, 3.965f, 0.88f);
        mCamera.transform.rotation = Quaternion.Euler(16.8f, 70f, 0f);
    }

    public void ShipCamera()
    {
        mCameraState = 1;

        mShip.GetComponent<com.ootii.Demos.PathFollower>().SpeedMultiplier = 0.65f;

        mCamera.GetComponent<FlyRig>().enabled = false;
        mCamera.GetComponent<BoneController>().GetMotor<LookAtMotor>().IsEnabled = false;

        mCamera.transform.parent = mShip.transform;
        mCamera.transform.localPosition = new Vector3(-2.0f, 0.0f, 0.75f);
        mCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FreeCamera()
    {
        mCameraState = 2;

        mShip.GetComponent<com.ootii.Demos.PathFollower>().SpeedMultiplier = 1f;

        mCamera.GetComponent<FlyRig>().enabled = true;
        mCamera.GetComponent<BoneController>().GetMotor<LookAtMotor>().IsEnabled = false;

        mCamera.transform.parent = null;
        mCamera.transform.position = new Vector3(-3.011f, 3.965f, 0.88f);
        mCamera.transform.rotation = Quaternion.Euler(16.8f, 70f, 0f);
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("LookAtMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("LookAtMotor");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();
    }
}
