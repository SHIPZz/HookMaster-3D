using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;

public class FingerPoseMotorCode : SceneCode
{
    private GameObject mHuman = null;
    private GameObject mGoblin = null;

    // Use this for initialization
    void Start()
    {
        mHuman = GameObject.Find("Human");
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.Weight = 1;
        lHumanMotor.RightCurl = 1;

        mGoblin = GameObject.Find("Goblin");
        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.Weight = 1;
        lGoblinMotor.RightCurl = 1;
    }

    // Clenches the fist
    public void Weight(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.Weight = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.Weight = rPercent;
    }

    // Clenches the fist
    public void CurlRightHand(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightCurl = rPercent;
    }

    // Clenches the fist
    public void CurlRightThumb(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightThumbCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightThumbCurl = rPercent;
    }

    // Clenches the fist
    public void CurlRightIndex(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightIndexCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightIndexCurl = rPercent;
    }

    // Clenches the fist
    public void CurlRightMiddle(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightMiddleCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightMiddleCurl = rPercent;
    }

    // Clenches the fist
    public void CurlRightRing(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightRingCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightRingCurl = rPercent;
    }

    // Clenches the fist
    public void CurlRightLittle(float rPercent)
    {
        FingerPoseMotor lHumanMotor = mHuman.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lHumanMotor.RightLittleCurl = rPercent;

        FingerPoseMotor lGoblinMotor = mGoblin.GetComponent<BoneController>().GetMotor<FingerPoseMotor>();
        lGoblinMotor.RightLittleCurl = rPercent;
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("SkeletonRaycast");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("SkeletonRaycast");
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
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human", mIsMotorEnabled);
    }
}
