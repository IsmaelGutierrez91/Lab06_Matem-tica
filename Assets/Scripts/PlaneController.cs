using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mathematics.Week6
{
    public class PlaneController : MonoBehaviour
    {
        [Header("Player Preferences")]
        [SerializeField] float playerXSpeed;
        [SerializeField] float playerYSpeed;
        [SerializeField] private float xDirection;
        [SerializeField] private MinMax xTreshHold;
        [SerializeField] private float yDirection;
        [SerializeField] private MinMax yTreshHold;
        private Rigidbody rb;
        float xRB;
        float yRB;
        int playerLives = 3;

        [Header("Controls Properties")]
        [SerializeField] private float pitchPlane;
        [SerializeField] private float pitchGain = 1f;
        [SerializeField] private MinMax pitchTreshHold;
        [SerializeField] private float rollPlane;
        [SerializeField] private float rollhGain = 1f;
        [SerializeField] private MinMax rollTreshHold;

        [Header("Rotation Data")]
        [SerializeField] private Quaternion qx = Quaternion.identity; //<0,,0,0,1>
        [SerializeField] private Quaternion qy = Quaternion.identity; //<0,,0,0,1>
        [SerializeField] private Quaternion qz = Quaternion.identity; //<0,,0,0,1>

        [SerializeField] private Quaternion r = Quaternion.identity; //<0,,0,0,1>
        private float anguloSen;
        private float anguloCos;

        protected float _pitchDirection = 0f;
        protected float _rollDirection = 0f;

        public static event Action OnLoseAllLives;
        public static event Action<int> OnCollisionWhitObstacle;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            rb.position = new Vector3(xRB + xDirection * playerXSpeed, yRB + yDirection * playerYSpeed, rb.position.z);
            if (playerLives <= 0)
            {
                OnLoseAllLives?.Invoke();
            }
        }
        private void FixedUpdate()
        {
            pitchPlane += _pitchDirection * pitchGain;

            pitchPlane = Mathf.Clamp(pitchPlane, pitchTreshHold.MinValue, pitchTreshHold.MaxValue);

            rollPlane += _rollDirection * rollhGain;

            rollPlane = Mathf.Clamp(rollPlane, rollTreshHold.MinValue, rollTreshHold.MaxValue);

            xRB = Mathf.Clamp(rb.position.x, xTreshHold.MinValue, xTreshHold.MaxValue);
            yRB = Mathf.Clamp(rb.position.y, yTreshHold.MinValue, yTreshHold.MaxValue);

            //rotacion z -> x -> y
            anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
            qz.Set(0, 0, anguloSen, anguloCos);

            anguloSen = Mathf.Sin(Mathf.Deg2Rad * pitchPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * pitchPlane * 0.5f);
            qx.Set(anguloSen, 0, 0, anguloCos);

            /*anguloSen = Mathf.Sin(Mathf.Deg2Rad * rollPlane * 0.5f);
            anguloCos = Mathf.Cos(Mathf.Deg2Rad * rollPlane * 0.5f);
            qy.Set(0, anguloSen, 0, anguloCos);*/

            //multiplicación y -> x -> z
            r = qy * qx * qz;

            transform.rotation = r;
        }

        //Pitch -> X Axis
        public void RotatePitch(InputAction.CallbackContext context)
        {
            _pitchDirection = context.ReadValue<float>() * -1;
        }
        public void YMovement(InputAction.CallbackContext context)
        {
            yDirection = context.ReadValue<float>();
        }
        //Roll -> Z Axis
        public void RotateRoll(InputAction.CallbackContext context)
        {
            _rollDirection = context.ReadValue<float>();
        }
        public void XMovement(InputAction.CallbackContext context)
        {
            xDirection = context.ReadValue<float>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Obstacle")
            {
                playerLives = playerLives - 1;
                OnCollisionWhitObstacle?.Invoke(playerLives);
            }
        }
    }
    [System.Serializable]
    public struct MinMax
    {
        public float MinValue;
        public float MaxValue;
    }
    
}
