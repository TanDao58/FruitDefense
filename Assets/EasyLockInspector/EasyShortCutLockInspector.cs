#if UNITY_EDITOR
namespace EasyShortCutLockInspector.Editor
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal class EasyShortCutLockInspector : MonoBehaviour
    {
        [MenuItem("Edit/Toggle Inspector Lock _g")]
        public static void Lock ()
        {
            ActiveEditorTracker.sharedTracker.isLocked = ! ActiveEditorTracker.sharedTracker.isLocked;
            ActiveEditorTracker.sharedTracker.ForceRebuild ();
        }

        [MenuItem("Edit/Toggle Inspector Lock _g", true)]
        public static bool Valid ()
        {
            return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
        }

        [MenuItem("Edit/Normal Time Scale _PGUP")]
        public static void NormalTimeScale()
        {
            Time.timeScale = 1f;
        }

        [MenuItem("Edit/Very Slow Time Scale _4")]
        public static void VerySlowTimeScale()
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 0.1f;
            } else
            {
                Time.timeScale = 1f;
            }
        }

        public static void LoadSceneIndex(int index) {
            string path = SceneUtility.GetScenePathByBuildIndex(index);
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                EditorSceneManager.OpenScene(path);
            }
        }

        [MenuItem("Edit/Load Scene 0 _F1")]
        public static void LoadScene0() {
            LoadSceneIndex(0);
        }

        [MenuItem("Edit/Load Scene 1 _F2")]
        public static void LoadScene1() {
            LoadSceneIndex(1);
        }

        [MenuItem("Edit/Load Scene 2 _F3")]
        public static void LoadScene2() {
            LoadSceneIndex(2);
        }

        [MenuItem("Edit/Load Scene 3 _F4")]
        public static void LoadScene3() {
            LoadSceneIndex(3);
        }

        [MenuItem("Edit/Load Scene 4 _F5")]
        public static void LoadScene4() {
            LoadSceneIndex(4);
        }

        [MenuItem("Edit/Load Scene 5 _F6")]
        public static void LoadScene5() {
            LoadSceneIndex(5);
        }

        [MenuItem("Edit/Load Scene 6 _F7")]
        public static void LoadScene6() {
            LoadSceneIndex(6);
        }

        [MenuItem("Edit/Load Scene 7 _F8")]
        public static void LoadScene7() {
            LoadSceneIndex(7);
        }

        [MenuItem("Edit/Load Scene 8 _F9")]
        public static void LoadScene8() {
            LoadSceneIndex(8);
        }


        [MenuItem("Edit/Load Scene 9 _F10")]
        public static void LoadScene9() {
            LoadSceneIndex(9);
        }
    }
}
#endif