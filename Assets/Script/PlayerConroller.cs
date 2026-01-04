using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
	public float moveSpeed = 10f;
[Header("Missile")]
	public GameObject MissiliePrefab;
	public float MuzzleSpawnPosition;
	public float DestroyTime = 5f;
public Transform MissileSpawnPoint;
private void Update()
{
		MovePlayer();
		PlayerShoot();
	}

	void MovePlayer()
	{
		float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
		float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

		transform.Translate(new Vector3(moveX, moveZ, 0));
	}
	 void PlayerShoot()
	{
		if (Input.GetButtonDown("Jump"))
		{
			SpawnMissile();
			SpawnMuzzleFlash();
			
			
			Debug.Log("Pew Pew!");
		}
}
void SpawnMissile()
	{
		GameObject gm = Instantiate(MissiliePrefab, MissileSpawnPoint.position, Quaternion.identity);
		gm.transform.SetParent(null);
			Destroy(gm, DestroyTime);
	}
	void SpawnMuzzleFlash()
	{
		GameObject muzzle = Instantiate(GameManager.instance.MuzzleFlashEffect, MissileSpawnPoint.position, Quaternion.identity);
		muzzle.transform.SetParent(null);
		Destroy(muzzle, DestroyTime);
			
	}
		private void OnCollisionEnter2D(Collision2D collision)
		{
			if (collision.gameObject.CompareTag("Enemy"))
			{
				GameObject gm = Instantiate(GameManager.instance.ParticleEffect, transform.position, Quaternion.identity);
           Destroy(gm, 2f);
		   Destroy(this.gameObject);
				Debug.Log("Player hit by Enemy!");
			}
		}
	
}
