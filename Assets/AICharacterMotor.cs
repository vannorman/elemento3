using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Elemento
{ 
    public class AICharacterMotor : MonoBehaviour
    {
        public float fallSpeed = 0;
        public float fatness = 0.25f;
        public float height = 0f;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            {

                // stick to ground
                if (Physics.Raycast(transform.position, Vector3.down, out var hit, 1f))
                {
                    transform.position = hit.point + Vector3.up * height;
                    fallSpeed = 0;
                }
                else
                {
                    transform.position -= Vector3.up * Time.deltaTime * fallSpeed;
                    fallSpeed += 0.098f;
                }

            }

            {
                // stay inside walls
                for (var i = 0; i < 8; i++)
                {
                    var rayDir = Quaternion.Euler(0, (360 / 8), 0) * transform.forward;
                    if (Physics.Raycast(transform.position, rayDir, out var hit, fatness))
                    {

                        transform.position = hit.point - rayDir * fatness;
                        return;
                    }
                }

            }
        }
    }
}
