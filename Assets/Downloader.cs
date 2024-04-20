using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Downloader : MonoBehaviour
{
    // URL del archivo AssetBundle en el servidor
    public string assetBundleURL = "https://api.auraxr.com/storage/1sdH5H53lQ/iSpYMnKtxq.";

    // Nombre del asset en el AssetBundle que quieres instanciar
    public string assetName = "nombre-del-asset";
    public GameObject father;
    // Inicia la descarga y carga del AssetBundle
    void Start()
    {
        StartCoroutine(DownloadAndLoadAssetBundle());
    }

    [Button]
    public void SetLAbel(string label)
    {
        EtiquetarTodosLosHijos(father, label);
    }

    void EtiquetarTodosLosHijos(GameObject gameObject, string label)
    {
        // Obtiene la cantidad de hijos del GameObject
        int numHijos = gameObject.transform.childCount;

        // Itera a través de todos los hijos
        for (int i = 0; i < numHijos; i++)
        {
            // Obtiene el hijo en el índice i
            Transform hijo = gameObject.transform.GetChild(i);

            // Asigna la etiqueta al hijo
            hijo.gameObject.tag = label;

            // Puedes realizar más operaciones aquí con cada hijo
            // Por ejemplo, etiquetar los hijos de este hijo recursivamente
            EtiquetarTodosLosHijos(hijo.gameObject, label);
        }
    }

    // Corrutina para descargar y cargar el AssetBundle
    IEnumerator DownloadAndLoadAssetBundle()
    {
        // Enviar una solicitud para descargar el archivo desde la URL
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleURL))
        {
            // Esperar hasta que la solicitud se complete
            yield return uwr.SendWebRequest();

            // Verificar si hubo errores durante la solicitud
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al descargar el AssetBundle: " + uwr.error);
                yield break;
            }

            // Obtener el AssetBundle descargado
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(uwr);

            if (bundle == null)
            {
                Debug.LogError("Error: AssetBundle es nulo");
                yield break;
            }

            string[] assetNames = bundle.GetAllAssetNames();

            // Imprime los nombres de los assets en la consola
            Debug.Log("Nombres de los assets en el AssetBundle:");
            foreach (string assetName in assetNames)
            {
                Debug.Log(assetName);
            }

            // Cargar el asset que deseas instanciar desde el AssetBundle
            GameObject assetToInstantiate = bundle.LoadAsset<GameObject>(assetName);

            if (assetToInstantiate == null)
            {
                Debug.LogError("Error: No se encontró el asset '" + assetName + "' en el AssetBundle");
                yield break;
            }

            // Instanciar el objeto en la escena
            Instantiate(assetToInstantiate);

            // Liberar el AssetBundle cuando ya no se necesite para ahorrar memoria
            bundle.Unload(false);
        }
    }
}
