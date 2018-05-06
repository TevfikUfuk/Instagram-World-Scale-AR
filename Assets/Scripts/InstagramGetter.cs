using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MiniJSON;
using Mapbox.Unity.Map;
using TMPro;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.Location;

public class InstagramGetter : MonoBehaviour
{


    List<GameObject> pictures = new List<GameObject>();

    public AbstractMap _map;
    public string access_token;

    private IEnumerator coroutine;

    ILocationProvider _locationProvider;
    // Use this for initialization



    void LocationUpdated(Location location)
    {
        _locationProvider.OnLocationUpdated -= LocationUpdated;
        coroutine = GetInstaPictures(location);
        StartCoroutine(coroutine);

    }


    void Start()
    {
        
        _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider;
        _locationProvider.OnLocationUpdated += LocationUpdated; ;



    }


    IEnumerator GetInstaPictures(Location Location) 
    {
         
        string url = "https://api.instagram.com/v1/media/search?lat="+Location.LatitudeLongitude.x+"&lng="+Location.LatitudeLongitude.y+"&distance=2000&access_token="+access_token;

        //string url2 = "https://api.instagram.com/v1/media/search?lat=39.8916215&lng=32.7856365&distance=500&access_token=3192546875.4a5a308.7b89236537ec41feac65ba9918534759";
        WWW www = new WWW(url);
        yield return www;
        string api_response = www.text;
        Debug.Log(api_response);

        IDictionary apiParse = (IDictionary)Json.Deserialize(api_response);
        IList instagramPicturesList = (IList)apiParse["data"];

        foreach (IDictionary instagramPicture in instagramPicturesList)
        {
            //main picture info
            IDictionary images = (IDictionary)instagramPicture["images"];
            IDictionary standardResolution = (IDictionary)images["standard_resolution"];
            string mainPic_url = (string)standardResolution["url"];
            Debug.Log(mainPic_url);

            WWW mainPic = new WWW(mainPic_url);
            yield return mainPic;


            //location info
            IDictionary location = (IDictionary)instagramPicture["location"];
            double lat = (double)location["latitude"];
            double lon = (double)location["longitude"];
            string placeName = (string)location["name"];

            GameObject instaPost = Instantiate(Resources.Load("InstaPost2")) as GameObject;
            instaPost.transform.SetParent(_map.transform);
            instaPost.transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = mainPic.texture;
            //var height = _map.QueryHeightData(new Mapbox.Utils.Vector2d(lat, lon));
            instaPost.transform.position = _map.GeoToWorldPosition(new Mapbox.Utils.Vector2d(lat, lon))+ new Vector3(0, 24, 0); ; //+ new Vector3(0, height * _map.transform.localScale.x + 0.25f, 0);
            //instaPost.transform.position = Conversions.GeoToWorldPosition(new Mapbox.Utils.Vector2d(lat, lon), _map.CenterMercator, _map.WorldRelativeScale).ToVector3xz() + new Vector3(0, 24, 0);

            //username,profile picture
            IDictionary user = (IDictionary)instagramPicture["user"];
            string userName = (string)user["username"];
            string profilePic_url = (string)user["profile_picture"];

            if (instagramPicture["caption"]!=null)
            {
                IDictionary caption = (IDictionary)instagramPicture["caption"];
                string caption_text = (string)caption["text"];
                instaPost.transform.GetChild(5).GetComponent<TextMeshPro>().text = caption_text;


            }
            else
            {
                instaPost.transform.GetChild(5).GetComponent<TextMeshPro>().text = "";

            }

            //likes
            IDictionary Likes = (IDictionary)instagramPicture["likes"];
            string likes = (string)Likes["count"].ToString();

            WWW profilePic = new WWW(profilePic_url);
            yield return profilePic;
            instaPost.transform.GetChild(1).GetComponent<MeshRenderer>().material.mainTexture = profilePic.texture;
            instaPost.transform.GetChild(2).GetComponent<TextMeshPro>().text = userName;
            instaPost.transform.GetChild(6).GetComponent<TextMeshPro>().text = userName;

            instaPost.transform.GetChild(3).GetComponent<TextMeshPro>().text = placeName;
            instaPost.transform.GetChild(4).GetComponent<TextMeshPro>().text = likes + " likes";
            pictures.Add(instaPost);

        }

    }




   

    // Update is called once per frame
    void Update()
    {



        for (int i = 0; i < pictures.Count; i++)
        {

            for (int j = 0; j < pictures.Count; j++)
            {

                if (i != j)
                {
                    if (pictures[i].transform.position == pictures[j].transform.position)
                    {
                        var pos = pictures[i].transform.position;
                        pos.y += 0.5f;
                        pictures[i].transform.position = pos;
                    }
                }

            }

        }

       

    }
}
