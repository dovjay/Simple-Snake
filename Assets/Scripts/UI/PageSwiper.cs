using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PageSwiper : MonoBehaviour
{
    public Scrollbar scrollbar;

    TouchControl touchControl;

    float scroll_pos = 0;
    float[] pos;

    private void Awake() {
        touchControl = new TouchControl();
    }

    // THIS CODE NEED MORE REFINEMENTS
    private void UpdateScroll(InputAction.CallbackContext context, bool action) {
        print(action);
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);

        for (int i = 0; i < pos.Length; i++) {
            pos[i] = distance * i;
        }

        if (action) {
            scroll_pos = scrollbar.value;
        } else {
            for (int i = 0; i < pos.Length; i++) {
                if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2)) {
                    scrollbar.value = Mathf.Lerp(scrollbar.value, pos[i], .1f);
                }
            }
        }
    }
}
