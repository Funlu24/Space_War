using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    public float moveSpeed = 10f;

    [Header("Missile")]
    public GameObject MissiliePrefab;
    public Transform MuzzleSpawnPosition;
    public float DestroyTime = 5f;
    public Transform MissileSpawnPoint;

    [Header("Components")]
    public Animator shipAnimator;
    
    // YENİ EKLENEN KISIM: Motor Efekti İçin Değişken
    [Header("Engine Effects")]
    public GameObject EngineThrustEffect; 

    private void Update()
    {
        MovePlayer();
        PlayerShoot();
        CheckEnemyAhead();
    }

    private void LateUpdate()
    {
        ClampPosition();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); 
        float moveZ = Input.GetAxis("Vertical"); // W tuşu veya Yukarı ok tuşu pozitif değer verir

        // YENİ EKLENEN KISIM: Motor Efektini Kontrol Etme
        if (EngineThrustEffect != null)
        {
            // Eğer W'ye basıyorsak (moveZ > 0) efekti aç, yoksa kapat
            if (moveZ > 0)
            {
                EngineThrustEffect.SetActive(true);
            }
            else
            {
                EngineThrustEffect.SetActive(false);
            }
        }

        // Animator Kodun (Vazgeçmediysen kalsın)
        if(shipAnimator != null)
        {
            shipAnimator.SetBool("IsMoving", moveX != 0 || moveZ != 0);
        }

        Vector3 movement = new Vector3(moveX, moveZ, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    // ... Diğer fonksiyonların (ClampPosition, CheckEnemyAhead, vs.) aynı kalacak ...
    
    void ClampPosition()
    {
        Vector3 viewPos = transform.position;
        // Düzelttiğimiz geniş ekran sınırları
        viewPos.x = Mathf.Clamp(viewPos.x, -9.0f, 9.0f); 
        viewPos.y = Mathf.Clamp(viewPos.y, -4.5f, 4.5f);
        transform.position = viewPos;
    }

    // ... Geri kalan kodlar aynı ...
    
    void CheckEnemyAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(MissileSpawnPoint.position, Vector2.up, 10f);
        Debug.DrawRay(MissileSpawnPoint.position, Vector3.up * 10f, Color.red);
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            Debug.Log("Düşman Kilitlendi!");
        }
    }

    void PlayerShoot()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SpawnMissile();
            SpawnMuzzleFlash();
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
        if (GameManager.instance.MuzzleFlashEffect != null)
        {
            GameObject muzzle = Instantiate(GameManager.instance.MuzzleFlashEffect, MissileSpawnPoint.position, Quaternion.identity);
            muzzle.transform.SetParent(null);
            Destroy(muzzle, DestroyTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameObject gm = Instantiate(GameManager.instance.ParticleEffect, transform.position, Quaternion.identity);
            Destroy(gm, 2f);
            Destroy(this.gameObject);
        }
    }
}