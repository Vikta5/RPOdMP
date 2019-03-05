using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetterRss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        rssreader reader = new rssreader("https://lenta.ru/rss");
        Debug.Log(reader.rowNews.item[0].description);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
