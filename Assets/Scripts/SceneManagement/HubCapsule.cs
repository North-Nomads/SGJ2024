using SGJ.Player;
using UnityEngine;

namespace SGJ.SceneManagement
{
    public class HubCapsule : MonoBehaviour
    {
        private const string PlayerTagName = "Player";
        private const int CombatSceneID = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTagName))
            {
                PlayerSaveController.ResetPlayerProgress();
                SceneController.LoadScene(CombatSceneID);
            }
        }
    }
}