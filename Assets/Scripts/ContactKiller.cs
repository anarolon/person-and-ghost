using UnityEngine;
using System.Collections;
using PersonAndGhost.Utils;
using PersonAndGhost.Ghost;
using PersonAndGhost.Person;
using UnityEngine.InputSystem;

namespace PersonAndGhost
{
    public class ContactKiller : MonoBehaviour
    {
        private IEnumerator OnCollisionEnter2D(Collision2D collision)
        {
            GameObject gameObjectToDestroy = collision.gameObject;

            if (gameObjectToDestroy.CompareTag(Utility.LEFTPLAYERTAG))
            {
                gameObjectToDestroy.GetComponent<PersonMovement>().isDead = true;
                gameObjectToDestroy.GetComponent<PlayerInput>().DeactivateInput();
                // Maybe wait until death anim is over
                yield return new WaitForSeconds(1);
                Destroy(gameObjectToDestroy.transform.parent.gameObject);

                Actions.OnRoomStateChange(false);
            }

            else if (gameObjectToDestroy.CompareTag(Utility.MONSTERTAG))
            {
                AIAgent monsterToDestroy = gameObjectToDestroy.GetComponent<AIAgent>();
                AIStateMachine stateMachineToCheck = monsterToDestroy.stateMachine;
                
                if (stateMachineToCheck.currentState == AIStateId.Possessed)
                {
                    GhostPossession possession = FindObjectOfType<GhostPossession>();

                    if (possession) 
                    {
                        possession.ChangePossession();

                        yield return new WaitWhile(() => possession.IsPossessing);
                    }

                    else
                    {
                        stateMachineToCheck.ChangeState(monsterToDestroy.initialState);
                    }

                    yield return new WaitWhile(() => 
                        stateMachineToCheck.currentState == AIStateId.Possessed);
                }

                Destroy(gameObjectToDestroy);

                Utility.ActionHandler(
                    Actions.Names.OnRequestAudio, Clips.EnemyDeath, this);
            } 
        }
    }
}
