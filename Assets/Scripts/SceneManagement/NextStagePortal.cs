using UnityEngine;
using UnityEngine.SceneManagement;

namespace SGJ.SceneManagement
{
    public class NextStagePortal : MonoBehaviour
    {
        private const string PlayerTagName = "Player";
        private const int CombatSceneID = 1;

        private void OnTriggerEnter(Collider other)
        {
            print(other.tag);
            if (other.CompareTag(PlayerTagName))
                OpenNewScene(CombatSceneID);
        }

        private void OpenNewScene(int id)
        {
            SceneManager.LoadScene(id);
        }
    }
}