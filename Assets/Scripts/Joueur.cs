using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Personnage que le joueur contrôle
/// </summary>
[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class Joueur : MonoBehaviour
{
    /// <summary>
    /// Rigidbody du joueur
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Controles du joueur
    /// </summary>
    private PlayerInput controles;

    [SerializeField, Tooltip("Vitesse d'accélération du joueur")]
    private float acceleration = 10;


    [SerializeField, Tooltip("Vitesse maximal du joueur en mètre/seconde")]
    private float vitesseMax = 5;

    /// <summary>
    /// Direction du mouvement selon les entrés du joueur
    /// </summary>
    private Vector2 directionMouvement;

    [SerializeField, Tooltip("Puissance du saut du joueur")]
    private float puissanceSaut = 20;

    /// <summary>
    /// Mouvement de la souris
    /// </summary>
    private Vector2 mouvementSouris;

    /// <summary>
    /// Rotation à effectuer sur l'axe des x
    /// </summary>
    private float rotationX;

    /// <summary>
    /// Rotation à effectuer sur l'axe des y
    /// </summary>
    private float rotationY;

    [SerializeField, Tooltip("Sensiblilité du mouvement de la caméra")]
    private float sensibilite;

    [SerializeField, Tooltip("Transform de la camera du joueur")]
    private Transform camera;

    [SerializeField, Tooltip("Masque du sol")]
    private LayerMask sol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controles = GetComponent<PlayerInput>();
    }

    void Start()
    {
        controles.actions.FindAction("Deplacer").performed += CommencerDeplacement;
        controles.actions.FindAction("Deplacer").canceled += ArreterDeplacement;
        controles.actions.FindAction("Sauter").performed += Sauter;
        controles.actions.FindAction("Regarder").performed += CommencerRegarder;
        controles.actions.FindAction("Regarder").canceled += ArreterRegarder;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RegarderHorizontal();
    }

    private void FixedUpdate()
    {
        Deplacement();
        RegarderVertical();
        Debug.DrawRay(transform.position, Vector3.down * 0.2f, Color.red);

    }

    /// <summary>
    /// Lit le input du joueur pour savoir dans quelle direction se déplacer
    /// </summary>
    /// <param name="context">Entrés du joueur</param>
    public void CommencerDeplacement(InputAction.CallbackContext context)
    {
        directionMouvement = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Réinitialise le déplacement du joueur à 0 afin de l'arrêter
    /// </summary>
    /// <param name="context">Entrés du joueur</param>
    public void ArreterDeplacement(InputAction.CallbackContext context)
    {
        directionMouvement = Vector2.zero;
        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    /// <summary>
    /// Prend les inputs du joueur losrqu'il bouge la souris
    /// </summary>
    /// <param name="contexte"></param>
    public void CommencerRegarder(InputAction.CallbackContext contexte)
    {
        mouvementSouris = contexte.ReadValue<Vector2>();
    }

    /// <summary>
    /// Remet les mouvements de la souris à zéro lorsqu'elle ne bouge plus
    /// </summary>
    /// <param name="contexte"></param>
    public void ArreterRegarder(InputAction.CallbackContext contexte)
    {
        mouvementSouris = Vector2.zero;
    }

    /// <summary>
    /// Rotation du RigidBody sur l'axe des y
    /// </summary>
    private void RegarderHorizontal()
    {
        float sourisX = mouvementSouris.x * sensibilite * Time.deltaTime;

        rotationY += sourisX;

        rb.MoveRotation(Quaternion.Euler(0, rotationY, 0));
    }

    /// <summary>
    /// Rotation de la caméra sur l'axe des x
    /// </summary>
    private void RegarderVertical()
    {
        float sourisY = mouvementSouris.y * sensibilite * Time.deltaTime;

        rotationX -= sourisY;
        rotationX = Mathf.Clamp(rotationX, -90, 90);

        camera.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    /// <summary>
    /// Applique la force de déplacement sur le joueur
    /// </summary>
    private void Deplacement()
    {
        rb.AddRelativeForce(directionMouvement.x * acceleration, 0f, directionMouvement.y * acceleration);

        Vector3 vitesseHorizontale = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        // Limite la vitesse du joueur en s'assurant que la longueur du vecteur de vélocité ne dépasse pas la vitesse maximal
        if (vitesseHorizontale.magnitude >= vitesseMax)
        {
            vitesseHorizontale = vitesseHorizontale.normalized * vitesseMax;
            rb.linearVelocity = new Vector3(vitesseHorizontale.x, rb.linearVelocity.y, vitesseHorizontale.z);
        }
    }

    /// <summary>
    /// Applique une force sur le personnage qui le fait sauter
    /// </summary>
    /// <param name="contexte">Entrés du joueur</param>
    public void Sauter(InputAction.CallbackContext contexte)
    {
        if (EnSaut()) return;
        rb.AddForce(Vector3.up * puissanceSaut, ForceMode.Impulse);
    }


    /// <summary>
    /// Vérifie si le joueur est n'est pas en contact avec le sol
    /// </summary>
    /// <returns>Vrai s'il n'est pas en contact avec le sol</returns>
    private bool EnSaut()
    {
        if (Physics.BoxCast(transform.position + Vector3.up * 0.4f, new Vector3(0.5f, 0.2f, 0.5f), Vector3.down, transform.rotation, 0.2f, sol))
        {
            Debug.Log("Au sol");
            return false;
        }

        return true;
    }
}
