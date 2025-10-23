using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private Transform player;

    public int monedas;
    public int collect;
    public bool canMove = true;

    public float posicionX;
    public float posicionY;
    public float posicionZ;


    public TextMeshProUGUI contadorTxt;

    public GameObject panelVictoria;
    public TextMeshProUGUI victoriaTxt;

    private int gatosTotales;
    private int gatosRestantes;


    public string nivel;

    public string saveName;

    
    private float tiempo;

    public Text puntosTxt;
    public Text tiempoTxt;
    public TextMeshProUGUI timerTxt;
    public float tiempoTotal = 60f;
    private float tiempoRestante;
    private int puntos = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        tiempoRestante = tiempoTotal;

        Gato[] gatos = FindObjectsOfType<Gato>(); gatosTotales = gatos.Length;
        gatosRestantes = gatosTotales;

        ActualizarContador();

        if (panelVictoria != null)
            panelVictoria.SetActive(false);
    }

    public void GatoLimpio()
    {
        gatosRestantes--;
        ActualizarContador();

        if (gatosRestantes <= 0)
        {
            if (panelVictoria != null)
                panelVictoria.SetActive(false);

            //if (TimerManager.Instance != null)
                //TimerManager.Instance.TerminarJuego();
        }
    }

    void ActualizarContador()
    {
        if (contadorTxt != null)
        {
            int gatosLimpios = gatosTotales ;
            contadorTxt.text = $"Cats Cleaned: {gatosLimpios}";
        }
    }

    private void Update()
    {
        /*void Update()
        {
            if (!corriendo) return;

            tiempo += Time.deltaTime;

            if (timerTxt != null)
                timerTxt.text = "Tiempo: " + tiempo.ToString("F2") + "s";
        }*/



        //puntosTxt.text = "Points: " + puntos;
        //tiempoTxt.text = "Time: " + Mathf.Ceil(tiempoRestante);
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
    }

    public void MandarAGuardar()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        posicionX = player.position.x;
        posicionY = player.position.y;
        posicionZ = player.position.z;

        nivel = SceneManager.GetActiveScene().name;
    }
    public void MoverJugador()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        player.GetComponent<CharacterController>().enabled = false;
        player.position = new Vector3(GameManager.Instance.posicionX, GameManager.Instance.posicionY, GameManager.Instance.posicionZ);
        player.GetComponent<CharacterController>().enabled = true;
    }

    public void MostrarPantallaVictoria(float tiempoFinal)
    {
        if (panelVictoria != null) panelVictoria.SetActive(true);
        if (victoriaTxt != null)
            victoriaTxt.text = "The cats have been cleaned \nTime: " + tiempoFinal.ToString("F2") + " s";
        Time.timeScale = 0f;
    }

    public void ReiniciarEscena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
