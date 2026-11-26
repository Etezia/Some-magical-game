using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static PlayerPrefsKeyManager;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private string gameScene;

	

	public void Exit() => Application.Quit();
}
