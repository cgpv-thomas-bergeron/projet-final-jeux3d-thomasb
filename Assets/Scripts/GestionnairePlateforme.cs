using System;
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

    /// <summary>
    /// Évenement qui s'encleche lorsque le joueur touche à une plateforme 
    /// </summary>
    public event Action OnPlateformeTouchee;

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
        DetecteurVide.Instance.OnTomberVide += Reinitialiser;
    }

    /// <summary>
    /// Ajoute une platforme et supprime la précédente 
    /// </summary>
    public void AjouterPlateforme()
    {
        // Direction dans laquelle la prochaine plateforme va être générée
        Vector2 directionAleatoire = UnityEngine.Random.insideUnitCircle.normalized;
        // Distance à laquelle la prochaine plateforme va être généré
        float distanceAleatoire = UnityEngine.Random.Range(distanceMin, distanceMax);
        // Position de la prochaine plateforme
        Vector3 nouvellePosition = plateformeActuelle.transform.position + new Vector3(directionAleatoire.x * distanceAleatoire, 1, directionAleatoire.y * distanceAleatoire);

        Plateforme nouvellePlateforme = PoolPlateforme.Instance.GetPlateforme();

        // Supression de la plateforme précédente
        if (plateformePrecedente != null)
        {
            SupprimerPlateforme(plateformePrecedente);
        }

        plateformePrecedente = plateformeActuelle;

        DetecteurVide.Instance.transform.position = plateformePrecedente.transform.position - new Vector3(0, 2, 0);

        nouvellePlateforme.transform.position = nouvellePosition;
        nouvellePlateforme.MettreContactFalse();
        nouvellePlateforme.OnCollisionJoueur += AjouterPlateforme;
        nouvellePlateforme.OnCollisionJoueur += RelayerContact;
        plateformeActuelle = nouvellePlateforme;
    }

    /// <summary>
    /// Génération de la première plateforme au début du jeu
    /// </summary>
    private void Initialiser()
    {
        Plateforme nouvellePlateforme = PoolPlateforme.Instance.GetPlateforme();
        nouvellePlateforme.transform.position = new Vector3(0, 0, 0);
        nouvellePlateforme.OnCollisionJoueur += AjouterPlateforme;
        plateformeActuelle = nouvellePlateforme;
    }

    /// <summary>
    /// Relaye l'évenement à celui du gestionnaire de plateforme
    /// </summary>
    private void RelayerContact()
    {
        OnPlateformeTouchee?.Invoke();
    }

    /// <summary>
    /// Réinitialise les plateformes pour recommencer
    /// </summary>
    private void Reinitialiser()
    {
        SupprimerPlateforme(plateformeActuelle);
        SupprimerPlateforme(plateformePrecedente);
        plateformeActuelle = null;
        plateformePrecedente = null;

        DetecteurVide.Instance.transform.position = new Vector3(0, -5, 0);

        Initialiser();
    }

    /// <summary>
    /// Supprime la plateforme du monde en désabonnant les évenements
    /// </summary>
    /// <param name="plateforme">La plateforme à supprimer</param>
    private void SupprimerPlateforme(Plateforme plateforme)
    {
        if (plateforme == null) return;

        plateforme.OnCollisionJoueur -= AjouterPlateforme;
        plateforme.OnCollisionJoueur -= RelayerContact;
        PoolPlateforme.Instance.RecupererPlateforme(plateforme);    
    }
}
