using System;
using UnityEngine;

/// <summary>
/// Détecte si le joueur tombe dans le vide
/// </summary>
public class DetecteurVide : MonoBehaviour
{
    /// <summary>
    /// Instance unique du détecteur de vide
    /// </summary>
    public static DetecteurVide Instance { get; private set; }

    /// <summary>
    /// Évenement qui s'enclenche lorsque le joueur tombe dans le vide
    /// </summary>
    public event Action OnTomberVide;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Joueur>())
        {
            OnTomberVide?.Invoke();
        }
    }
}
