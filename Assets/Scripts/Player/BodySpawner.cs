using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodySpawner : MonoBehaviour
{
    [SerializeField] PlayerBody bodyPrefab;
    [SerializeField] GameObject playerPrefab;

    private Playground playground;

    private void Awake() {
        playground = FindObjectOfType<Playground>();
    }

    public PlayerBody GrowTail() {
        PlayerBody lastBody = playerPrefab.transform.GetChild(playerPrefab.transform.childCount-1).GetComponent<PlayerBody>();
        PlayerController playerController = playerPrefab.GetComponent<PlayerController>();
        Vector2 spawnBodyPosition = lastBody.transform.localPosition + (-(Vector3)lastBody.GetPrevDirection() * playerController.moveRange);
        if (!Utils.InsideBoundary(spawnBodyPosition, playground)) {
            if (lastBody.GetPrevDirection() == Vector2.left || lastBody.GetPrevDirection() == Vector2.right) {
                spawnBodyPosition = new Vector3(-lastBody.transform.localPosition.x, lastBody.transform.localPosition.y, 0f);
            } else if (lastBody.GetPrevDirection() == Vector2.up || lastBody.GetPrevDirection() == Vector2.down) {
                spawnBodyPosition = new Vector3(lastBody.transform.localPosition.x, -lastBody.transform.localPosition.y, 0f);
            }
        }

        
        PlayerBody newBody = Instantiate(bodyPrefab, spawnBodyPosition, Quaternion.identity, transform);
        newBody.SetDirection(lastBody.GetPrevDirection());
        newBody.ChangeTailDirection();
        lastBody.ChangeBodySprite();
        return newBody;
    }
}
