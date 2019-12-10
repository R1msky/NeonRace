using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerObject))]
public class PlayerController : MonoBehaviour
{
  
        PlayerObject playerObject;

        void Awake()
        {
            playerObject = GetComponent<PlayerObject>();
        }

        void Update()
        {
      
            Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePoint.z = 0;
            Vector3 movePos = (mousePoint - transform.position).normalized;

            playerObject.SetMovePosition(movePos);
        }
    
}
