using UnityEngine;
using UnityEngine.UI;
using SoundManager; // ใช้ namespace ของ MusicManager

public class MuteButtonUI : MonoBehaviour
{
    public Button muteButton;   // ปุ่ม UI
    public Image muteIcon;      // ไอคอนในปุ่ม
    public Sprite unmuteSprite; // รูปไอคอนเปิดเสียง
    public Sprite muteSprite;   // รูปไอคอนปิดเสียง

    private void Start()
    {
        UpdateIcon(); // อัปเดตไอคอนตอนเริ่มต้น
        muteButton.onClick.AddListener(OnMuteButtonClicked); // เพิ่ม Listener
    }

    private void OnMuteButtonClicked()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.ToggleMute(); // สลับสถานะเปิด/ปิดเสียง
            UpdateIcon(); // อัปเดตไอคอนใหม่
        }
    }

    private void UpdateIcon()
    {
        if (MusicManager.instance != null && MusicManager.instance.IsMuted())
        {
            muteIcon.sprite = muteSprite; // เปลี่ยนเป็นไอคอนปิดเสียง
        }
        else
        {
            muteIcon.sprite = unmuteSprite; // เปลี่ยนเป็นไอคอนเปิดเสียง
        }
    }
}
