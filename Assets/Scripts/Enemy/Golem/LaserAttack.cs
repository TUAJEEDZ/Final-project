using System.Collections;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private Transform firePoint;  // Where the laser spawns

    private bool canAttack = true;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("ไม่พบ Player! โปรดตรวจสอบให้แน่ใจว่าวัตถุ Player มีแท็ก 'Player'.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectRange && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        canAttack = false;

        // สร้างเลเซอร์และแสดงตำแหน่ง
        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        Debug.Log($"เลเซอร์ถูกสร้างที่ตำแหน่ง: {laser.transform.position}");

        Animator laserAnim = laser.GetComponent<Animator>();
        if (laserAnim != null)
        {
            laserAnim.SetTrigger("Fire");
        }

        // รอให้เลเซอร์ทำงานเสร็จ
        yield return new WaitForSeconds(3f);

        // ทำลายเลเซอร์หลังจากการกระทำเสร็จสิ้น
        Destroy(laser);

        // รอช่วงคูลดาวน์
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // Optional: visualize detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
