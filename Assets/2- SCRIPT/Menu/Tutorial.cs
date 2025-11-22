using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private GameObject tuto;
    [SerializeField] private GameObject[] slides;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject nextButton;
    private int indiceActual = 0;

    void Start()
    {
        StartCoroutine(StartTuto());
        UpdateSlide(0);
    }

    public void NextSlide()
    {
        if (indiceActual < slides.Length - 1)
        {
            indiceActual++;
            UpdateSlide(indiceActual);
        }
    }

    public void BackSlide()
    {
        if (indiceActual > 0)
        {
            indiceActual--;
            UpdateSlide(indiceActual);
        }
    }

    void UpdateSlide(int indice)
    {
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive(i == indice);
        }

        backButton.SetActive(indiceActual > 0);
        nextButton.SetActive(indiceActual < slides.Length - 1);
    }

    IEnumerator StartTuto()
    {
        tuto.SetActive(false);
        yield return new WaitForSeconds(1);
        tuto.SetActive(true);
    }
}
