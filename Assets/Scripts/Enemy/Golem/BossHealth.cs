using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BossHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // จำนวนเริ่มต้นของสุขภาพบอส
    private float currentHealth;
    [SerializeField] private GameObject monsterPrefab; // Prefab ของมอนสเตอร์ที่จะ spawn
    [SerializeField] private GameObject deathVFXPrefab; // เอฟเฟกต์เมื่อบอสตาย
    [SerializeField] private float spawnRadius = 5f; // ระยะห่างจากบอสในการ spawn มอนสเตอร์
    [SerializeField] private int numMonstersToSpawn = 3; // จำนวนมอนสเตอร์ที่จะ spawn
    private bool hasSpawnedMonsters = false; // เช็คว่าได้ spawn มอนสเตอร์หรือยัง
    private InventoryManager inventoryManager;
    private Player player;


    void Start()
    {
        currentHealth = maxHealth; // ตั้งค่าจำนวนสุขภาพปัจจุบันเป็นจำนวนสูงสุด

    }
    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>(); // ใช้ FindObjectOfType แทน GetComponent
        player = FindObjectOfType<Player>(); // หรือกำหนดการเข้าถึงผู้เล่นโดยตรง

    }

    void Update()
    {
        // ตรวจสอบเลือดบอสและ spawn มอนสเตอร์เมื่อเลือดน้อยกว่า 50%
        if (currentHealth <= maxHealth * 0.5f && !hasSpawnedMonsters)
        {
            SpawnMonsters();
            hasSpawnedMonsters = true; // ทำให้ spawn ได้ครั้งเดียว
        }
    }

    // ฟังก์ชันสำหรับการสร้างมอนสเตอร์
    void SpawnMonsters()
    {
        HashSet<Vector2> spawnedPositions = new HashSet<Vector2>(); // ใช้สำหรับเช็คไม่ให้ซ้อนทับกัน

        for (int i = 0; i < numMonstersToSpawn; i++)
        {
            Vector2 spawnPosition;

            // พยายามหาตำแหน่งที่ไม่ซ้อนทับกับมอนสเตอร์ที่สร้างก่อนหน้านี้
            do
            {
                // สุ่มตำแหน่งในระยะ spawnRadius รอบๆ ด้านหน้าบอส
                float offsetX = Random.Range(1f, spawnRadius); // ให้เกิดด้านหน้าบอส
                float offsetY = Random.Range(-spawnRadius, spawnRadius); // สุ่มในระยะ Y

                spawnPosition = new Vector2(transform.position.x + offsetX, transform.position.y + offsetY);

            } while (spawnedPositions.Contains(spawnPosition)); // ตรวจสอบว่าตำแหน่งไม่ซ้ำ

            // บันทึกตำแหน่งที่เกิดไว้
            spawnedPositions.Add(spawnPosition);

            // สร้างมอนสเตอร์ที่ตำแหน่งที่สุ่มได้
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // ฟังก์ชันรับความเสียหาย
    public void TakeDamage(float damage)
    {
        currentHealth -= damage; // ลดจำนวนสุขภาพตามค่าความเสียหาย
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth); // ทำให้สุขภาพไม่ต่ำกว่า 0

        Debug.Log("Boss took damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            DetectDeath(); // เรียกเมื่อบอสตาย
        }
    }

    // ฟังก์ชันสำหรับตรวจสอบการตาย
    public void DetectDeath()
    {
        if (currentHealth <= 0)
        {
            Instantiate(deathVFXPrefab, transform.position, Quaternion.identity);
            Debug.Log("Boss is dead.");
            Die();
        }
    }

    // ฟังก์ชันสำหรับจัดการเมื่อบอสตาย
    private void Die()
    {

        Debug.Log("Boss died.");
        Destroy(gameObject); // ทำลายบอสเมื่อสุขภาพเป็น 0

        // วาร์ป player กลับไปยังฉากที่ชื่อ "main"
        SceneManager.LoadScene("main");
        GameManager.instance.mapManager.SetFarmOn(true);
        inventoryManager.Add("Backpack", "Fertilizer", 40);
        inventoryManager.Add("Backpack", "Sword lnw", 1);

    }

}
