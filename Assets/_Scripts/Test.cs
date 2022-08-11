using UnityEngine;

namespace _Scripts {
    public class Test : MonoBehaviour{
        void Start () {

#if UNITY_EDITOR
            Debug.Log("Unity Editor");
#endif

#if UNITY_IOS
            Debug.Log("iOS");
#endif

#if UNITY_STANDALONE_OSX
        Debug.Log("Standalone OSX");
#endif

#if UNITY_STANDALONE_WIN
      Debug.Log("Standalone Windows");
#endif

        } 
    }
}
