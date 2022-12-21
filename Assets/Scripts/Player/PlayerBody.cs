using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isProcessingFood = false;
    public bool isCorner = false;

    [SerializeField]
    private Sprite bodySprite;
    [SerializeField]
    private Sprite tailSprite;
    [SerializeField]
    private Sprite cornerSprite;
    [SerializeField]
    private Sprite eatingBodySprite;
    [SerializeField]
    private Sprite eatingCornerSprite;
    [SerializeField]
    private Sprite eatingTailSprite;

    private Vector2 moveDirection = Vector2.up;
    private Vector2 prevMoveDirection = Vector2.up;
    private int index;

    private Playground playground;
    private PlayerController playerController;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playground = FindObjectOfType<Playground>();
        playerController = transform.parent.GetComponent<PlayerController>();
        index = transform.GetSiblingIndex();
    }

    public int GetIndex() => index;

    public Vector2 GetDirection() => moveDirection;

    public Vector2 GetPrevDirection() => prevMoveDirection;

    public void SetDirection(Vector2 v) => moveDirection = v;

    public void Move(float moveRange) {
        prevMoveDirection = moveDirection;
        transform.position += (Vector3)moveDirection * moveRange;
    }

    public void MoveBodyOutsideBoundary() {
        if (Utils.InsideBoundary(transform.position, playground)) return;
        if (!playerController.enabled) return;

        if (prevMoveDirection == Vector2.left || prevMoveDirection == Vector2.right) {
            float xPos = Mathf.Sign(transform.position.x) * -(Mathf.Abs(transform.position.x) - playground.marginStep);
            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        } else if (prevMoveDirection == Vector2.up || prevMoveDirection == Vector2.down) {
            float yPos = Mathf.Sign(transform.position.y) * -(Mathf.Abs(transform.position.y) - playground.marginStep);
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
        }
    }

    public void StartFoodProcess() => isProcessingFood = true;

    public void FoodProcess() {
        if (index == 0) return;

        PlayerBody prevBody = transform.parent.GetChild(index - 1).GetComponent<PlayerBody>();
        if (prevBody.isProcessingFood) {
            prevBody.isProcessingFood = false;
            if (prevBody.index > 0)
                prevBody.spriteRenderer.sprite = prevBody.isCorner ? prevBody.cornerSprite : prevBody.bodySprite;

            isProcessingFood = true;
            
            if (index == transform.parent.childCount - 1)
                spriteRenderer.sprite = eatingTailSprite;
            else
                spriteRenderer.sprite = isCorner ? eatingCornerSprite : eatingBodySprite;

            return;
        }

        if (isProcessingFood) {
            isProcessingFood = false;
            spriteRenderer.sprite = isCorner ? cornerSprite : bodySprite;
            PlayerBody newBody = transform.parent.GetComponent<BodySpawner>().GrowTail();
            newBody.spriteRenderer.sprite = newBody.tailSprite;
        }
    }

    public void ChangeBodySprite() {
        if (index == 0) {
            ChangeHeadDirection();
            return;
        }

        if (index == transform.parent.childCount - 1) {
            ChangeTailDirection();
            return;
        }

        ChangeCornerDirection();        
    }

    public void ChangeTailDirection() {
        Vector3 prevBodyDir = (transform.position - transform.parent.GetChild(index - 1).position);
        float flipTail = 0f;

        if (Mathf.Abs(prevBodyDir.x) > 1 || Mathf.Abs(prevBodyDir.y) > 1)
            flipTail = 180f;

        if (prevBodyDir.normalized == Vector3.down)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f + flipTail);
        else if (prevBodyDir.normalized == Vector3.left)
            transform.rotation = Quaternion.Euler(0f, 0f, -90f + flipTail);
        else if (prevBodyDir.normalized == Vector3.up)
            transform.rotation = Quaternion.Euler(0f, 0f, 180f + flipTail);
        else if (prevBodyDir.normalized == Vector3.right)
            transform.rotation = Quaternion.Euler(0f, 0f, 90f + flipTail);
    }

    private void ChangeHeadDirection() {
        if (moveDirection == Vector2.up) {
            transform.rotation = Quaternion.identity;
        } else if (moveDirection == Vector2.left) {
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        } else if (moveDirection == Vector2.down) {
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        } else if (moveDirection == Vector2.right) {
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
        }
    }

    private void ChangeCornerDirection() {
        isCorner = false;
        Vector3 prevBodyPos = transform.position - transform.parent.GetChild(index - 1).position;
        Vector3 nextBodyPos = transform.position - transform.parent.GetChild(index + 1).position;

        if (
            (int)prevBodyPos.normalized.x == 0 && (int)nextBodyPos.normalized.x == 0
        ) {
            spriteRenderer.sprite = bodySprite;
            transform.rotation = Quaternion.identity;
            return;
        } 
        else if (
            (int)prevBodyPos.normalized.y == 0 && (int)nextBodyPos.normalized.y == 0
        ) {
            spriteRenderer.sprite = bodySprite;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            return;
        }
        else if (
            (prevBodyPos.normalized == Vector3.up && nextBodyPos.normalized == Vector3.right) || 
            (prevBodyPos.normalized == Vector3.right && nextBodyPos.normalized == Vector3.up)
        ) {
            spriteRenderer.sprite = cornerSprite;
            CheckBodyPosition(prevBodyPos, nextBodyPos, "upright");
            return;
        } 
        else if (
            (prevBodyPos.normalized == Vector3.right && nextBodyPos.normalized == Vector3.down) ||
            (prevBodyPos.normalized == Vector3.down && nextBodyPos.normalized == Vector3.right)
        ) {
            spriteRenderer.sprite = cornerSprite;
            CheckBodyPosition(prevBodyPos, nextBodyPos, "downright");
            return;
        } 
        else if (
            (prevBodyPos.normalized == Vector3.down && nextBodyPos.normalized == Vector3.left) ||
            (prevBodyPos.normalized == Vector3.left && nextBodyPos.normalized == Vector3.down)
        ) {
            spriteRenderer.sprite = cornerSprite;
            CheckBodyPosition(prevBodyPos, nextBodyPos, "downleft");
            return;
        } 
        else if (
            (prevBodyPos.normalized == Vector3.left && nextBodyPos.normalized == Vector3.up) ||
            (prevBodyPos.normalized == Vector3.up && nextBodyPos.normalized == Vector3.left)
        ) {
            spriteRenderer.sprite = cornerSprite;
            CheckBodyPosition(prevBodyPos, nextBodyPos, "upleft");
            return;
        }
    }

    private void CheckBodyPosition(Vector3 prevBodyPos, Vector3 nextBodyPos, string cornerDir) {
        isCorner = true;

        if (
            (prevBodyPos.x < -1 && nextBodyPos.y < -1) ||
            (nextBodyPos.x < -1 && prevBodyPos.y < -1)
        ) {
            CornerRotation("upright");
        } 
        else if (
            (prevBodyPos.x > 1 && nextBodyPos.y < -1) ||
            (nextBodyPos.x > 1 && prevBodyPos.y < -1)
        ) {
            CornerRotation("upleft");
        }
        else if (
            (prevBodyPos.x < -1 && nextBodyPos.y > 1) ||
            (nextBodyPos.x < -1 && prevBodyPos.y > 1)
        ) {
            CornerRotation("downright");
        }
        else if (
            (prevBodyPos.x > 1 && nextBodyPos.y > 1) ||
            (nextBodyPos.x > 1 && prevBodyPos.y > 1)
        ) {
            CornerRotation("downleft");
        }
        else if (prevBodyPos.y > 1 || nextBodyPos.y > 1) {
            if (cornerDir.Contains("left")) CornerRotation("downleft");
            else if (cornerDir.Contains("right")) CornerRotation("downright");
        } else if (prevBodyPos.y < -1 || nextBodyPos.y < -1) {
            if (cornerDir.Contains("left")) CornerRotation("upleft");
            else if (cornerDir.Contains("right")) CornerRotation("upright");
        }
        else if (prevBodyPos.x > 1 || nextBodyPos.x > 1) {
            if (cornerDir.Contains("up")) CornerRotation("upleft");
            else if (cornerDir.Contains("down")) CornerRotation("downleft");
        } 
        else if (prevBodyPos.x < -1 || nextBodyPos.x < -1) {
            if (cornerDir.Contains("up")) CornerRotation("upright");
            else if (cornerDir.Contains("down")) CornerRotation("downright");
        } 
        else {
            CornerRotation(cornerDir);
        }
    }

    private void CornerRotation(string cornerDir) {
        switch (cornerDir) {
            case "upright":
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case "downright":
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case "downleft":
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case "upleft":
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            default:
                Debug.LogError("Check your string value!");
                break;
        }
    }
}
