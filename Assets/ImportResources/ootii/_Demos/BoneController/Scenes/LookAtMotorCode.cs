using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;

public class LookAtMotorCode : SceneCode
{
    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("SwingAtMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("SwingAtMotor");
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

    /// <summary>
    /// Sets the blend weight of the motor
    /// </summary>
    /// <param name="rValue"></param>
    public void SetWeight(float rValue)
    {
        SetWeight("Human", rValue);
        SetWeight("Dinosaur", rValue);
        SetWeight("Goblin", rValue);
    }

    /// <summary>
    /// Simplifies setting the weight
    /// </summary>
    /// <param name="rName"></param>
    /// <param name="rWeight"></param>
    private void SetWeight(string rName, float rWeight)
    {
        GameObject lObject = GameObject.Find(rName);
        if (lObject == null) { return; }

        BoneController lSkeleton = lObject.GetComponent<BoneController>();
        if (lSkeleton == null) { return; }

        LookAtMotor lMotor = lSkeleton.GetMotor<LookAtMotor>();
        if (lMotor == null) { return; }

        lMotor.Weight = rWeight;
    }
}
