using UnityEngine;

/// <summary>
/// Classe qui gère les plateformes dans le jeu
/// </summary>
public class GestionnairePlateforme : MonoBehaviour
{
    /// <summary>
    /// Instance unique du gestionnaire de plateforme
    /// </summary>
    public static GestionnairePlateforme Instance { get; private set; }

    [SerializeField, Tooltip("Distance maximal à laquelle une plateforme peut être comparativement à la plateforme actuelle")]
    private float distanceMax;

    [SerializeField, Tooltip("Distance minimale à laquelle une plateforme peut être comparativement à la plateforme actuelle ")]
    private float distanceMin;

    /// <summary>
    /// Plateforme sur laquelle le joueur doit sauter
    /// </summary>
    private Plateforme plateformeActuelle;

    /// <summary>
    /// Plateforme sur laquelle le joueur est
    /// </summary>
    private Plateforme plateformePrecedente;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Initialiser();
    }

    /// <summary>
    /// Ajoute une platforme et supprime la précédente 
    /// </summary>
    public void AjouterPlateforme()
    {
        // Direction dans laquelle la prochaine plateforme va être générée
        Vector2 directionAleatoire = Random.insideUnitCircle.normalized;
        // Distance à laquelle la prochaine plateforme va être généré
        float distanceAleatoire = Random.Range(distanceMin, distanceMax);
        // Position de la prochaine plateforme
        Vector3 nouvellePosition = plateformeActuelle.transform.position + new Vector3(directionAleatoire.x * distanceAleatoire, 1, directionAleatoire.y * distanceAleatoire);

        Plateforme nouvellePlateforme = PoolPlateforme.Instance.GetPlateforme();

        // Supression de la plateforme précédente
        if (plateformePrecedente != null)
        {
            plateformePrecedente.collisionJoueur -= AjouterPlateforme;
            PoolPlateforme.Instance.RecupererPlateforme(plateformePrecedente);
        }

        plateformePrecedente = plateformeActuelle;

        nouvellePlateforme.transform.position = nouvellePosition;
        nouvellePlateforme.MettreContactFalse();
        nouvellePlateforme.collisionJoueur += AjouterPlateforme;
        plateformeActuelle = nouvellePlateforme;
    }

    /// <summary>
    /// Génération de la première plateforme au début du jeu
    /// </summary>
    private void Initialiser()
    {
        Plateforme nouvellePlateforme = PoolPlateforme.Instance.GetPlateforme();
        nouvellePlateforme.transform.position = new Vector3(0, 0, 0);
        nouvellePlateforme.collisionJoueur += AjouterPlateforme;
        plateformeActuelle = nouvellePlateforme;
    }
}
