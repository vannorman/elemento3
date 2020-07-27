using Elemento;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator anim;
    public AICharacter aiCharacter;
    public bool oneShot = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool triggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<RigidbodyFollow>())
        {
            if (oneShot && triggered)
            {
                return;
            }
            triggered = true;
        
            anim.SetTrigger("TurnAndPoint");

            var sayAfterSeconds = 1.5f;
            Invoke("Say", sayAfterSeconds);
            
            var lookRightElapsedTime = 2f;
            Invoke("EnterAttackMode", lookRightElapsedTime);
            
        }
    }

    void EnterAttackMode()
    {
        //aiCharacter.AttackPlayer();
    }

    void Say()
    {
        aiCharacter.Say(aiCharacter.prisonerEscaped);
        aiCharacter.LookAtPlayer();
    }
}
