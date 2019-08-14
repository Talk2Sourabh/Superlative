using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;
using UnityEngine.UI;

public class UploadImageFromJson : MonoBehaviour
{
    [SerializeField]
    private GameObject _canvas ;

    [SerializeField]
    private Image _imagesPrefab;

    [SerializeField]
    private Transform _imagesParent;

    //private string _jsonFilePath = "D:\\Sourabh\\Random\\ScrollView\\Assets\\Project\\Resources\\document.json";
    private void Start()
    {
        StartCoroutine(LoadImage());
    }
    
    IEnumerator LoadImage()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.LogError("InternetNotReachable");
        }
        else
        {
            _canvas.SetActive(false);

            TextAsset _tempText = Resources.Load("document") as TextAsset;

            JsonData imageData = JsonMapper.ToObject(_tempText.text);

            for (int i = 0; i < imageData["promotions"].Count; i++)
            {
                WWW www = new WWW(imageData["promotions"][i]["imageLink"].ToString());
                yield return www;
                if (www.error == null)
                {
                    Debug.Log("TestingLoadingImage");
                    Texture2D _tempTexture = new Texture2D(256, 256);
                    _tempTexture = www.texture;
                    Sprite _tempSprite = Sprite.Create(_tempTexture, new Rect(0, 0, _tempTexture.width, _tempTexture.height), new Vector2());
                    Image _tempImage = Instantiate(_imagesPrefab, Vector3.zero, Quaternion.identity, _imagesParent);
                    _tempImage.sprite = _tempSprite;
                }
                else
                {
                    Debug.LogError("ErrorWhileFetching");
                }
            }
            _canvas.SetActive(true);

        }

    }
}
