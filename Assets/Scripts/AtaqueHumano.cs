using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AtaqueHumano : MonoBehaviour
{
    public const string TextRobot = "Vida Robot: ";

    public Transform Robot;
    public static Animator Animator;
    public Text VidaRobot;
    public Text VidaHumano;
    public AudioSource audioSource;
    public AudioClip audioclipPunch;
    public AudioClip audioclipKick;
    private System.Random random = new System.Random();

    static int HPRobot;
    // Start is called before the first frame update
    void Start()
    {
        HPRobot = 50;
        Animator = GetComponent<Animator>();
        VidaRobot.text = TextRobot + HPRobot;
    }

    // Update is called once per frame
    void Update()
    {

        if (HPRobot <= 0)
        {
            KillRobot();
        }
        VidaRobot.text = TextRobot + HPRobot;
        transform.LookAt(Robot);
        Vector3 direction = Robot.transform.position - this.transform.position;
        
        
    }

    public void Punch()
    {

        if (Vector3.Distance(Robot.transform.position, this.transform.position) < 1.5 &&
          !Animator.GetCurrentAnimatorStateInfo(0).IsName("Punching")
          &&
          !Animator.GetCurrentAnimatorStateInfo(0).IsName("Kicking")
          &&
          !Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned") &&
          !AtaqueRobot.Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            StartCoroutine(DoAnimation("Punching",Animator));

        }
        
    }
    public void KillRobot()
    {
        StartCoroutine(DoAnimation("Stunned", AtaqueRobot.Animator));
    }

    public void Kick()
    {
        if (Vector3.Distance(Robot.transform.position, this.transform.position) < 1.5 &&
           !Animator.GetCurrentAnimatorStateInfo(0).IsName("Punching")
           &&
           !Animator.GetCurrentAnimatorStateInfo(0).IsName("Kicking")
           &&
           !Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned") &&
           !AtaqueRobot.Animator.GetCurrentAnimatorStateInfo(0).IsName("Stunned"))
        {
            StartCoroutine(DoAnimation("Kicking",Animator));
        }
    }
    IEnumerator DoAnimation(string name,Animator animator)
    {
        animator.Play(name);
        float time = animator.runtimeAnimatorController.animationClips.First(x => x.name == name).length;
        int daño = random.Next(3, 5);
        switch (name)
        {
            case "Punching":
                
                yield return new WaitForSeconds(time/2.0f); //La vida se decrementa a la mitad de la animación
                audioSource.PlayOneShot(audioclipPunch);
                HPRobot -= daño;
                break;
            case "Kicking":
                yield return new WaitForSeconds(time / 2.0f); //La vida se decrementa a la mitad de la animación
                daño = random.Next(3, 7);
                audioSource.PlayOneShot(audioclipKick);
                HPRobot -= daño;
                break;
            case "Stunned":
                yield return new WaitForSeconds(time/2.0f); //Se cambia de escena cuando está cayendo
                SceneManager.LoadScene("GanaHumano");
                break;

        }
        if (HPRobot < 0)
            HPRobot = 0;

        
       
    }
}
