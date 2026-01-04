using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    public float moveSpeed = 10f;

    [Header("Missile")]
    public GameObject MissiliePrefab;
    public Transform MuzzleSpawnPosition; // Mermi çıkış noktası ile efekt noktası aynı olabilir
    public float DestroyTime = 5f;
    public Transform MissileSpawnPoint;

    [Header("Components")]
    // [GEREKSİNİM 10] Animator referansı
    public Animator shipAnimator; 

    private void Update()
    {
        MovePlayer();
        PlayerShoot();
        CheckEnemyAhead(); // Raycast fonksiyonunu çağırıyoruz
    }

    // [GEREKSİNİM 6] LateUpdate: Hareket bittikten sonra sınırları kontrol etmek için idealdir.
    private void LateUpdate()
    {
        ClampPosition();
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal"); // GetAxis raw değeri -1 ile 1 arasındadır.
        float moveZ = Input.GetAxis("Vertical");

        // [GEREKSİNİM 10] Animator: Hareket varsa animasyonu tetikle (Örn: Motor parlaklığı artar)
        if(shipAnimator != null)
        {
            // "IsMoving" adında bir bool parametresi olduğunu varsayıyoruz.
            shipAnimator.SetBool("IsMoving", moveX != 0 || moveZ != 0);
        }

        Vector3 movement = new Vector3(moveX, moveZ, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    // [GEREKSİNİM 6] Ekran sınırlarını belirleme (LateUpdate içinde kullanılır)
    void ClampPosition()
    {
        Vector3 viewPos = transform.position;
        // X ekseninde ekran dışına çıkmayı engelle (Değerleri sahnene göre ayarla)
        viewPos.x = Mathf.Clamp(viewPos.x, -2.5f, 2.5f); 
        viewPos.y = Mathf.Clamp(viewPos.y, -4.5f, 4.5f);
        transform.position = viewPos;
    }

    // [GEREKSİNİM 5] RayCast: Öndeki düşmanları algılama sistemi
    void CheckEnemyAhead()
    {
        // Geminin ucundan yukarı doğru bir ışın gönder
        RaycastHit2D hit = Physics2D.Raycast(MissileSpawnPoint.position, Vector2.up, 10f);
        
        // Editörde görmek için kırmızı çizgi çiz
        Debug.DrawRay(MissileSpawnPoint.position, Vector3.up * 10f, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Düşman Kilitlendi! Ateş Serbest!");
                // İleride buraya otomatik ateş etme veya hedef imleci rengi değiştirme eklenebilir.
            }
        }
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
            Debug.Log("Player hit by Enemy!");
        }
    }
}