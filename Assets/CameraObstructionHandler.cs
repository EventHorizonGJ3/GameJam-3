using System.Collections.Generic;
using UnityEngine;

public class CameraObstructionHandler : MonoBehaviour
{
    [SerializeField] public Transform player;
    [SerializeField] public LayerMask obstructionLayer;
    
    private List<MeshRenderer> obstructedRenderers = new List<MeshRenderer>();

    private void Update()
    {
        HandleObstuctions();
    }

    void HandleObstuctions()
    {
        foreach (MeshRenderer renderer in obstructedRenderers)
        {
            if (renderer != null)
            {
                renderer.enabled = true;
            }
        }
        obstructedRenderers.Clear();

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        Ray ray = new Ray(transform.position, directionToPlayer);
        RaycastHit[] hits = Physics.RaycastAll(ray, distanceToPlayer, obstructionLayer);

        foreach (RaycastHit hit in hits)
        {
            MeshRenderer renderer = hit.transform.GetComponent<MeshRenderer > ();
            if (renderer != null)
            {
                renderer.enabled = false;
                obstructedRenderers.Add(renderer);
            }
        }
        
    }
}
