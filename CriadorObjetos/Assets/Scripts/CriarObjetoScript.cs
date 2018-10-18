using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class CriarObjetoScript : MonoBehaviour
{
    public TMP_InputField posX;
    public TMP_InputField posY;
    public TMP_InputField posZ;

    public TMP_InputField rotX;
    public TMP_InputField rotY;
    public TMP_InputField rotZ;

    public Toggle useLocal;

    public GameObject novoObjeto;

    Vector3 novaPos;
    Vector3 novaRot;

    public void CriarObjeto()
    {
        if (useLocal.isOn)
        {
            PegarCoordenadasLocal();
        }
        else
        {
            StartCoroutine(PegarCoordenadasGithub());
        }

    }

    void PegarCoordenadasLocal()
    {
        try
        {
            novaPos = new Vector3(
            float.Parse(posX.text),
            float.Parse(posY.text),
            float.Parse(posZ.text));

            novaRot = new Vector3(
            float.Parse(rotX.text),
            float.Parse(rotY.text),
            float.Parse(rotZ.text));
        }
        catch (System.Exception)
        {
            Debug.LogWarning("Campo Vazio");
            return;
        }
        Instantiate(novoObjeto, novaPos, Quaternion.Euler(novaRot));
    }

    IEnumerator PegarCoordenadasGithub()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://raw.githubusercontent.com/tiofih/CriadorObjetosUnity/master/file.txt");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string downloadText = www.downloadHandler.text;
            Debug.Log(downloadText);
            string[] split = downloadText.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            novaPos = new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
            novaRot = new Vector3(float.Parse(split[3]), float.Parse(split[4]), float.Parse(split[5]));
            Instantiate(novoObjeto, novaPos, Quaternion.Euler(novaRot));
        }
    }
}
