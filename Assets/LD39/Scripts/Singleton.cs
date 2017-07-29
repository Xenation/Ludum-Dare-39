using UnityEngine;

namespace LD39 {
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		private static T instance;

		public static T I {
			get {
				if (instance == null) {
					instance = FindObjectOfType<T>();
					if (instance == null) {
						Debug.LogError("Could not find any instance of Singleton " + typeof(T).Name + "!");
					}
				}
				return instance;
			}
		}

	}
}
