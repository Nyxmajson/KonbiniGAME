using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetPath : MonoBehaviour
{
    [Header("Design")]
    public bool Startpatrol;
    public Transform[] patrolPoints;
    public int targetPoint = 0;
    public int pointAnalyse;
    public float speed = 2f;

    private bool hasShownFeedback = false;
    private bool hasNotifiedEnd = false;

    private ItemData itemData;
    private Paiement paiement;
    private Vendeur vendeur;

    [Header("Parent Settings")]
    public Transform parentDuringPath;       // Parent temporaire pendant le déplacement
    public Transform dropZone;               // Nouveau parent à la fin (peut être null)

    public void Init(ItemData item, Paiement paiementRef)
    {
        itemData = item;
        paiement = paiementRef;

        // On change de parent si défini
        if (parentDuringPath != null)
            transform.parent = parentDuringPath;
    }

    private void Update()
    {
        if (!Startpatrol || targetPoint >= patrolPoints.Length) return;

        Transform target = patrolPoints[targetPoint];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            if (targetPoint == pointAnalyse && !hasShownFeedback)
            {
                paiement.DisplayItemFeedback(itemData);
                hasShownFeedback = true;
            }

            if (targetPoint == patrolPoints.Length - 1 && !hasNotifiedEnd)
            {
                hasNotifiedEnd = true;
                StartCoroutine(WaitThenNotify(1f));
            }

            targetPoint++;
        }
    }

    private IEnumerator WaitThenNotify(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Change de parent final si défini
        if (dropZone != null)
            transform.parent = dropZone;
        else
            transform.parent = null; // le rend indépendant

        // Ajoute un Rigidbody pour faire tomber
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.mass = 1f;
        }

        // Désactive IsTrigger sur tous les SphereColliders
        foreach (SphereCollider sphere in GetComponents<SphereCollider>())
        {
            sphere.isTrigger = false;
        }

        // Notifie le paiement
        paiement.NotifyItemFinished();
    }
}
