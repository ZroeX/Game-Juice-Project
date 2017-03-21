using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveScript : MonoBehaviour
{

    public float MoveSpeed = 100, JumpVelocity = 7;
    public GameObject CameraObject;
    public ParticleSystem ParticleJumpDown;

    private Vector3 CameraOffset;
    private Rigidbody Rb;
    private Vector3 movement;
    private Animator PlayerAnimator;
    private int JumpHash = Animator.StringToHash("Jump");
    private AudioSource audioFX;

    bool CanParticlePlay = false;
    bool CanAudioPlay = false;
    private bool IsGrounded = true;

    // Use this for initialization
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerAnimator = gameObject.GetComponent<Animator>();
        audioFX = gameObject.GetComponent<AudioSource>();
        CameraOffset = CameraObject.transform.position - transform.position;
        CanParticlePlay = false;
        CanAudioPlay = false;
        FindMedianSortedArrays(new int[2] {1,2 } , new int[2] {3,4});
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHrizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        //SampleMoveControl(moveHrizontal, moveVertical);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            SampleJampControl();
        }
    }

    void LateUpdate()
    {
        CameraObject.transform.position = transform.position + CameraOffset;
    }

    private void SampleMoveControl(float h, float v)
    {
        movement = Rb.velocity;
        movement.x = h * MoveSpeed * Time.deltaTime;
        movement.z = v * MoveSpeed * Time.deltaTime;

        Rb.velocity = movement;
    }

    private void SampleJampControl()
    {
        IsGrounded = false;
        PlayerAnimator.SetTrigger(JumpHash);
        if (CanAudioPlay == true)
        {
            audioFX.Play();
        }
    }

    public void JumpEndEvent()
    {
        IsGrounded = true;
        PlayerAnimator.ResetTrigger(JumpHash);
    }

    public void JumpEndSmoke()
    {
        if (CanParticlePlay == true)
        {
            ParticleJumpDown.Play();
        }
    }

    public void UI_Anim_DropDownListChange(int _value)
    {
        switch (_value)
        {
            case 0:
                JumpHash = Animator.StringToHash("Jump");
                break;
            case 1:
                JumpHash = Animator.StringToHash("Jump2");
                break;
            case 2:
                JumpHash = Animator.StringToHash("Jump3");
                break;
            case 3:
                JumpHash = Animator.StringToHash("Jump4");
                break;
        }
    }

    public void UI_Particle_ToggleChange(bool _value)
    {
        CanParticlePlay = _value;
    }

    public void UI_Audio_ToggleChange(bool _value)
    {
        CanAudioPlay = _value;
    }

    public void GoToFinalScene() {
        SceneManager.LoadScene(1);
    }

    public double FindMedianSortedArrays(int[] A, int[] B)
    {
        int m = A.Length, n = B.Length;
        int l = (m + n + 1) / 2; // 2 + 2 + 1 
        int r = (m + n + 2) / 2; // 2 + 2 + 2
        Debug.Log( "l = " + l + ", r = " + r);
        return (getkth(A, 0, B, 0, l , " first") + getkth(A, 0, B, 0, r , "second")) / 2.0;
    }

    public double getkth(int[] A, int aStart, int[] B, int bStart, int k ,string _checkVal = null)
    {
        if (_checkVal != null) Debug.Log(_checkVal); else Debug.Log("-------------");

        string checkinsetVal = string.Format("A = {0} , aStart = {1} , B = {2} , BStart = {3} , k = {4}", A.Length, aStart, B.Length, bStart, k);
        Debug.Log(checkinsetVal);


        if (aStart > A.Length - 1) return B[bStart + k - 1];
        if (bStart > B.Length - 1) return A[aStart + k - 1];
        if (k == 1) return Mathf.Min(A[aStart], B[bStart]);

        int aMid = int.MaxValue;
        int bMid = int.MaxValue;
        if (aStart + k / 2 - 1 < A.Length) aMid = A[aStart + k / 2 - 1];
        if (bStart + k / 2 - 1 < B.Length) bMid = B[bStart + k / 2 - 1];

        if (aMid < bMid)
            return getkth(A, aStart + k / 2, B, bStart, k - k / 2);// Check: aRight + bLeft 
        else
            return getkth(A, aStart, B, bStart + k / 2, k - k / 2);// Check: bRight + aLeft
    }
}
