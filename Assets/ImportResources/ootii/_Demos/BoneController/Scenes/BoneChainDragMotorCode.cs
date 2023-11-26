using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;
using com.ootii.Utilities.Debug;

public class BoneChainDragMotorCode : SceneCode
{
    public GameObject mDinosaur = null;

    // Use this for initialization
    void Start()
    {
        mDinosaur = GameObject.Find("Dinosaur");
    }

    /// <summary>
    /// Shoot a ray at our target
    /// </summary>
    public void RotateDinosaur(float rPercent)
    {
        mDinosaur.transform.rotation = Quaternion.Euler(0f, 360f * rPercent, 0f);
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("LimbReachMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("LimbReachMotor");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Drag Sphere", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Drag Sphere", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);
    }
}
