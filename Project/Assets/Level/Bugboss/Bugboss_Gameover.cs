using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bugboss_Gameover : MonoBehaviour, DeathBehaviour {
	

	public void OnDeath() {
		SceneManager.LoadScene("Menu/Scene/Main");
	}

}