using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugMute : MonoBehaviour {
	[SerializeField]
	public AudioSource bgmAudio;



	// Use this for initialization
	void Start () {
		bgmAudio.mute = GetComponent<Toggle> ().isOn;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void ChangeToggle() {
		Debug.Log ("change in");
		bgmAudio.mute = GetComponent<Toggle> ().isOn;
	}
}
