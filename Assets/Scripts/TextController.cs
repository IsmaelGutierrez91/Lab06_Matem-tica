using UnityEngine;
using TMPro;
using Mathematics.Week6;

public class TextController : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] string tipeOfText;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void UpdateLivesText(int playerLives)
    {
        if (tipeOfText == "Time")
        {
            text.SetText("Lives: " + playerLives);
        }
    }
    public void UpdatePointsText(float points)
    {
        if (tipeOfText == "Points")
        {
            text.SetText("Points: " + (int)points);
        }
    }
    private void OnEnable()
    {
        PlaneController.OnCollisionWhitObstacle += UpdateLivesText;
        GameManager.OnObtainPoints += UpdatePointsText;
    }
    private void OnDisable()
    {
        PlaneController.OnCollisionWhitObstacle -= UpdateLivesText;
        GameManager.OnObtainPoints -= UpdatePointsText;
    }
}
