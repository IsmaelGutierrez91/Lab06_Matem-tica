using Mathematics.Week6;
using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] float points;

    public static event Action<float> OnObtainPoints;
    private void Awake()
    {
        Time.timeScale = 1.0f;
        popUp.SetActive(false);
    }
    private void FixedUpdate()
    {
        points = points + Time.deltaTime;
        OnObtainPoints?.Invoke(points);
    }
    public void StopGame()
    {
        popUp.SetActive(true);
        text.SetText(((int)points).ToString());
        Time.timeScale = 0f;
    }
    public void ChangeSceneButton(string sceneToLoad)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        PlaneController.OnLoseAllLives += StopGame;
    }
    private void OnDisable()
    {
        PlaneController.OnLoseAllLives -= StopGame;
    }
}
