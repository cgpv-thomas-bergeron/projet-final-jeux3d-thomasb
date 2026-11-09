using System;
using UnityEngine;

/// <summary>
/// Plateforme sur laquelle le joueur doit sauter
/// </summary>
public class Plateforme : MonoBehaviour
{
    /// <summary>
    /// Détermine si le joueur est déjà entré en contact avec la plateforme
    /// </summary>
    private bool contact = false;

    /// <summary>
    /// Évenement lorsque le joueur entre en contact avec la plateforme
    /// </summary>
    public event Action collisionJoueur;

    /// <summary>
    /// Gère les collision avec la plateforme
    /// </summary>
    /// <param name="collision">Description de la collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Joueur joueur) && !contact)
        {
            collisionJoueur?.Invoke();
            contact = true;
        }
    }

    /// <summary>
    /// Réinitialise le contact à false, car les plateformes sont récupérés
    /// </summary>
    public void MettreContactFalse()
    {
        contact = false;
    }
}
