using UnityEngine.SceneManagement;

namespace SGJ.SceneManagement
{
    public static class SceneController 
    {
        public static void LoadScene(int id)
        {
            SceneManager.LoadScene(id);
        }

        public static void ReturnToHub()
        {
            SceneManager.LoadScene(0);
        }

        public static void ReloadScene()
        {
            var currentSceneID = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneID);
        }
    }
}