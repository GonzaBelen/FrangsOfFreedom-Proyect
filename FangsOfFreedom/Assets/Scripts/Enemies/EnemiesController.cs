using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    private TakeDamage takeDamage;
    private AnimationController animationController;
    //[SerializeField] private AudioSource clip;

    private void Start()
    {
        takeDamage = GetComponent<TakeDamage>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            takeDamage.BeginRespawn();
        }
    }

    public void TakeDamage()
    {
        animationController.ChangeAnimation("Death");
        //clip.Play();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemie"), true);
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    // private IEnumerator DestruirDespuesDeAnimacion()
    // {
    //     // Esperar un frame para que la animación comience
    //     yield return null;

    //     // Esperar hasta que la animación termine
    //     while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
    //     {
    //         yield return null;
    //     }

    //     // Obtener la posición actual del transform
    //     Vector3 posicionActual = transform.position;

    //     // Ajustar la posición en el eje Y
    //     float offsetY = 2f; // Puedes ajustar este valor según tus necesidades
    //     Vector3 nuevaPosicion = new Vector3(posicionActual.x, posicionActual.y + offsetY, posicionActual.z);

    //     // Instanciar la sangre con la nueva posición
    //     Instantiate(sangrePrefab, nuevaPosicion, Quaternion.identity);

    //     // Destruir el objeto
    //     Destroy(gameObject);
    // }
}
