using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BloodEffect : MonoBehaviour {

    public Image[] blood;

    void Start()
    {
        foreach (var image in blood)
        {
            image.gameObject.SetActive(false);
        }
    }

    public void ActivateBloodSpatter () {

        foreach (var image in blood)
        {
            if (!image.gameObject.activeSelf)
            {
                image.gameObject.SetActive(true);

                StartCoroutine(DeactivateBloodSpatter(image));

                break;
            }
        }

    }

    IEnumerator DeactivateBloodSpatter(Image image)
    {

        yield return new WaitForSeconds(10);

        image.gameObject.SetActive(false);
    }
}
