using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using com.ootii.Actors.BoneControllers;

public class FootMotorInvertedCode : SceneCode
{
    GameObject mCube = null;
    Vector3 mCubePosition = Vector3.zero;

    float mCubeY = 0f;
    float mCubeX = 0f;

    // Use this for initialization
    void Start()
    {
        mCube = GameObject.Find("Cube");
        mCubePosition = mCube.transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        float lDistance = 0.5f;
        Vector3 lOffset = new Vector3(mCubeX * lDistance, mCubeY * lDistance, 0f);

        mCube.transform.position = Vector3.Lerp(mCube.transform.position, mCubePosition + lOffset, 0.5f);

        // Continue with the update
        base.Update();
    }

    /// <summary>
    /// Allows us to raise and lower the platform
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveVertical(float rValue)
    {
        mCubeY = rValue;
    }

    /// <summary>
    /// Allows us to raise and lower the platform
    /// </summary>
    /// <param name="rValue"></param>
    public void MoveHorizontal(float rValue)
    {
        mCubeX = rValue;
    }

    /// <summary>
    /// Loads the next demo scene
    /// </summary>
    public override void NextDemo()
    {
#if UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
        Application.LoadLevel("BoneChainDragMotor");
#else
        UnityEngine.SceneManagement.SceneManager.LoadScene("BoneChainDragMotor");
#endif
    }

    /// <summary>
    /// Enables all the bone motors
    /// </summary>
    public override void EnableMotors()
    {
        base.EnableMotors();

        EnableMotors("Human Down", mIsMotorEnabled);
        EnableMotors("Human Up", mIsMotorEnabled);
        EnableMotors("Human Left", mIsMotorEnabled);
        EnableMotors("Human Right", mIsMotorEnabled);
    }

    /// <summary>
    /// Disables all the bone motors
    /// </summary>
    public override void DisableMotors()
    {
        base.DisableMotors();

        EnableMotors("Human Down", mIsMotorEnabled);
        EnableMotors("Human Up", mIsMotorEnabled);
        EnableMotors("Human Left", mIsMotorEnabled);
        EnableMotors("Human Right", mIsMotorEnabled);
    }
}
