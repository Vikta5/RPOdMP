using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Learn : MonoBehaviour
{
    public GameObject arrow;
    public Text text;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = arrow.GetComponent<Animator>();

        if (PlayerPrefs.GetInt("FirstStart") != 1)
        {
            PlayerPrefs.SetInt("FirstStart", 1);
            
            text.text = "Нажмите здесь, чтобы добавить заметку";
        }

        else
        {
            arrow.SetActive(false);
            text.gameObject.SetActive(false);
            enabled = false;
            return;
        }
    }

    IEnumerator LearnEnd()
    {
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        text.gameObject.SetActive(false);
    }

    public void pressAdd()
    {
        anim.SetTrigger("edit");
        text.text = "Введите текст заметки и сохраните ее";
    }

    public void pressSave()
    {
        arrow.SetActive(false);
        text.text = "Чтобы редактировать заметку, нажмите на нее. \r\nЧтобы пометить или удалить заметку, нажмите и удерживайте.";
            StartCoroutine(LearnEnd());
    }
}
