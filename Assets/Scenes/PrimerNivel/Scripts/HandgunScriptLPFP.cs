using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class HandgunScriptLPFP : MonoBehaviour {

	
	Animator anim;


	
	public Camera gunCamera;


	public float fovSpeed = 15.0f;
	

	public float defaultFov = 40.0f;

	public float aimFov = 15.0f;


	public string weaponName;
	private string storedWeaponName;



	public bool weaponSway;

	public float swayAmount = 0.02f;
	public float maxSwayAmount = 0.06f;
	public float swaySmoothValue = 4.0f;

	private Vector3 initialSwayPosition;



	public float sliderBackTimer = 1.58f;
	private bool hasStartedSliderBack;


	public bool autoReload;

	public float autoReloadDelay;

	private bool isReloading;


	private bool hasBeenHolstered = false;

	private bool holstered;

	private bool isRunning;

	private bool isAiming;

	private bool isWalking;

	private bool isInspecting;

	
	private int currentAmmo;

	[Tooltip("How much ammo the weapon should have.")]
	public int ammo;

	private bool outOfAmmo;

	
	public float bulletForce = 400;
	
	public float showBulletInMagDelay = 0.6f;
	
	public SkinnedMeshRenderer bulletInMagRenderer;
    
	public float grenadeSpawnDelay = 0.35f;


	public bool randomMuzzleflash = false;

	private int minRandomValue = 1;

	[Range(2, 25)]
	public int maxRandomValue = 5;

	private int randomMuzzleflashValue;

	public bool enableMuzzleflash = true;
	public ParticleSystem muzzleParticles;
	public bool enableSparks = true;
	public ParticleSystem sparkParticles;
	public int minSparkEmission = 1;
	public int maxSparkEmission = 7;


	public Light muzzleflashLight;
	public float lightDuration = 0.02f;


	public AudioSource mainAudioSource;

	public AudioSource shootAudioSource;


	public Text timescaleText;
	public Text currentWeaponText;
	public Text currentAmmoText;
	public Text totalAmmoText;

	[System.Serializable]
	public class prefabs
	{  

		public Transform bulletPrefab;
		public Transform casingPrefab;
		public Transform grenadePrefab;
	}
	public prefabs Prefabs;
	
	[System.Serializable]
	public class spawnpoints
	{  


		public Transform casingSpawnPoint;

		public Transform bulletSpawnPoint;

		public Transform grenadeSpawnPoint;
	}
	public spawnpoints Spawnpoints;

	[System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
		public AudioClip takeOutSound;
		public AudioClip holsterSound;
		public AudioClip reloadSoundOutOfAmmo;
		public AudioClip reloadSoundAmmoLeft;
		public AudioClip aimSound;
	}
	public soundClips SoundClips;

	private bool soundHasPlayed = false;

	private void Awake () 
	{

		anim = GetComponent<Animator>();

		currentAmmo = ammo;

		muzzleflashLight.enabled = false;
	}

	private void Start () {

		storedWeaponName = weaponName;

		currentWeaponText.text = weaponName;

		totalAmmoText.text = ammo.ToString();


		initialSwayPosition = transform.localPosition;


		shootAudioSource.clip = SoundClips.shootSound;
	}

	private void LateUpdate () {

		if (weaponSway == true) {
			float movementX = -Input.GetAxis ("Mouse X") * swayAmount;
			float movementY = -Input.GetAxis ("Mouse Y") * swayAmount;

			movementX = Mathf.Clamp 
				(movementX, -maxSwayAmount, maxSwayAmount);
			movementY = Mathf.Clamp 
				(movementY, -maxSwayAmount, maxSwayAmount);

			Vector3 finalSwayPosition = new Vector3 
				(movementX, movementY, 0);
			transform.localPosition = Vector3.Lerp 
				(transform.localPosition, finalSwayPosition + 
				initialSwayPosition, Time.deltaTime * swaySmoothValue);
		}
	}
	
	private void Update () {


		if(Input.GetButton("Fire2") && !isReloading && !isRunning && !isInspecting) 
		{
			
			gunCamera.fieldOfView = Mathf.Lerp (gunCamera.fieldOfView,
				aimFov, fovSpeed * Time.deltaTime);
			
			isAiming = true;

			anim.SetBool ("Aim", true);

			if (!soundHasPlayed) 
			{
				mainAudioSource.clip = SoundClips.aimSound;
				mainAudioSource.Play ();
	
				soundHasPlayed = true;
			}
		} 
		else 
		{

			gunCamera.fieldOfView = Mathf.Lerp(gunCamera.fieldOfView,
				defaultFov,fovSpeed * Time.deltaTime);

			isAiming = false;
	
			anim.SetBool ("Aim", false);
		}
		
		if (randomMuzzleflash == true) {
			randomMuzzleflashValue = Random.Range (minRandomValue, maxRandomValue);
		}


		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			Time.timeScale = 1.0f;
			timescaleText.text = "1.0";
		}

		if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			Time.timeScale = 0.5f;
			timescaleText.text = "0.5";
		}

		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			Time.timeScale = 0.25f;
			timescaleText.text = "0.25";
		}
	
		if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			Time.timeScale = 0.1f;
			timescaleText.text = "0.1";
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) 
		{
			Time.timeScale = 0.0f;
			timescaleText.text = "0.0";
		}


		currentAmmoText.text = currentAmmo.ToString ();


		AnimationCheck ();


		if (Input.GetKeyDown (KeyCode.Q) && !isInspecting) 
		{
			anim.Play ("Knife Attack 1", 0, 0f);
		}
		if (Input.GetKeyDown (KeyCode.F) && !isInspecting) 
		{
			anim.Play ("Knife Attack 2", 0, 0f);
		}
			

		if (Input.GetKeyDown (KeyCode.G) && !isInspecting) 
		{
			StartCoroutine (GrenadeSpawnDelay ());

			anim.Play("GrenadeThrow", 0, 0.0f);
		}

	
		if (currentAmmo == 0) 
		{
	
			currentWeaponText.text = "OUT OF AMMO";

			outOfAmmo = true;

			if (autoReload == true && !isReloading) 
			{
				StartCoroutine (AutoReload ());
			}
				
	
			anim.SetBool ("Out Of Ammo Slider", true);
		
			anim.SetLayerWeight (1, 1.0f);
		} 
		else 
		{
	
			currentWeaponText.text = storedWeaponName.ToString ();
		
			outOfAmmo = false;
		
			anim.SetLayerWeight (1, 0.0f);
		}

	
		if (Input.GetMouseButtonDown (0) && !outOfAmmo && !isReloading && !isInspecting && !isRunning) 
		{
			anim.Play ("Fire", 0, 0f);
	
			muzzleParticles.Emit (1);
				
	
			currentAmmo -= 1;

			shootAudioSource.clip = SoundClips.shootSound;
			shootAudioSource.Play ();

	
			StartCoroutine(MuzzleFlashLight());

			if (!isAiming) 
			{
				anim.Play ("Fire", 0, 0f);
		
				muzzleParticles.Emit (1);

				if (enableSparks == true) 
				{
			
					sparkParticles.Emit (Random.Range (1, 6));
				}
			} 
			else 
			{
				anim.Play ("Aim Fire", 0, 0f);
					
			
				if (!randomMuzzleflash) {
					muzzleParticles.Emit (1);
				
				} 
				else if (randomMuzzleflash == true) 
				{
				
					if (randomMuzzleflashValue == 1) 
					{
						if (enableSparks == true) 
						{
				
							sparkParticles.Emit (Random.Range (1, 6));
						}
						if (enableMuzzleflash == true) 
						{
							muzzleParticles.Emit (1);
				
							StartCoroutine (MuzzleFlashLight ());
						}
					}
				}
			}
				
	
			var bullet = (Transform)Instantiate (
				Prefabs.bulletPrefab,
				Spawnpoints.bulletSpawnPoint.transform.position,
				Spawnpoints.bulletSpawnPoint.transform.rotation);

		
			bullet.GetComponent<Rigidbody>().velocity = 
			bullet.transform.forward * bulletForce;

		
			Instantiate (Prefabs.casingPrefab, 
				Spawnpoints.casingSpawnPoint.transform.position, 
				Spawnpoints.casingSpawnPoint.transform.rotation);
		}

	
		if (Input.GetKeyDown (KeyCode.T)) 
		{
			anim.SetTrigger ("Inspect");
		}

	
		if (Input.GetKeyDown (KeyCode.E) && !hasBeenHolstered) 
		{
			holstered = true;

			mainAudioSource.clip = SoundClips.holsterSound;
			mainAudioSource.Play();

			hasBeenHolstered = true;
		} 
		else if (Input.GetKeyDown (KeyCode.E) && hasBeenHolstered) 
		{
			holstered = false;

			mainAudioSource.clip = SoundClips.takeOutSound;
			mainAudioSource.Play ();

			hasBeenHolstered = false;
		}

	
		if (holstered == true) 
		{
			anim.SetBool ("Holster", true);
		} 
		else 
		{
			anim.SetBool ("Holster", false);
		}

	
		if (Input.GetKeyDown (KeyCode.R) && !isReloading && !isInspecting) 
		{
		
			Reload ();

			if (!hasStartedSliderBack) 
			{
				hasStartedSliderBack = true;
				StartCoroutine (HandgunSliderBackDelay());
			}
		}

	
		if (Input.GetKey (KeyCode.W) && !isRunning || 
			Input.GetKey (KeyCode.A) && !isRunning || 
			Input.GetKey (KeyCode.S) && !isRunning || 
			Input.GetKey (KeyCode.D) && !isRunning) 
		{
			anim.SetBool ("Walk", true);
		} else {
			anim.SetBool ("Walk", false);
		}

		if ((Input.GetKey (KeyCode.W) && Input.GetKey (KeyCode.LeftShift))) 
		{
			isRunning = true;
		} else {
			isRunning = false;
		}
		
		
		if (isRunning == true) {
			anim.SetBool ("Run", true);
		} else {
			anim.SetBool ("Run", false);
		}
	}

	private IEnumerator HandgunSliderBackDelay () {
	
		yield return new WaitForSeconds (sliderBackTimer);
		
		anim.SetBool ("Out Of Ammo Slider", false);
		
		anim.SetLayerWeight (1, 0.0f);

		hasStartedSliderBack = false;
	}

	private IEnumerator GrenadeSpawnDelay () {
		
		yield return new WaitForSeconds (grenadeSpawnDelay);
		
		Instantiate(Prefabs.grenadePrefab, 
			Spawnpoints.grenadeSpawnPoint.transform.position, 
			Spawnpoints.grenadeSpawnPoint.transform.rotation);
	}

	private IEnumerator AutoReload () {

		if (!hasStartedSliderBack) 
		{
			hasStartedSliderBack = true;

			StartCoroutine (HandgunSliderBackDelay());
		}
	
		yield return new WaitForSeconds (autoReloadDelay);

		if (outOfAmmo == true) {
		
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

	
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
			
				StartCoroutine (ShowBulletInMag ());
			}
		} 
	
		currentAmmo = ammo;
		outOfAmmo = false;
	}

	
	private void Reload () {
		
		if (outOfAmmo == true) 
		{
		
			anim.Play ("Reload Out Of Ammo", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundOutOfAmmo;
			mainAudioSource.Play ();

			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = false;
			
				StartCoroutine (ShowBulletInMag ());
			}
		} 
		else 
		{
		
			anim.Play ("Reload Ammo Left", 0, 0f);

			mainAudioSource.clip = SoundClips.reloadSoundAmmoLeft;
			mainAudioSource.Play ();

		
			if (bulletInMagRenderer != null) 
			{
				bulletInMagRenderer.GetComponent
				<SkinnedMeshRenderer> ().enabled = true;
			}
		}

		currentAmmo = ammo;
		outOfAmmo = false;
	}


	private IEnumerator ShowBulletInMag () {
	
		yield return new WaitForSeconds (showBulletInMagDelay);
		bulletInMagRenderer.GetComponent<SkinnedMeshRenderer> ().enabled = true;
	}

	
	private IEnumerator MuzzleFlashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (lightDuration);
		muzzleflashLight.enabled = false;
	}

	
	private void AnimationCheck () 
	{
	
		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Out Of Ammo") || 
			anim.GetCurrentAnimatorStateInfo (0).IsName ("Reload Ammo Left")) 
		{
			isReloading = true;
		} 
		else 
		{
			isReloading = false;
		}

		if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Inspect")) 
		{
			isInspecting = true;
		} 
		else 
		{
			isInspecting = false;
		}
	}
}
