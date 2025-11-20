using UnityEngine;

/// <summary>
/// Gestisce le statistiche del character. (es. <see cref="_walkSpeed"/>)<br/>
/// Per il momento funge più da Struct; ma nel caso volessimo avere un controllo più granulare sui parametri del personaggio, si può estendere la funzionalità di questa classe
/// </summary>
public class CharacterStats : MonoBehaviour
{
    // Campi statici/costanti

    // Campi pubblici/serializzati

    // Campi privati
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _jumpHeight = 4f;
    [SerializeField] private float _gravity = 9.81f;

    // Proprietà
    public float WalkSpeed => _walkSpeed;
    public float JumpHeight => _jumpHeight;
    public float Gravity => _gravity;

    // Metodi MonoBehaviour di Unity
    //  Awake
    //  OnEnable
    //  Start
    //  Update
    //  FixedUpdate
    //  LateUpdate
    //  OnDisable
    //  OnDestroy
    // Metodi pubblici
    // Metodi privati
}
