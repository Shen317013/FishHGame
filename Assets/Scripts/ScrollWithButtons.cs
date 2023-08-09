using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollWithButtons : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Button leftButton;
    public Button rightButton;
    public float scrollDistance = 200f;
    public float smoothTime = 0.2f;

    private bool isScrolling = false;
    private float targetPosition;

    private void Start()
    {
        // 初始状态下显示右边按钮
        ShowRightButton();
    }

    private void Update()
    {
        // 检查左边按钮的显示状态
        if (scrollRect.content.anchoredPosition.x <= 0)
        {
            HideLeftButton();
        }
        else
        {
            ShowRightButton();
        }

        // 检查右边按钮的显示状态
        if (scrollRect.content.anchoredPosition.x >= scrollRect.content.rect.width - scrollRect.viewport.rect.width)
        {
            HideRightButton();
        }
        else
        {

            ShowLeftButton();
        }
    }

    public void ScrollToLeft()
    {
        if (!isScrolling && scrollRect.content.anchoredPosition.x < 0)
        {
            targetPosition = Mathf.Min(scrollRect.content.anchoredPosition.x + scrollDistance, 0);
            StartCoroutine(ScrollToPosition(targetPosition));
        }
    }

    public void ScrollToRight()
    {
        if (!isScrolling && scrollRect.content.anchoredPosition.x > -(scrollRect.content.rect.width - scrollRect.viewport.rect.width))
        {
            targetPosition = Mathf.Max(scrollRect.content.anchoredPosition.x - scrollDistance, -(scrollRect.content.rect.width - scrollRect.viewport.rect.width));
            StartCoroutine(ScrollToPosition(targetPosition));
        }
    }

    private IEnumerator ScrollToPosition(float target)
    {
        isScrolling = true;

        Vector2 startPosition = scrollRect.content.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < smoothTime)
        {
            float newPosition = Mathf.Lerp(startPosition.x, target, elapsedTime / smoothTime);
            scrollRect.content.anchoredPosition = new Vector2(newPosition, scrollRect.content.anchoredPosition.y);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollRect.content.anchoredPosition = new Vector2(target, scrollRect.content.anchoredPosition.y);
        isScrolling = false;
    }

    private void ShowLeftButton()
    {
        leftButton.gameObject.SetActive(true);
    }

    private void HideLeftButton()
    {
        leftButton.gameObject.SetActive(false);
    }

    private void ShowRightButton()
    {
        rightButton.gameObject.SetActive(true);
    }

    private void HideRightButton()
    {
        rightButton.gameObject.SetActive(false);
    }
}
