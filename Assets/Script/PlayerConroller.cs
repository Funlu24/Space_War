using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
	public float moveSpeed = 10f;
[Header("Missile")]
	public GameObject MissiliePrefab;
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
			GameObject gm = Instantiate(MissiliePrefab, MissileSpawnPoint.position, Quaternion.identity);
			gm.transform.SetParent(null);
			Destroy(gm, DestroyTime);
			Debug.Log("Pew Pew!");
		}
	}
}
