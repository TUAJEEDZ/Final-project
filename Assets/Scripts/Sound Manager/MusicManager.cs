using UnityEngine;

namespace SoundManager
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager instance; // Singleton instance
        public AudioSource[] audioSources;  // Array สำหรับเก็บ AudioSource หลายตัว
        private bool isMuted = false; // สถานะเสียงเปิดหรือปิด

        private void Awake()
        {
            if (instance == null)
            {
                instance = this; // กำหนด instance
                DontDestroyOnLoad(gameObject); // คง MusicManager ข้าม Scene
            }
            else
            {
                Destroy(gameObject); // ทำลาย instance ที่ซ้ำ
            }
        }

        public void ToggleMute()
        {
            isMuted = !isMuted; // สลับสถานะเสียง
            foreach (var audioSource in audioSources) // ทำการสลับเสียงให้กับทุกตัวใน array
            {
                audioSource.mute = isMuted; // ปิด/เปิดเสียง
            }
        }

        public bool IsMuted()
        {
            return isMuted; // คืนค่าสถานะว่าเสียงปิดหรือเปิด
        }
    }
}
