using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public GameObject waveAnnouncementPanel;

    public void ShowWaveAnnouncement(string waveName)
    {
        if (waveText != null) waveText.text = waveName;

        if (waveAnnouncementPanel != null)
        {
            waveAnnouncementPanel.SetActive(true);
            Invoke(nameof(HideWaveAnnouncement), 2f);
        }
    }

    private void HideWaveAnnouncement()
    {
        if (waveAnnouncementPanel != null) waveAnnouncementPanel.SetActive(false);
    }
}