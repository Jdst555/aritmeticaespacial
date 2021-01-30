using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public int currentPlayerScore = 0;
    public GameObject digit;//objeto
    public GameObject enemy;
    public GameObject[] asteroids;
    public GameObject bonusObject;
    public Timer timer;
    public GameObject healthBoost;
    public Transform digitAnimationTarget;
    public GameObject animationDigit;
    public ProblemUI problemUI;
    public AudioClip enemyExplosionSound;
    public float enemySpawnInterval = 5;
    public float problemsSetTime = 5;
    public int problemSetSize = 5;
    public GameObject timeOverText;
    private GameObject mainUI; //objeto que contiene la interface principal durante el juego
    private TextMeshProUGUI scoreTMP;
    private int numberOfDigits;
    
    private bool gameIsPaused;
    private ProblemManager problemManager;
    private int enemyDestroyBonus = 50;
    private int digitReward = 100;
    private int digitPunish = -50;
    private int problemReward = 500;
    
    private int problemsSolved = 0;
    private List<Problem> listProblemsSolved = new List<Problem>();
    private int healingValue = 33;
    private AudioSource audioSource;
    private float lastEnemySpawnTime = 0;
    private List<GameObject> enemiesList = new List<GameObject>();
    private float timeAcc = 0;
    private bool running = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        problemManager = new ProblemManager();
        
        player = GameObject.FindGameObjectWithTag("Player");
        Player.OnPlayerDie += GameOverSequence;//asignar metodo GameOverSequence a evento OnPlayerDie ***(delegate)***
        Player.OnGoodDigitFound += GoodDigitFound;
        Player.OnBadDigitFound += BadDigitFound;
        Timer.OnTimeOver += TimeOverAction;
        Enemy.OnEnemyDestroyEvent += EnemyDestroySequence;
        mainUI = GameObject.Find("MainUI");
        scoreTMP = GameObject.Find("Score text").GetComponent<TextMeshProUGUI>();//accede al componente TMP "scoreText" que contiene el UI del puntaje

    }
    void Start()
    {
        SpawnAsteroids(10, 10);//provisional
        SpawnObjects(5);//prov
        UpdatePlayerScore(0);
        gameIsPaused = false;
        timer.StartTimer(problemsSetTime);
        SetOfProblemsText(0, problemSetSize);
        
    }
    // Update is called once per frame
    void Update()
    {
        if (!running)
        {
            ShowInstructions();
        }
        
        timeAcc += Time.deltaTime;
        if (running)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Exit();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                //Debug.Log("UDATE. call TogglePauseGame");
                TogglePauseGame();
            }

            if (problemManager.GetCurrentProblem() == null)
            {
                numberOfDigits = problemManager.MakeProblem((ProblemTypes)Random.Range(0, 2));
                problemUI.ShowProblem(problemManager.GetCurrentProblem().strProblem, numberOfDigits);
            }

            if (problemManager.GetCurrentProblem().isSolved)
            {
                numberOfDigits = problemManager.MakeProblem((ProblemTypes)Random.Range(0, 2));
                problemUI.ShowProblem(problemManager.GetCurrentProblem().strProblem, numberOfDigits);
            }
            if (problemsSolved == problemSetSize)
            {
                VictorySequence();
            }

            if (timeAcc - lastEnemySpawnTime > enemySpawnInterval)
            {
                for (int i = 0; i < 1; i++)
                {

                    float randomDirection = Random.Range(0f, 360f);
                    randomDirection *= Mathf.Deg2Rad;
                    float distance = Random.Range(12, 16);
                    Vector2 spawnPosition = new Vector2(distance * Mathf.Cos(randomDirection), distance * Mathf.Sin(randomDirection));
                    spawnPosition += (Vector2)player.transform.position;
                    SpawnEnemy(spawnPosition, ((Vector2)player.transform.position - spawnPosition).normalized);
                    lastEnemySpawnTime = timeAcc;
                }

            }
        }
        
    }
    private void OnDisable()
    {
        Player.OnPlayerDie -= GameOverSequence;
        Player.OnGoodDigitFound -= GoodDigitFound;
        Player.OnBadDigitFound -= BadDigitFound;
        Timer.OnTimeOver -= TimeOverAction;
        Enemy.OnEnemyDestroyEvent -= EnemyDestroySequence;
    }
    public void SpawnEnemy(Vector3 spawnPosition, Vector2 spawnRotation)
    {
 
        GameObject thisEnemyInstance = Instantiate(enemy, spawnPosition, Quaternion.Euler(spawnRotation));
        enemiesList.Add(thisEnemyInstance);
    }
    //provisional
    private void SpawnObjects(int modulo)
    {
        Queue<int> integersQueue = new Queue<int>();
        for (int i = 0; i < 11; i++)
        {
            for (int n = 0; n < 10; n++)
            {
                integersQueue.Enqueue(n);
            }
            
        }
        for (int n = 0; n < 5; n++)
        {
            integersQueue.Enqueue(Random.Range(0,10));
        }
        int counter = 0;
        //Debug.Log("SPAWN_OBJECT. inicio");
        int dimension = 25;
        int minX = -dimension;
        int maxX = dimension;
        int minY = -dimension;
        int maxY = dimension;

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                if (x % modulo == 0 && y % modulo == 0)
                {
                    if (x == 0 && y == 0)
                    {
                        break;
                    }
                    GameObject digitObject = Instantiate(digit, new Vector3(x, y), Quaternion.identity);
                    counter += 1;
                   
                    digitObject.GetComponent<Digit>().SetDigit(integersQueue.Dequeue());
                    //Debug.Log("SPAWN_OBJECT. Digit creado: " + digitObject.GetComponent<Digit>().GetDigit());
                }
            }
        }
        Debug.Log("Spawned objects: " + counter);
    }
    private void RespawnDigit(int intDigit)
    {
        float randomDirection = Random.Range(0f, 360f);
        //Debug.Log("RESPAWN_DIGIT Random direcction: " + randomDirection);
        randomDirection *= Mathf.Deg2Rad;
        //Debug.Log("RESPAWN_DIGIT Radians direction: " + randomDirection);
        float distance = 20;
        Vector2 respawnLocation = new Vector2(distance * Mathf.Cos(randomDirection), distance * Mathf.Sin(randomDirection));
        GameObject digitObject = Instantiate(digit, respawnLocation + (Vector2)player.transform.position, Quaternion.identity);
        digitObject.GetComponent<Digit>().SetDigit(intDigit);
        //Debug.Log("RESPAWN_DIGIT Respawn location: " + respawnLocation);
    }
    //provisional
    private void SpawnAsteroids(float dist, int num)
    {
        for (int i = 0; i < num; i++)
        {
            Vector2 vector = Random.insideUnitCircle.normalized;
            vector *= dist;
            GameObject asteroid = asteroids[Random.Range(0, asteroids.Length-1)];
            Instantiate(asteroid, player.transform.position + new Vector3(vector.x, vector.y, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
        }

    }
    public Problem GetCurrentProblem()
    {
        return problemManager.GetCurrentProblem();
    }
    public int GetCurrentDigitToFind()
    {
        //Debug.Log("GAME_MANAGER/GET_CURRENT_DIGIT_TO_FIND return: " + problemManager.GetCurrentDigitToFind());
        return problemManager.GetCurrentDigitToFind();
    }
   
    private void GameOverSequence()
    {
        GameObject gameOverCanvas = GameObject.Find("MainUI").transform.Find("GameOverCanvas").gameObject;
        gameOverCanvas.SetActive(true);
        GameObject.Find("Final score").GetComponent<TextMeshProUGUI>().SetText(currentPlayerScore.ToString());
    }
    private void VictorySequence()
    {
        Time.timeScale = 0;
        int score = currentPlayerScore;
        scoreTMP.SetText(string.Format("Puntos: \n{0,4:D6}", score));
        GameObject victoryCanvas = GameObject.Find("MainUI").transform.Find("VictoryCanvas").gameObject;
        victoryCanvas.SetActive(true);
        victoryCanvas.transform.Find("Final score").GetComponent<TextMeshProUGUI>().SetText(currentPlayerScore.ToString());
    }
    private void GoodDigitFound()
    {
        int provisionalOldDigitToFind = GetCurrentDigitToFind();
        RespawnDigit(provisionalOldDigitToFind);
        //Debug.Log("GOOD_DIGIT_FOUND. Old score is: " + currentPlayerScore);
        if (problemManager.GetCurrentProblem().SetDigitToFind())
        {
            problemUI.SetGoodDigit(provisionalOldDigitToFind);
            UpdatePlayerScore(digitReward);
            problemUI.AdvanceArrow();
        }
        else {
            Debug.Log("GOOD_DIGIT_FOUND. Empty Queue. Problem Solved");
            RewardProblemSolved();
            StartCoroutine("SetLastGoodDigit", provisionalOldDigitToFind);
            problemsSolved += 1;
            listProblemsSolved.Add(problemManager.GetCurrentProblem());
            SetOfProblemsText(problemsSolved, problemSetSize);
        }
        
    }
    private IEnumerator SetLastGoodDigit(int digit)
    {
        problemUI.SetGoodDigit(digit);
        yield return new WaitForSeconds(2);
        problemManager.GetCurrentProblem().isSolved = true;
        problemUI.ResetArrow();
        problemUI.ClearDigits();
    }
    private void BadDigitFound()
    {
        int provisionalOldDigitToFind = GetCurrentDigitToFind();
        RespawnDigit(provisionalOldDigitToFind);
        UpdatePlayerScore(digitPunish);
    }
    private void RewardProblemSolved()
    {
        UpdatePlayerScore(problemReward);
    }
    private void UpdatePlayerScore(int scoreIncrementDecrement)
    {
        //Debug.Log("UPDATE_PLAYER_SCORE. Player score: " + currentPlayerScore);

        //incrementar la variable score de Player
        player.GetComponent<Player>().score += scoreIncrementDecrement;

        //actualizar lavariable currentPlayerScore de GameManager (esta clase)
        currentPlayerScore = player.GetComponent<Player>().score;

        //llamar a UpdateCurrentScore que muestra el puntaje actual
        UpdateCurrentScoreUI();

        //llamar a la animacion de puntaje obtenido
        InstantiateCaptureAnimation(player.transform, scoreIncrementDecrement);
    }
    private void SetOfProblemsText(int numberOfProblemsSolved, int setOfProblemsSize)
    {
        GameObject.Find("Problems set text").GetComponent<TextMeshProUGUI>().SetText(string.Format("{0} de {1} resueltos", numberOfProblemsSolved, setOfProblemsSize));
    }
    //actualiza y muestra en la UI el puntaje actual
    private void UpdateCurrentScoreUI()
    {
        int score = currentPlayerScore;

        //enviar la cadena de texto formateado al UI de puntaje
        if (score < 0)
        {
            scoreTMP.faceColor = new Color(1, 0, 0, 1);
        }
        else if (score >= 0)
        {
            scoreTMP.faceColor = new Color(1, 1, 1, 1);
        }
        
        StartCoroutine("ScoreAnimationCoroutine");
       
        
        //Debug.Log("UPDATE_CURRENT_SCORE_UI. " + scoreTMP.text);
    }
    private IEnumerator ScoreAnimationCoroutine()
    {
        int times = 15;
        while (times > 0)
        {
            int a, b, c, d, e, f;
            a = Random.Range(0, 10);
            b = Random.Range(0, 10);
            c = Random.Range(0, 10);
            d = Random.Range(0, 10);
            e = Random.Range(0, 10);
            f = Random.Range(0, 10);
            string numbersString = string.Concat(a,b,c,d,e);
            scoreTMP.SetText(string.Format("Puntos: \n{0:}", numbersString));
            yield return new WaitForSeconds(0.005f);
            times -= 1;
        }
        int score = currentPlayerScore;
        scoreTMP.SetText(string.Format("Puntos: \n{0,4:D6}", score));
    }
    public void InstantiateCaptureAnimation(Transform animationTransform, int amount)
    {
        Vector3 position = animationTransform.position;
        position += new Vector3(0, 1, 0);
        
        if (amount > 0)
        {
            GameObject anim = Instantiate(bonusObject, position, Quaternion.identity);
            anim.GetComponentInChildren<TextMeshProUGUI>().SetText("+" + amount.ToString());
            Destroy(anim, 2);
        }
        if (amount < 0)
        {
            GameObject anim = Instantiate(bonusObject, position, Quaternion.identity);
            TextMeshProUGUI tmp = anim.GetComponentInChildren<TextMeshProUGUI>();
            tmp.faceColor = new Color(1, 0, 0, 1);
            tmp.SetText(amount.ToString());
            Destroy(anim, 2);
        }
    }
    private void EnemyDestroySequence(Transform enemyTransform)
    {
        
        GameObject healthBoostGameObjectInstance = Instantiate(healthBoost, enemyTransform.position, Quaternion.identity);
        healthBoostGameObjectInstance.GetComponent<HealthBoost>().healingValue = healingValue;
        UpdatePlayerScore(enemyDestroyBonus);
        audioSource.clip = enemyExplosionSound;
        audioSource.Play();
        //InstantiateCaptureAnimation(enemyTransform, enemyDestroyBonus);




    }
    private void TimeOverAction()
    {
        player.GetComponent<Player>().StartCoroutine("playerDestructionSequence");
        GameOverSequence();
        timeOverText.SetActive(true);
    }

    private void ShowInstructions()
    {
        
        Time.timeScale = 0;
        GameObject instructionsCanvas = GameObject.Find("MainUI").transform.Find("Instructions canvas").gameObject;
        instructionsCanvas.SetActive(true);
        
        
        running = Input.anyKeyDown;
        if (running)
        {
            instructionsCanvas.SetActive(false);
            Time.timeScale = 1;
        }
        
    }
    public void Restart()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }
    private void TogglePauseGame()
    {

        //si el juego esta pausado
        if (gameIsPaused)
        {
            //Debug.Log("TOGGLE_PAUSE_GAME. Unpause branch. GameIsPaused before change: " + gameIsPaused);
            Time.timeScale = 1;
            gameIsPaused = false;
            
        }
        //si el juego no esta pausado
        else if (!gameIsPaused)
        {
            //Debug.Log("TOGGLE_PAUSE_GAME. Pause branch. GameIsPaused before change: " + gameIsPaused);
            Time.timeScale = 0;
            gameIsPaused = true;
        }
        
    }
    private void Exit()
    {
        Application.Quit();
    }
}
