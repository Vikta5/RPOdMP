using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;


public class rssreader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    XmlTextReader rssReader;
    XmlDocument rssDoc;
    XmlNode nodeRss;
    XmlNode nodeChannel;
    XmlNode nodeItem;
    public channel rowNews;

    // this is the root channel information within the RSS
    // I have supplied the fields here that are in the UNITY RSS feed
    public struct channel
    {
        public string title;
        public string link;
        public string description;
        public string docs;
        public string managingEditor;
        public string webMaster;
        public string lastBuildDate;
        // this is our collection of RSS items
        public List<items> item;
    }

    // again, all values here are the same as what exists in the UNITY RSS feed
    public struct items
    {
        public string title;
        public string category;
        public string creator;
        public string guid;
        public string link;
        public string pubDate;
        public string description;
    }

    // our constructor takes the URL to the feed
    public rssreader(string feedURL)
    {
        // setup the channel structure
        rowNews = new channel();
        // make the list available to write to
        rowNews.item = new List<items>();
        rssReader = new XmlTextReader(feedURL);
        rssDoc = new XmlDocument();
        rssDoc.Load(rssReader);
        // Loop for the <rss> tag
        if (rssDoc != null)
        {
            for (int i = 0; i < rssDoc.ChildNodes.Count; i++)
            {
                // If it is the rss tag
                if (rssDoc.ChildNodes[i].Name == "rss")
                {
                    // <rss> tag found
                    nodeRss = rssDoc.ChildNodes[i];
                }
            }
        }

        // Loop for the <channel> tag
        if (nodeRss != null)
        {
            for (int i = 0; i < nodeRss.ChildNodes.Count; i++)
            {
                // If it is the channel tag
                if (nodeRss.ChildNodes[i].Name == "channel")
                {
                    // <channel> tag found
                    nodeChannel = nodeRss.ChildNodes[i];
                }
            }
        }

        // this is our channel header information
        if (rowNews.title != null)
        {
            rowNews.title = nodeChannel["title"].InnerText;
        }
        if (rowNews.link != null)
        {
            rowNews.link = nodeChannel["link"].InnerText;
        }
        if (rowNews.description != null)
        {
            rowNews.description = nodeChannel["description"].InnerText;
        }
        if (rowNews.docs != null)
        {
            rowNews.docs = nodeChannel["docs"].InnerText;
        }
        if (rowNews.lastBuildDate != null)
        {
            rowNews.lastBuildDate = nodeChannel["lastBuildDate"].InnerText;
        }
        if (rowNews.managingEditor != null)
        {
            rowNews.managingEditor = nodeChannel["managingEditor"].InnerText;
        }
        if (rowNews.webMaster != null)
        {
            rowNews.webMaster = nodeChannel["webMaster"].InnerText;
        }

        // here we have our feed items
        if (nodeChannel != null)
        {
            for (int i = 0; i < nodeChannel.ChildNodes.Count; i++)
            {
                if (nodeChannel.ChildNodes[i].Name == "item")
                {
                    nodeItem = nodeChannel.ChildNodes[i];
                    // create an empty item to fill
                    items itm = new items();
                    if (nodeItem.InnerXml.Contains("title"))
                    {
                        itm.title = nodeItem["title"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("link"))
                    {
                        itm.link = nodeItem["link"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("category"))
                    {
                        itm.category = nodeItem["category"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("dc:creator"))
                    {
                        itm.creator = nodeItem["dc:creator"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("guid"))
                    {
                        itm.guid = nodeItem["guid"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("pubDate"))
                    {
                        itm.pubDate = nodeItem["pubDate"].InnerText;
                    }
                    if (nodeItem.InnerXml.Contains("description"))
                    {
                        itm.description = nodeItem["description"].InnerText;
                    }
                    // add the item to the channel items list
                    rowNews.item.Add(itm);
                }
            }
        }

    }
}
