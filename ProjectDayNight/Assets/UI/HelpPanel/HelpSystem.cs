using UnityEngine;

public class HelpSystem : MonoBehaviour
{
    public GameObject[] pages;
    private int currentPage = 0;

    void Start()
    {
        ShowPage(0);
    }

    public void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
        currentPage = index;
    }

    public void NextPage()
    {
        int next = (currentPage + 1) % pages.Length;
        ShowPage(next);
    }

    public void PrevPage()
    {
        int prev = (currentPage - 1 + pages.Length) % pages.Length;
        ShowPage(prev);
    }
}

