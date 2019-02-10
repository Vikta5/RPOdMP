﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class NoteManager : MonoBehaviour
{
    //класс для глобального отслеживания состояний игры. В частности музыки. Не учничтожается при загрузке нового уровня 
    public static NoteManager Instance
    {
        get
        {
            return Instance;
        }
    }

    public static NoteManager instance = null;

    public Transform content;
    public GameObject notePrefab, deletePanel;
    public InputField headerNote, textNote, searchField;
    public GameObject noteEdit;

    List<RaycastResult> results = new List<RaycastResult>();
    List<GameObject> note = new List<GameObject>();
    Dictionary<GameObject, string> pathToNote = new Dictionary<GameObject, string>();
    GameObject noteToDeleteOrEdit;
    int counter = 0;
    float timer = 0;
    bool editNote = false;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(transform.gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        counter = PlayerPrefs.GetInt("counter");
        int i = 0;
        int index = 0;

        while (i < counter)
        {
            string path = Application.persistentDataPath + "/Note" + i + ".txt";
            FileInfo fi1 = new FileInfo(path);

            if (File.Exists(path))
            {               
                note.Add(Instantiate(notePrefab, content));
                pathToNote.Add(note[index], path);

                using (StreamReader sr = fi1.OpenText())
                {
                    string line;
                    bool split = false;
                    //и считываем построчно, заодно проверяя, есть ли внутри такой же вариант сборки, как сейчас
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!split)
                        {
                            if (line != "<EndOfHeaderStartText>")
                            {
                                note[index].transform.GetChild(0).GetComponent<Text>().text += line;
                            }

                            else
                            {
                                split = true;
                            }
                        }

                        else
                        {
                            note[index].transform.GetChild(1).GetComponent<Text>().text += line;
                        }
                    }

                    split = false;
                }

                Debug.Log("Note" + i);
                Debug.Log(PlayerPrefs.GetString("Note" + i));
                note[index].GetComponent<Image>().color = SwitchColor(PlayerPrefs.GetString("Note" + i));
                index++;
                content.GetComponent<RectTransform>().offsetMax = new Vector2(content.GetComponent<RectTransform>().offsetMax.x, content.GetComponent<RectTransform>().offsetMax.y + 53.68f);
            }

            i++;
        }

        content.GetComponent<RectTransform>().localPosition = new Vector2(content.GetComponent<RectTransform>().localPosition.x, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            results.Clear();
            GraphicRaycaster gr = this.GetComponent<GraphicRaycaster>();
            PointerEventData ped = new PointerEventData(null);
            ped.position = Input.mousePosition;
            gr.Raycast(ped, results);
        }

        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;

            if (timer > 2f && timer < 2.02f)
            {
                Handheld.Vibrate();
            }
        }

        if (Input.GetMouseButtonUp(0) && timer > 2f)
        {
            if (results[0].gameObject.tag == "Note")
            {
                deletePanel.SetActive(true);
                noteToDeleteOrEdit = results[0].gameObject.transform.parent.gameObject;
            }

            timer = 0;
        }

        if (noteEdit.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
    }

    public void AddNote()
    {
        headerNote.text = string.Empty;
        textNote.text = string.Empty;
        noteEdit.SetActive(true);
    }

    public void Help()
    {
        Debug.Log("Help");
        Application.OpenURL("http://inglip.ru/");
    }

    public void EditNote(GameObject note)
    {
        if (timer < 2f)
        {
            headerNote.text = note.transform.GetChild(0).GetComponent<Text>().text;
            textNote.text = note.transform.GetChild(1).GetComponent<Text>().text;
            editNote = true;
            noteToDeleteOrEdit = note;
            noteEdit.SetActive(true);
        }
    }

    public void SaveNote()
    {
        string path;

        if (!editNote)
        {
            path = Application.persistentDataPath + "/Note" + counter + ".txt";
        }

        else
        {
            path = pathToNote[noteToDeleteOrEdit];
            editNote = false;
        }

        FileInfo fi1 = new FileInfo(path);

        if (!File.Exists(path))
        {
            note.Add(Instantiate(notePrefab, content));

            using (StreamWriter sw = fi1.CreateText())
            {
                if (headerNote.text == string.Empty)
                {
                    sw.WriteLine(System.DateTime.Now.ToString());
                    note[note.Count - 1].transform.GetChild(0).GetComponent<Text>().text = System.DateTime.Now.ToString();

                }

                else
                {
                    sw.WriteLine(headerNote.text);
                    note[note.Count - 1].transform.GetChild(0).GetComponent<Text>().text = headerNote.text;
                }

                sw.WriteLine("<EndOfHeaderStartText>");
                sw.WriteLine(textNote.text);
                note[note.Count - 1].transform.GetChild(1).GetComponent<Text>().text = textNote.text;
            }

            headerNote.text = string.Empty;
            textNote.text = string.Empty;
            noteEdit.SetActive(false);

            counter++;
            PlayerPrefs.SetInt("counter", counter);
            content.GetComponent<RectTransform>().offsetMax = new Vector2(content.GetComponent<RectTransform>().offsetMax.x, content.GetComponent<RectTransform>().offsetMax.y + 53.68f);
        }

        else
        {
          using (StreamWriter sw = fi1.CreateText())
            {
                if (headerNote.text == string.Empty)
                {
                    sw.WriteLine(System.DateTime.Now.ToString());
                    noteToDeleteOrEdit.transform.GetChild(0).GetComponent<Text>().text = System.DateTime.Now.ToString();

                }

                else
                {
                    sw.WriteLine(headerNote.text);
                    noteToDeleteOrEdit.transform.GetChild(0).GetComponent<Text>().text = headerNote.text;
                }

                sw.WriteLine("<EndOfHeaderStartText>");
                sw.WriteLine(textNote.text);
                noteToDeleteOrEdit.transform.GetChild(1).GetComponent<Text>().text = textNote.text;
            }

            headerNote.text = string.Empty;
            textNote.text = string.Empty;
            noteEdit.SetActive(false);
        }
    }

    public void Cancel()
    {
        headerNote.text = string.Empty;
        textNote.text = string.Empty;
        noteEdit.SetActive(false);
    }

    public void YesDelete()
    {
        FileInfo fi1 = new FileInfo(pathToNote[noteToDeleteOrEdit]);
        fi1.Delete();
        noteToDeleteOrEdit.SetActive(false);
        note.Remove(noteToDeleteOrEdit);
        deletePanel.SetActive(false);
        results.Clear();
        content.GetComponent<RectTransform>().offsetMax = new Vector2(content.GetComponent<RectTransform>().offsetMax.x, content.GetComponent<RectTransform>().offsetMax.y - 53.68f);
    }

    public void NoDelete()
    {
        deletePanel.SetActive(false);
        results.Clear();
    }

    public void Search()
    {
        for (int i = 0; i < note.Count; i++)
        {
            if (note[i].transform.GetChild(0).GetComponent<Text>().text.ToUpper().Contains(searchField.text.ToUpper())
                || note[i].transform.GetChild(1).GetComponent<Text>().text.ToUpper().Contains(searchField.text.ToUpper()))
            {
                note[i].GetComponent<Image>().color = Color.red;
            }

            else
            {
                note[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void AddTag(Text tag)
    {
        Color color = SwitchColor(tag.text);
        noteToDeleteOrEdit.GetComponent<Image>().color = color;
        string str = pathToNote[noteToDeleteOrEdit];
        str = str.Replace((Application.persistentDataPath + "/"), "");
        str = str.Replace(".txt", "");
        PlayerPrefs.SetString(str, tag.text);
    }

    Color SwitchColor(string tag)
    {
        Color color;

        switch (tag)
        {
            case "Red":
                {
                    color = new Color(0.97f, 0.42f, 0.36f, 0.768f);
                    break;
                }
            case "Yellow":
                {
                    color = new Color(1, 0.9f, 0.64f, 0.768f);
                    break;
                }
            case "Blue":
                {
                    color = new Color(0.4f, 0.55f, 1, 0.768f);
                    break;
                }
            case "Purple":
                {
                    color = new Color(0.87f, 0.58f, 0.88f, 0.768f);
                    break;
                }
            case "Green":
                {
                    color = new Color(0.73f, 0.98f, 0.64f, 0.768f);
                    break;
                }
            case "Azure":
                {
                    color = new Color(0.55f, 0.94f, 1, 0.768f);
                    break;
                }

            default:
                {
                    color = new Color(1, 1, 1, 0.4f);
                    break;
                }
        }

        return color;
    }
}