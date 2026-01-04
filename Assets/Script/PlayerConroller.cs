using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    [Header("Movement & Health")]
    public float moveSpeed = 10f;
    public int health = 3; // 3 Canımız var
    private bool isInvulnerable = false; // Şu an hasar almaz modunda mı?

    // --- YENİ EKLENEN KISIM: KALP SİSTEMİ ---
    [Header("UI Elements")]
    public GameObject[] heartIcons; // Editörden 3 kalp görselini buraya sürükleyeceğiz

    [Header("Missile")]
    public GameObject MissiliePrefab;
    public Transform MuzzleSpawnPosition;
    public float DestroyTime = 5f;
    public Transform MissileSpawnPoint;

    [Header("Components & Effects")]
    public Animator shipAnimator;          // Animasyon
    public GameObject EngineThrustEffect;  // Motor Efekti
    private SpriteRenderer spriteRenderer; // Yanıp sönme efekti için

    private void Start()
    {
        // Geminin üzerindeki görsel bileşeni alıyoruz
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        float moveZ = Input.GetAxis("Vertical");

        // Motor Efekt Kontrolü (W'ye basınca çalışsın)
        if (EngineThrustEffect != null)
        {
            if (moveZ > 0) EngineThrustEffect.SetActive(true);
            else EngineThrustEffect.SetActive(false);
        }

        // Animasyon Kontrolü
        if (shipAnimator != null)
        {
            shipAnimator.SetBool("IsMoving", moveX != 0 || moveZ != 0);
        }

        Vector3 movement = new Vector3(moveX, moveZ, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void ClampPosition()
    {
        Vector3 viewPos = transform.position;
        // Ekran sınırları (-9 ile 9 arası genişletildi)
        viewPos.x = Mathf.Clamp(viewPos.x, -9.0f, 9.0f);
        viewPos.y = Mathf.Clamp(viewPos.y, -4.5f, 4.5f);
        transform.position = viewPos;
    }

    void CheckEnemyAhead()
    {
        RaycastHit2D hit = Physics2D.Raycast(MissileSpawnPoint.position, Vector2.up, 10f);
        // Debug çizgisi
        Debug.DrawRay(MissileSpawnPoint.position, Vector3.up * 10f, Color.red);
        
        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            // Düşman algılandı
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

    // ÇARPIŞMA MANTIĞI
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Eğer ölümsüzlük modundaysak hasar alma
            if (isInvulnerable) return;

            // Bize çarpan düşmanı yok et ve efekt çıkar
            GameObject impactEffect = Instantiate(GameManager.instance.ParticleEffect, transform.position, Quaternion.identity);
            Destroy(impactEffect, 2f);
            Destroy(collision.gameObject);

            // Hasar al
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        health--; // Canı 1 azalt

        // --- EKRANDAKİ KALPLERİ GÜNCELLE ---
        // Eğer health 2'ye düştüyse heartIcons[2] (yani 3. kalp) kapanır.
        if (health >= 0 && health < heartIcons.Length)
        {
            heartIcons[health].SetActive(false);
        }

        Debug.Log("Hasar Alındı! Kalan Can: " + health);

        if (health <= 0)
        {
            // CAN BİTTİ, ÖLÜM
            GameManager.instance.GameOver(); // Menüyü çağır
            Destroy(gameObject); // Oyuncuyu yok et
        }
        else
        {
            // CAN VAR, YANIP SÖNME EFEKTİ
            StartCoroutine(BlinkRoutine());
        }
    }

    // YANIP SÖNME EFEKTİ (COROUTINE)
    IEnumerator BlinkRoutine()
    {
        isInvulnerable = true; // Tekrar hasar almayı engelle

        // 5 kere yanıp sön
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false; // Görünmez ol
            yield return new WaitForSeconds(0.1f); 
            spriteRenderer.enabled = true; // Görünür ol
            yield return new WaitForSeconds(0.1f); 
        }

        isInvulnerable = false; // Artık tekrar hasar alabiliriz
    }
}