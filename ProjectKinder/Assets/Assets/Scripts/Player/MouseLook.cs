using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[Serializable]
public class MouseLook
{
    public float xSensitivity = 2f;
    public float ySensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float minimumX = -90F;
    public float maximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;


    private Quaternion characterTargetRotation;
    private Quaternion cameraTargetRotation;
    private bool cursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        characterTargetRotation = character.localRotation;
        cameraTargetRotation = camera.localRotation;
    }


    public void LookRotation(Transform character, Transform camera)
    {
        float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * xSensitivity;
        float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * ySensitivity;

        characterTargetRotation *= Quaternion.Euler (0f, yRot, 0f);
        cameraTargetRotation *= Quaternion.Euler (-xRot, 0f, 0f);

        if(clampVerticalRotation)
            cameraTargetRotation = ClampRotationAroundXAxis (cameraTargetRotation);

        if(smooth)
        {
            character.localRotation = Quaternion.Slerp (character.localRotation, characterTargetRotation,
                smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp (camera.localRotation, cameraTargetRotation,
                smoothTime * Time.deltaTime);
        }
        else
        {
            character.localRotation = characterTargetRotation;
            camera.localRotation = cameraTargetRotation;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if(!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

        angleX = Mathf.Clamp (angleX, minimumX, maximumX);

        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
