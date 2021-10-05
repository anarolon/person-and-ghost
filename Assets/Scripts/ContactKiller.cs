using UnityEngine;
using System.Collections;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;

namespace PersonAndGhost
{
    public class ContactKiller : MonoBehaviour
    {
        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            GameObject gameObjectToDestroy = collision.gameObject;

            if (gameObjectToDestroy.CompareTag(Utility.LEFTPLAYERTAG))
            {
                Destroy(gameObjectToDestroy.transform.parent.gameObject);

                Actions.OnPuzzleFail();
            }

            else if (gameObjectToDestroy.CompareTag(Utility.MONSTERTAG))
            {
                AIStateMachine stateMachineToCheck =
                    gameObjectToDestroy.GetComponent<AIAgent>().stateMachine;
                
                if (stateMachineToCheck.currentState == AIStateId.Possessed)
                {
                    FindObjectOfType<GhostPossession>().ChangePossession();

                    yield return new WaitWhile(() => 
                        stateMachineToCheck.currentState == AIStateId.Possessed);
                }

                Destroy(gameObjectToDestroy);
            } 
        }
    }
}
