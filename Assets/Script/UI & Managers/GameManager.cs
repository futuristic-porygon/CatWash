<<<<<<< HEAD
=======
﻿using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
>>>>>>> origin/master
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

<<<<<<< HEAD
    public int limiteGatosLogro = 10;


=======
>>>>>>> origin/master
    public TextMeshProUGUI contadorTxt;

    public GameObject panelVictoria;
    public TextMeshProUGUI victoriaTxt;

    private int gatosTotales;
    private int gatosRestantes;

    public string nivel;
    public string saveName;

<<<<<<< HEAD

    private float tiempo;

=======
>>>>>>> origin/master
    public Text puntosTxt;
    public Text tiempoTxt;
    public TextMeshProUGUI timerTxt;
    public float tiempoTotal = 60f;

    private float tiempoRestante;
    private int puntos = 0;

<<<<<<< HEAD
    public GameObject canvasLogros;
    public TextMeshProUGUI textoLogros;
    public bool logroDesbloqueado = false;
    public bool canvasVisible = false;
    public string mensajeDefault;


=======
    // 🔹 Variables nuevas
    [Header("Control de pausa por gatos limpios")]
    public int gatosPorPausa = 5;        // Cada cuántos gatos limpios se pausa el timer
    public float duracionPausa = 10f;    // Duración de la pausa en segundos
    private int gatosLimpiosActual = 0;  // Contador interno de gatos limpios
    private bool enPausa = false;

    private Timer timer;
>>>>>>> origin/master

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

        // Buscar gatos iniciales
        Gato[] gatos = FindObjectsOfType<Gato>();
        gatosTotales = gatos.Length;
        gatosRestantes = gatosTotales;

        ActualizarContador();

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        StartCoroutine(EsperarYAsignarTimer());
    }

    private IEnumerator EsperarYAsignarTimer()
    {
        yield return new WaitForSeconds(0.2f);
        timer = FindObjectOfType<Timer>();

        if (timer == null)
            Debug.LogWarning("⚠️ GameManager no encontró el Timer en la escena.");
        else
            Debug.Log("✅ Timer detectado correctamente por GameManager.");
    }

    public void RegisterGato()
    {
        this.gatosTotales++;
        this.gatosRestantes++;
        this.ActualizarContador();
    }

    public void GatoLimpio()
    {
<<<<<<< HEAD
        this.gatosRestantes--;
        this.ActualizarContador();

        int gatosLimpiosActual = this.gatosTotales - this.gatosRestantes;

        if (gatosLimpiosActual >= this.limiteGatosLogro)
=======
        gatosRestantes--;
        gatosLimpiosActual++;
        ActualizarContador();

        // 🔹 Verificar si toca pausar el timer
        if (!enPausa && gatosLimpiosActual % gatosPorPausa == 0)
        {
            StartCoroutine(PausarTimerTemporalmente());
        }

        if (gatosRestantes <= 0)
>>>>>>> origin/master
        {
            this.DesbloquearLogroContador();
        }

        if (this.gatosRestantes <= 0)
        {

            if (panelVictoria != null)
                panelVictoria.SetActive(false);
<<<<<<< HEAD

            //if (TimerManager.Instance != null)
            //TimerManager.Instance.TerminarJuego();
=======
>>>>>>> origin/master
        }
    }

    private IEnumerator PausarTimerTemporalmente()
    {
        enPausa = true;

        if (timer != null)
        {
            timer.Pausar(true);
            timer.CambiarColor(Color.yellow);
        }

        Debug.Log($"⏸️ Timer pausado por {duracionPausa} s (gatos limpios: {gatosLimpiosActual})");

        yield return new WaitForSeconds(duracionPausa);

        if (timer != null)
        {
            timer.Pausar(false);
            timer.CambiarColor(Color.white);
        }

        Debug.Log("▶️ Timer reanudado");
        enPausa = false;
    }

    void ActualizarContador()
    {
        if (this.contadorTxt != null)
        {
<<<<<<< HEAD
            int gatosLimpios = this.gatosTotales - this.gatosRestantes;
            this.contadorTxt.text = string.Format("Cats Cleaned: {0}/{1}", gatosLimpios, this.gatosTotales);
=======
            int gatosLimpios = gatosTotales - gatosRestantes;
            contadorTxt.text = $"Cats Cleaned: {gatosLimpios}";
>>>>>>> origin/master
        }
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

    public void DesbloquearLogroContador()
    {

        if (this.logroDesbloqueado)
        {
            return;
        }

        this.logroDesbloqueado = true;

        this.ActualizarTextoLogros();

        if (this.canvasLogros != null)
        {
            this.canvasLogros.SetActive(true);
            this.canvasVisible = true;

            Invoke("OcultarCanvasLogros", 5f);
        }

    }

    public void OcultarCanvasLogros()
    {
        if(this.canvasLogros != null)
        {
            this.canvasLogros.SetActive(false);
            this.canvasVisible = false;
        }
    }

    public void ActualizarTextoLogros()
    {
        if (this.textoLogros != null)
        {
            if (this.logroDesbloqueado)
            {
                this.textoLogros.text = this.mensajeDefault;
            }
        }
    }


}
