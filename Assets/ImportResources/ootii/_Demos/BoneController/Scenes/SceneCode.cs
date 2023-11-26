using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;
using com.ootii.Utilities;

public class SceneCode : MonoBehaviour
{
    public bool ShowProfiling = false;

    protected bool mIsMotorEnabled = true;

    /// <summary>
    /// Basic updating
    /// </summary>
    public virtual void Update()
    {
        if (ShowProfiling)
        {
            com.ootii.Utilities.Profiler.ScreenWrite("", 5);
        }
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public virtual void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("BindPoseMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("BindPoseMotor");
#endif
    }

    /// <summary>
    /// Toggles the motors on and off
    /// </summary>
    public void ToggleMotors()
    {
        mIsMotorEnabled = !mIsMotorEnabled;

        if (mIsMotorEnabled)
        {
            EnableMotors();
        }
        else
        {
            DisableMotors();
        }
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public virtual void EnableMotors()
    {
        mIsMotorEnabled = true;
        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);

        SetMotorState("(motors are on)");
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public virtual void DisableMotors()
    {
        mIsMotorEnabled = false;
        EnableMotors("Goblin", mIsMotorEnabled);
        EnableMotors("Human", mIsMotorEnabled);
        EnableMotors("Dinosaur", mIsMotorEnabled);

        SetMotorState("(motors are off)");
    }

    /// <summary>
    /// Enables all the motors for the object
    /// </summary>
    /// <param name="rObjectName"></param>
    /// <param name="rEnable"></param>
    protected virtual void EnableMotors(string rName, bool rEnable)
    {
        GameObject lObject = GameObject.Find(rName);
        if (lObject != null)
        {
            BoneController lSkeleton = lObject.GetComponent<BoneController>();
            if (lSkeleton != null)
            {
                for (int i = 0; i < lSkeleton.Motors.Count; i++)
                {
                    lSkeleton.Motors[i].IsEnabled = rEnable;
                }
            }
        }
    }

    /// <summary>
    /// Set the motor state text
    /// </summary>
    /// <param name="rText"></param>
    protected virtual void SetMotorState(string rText)
    {
        GameObject lObject = GameObject.Find("Motor State");
        if (lObject != null)
        {
            Text lText = lObject.GetComponent<Text>();
            if (lText != null)
            {
                lText.text = rText;
            }
        }
    }
}
