using System.Collections;
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

    public Text puntosTxt;
    public Text tiempoTxt;
    public TextMeshProUGUI timerTxt;
    public float tiempoTotal = 60f;

    private float tiempoRestante;
    private int puntos = 0;

    // --------------------------
    // LOGRO GENERAL (ya existente)
    public int limiteGatosLogro = 10;
    public GameObject canvasLogros;
    public TextMeshProUGUI textoLogros;
    public bool logroDesbloqueado = false;
    public bool canvasVisible = false;
    public string mensajeDefault;
    // --------------------------

    // --------------------------
    // 🟦 NUEVO: LOGRO JETPACK
    [Header("Logro Jetpack")]
    public Animator jetpackPopupAnimator;              // Popup de 3 segundos
    public int gatosParaJetpack = 20;
    public bool jetpackDesbloqueado = false;

    public Jetpack jetpack;                      // Referencia al script Jetpack
    public GameObject barraCombustibleUI;        // UI de combustible (se oculta al inicio)
    // --------------------------

    // --------------------------
    // PAUSA DEL TIMER
    [Header("Control de pausa por gatos limpios")]
    public int gatosPorPausa = 5;
    public float duracionPausa = 10f;

    private int gatosLimpiosActual = 0;
    private bool enPausa = false;
    // --------------------------

    private Timer timer;

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

        // Buscar gatos al inicio
        Gato[] gatos = FindObjectsOfType<Gato>();
        gatosTotales = gatos.Length;
        gatosRestantes = gatosTotales;

        ActualizarContador();

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        if (barraCombustibleUI != null)
            barraCombustibleUI.SetActive(false);   // Ocultar UI al inicio

        
        StartCoroutine(EsperarYAsignarTimer());
    }

    private IEnumerator EsperarYAsignarTimer()
    {
        yield return new WaitForSeconds(0.2f);
        timer = FindObjectOfType<Timer>();
    }

    // --------------------------------------------
    // CUANDO SE LIMPIA UN GATO
    // --------------------------------------------
    public void GatoLimpio()
    {
        gatosRestantes--;
        gatosLimpiosActual++;
        ActualizarContador();

        // PAUSA DEL TIMER
        if (!enPausa && gatosLimpiosActual % gatosPorPausa == 0)
            StartCoroutine(PausarTimerTemporalmente());

        // LOGRO GENERAL
        if (gatosLimpiosActual >= limiteGatosLogro)
            DesbloquearLogroContador();

        // 🟦 LOGRO JETPACK
        if (!jetpackDesbloqueado && gatosLimpiosActual >= gatosParaJetpack)
            DesbloquearJetpack();

        // VICTORIA
        if (gatosRestantes <= 0)
        {
            if (panelVictoria != null)
                panelVictoria.SetActive(false);
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

        yield return new WaitForSeconds(duracionPausa);

        if (timer != null)
        {
            timer.Pausar(false);
            timer.CambiarColor(Color.white);
        }

        enPausa = false;
    }

    void ActualizarContador()
    {
        if (contadorTxt != null)
        {
            int gatosLimpios = gatosTotales - gatosRestantes;
            contadorTxt.text = $"Cats Cleaned: {gatosLimpios}";
        }
    }

    // --------------------------------------------
    // LOGRO GENERAL
    // --------------------------------------------
    public void DesbloquearLogroContador()
    {
        if (logroDesbloqueado) return;

        logroDesbloqueado = true;

        ActualizarTextoLogros();

        if (canvasLogros != null)
        {
            canvasLogros.SetActive(true);
            canvasVisible = true;

            Invoke("OcultarCanvasLogros", 5f);
        }
    }

    public void ActualizarTextoLogros()
    {
        if (textoLogros != null && logroDesbloqueado)
        {
            textoLogros.text = mensajeDefault;
        }
    }

    public void OcultarCanvasLogros()
    {
        if (canvasLogros != null)
        {
            canvasLogros.SetActive(false);
            canvasVisible = false;
        }
    }

    // --------------------------------------------
    // 🟦🟦 DESBLOQUEAR JETPACK 🟦🟦
    // --------------------------------------------
    public void DesbloquearJetpack()
    {
        jetpackDesbloqueado = true;
        barraCombustibleUI.SetActive(true);

        if (jetpack != null)
            jetpack.SetJetpackState(true);

        Debug.Log("🚀 Jetpack desbloqueado!");

        // 🔥 REPRODUCIR ANIMACIÓN
        if (jetpackPopupAnimator != null)
            jetpackPopupAnimator.SetTrigger("Mostrar");

        // También mostrar texto de logro si quieres
        if (!canvasVisible && canvasLogros != null)
        {
            textoLogros.text = "Jetpack Unlocked!";
            canvasLogros.SetActive(true);
            canvasVisible = true;

            Invoke(nameof(OcultarCanvasLogros), 5f);
        }
    }

}
