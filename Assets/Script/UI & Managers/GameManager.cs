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

    public int limiteGatosLogro = 10;
    public GameObject canvasLogros;
    public TextMeshProUGUI textoLogros;
    public bool logroDesbloqueado = false;
    public bool canvasVisible = false;
    public string mensajeDefault;

    [Header("Logro Jetpack")]
    public Animator jetpackPopupAnimator;
    public int gatosParaJetpack = 20;
    public bool jetpackDesbloqueado = false;

    public Jetpack jetpack;
    public GameObject barraCombustibleUI;

    [Header("Logro Patines")]
    public int gatosParaPatines = 15;
    public bool patinesDesbloqueados = false;
    public Animator patinesPopupAnimator;
    public GameObject iconoPatinesUI;

    [Header("Control de pausa por gatos limpios")]
    public int gatosPorPausa = 5;
    public float duracionPausa = 10f;

    [Header("Notificación Timer")]
    public Animator timerPopupAnimator;

    private int gatosLimpiosActual = 0;
    private bool enPausa = false;

    private Timer timer;

    [Header("Arma 2")]
    public bool arma2Desbloqueada = false;
    public Animator arma2PopupAnimator;
    public GameObject arma2AmmoUI;
    [Header("Arma 3")]
    public bool arma3Desbloqueada = false;
    public Animator arma3PopupAnimator;
    public GameObject arma3AmmoUI;     
    public int gatosParaArma2 = 8;
    public int gatosParaArma3 = 15;

    private void Awake()
    {
        // --- Singleton ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Escuchar cambios de escena
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // --- AUTODESTRUIR si entramos al menú principal ---
        if (scene.name == "Main Menu")   // <-- Cambia esto si tu menú tiene otro nombre
        {
            Debug.Log("GameManager destruido al entrar al menú principal.");
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Evitar fuga de eventos
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void Start()
    {
        ScoreManager.Instance?.ResetScore();

        tiempoRestante = tiempoTotal;

        // Buscar gatos al inicio
        Gato[] gatos = FindObjectsOfType<Gato>();
        gatosTotales = gatos.Length;
        gatosRestantes = gatosTotales;

        ActualizarContador();

        if (panelVictoria != null)
            panelVictoria.SetActive(false);

        if (barraCombustibleUI != null)
            barraCombustibleUI.SetActive(false);

        StartCoroutine(EsperarYAsignarTimer());
        if (arma2AmmoUI != null)
            arma2AmmoUI.SetActive(false);

        if (arma3AmmoUI != null)
            arma3AmmoUI.SetActive(false);
    }

    private IEnumerator EsperarYAsignarTimer()
    {
        yield return new WaitForSeconds(0.2f);
        timer = FindObjectOfType<Timer>();
    }

    public void GatoLimpio()
    {
        gatosRestantes--;
        gatosLimpiosActual++;
        ActualizarContador();

        if (!enPausa && gatosLimpiosActual % gatosPorPausa == 0)
            StartCoroutine(PausarTimerTemporalmente());

        // LOGRO GENERAL
        if (gatosLimpiosActual >= limiteGatosLogro)
            DesbloquearLogroContador();

        // JETPACK
        if (!jetpackDesbloqueado && gatosLimpiosActual >= gatosParaJetpack)
            DesbloquearJetpack();

        // PATINES
        if (!patinesDesbloqueados && gatosLimpiosActual >= gatosParaPatines)
            DesbloquearPatines();

        // ------------------------------------------------------------
        //             DESBLOQUEO DE ARMAS AQUI
        // ------------------------------------------------------------
        if (!arma2Desbloqueada && gatosLimpiosActual >= gatosParaArma2)
        {
            arma2Desbloqueada = true;
            Debug.Log("Arma 2 desbloqueada!");

            if (arma2AmmoUI != null)
                arma2AmmoUI.SetActive(true);

            if (arma2PopupAnimator != null)
                arma2PopupAnimator.SetTrigger("Mostrar");
        }

        if (!arma3Desbloqueada && gatosLimpiosActual >= gatosParaArma3)
        {
            arma3Desbloqueada = true;
            Debug.Log("Arma 3 desbloqueada!");

            if (arma3AmmoUI != null)
                arma3AmmoUI.SetActive(true);

            if (arma3PopupAnimator != null)
                arma3PopupAnimator.SetTrigger("Mostrar");
        }        
    }

    private IEnumerator PausarTimerTemporalmente()
    {
        enPausa = true;

        if (timer != null)
        {
            timer.Pausar(true);
            timer.CambiarColor(Color.green);
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

    public void DesbloquearJetpack()
    {
        jetpackDesbloqueado = true;
        barraCombustibleUI.SetActive(true);

        if (jetpack != null)
            jetpack.SetJetpackState(true);

        Debug.Log("Jetpack Unlocked!");

        if (jetpackPopupAnimator != null)
        {
            jetpackPopupAnimator.SetTrigger("Mostrar");
            Invoke(nameof(OcultarPopupJetpack), 3f);
        }

        if (!canvasVisible && canvasLogros != null)
        {
            textoLogros.text = "Jetpack Unlocked!";
            canvasLogros.SetActive(true);
            canvasVisible = true;
        }
    }

    private void OcultarPopupJetpack()
    {
        if (jetpackPopupAnimator != null)
            jetpackPopupAnimator.SetTrigger("Ocultar");
    }

    public void DesbloquearPatines()
    {
        patinesDesbloqueados = true;

        if (iconoPatinesUI != null)
            iconoPatinesUI.SetActive(true);

        if (patinesPopupAnimator != null)
            patinesPopupAnimator.SetTrigger("Mostrar");

        Invoke(nameof(OcultarPopupPatines), 3f);

        if (canvasLogros != null)
        {
            textoLogros.text = "Roller Skates Unlocked!";
            canvasLogros.SetActive(true);
            canvasVisible = true;

            Invoke(nameof(OcultarCanvasLogros), 5f);
        }
    }

    private void OcultarPopupPatines()
    {
        if (patinesPopupAnimator != null)
            patinesPopupAnimator.SetTrigger("Ocultar");
    }

    private void OcultarCanvasLogros()
    {
        if (canvasLogros != null)
        {
            canvasLogros.SetActive(false);
            canvasVisible = false;
        }
    }

    public void MostrarPopupTimer()
    {
        if (timerPopupAnimator != null)
        {
            timerPopupAnimator.SetTrigger("Mostrar");
            Invoke(nameof(OcultarPopupTimer), 3f);
        }
    }

    private void OcultarPopupTimer()
    {
        if (timerPopupAnimator != null)
            timerPopupAnimator.SetTrigger("Ocultar");
    }
}
