using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;

public class AssetBundleBuilder : MonoBehaviour
{
    // URL del archivo AssetBundle en el servidor
    public string assetBundleURL = "https://example.com/tu-assetbundle.unity3d";

    // Nombre completo del asset que quieres instanciar
    public string assetName = "assets/prefabs/munal/gp_munalportatil_fourdcube 3.prefab";

    
    // Inicia la descarga y carga del AssetBundle
    void Start()
    {
        StartCoroutine(DownloadAndLoadAsset());
    }
    
    IEnumerator DownloadAndLoadAsset()
    {
        // Enviar una solicitud para descargar el AssetBundle desde la URL
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(assetBundleURL))
        {
            // Esperar a que la solicitud se complete
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

            // Cargar el asset que deseas instanciar desde el AssetBundle usando la ruta completa
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
