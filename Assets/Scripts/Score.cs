using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Score : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    private float timeScore;

    float min, sec;

    private void Start()
    {
        timeScore = 0;
    }

    private void Update()
    {
        timeScore += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        sec = (int)timeScore % 60;
        min = (int)timeScore / 60;
        scoreText.text = string.Format("Time: '{0} ''{1}", min, sec);
    }

    public void OnDeath()
    {
        this.enabled = false;
        float highscore = PlayerPrefs.GetFloat("HIGHSCORE", 0.0f);

        sec = (int)timeScore % 60;
        min = (int)timeScore / 60;
        scoreText.text = string.Format("Time: '{0} ''{1}", min, sec);

        float minH, secH;
        secH = (int)highscore % 60;
        minH = (int)highscore / 60;
        scoreText.text += string.Format("\nHighScore Time: '{0} ''{1}", minH, secH);

        highscore = Mathf.Max(highscore, timeScore);
        PlayerPrefs.SetFloat("HIGHSCORE", highscore);
    }

    public void OnRevive()
    {
        Start();
        this.enabled = true;
    }
}