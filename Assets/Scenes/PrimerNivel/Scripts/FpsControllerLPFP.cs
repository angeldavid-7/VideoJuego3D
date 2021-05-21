using System;
using System.Linq;
using UnityEngine;

namespace FPSControllerLPFP
{
    
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(AudioSource))]
    public class FpsControllerLPFP : MonoBehaviour
    {
#pragma warning disable 649
		[Header("Brazos")]
        [Tooltip("Obtiene los componentes de los brazos ."), SerializeField]
        private Transform arms;

        [Tooltip("Configura la posicion de los brazos."), SerializeField]
        private Vector3 armPosition;

		[Header("Audio Clips")]
        [Tooltip("Clip de audio al caminar."), SerializeField]
        private AudioClip walkingSound;

        [Tooltip("Clip de audio al correr."), SerializeField]
        private AudioClip runningSound;

		[Header("Configuracion de movimientos")]
        [Tooltip("Velocidad del personaje al caminar o hacer strafing."), SerializeField]
        private float walkingSpeed = 5f;

        [Tooltip("Velocidad del personaje al correr."), SerializeField]
        private float runningSpeed = 9f;

        [Tooltip("Tiempo que se tarda en llegar a la maxima velocidad."), SerializeField]
        private float movementSmoothness = 0.125f;

        [Tooltip("Fuerza de salto."), SerializeField]
        private float jumpForce = 35f;

		[Header("Ajustes adicionales")]
        [Tooltip("Velocidad de rotacion de la camara."), SerializeField]
        private float mouseSensitivity = 7f;

        [Tooltip("Tiempo en el que se tardad en dar una rotacion."), SerializeField]
        private float rotationSmoothness = 0.05f;

        [Tooltip("Limite de angulo de la camara para el eje -x."),
         SerializeField]
        private float minVerticalAngle = -90f;

        [Tooltip("Limite de angulo de la camara para el eje x."),
         SerializeField]
        private float maxVerticalAngle = 90f;

        [Tooltip("Configuracion de teclas relacionadas al movimiento."), SerializeField]
        private FpsInput input;
#pragma warning restore 649

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;
        private AudioSource _audioSource;
        private SmoothRotation _rotationX;
        private SmoothRotation _rotationY;
        private SmoothVelocity _velocityX;
        private SmoothVelocity _velocityZ;
        private bool _isGrounded;

        private readonly RaycastHit[] _groundCastResults = new RaycastHit[8];
        private readonly RaycastHit[] _wallCastResults = new RaycastHit[8];

        
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _collider = GetComponent<CapsuleCollider>();
            _audioSource = GetComponent<AudioSource>();
			arms = AssignCharactersCamera();
            _audioSource.clip = walkingSound;
            _audioSource.loop = true;
            _rotationX = new SmoothRotation(RotationXRaw);
            _rotationY = new SmoothRotation(RotationYRaw);
            _velocityX = new SmoothVelocity();
            _velocityZ = new SmoothVelocity();
            Cursor.lockState = CursorLockMode.Locked;
            ValidateRotationRestriction();
        }
		
        //asignaciones de camara
        private Transform AssignCharactersCamera()
        {
            var t = transform;
			arms.SetPositionAndRotation(t.position, t.rotation);
			return arms;
        }
        
        
        private void ValidateRotationRestriction()
        {
            minVerticalAngle = ClampRotationRestriction(minVerticalAngle, -90, 90);
            maxVerticalAngle = ClampRotationRestriction(maxVerticalAngle, -90, 90);
            if (maxVerticalAngle >= minVerticalAngle) return;
            Debug.LogWarning("maxVerticalAngle should be greater than minVerticalAngle.");
            var min = minVerticalAngle;
            minVerticalAngle = maxVerticalAngle;
            maxVerticalAngle = min;
        }

        private static float ClampRotationRestriction(float rotationRestriction, float min, float max)
        {
            if (rotationRestriction >= min && rotationRestriction <= max) return rotationRestriction;
            var message = string.Format("Rotation restrictions should be between {0} and {1} degrees.", min, max);
            Debug.LogWarning(message);
            return Mathf.Clamp(rotationRestriction, min, max);
        }
			
        // Validar si el personaje esta en el piso
        private void OnCollisionStay()
        {
            var bounds = _collider.bounds;
            var extents = bounds.extents;
            var radius = extents.x - 0.01f;
            Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
                _groundCastResults, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);
            if (!_groundCastResults.Any(hit => hit.collider != null && hit.collider != _collider)) return;
            for (var i = 0; i < _groundCastResults.Length; i++)
            {
                _groundCastResults[i] = new RaycastHit();
            }

            _isGrounded = true;
        }
			
        
        // Arreglar el movimiento del personaje y la rotacion de la camara cada framerate
        private void FixedUpdate()
        {
            // Se usa FixedUpdate en lugar de Update porque este código se ocupa de la física y el suavizado.
            RotateCameraAndCharacter();
            MoveCharacter();
            _isGrounded = false;
        }

        // Mueve la cámara hacia el personaje, procesa los saltos y reproduce sonidos en cada frame.
        private void Update()
        {
			arms.position = transform.position + transform.TransformVector(armPosition);
            Jump();
            PlayFootstepSounds();
        }

        private void RotateCameraAndCharacter()
        {
            var rotationX = _rotationX.Update(RotationXRaw, rotationSmoothness);
            var rotationY = _rotationY.Update(RotationYRaw, rotationSmoothness);
            var clampedY = RestrictVerticalRotation(rotationY);
            _rotationY.Current = clampedY;
			var worldUp = arms.InverseTransformDirection(Vector3.up);
			var rotation = arms.rotation *
                           Quaternion.AngleAxis(rotationX, worldUp) *
                           Quaternion.AngleAxis(clampedY, Vector3.left);
            transform.eulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
			arms.rotation = rotation;
        }

        //Devuelve la rotación de destino de la cámara alrededor del eje y sin suavizado.
        private float RotationXRaw
        {
            get { return input.RotateX * mouseSensitivity; }
        }

        // Devuelve la rotación de destino de la cámara alrededor del eje x sin suavizado.
        private float RotationYRaw
        {
            get { return input.RotateY * mouseSensitivity; }
        }

        // Sujeta la rotación de la cámara alrededor del eje x
        // entre "minVerticalAngle" y "maxVerticalAngle" 
        private float RestrictVerticalRotation(float mouseY)
        {
			var currentAngle = NormalizeAngle(arms.eulerAngles.x);
            var minY = minVerticalAngle + currentAngle;
            var maxY = maxVerticalAngle + currentAngle;
            return Mathf.Clamp(mouseY, minY + 0.01f, maxY - 0.01f);
        }
			
        // Normalizar un angulo entre -180 y 180 grados.
        
        private static float NormalizeAngle(float angleDegrees)
        {
            while (angleDegrees > 180f)
            {
                angleDegrees -= 360f;
            }

            while (angleDegrees <= -180f)
            {
                angleDegrees += 360f;
            }

            return angleDegrees;
        }

        private void MoveCharacter()
        {
            var direction = new Vector3(input.Move, 0f, input.Strafe).normalized;
            var worldDirection = transform.TransformDirection(direction);
            var velocity = worldDirection * (input.Run ? runningSpeed : walkingSpeed);
            //Comprueba si hay colisiones para que el personaje no se atasque al saltar contra las paredes.
            var intersectsWall = CheckCollisionsWithWalls(velocity);
            if (intersectsWall)
            {
                _velocityX.Current = _velocityZ.Current = 0f;
                return;
            }

            var smoothX = _velocityX.Update(velocity.x, movementSmoothness);
            var smoothZ = _velocityZ.Update(velocity.z, movementSmoothness);
            var rigidbodyVelocity = _rigidbody.velocity;
            var force = new Vector3(smoothX - rigidbodyVelocity.x, 0f, smoothZ - rigidbodyVelocity.z);
            _rigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        private bool CheckCollisionsWithWalls(Vector3 velocity)
        {
            if (_isGrounded) return false;
            var bounds = _collider.bounds;
            var radius = _collider.radius;
            var halfHeight = _collider.height * 0.5f - radius * 1.0f;
            var point1 = bounds.center;
            point1.y += halfHeight;
            var point2 = bounds.center;
            point2.y -= halfHeight;
            Physics.CapsuleCastNonAlloc(point1, point2, radius, velocity.normalized, _wallCastResults,
                radius * 0.04f, ~0, QueryTriggerInteraction.Ignore);
            var collides = _wallCastResults.Any(hit => hit.collider != null && hit.collider != _collider);
            if (!collides) return false;
            for (var i = 0; i < _wallCastResults.Length; i++)
            {
                _wallCastResults[i] = new RaycastHit();
            }

            return true;
        }

        private void Jump()
        {
            if (!_isGrounded || !input.Jump) return;
            _isGrounded = false;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void PlayFootstepSounds()
        {
            if (_isGrounded && _rigidbody.velocity.sqrMagnitude > 0.1f)
            {
                _audioSource.clip = input.Run ? runningSound : walkingSound;
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
            }
        }
			
       
        private class SmoothRotation
        {
            private float _current;
            private float _currentVelocity;

            public SmoothRotation(float startAngle)
            {
                _current = startAngle;
            }
				
            
            public float Update(float target, float smoothTime)
            {
                return _current = Mathf.SmoothDampAngle(_current, target, ref _currentVelocity, smoothTime);
            }

            public float Current
            {
                set { _current = value; }
            }
        }
			
        
        private class SmoothVelocity
        {
            private float _current;
            private float _currentVelocity;

            
            public float Update(float target, float smoothTime)
            {
                return _current = Mathf.SmoothDamp(_current, target, ref _currentVelocity, smoothTime);
            }

            public float Current
            {
                set { _current = value; }
            }
        }
			
        // Mapeado de botones
        [Serializable]
        private class FpsInput
        {
            [Tooltip("The name of the virtual axis mapped to rotate the camera around the y axis."),
             SerializeField]
            private string rotateX = "Mouse X";

            [Tooltip("The name of the virtual axis mapped to rotate the camera around the x axis."),
             SerializeField]
            private string rotateY = "Mouse Y";

            [Tooltip("The name of the virtual axis mapped to move the character back and forth."),
             SerializeField]
            private string move = "Horizontal";

            [Tooltip("The name of the virtual axis mapped to move the character left and right."),
             SerializeField]
            private string strafe = "Vertical";

            [Tooltip("The name of the virtual button mapped to run."),
             SerializeField]
            private string run = "Fire3";

            [Tooltip("The name of the virtual button mapped to jump."),
             SerializeField]
            private string jump = "Jump";

            
            public float RotateX
            {
                get { return Input.GetAxisRaw(rotateX); }
            }
				         
                    
            public float RotateY
            {
                get { return Input.GetAxisRaw(rotateY); }
            }
				        
                  
            public float Move
            {
                get { return Input.GetAxisRaw(move); }
            }
				       
          
            public float Strafe
            {
                get { return Input.GetAxisRaw(strafe); }
            }
				    
            
            public bool Run
            {
                get { return Input.GetButton(run); }
            }
				     
                 
            public bool Jump
            {
                get { return Input.GetButtonDown(jump); }
            }
        }
    }
}