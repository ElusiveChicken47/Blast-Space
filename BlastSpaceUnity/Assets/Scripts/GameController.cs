using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int numEnemy = 0;
    private GameObject[] enemies;

    [SerializeField]
    private GameObject spawner1;

    [SerializeField]
    private GameObject spawner2;

    [SerializeField] 
    private int lastTime = 2;

    [SerializeField]
    private float iFrameTime = 0.5f;

    public Material iFrameMaterial;

    private ParticleSystem poof;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //int gameEnded = 0;
        //while (gameEnded == 0)
        //{
            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndGame();
                Count();
                //gameEnded = 1;
            }
        //}
    }

    void EndGame()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Destroy(spawner1);
        Destroy(spawner2);

        Rigidbody rb;

        foreach (GameObject enemyClone in enemies)
        {
            enemyClone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        GameObject mainChar = GameObject.FindGameObjectWithTag("Player");
        Animator animator = mainChar.GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = false;
        }
        StartCoroutine(ImpactFrame());

        WaitToEnd(lastTime + iFrameTime);
    }
    void Count()
    {
        numEnemy = enemies.Length;
        foreach (GameObject enemyClone in enemies)
        {
            poof = enemyClone.GetComponent<ParticleSystem>();
            if (poof != null)
            {
                poof.Play();
            }
            StartCoroutine(LastingEnemies(enemyClone));
        }
    }
    IEnumerator LastingEnemies(GameObject obj)
    {
        yield return new WaitForSeconds(lastTime);

        obj.GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator ImpactFrame()
    {
        Camera.main.GetComponent<Camera>().SetReplacementShader(iFrameMaterial.shader, null);
        yield return new WaitForSeconds(iFrameTime);
        Camera.main.GetComponent<Camera>().ResetReplacementShader();
    }
    IEnumerator WaitToEnd(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerPrefs.SetInt("Score", numEnemy);
        SceneManager.LoadScene("HighScoreScene");
    }
}
