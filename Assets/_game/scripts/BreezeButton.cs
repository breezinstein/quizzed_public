using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class BreezeButton : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(Click(eventData));
    }

    IEnumerator Click(PointerEventData eventData)
    {
        if (!interactable)
        {
          yield return null;
        }
        else
        {
        if (targetGraphic != null)
        {
            targetGraphic.transform.DOPunchScale(new Vector3(-0.5f, 0f, 0f), 0.2f, vibrato: 2).OnComplete(ResetScale);
        }
        else
        {
            transform.DOPunchScale(new Vector3(-0.5f, 0f, 0f), 0.2f, vibrato: 2).OnComplete(ResetScale);
        }
        yield return new WaitForSeconds(0.1f);
        base.OnPointerClick(eventData);
        }
    }

    void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
}
