using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;

/// <summary>
///  Player controller used during end game scene.
/// </summary>
[RequireComponent(typeof (CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class EndGamePlayerController : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private FOVKick m_FovKick = new FOVKick();
    [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
    [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
    [SerializeField] private float m_StepInterval;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.

    public GameObject mirror;
    private GameObject blackScreen;
    public bool isMirrorSeen;

    private Camera m_Camera;
    private CharacterController m_CharacterController;
    private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private AudioSource m_AudioSource;
    private Vector3 m_MoveDir = Vector3.zero;
    private Vector2 m_Input;
    private CollisionFlags m_CollisionFlags;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_FovKick.Setup(m_Camera);
        m_HeadBob.Setup(m_Camera, m_StepInterval);
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_AudioSource = GetComponent<AudioSource>();
        m_MouseLook.Init(transform, m_Camera.transform);

        isMirrorSeen = false;
        blackScreen = GameObject.Find("BlackScreen").transform.Find("Panel").gameObject;
    }

    void Update()
    {
        if (isMirrorSeen) return;

        RotateView();
        CheckMirrorInView();    // Check if player has seen the mirror
    }

    void FixedUpdate()
    {
        if (isMirrorSeen) return;

        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                           m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;
        m_MoveDir.y = -m_StickToGroundForce;

        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        UpdateCameraPosition(speed);

        m_MouseLook.UpdateCursorLock();
    }

    private void CheckMirrorInView()
    {
        Camera cam = transform.Find("FirstPersonCharacter").GetComponent<Camera>();
        Vector3 viewPos = cam.WorldToViewportPoint(mirror.transform.position);
        if (viewPos.x > 0 && viewPos.x < 1 &&
            viewPos.y > 0 && viewPos.y < 1 &&
            viewPos.z > 0 && viewPos.z < 2)
        {
            // Center of mirror is in view
            isMirrorSeen = true;
            StartCoroutine(FadeToBlack());
        }
    }

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, m_Camera.transform);
    }

    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed)) * Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }

    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition = m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude + speed);
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }

    private void GetInput(out float speed)
    {
        // Read input
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        float vertical = CrossPlatformInputManager.GetAxis("Vertical");

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        //m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
        m_IsWalking = true;     // Set this to always walking to prevent running
#endif
        // set the desired speed to be walking or running
        //speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        speed = 2;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }

    // Fades the screen to black
    private IEnumerator FadeToBlack()
    {
        yield return new WaitForSeconds(1f);

        Color currentColor = blackScreen.GetComponent<Image>().color;
        while (currentColor.a < 1f)
        {
            currentColor = blackScreen.GetComponent<Image>().color;
            blackScreen.GetComponent<Image>().color = new Color(currentColor.r, currentColor.g, currentColor.b, currentColor.a + 0.01f);
            yield return new WaitForSeconds(0.02f);
        }
    }
}
