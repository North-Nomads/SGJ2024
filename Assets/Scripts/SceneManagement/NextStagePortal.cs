using SGJ.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGJ.SceneManagement
{
    public class NextStagePortal : MonoBehaviour
    {
        [SerializeField] private bool shouldRestartProgress;
        private const string PlayerTagName = "Player";
        private const int CombatSceneID = 1;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerTagName))
            {
                if (shouldRestartProgress)
                    PlayerSaveController.ResetPlayerProgress();

                OpenNewScene(CombatSceneID);
            }
        }

        private void OpenNewScene(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}