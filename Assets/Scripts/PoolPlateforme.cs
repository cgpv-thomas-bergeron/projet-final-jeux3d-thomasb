using System.Collections.Generic;
using UnityEngine;

// Inspiré de https://cours-alexandre-ouellet.github.io/jeux-3d/abc/ameliorations-code-tour-1/

/// <summary>
/// Pool des plateforme
/// </summary>
public class PoolPlateforme : MonoBehaviour
{
    /// <summary>
    /// Instance Unique du pool de plateforme
    /// </summary>
    public static PoolPlateforme Instance { get; private set; }

    /// <summary>
    /// Liste des plateformes disponibles pour être utilisées
    /// </summary>
    private List<Plateforme> plateformesDisponibles;

    [SerializeField, Tooltip("Prefab de la plateforme à générer")]
    private Plateforme prefabPlateforme;

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

        plateformesDisponibles = new List<Plateforme>();
    }

    /// <summary>
    /// Retourne une plateforme dans la liste des plateformes disponibles si il y en a, sinon retourne une nouvelle platforme
    /// </summary>
    /// <returns>Une plateforme</returns>
    public Plateforme GetPlateforme() 
    {
        if (plateformesDisponibles.Count > 0)
        {
            Plateforme plateforme = plateformesDisponibles[0];
            plateformesDisponibles.RemoveAt(0);
            plateforme.gameObject.SetActive(true);
            return plateforme;
        }
        else
        {
            Plateforme plateforme = Instantiate(prefabPlateforme, transform);
            return plateforme;
        }
    }
    
    /// <summary>
    /// Récupère une plateforme en l'ajoutant à la liste des plateformes disponibles
    /// </summary>
    /// <param name="plateforme">Plateforme à récuperer</param>
    public void RecupererPlateforme(Plateforme plateforme)
    {
        plateforme.gameObject.SetActive(false);
        plateformesDisponibles.Add(plateforme);
    }
}
