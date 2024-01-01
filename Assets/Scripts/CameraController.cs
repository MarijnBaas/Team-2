using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float minDistance = 0.5f;
    [SerializeField] private float fractionSpeed = 0.2f;
    private Vector2 diff = new Vector2();
    private void Update()
    {
        // Camera follows player directly
        // transform.position = new Vector3(player.position.x, player.position.y, -10f);

        // Smooth follow
        diff = player.position - transform.position;

        if(diff.magnitude > minDistance) {
            float speed = Mathf.Max(fractionSpeed*diff.magnitude, minSpeed);
            Vector2 delta = diff*((speed*Time.deltaTime)/diff.magnitude);
            transform.position = transform.position + new Vector3(delta.x, delta.y, 0);
        }
    }
}
