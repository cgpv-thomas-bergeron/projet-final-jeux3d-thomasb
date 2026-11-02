using System.Collections;
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

    /// <summary>
    /// Vitesse d'accélération du joueur
    /// </summary>
    [SerializeField, Tooltip("Vitesse d'accélération")]
    private float acceleration = 10;

    /// <summary>
    /// Vitesse maximal du joueur en mètre/seconde
    /// </summary>
    [SerializeField, Tooltip("Vitesse max")]
    private float vitesseMax = 5;

    /// <summary>
    /// Direction du mouvement selon les entrés du joueur
    /// </summary>
    private Vector2 directionMouvement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controles = GetComponent<PlayerInput>();
    }

    void Start()
    {
        controles.actions.FindAction("Deplacer").performed += CommencerDeplacement;
        controles.actions.FindAction("Deplacer").canceled += ArreterDeplacement;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Deplacement();
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
    }

    /// <summary>
    /// Applique la force de déplacement sur le joueur
    /// </summary>
    private void Deplacement()
    {
        rb.AddRelativeForce(directionMouvement.x * acceleration, 0f, directionMouvement.y * acceleration);

        // Limite la vitesse du joueur en s'assurant que la longueur du vecteur de vélocité ne dépasse pas la vitesse maximal
        if (rb.linearVelocity.magnitude >= vitesseMax)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * vitesseMax;
        }
    }
}
