using UnityEngine;
using System.Collections;


public class BulletScript : MonoBehaviour {

	
	public float destroyAfter;
	
	public bool destroyOnImpact = false;
	
	public float minDestroyTime;
	
	public float maxDestroyTime;

	
	public Transform [] metalImpactPrefabs;

	private void Start () 
	{
		
		StartCoroutine (DestroyAfter ());
	}

	
	private void OnCollisionEnter (Collision collision) 
	{
		
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer ());
		}
		
		else 
		{
			Destroy (gameObject);
		}

		
		if (collision.transform.tag == "Metal") 
		{
			
			Instantiate (metalImpactPrefabs [Random.Range 
				(0, metalImpactPrefabs.Length)], transform.position, 
				Quaternion.LookRotation (collision.contacts [0].normal));
		
			Destroy(gameObject);
		}

	
		if (collision.transform.tag == "Target") 
		{
			
			collision.transform.gameObject.GetComponent
				<TargetScript>().isHit = true;
		
			Destroy(gameObject);
		}
			
	
		if (collision.transform.tag == "ExplosiveBarrel") 
		{
			
			collision.transform.gameObject.GetComponent
				<ExplosiveBarrelScript>().explode = true;
	
			Destroy(gameObject);
		}
	}

	private IEnumerator DestroyTimer () 
	{
		
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
	
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
	
		yield return new WaitForSeconds (destroyAfter);
		
		Destroy (gameObject);
	}
}
