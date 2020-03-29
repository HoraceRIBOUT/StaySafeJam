using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public CameraManagement cameraScreen;
    public Move hero;
    public SoundManager sndManager;
    public FeedbackVal feedbackValue;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LaunchWolf()
    {
        if (lastLaunch)
        {
            return;
        }
        lastLaunch = true;

        Vector3 minL = new Vector3(0, 0, 43);
        Vector3 maxL = new Vector3(88, 0, 0);
        Vector3 minR = new Vector3(-88, 0, 0);
        Vector3 maxR = new Vector3(0, 0, -43);
        Vector2 minMaxHeight = new Vector2(0.4f, 0.4f);
        Vector2 minMaxLarger = new Vector2(0.4f, 0.4f);

        for (int i = 0; i < 6; i++)
        {
            float lerpX = Random.Range(0f, 1f);
            float lerpY = Random.Range(0f, 1f);
            Vector3 position = Vector3.Lerp(Vector3.Lerp(minL, maxL, lerpX), Vector3.Lerp(minR, maxR, lerpX), lerpY);
            Vector3 randomValue = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            GameObject wolf = Instantiate(wolfToSpawn, position + randomValue, Quaternion.identity);

            wolf.GetComponent<Square>().resistance = 24;
        }

    }

    public GameObject wolfToSpawn;
    public bool lastLaunch = false;
}
