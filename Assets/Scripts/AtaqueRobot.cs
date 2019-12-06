using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AtaqueRobot : MonoBehaviour
{
    public const string TextHumano = "Vida Humano: ";

    public Transform Humano;
    public static Animator Animator;
    public Text VidaHumano;
    public Text VidaRobot;
    public AudioSource audioSource;
    public AudioClip audioclip;

    static int HPHumano;
    private System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        HPHumano = 50;
        Animator = GetComponent<Animator>();
        VidaHumano.text = TextHumano + HPHumano;
    }

    // Update is called once per frame
    void Update()
    {
        if (HPHumano <= 0)
        {
            KillHuman();
        }
        VidaHumano.text = TextHumano + HPHumano;
        transform.LookAt(Humano);
        Vector3 direction = Humano.transform.position - this.transform.position;
        if (Vector3.Distance(Humano.transform.position, this.transform.position) < 1.5 &&
            !Animator.GetCurrentAnimatorStateInfo(0).IsName("Cross Punch")
             &&
            !Animator.GetCurrentAnimatorStateInfo(0).IsName("Kicking")
            &&
            !Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned")
            &&
            !AtaqueHumano.Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {

            //Para asegurarse que no ataca de continuo, hay un poco de tiempo entre animación y animación
            var attackType = random.Next(1,20);
            if (attackType > 16)
                Punch();
            
         }
        
        
    }

    public void KillHuman()
    {
        StartCoroutine(DoAnimation("Stunned", AtaqueHumano.Animator));
    }

    public void Punch()
    {
        StartCoroutine(DoAnimation("Cross Punch",Animator));
    }


    IEnumerator DoAnimation(string name,Animator animator)
    {
        animator.Play(name);
        float time = animator.runtimeAnimatorController.animationClips.First(x => x.name == name).length;
        int daño = random.Next(5, 9);
        switch (name) {
            case "Cross Punch":
                yield return new WaitForSeconds(time/2.0f); //La vida se decrementa a la mitad de la animación
                audioSource.PlayOneShot(audioclip);
                HPHumano -= daño;
                break;
            case "Stunned":
                yield return new WaitForSeconds(time/2.0f); // Se espera a que se acabe la animación
                SceneManager.LoadScene("GanaRobot"); 
                break;
        }
        if (HPHumano < 0)
            HPHumano = 0;
       
    }
}
