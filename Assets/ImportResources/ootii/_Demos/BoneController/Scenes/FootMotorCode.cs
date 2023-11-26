using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;

public class FootMotorCode : SceneCode
{
    GameObject mHuman3 = null;
    Vector3 mHuman3Position = Vector3.zero;

    GameObject mPlatform1 = null;
    Vector3 mPlatform1Position = Vector3.zero;

    GameObject mPlatform2 = null;
    Vector3 mPlatform2Position = Vector3.zero;

    float mPlatformOffset = 0f;

    // Use this for initialization
    void Start()
    {
        mHuman3 = GameObject.Find("Human3");
        if (mHuman3 != null)
        {
            mHuman3Position = mHuman3.transform.position;
        }

        mPlatform1 = GameObject.Find("Platform1");
        mPlatform1Position = mPlatform1.transform.position;

        mPlatform2 = GameObject.Find("Platform2");
        mPlatform2Position = mPlatform2.transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        mPlatform1.transform.position = Vector3.Lerp(mPlatform1.transform.position, mPlatform1Position + (Vector3.up * mPlatformOffset * 0.15f), 0.1f);
        mPlatform2.transform.position = Vector3.Lerp(mPlatform2.transform.position, mPlatform2Position + (Vector3.up * mPlatformOffset * 0.15f), 0.1f);

        if (mHuman3 != null && mHuman3.transform.position.z < -8f)
        {
            mHuman3.transform.position = mHuman3Position;
        }

        // Continue with the update
        base.Update();
    }

    /// <summary>
    /// Allows us to raise and lower the platform
    /// </summary>
    /// <param name="rValue"></param>
    public void RaisePlatform(float rValue)
    {
        mPlatformOffset = rValue;
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("FootMotorInverted");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("FootMotorInverted");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();

        EnableMotors("Human1", mIsMotorEnabled);
        EnableMotors("Human2", mIsMotorEnabled);
        EnableMotors("Human3", mIsMotorEnabled);
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Human1", mIsMotorEnabled);
        EnableMotors("Human2", mIsMotorEnabled);
        EnableMotors("Human3", mIsMotorEnabled);
    }
}
