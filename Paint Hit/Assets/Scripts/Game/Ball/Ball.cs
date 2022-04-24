using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private Transform splashEffectPrefab;
    private ParticleSystem splashParticleEffect;
    private bool hit;

    private void Awake() {
        splashParticleEffect = GameObject.Find("SplashParticleEffect").GetComponent<ParticleSystem>();
        splashParticleEffect.GetComponent<ParticleSystemRenderer>().material.color = ColorManager.Instance.GetCurrentColor();       //splash particle effect color

        splashEffectPrefab = Resources.Load<Transform>("Prefabs/pfSplash");
        splashEffectPrefab.GetComponent<SpriteRenderer>().color = ColorManager.Instance.GetCurrentColor();                          //splash effects color

        GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();                                      //balls color
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.tag == "unColored" && !hit) {        //If the collided area is not colored, paint it
            hit = true;

            Collider _collider = collision.collider;
            _collider.GetComponent<MeshRenderer>().enabled = true;                                              
            _collider.GetComponent<MeshRenderer>().material.color = ColorManager.Instance.GetCurrentColor();    //Paint the collided area
            _collider.tag = "Colored";



            if (_collider.transform.childCount > 0) { //If the part has a point
                GameManager.GetPoint();
                UIManager.RefreshPointUI();
                Destroy(_collider.transform.GetChild(0).gameObject);
            }


            Transform splashEffect = Instantiate(splashEffectPrefab);
            splashEffect.SetParent(_collider.transform);
            splashEffect.localRotation = Quaternion.Euler(0, -90, -90);
            splashEffect.localPosition = new Vector3(12, 0, 0);
            Destroy(splashEffect.gameObject, 0.25f);

            splashParticleEffect.Play();

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SphereCollider>().enabled = false;
            Destroy(this.gameObject, 0.05f);
        }

        if ((collision.collider.tag == "Colored" || collision.collider.tag == "AlreadyColored") && !hit) {      //If the collided area is colored and hit it for second, game over
            hit = true;
            Collider _collider = collision.collider;
            _collider.GetComponent<MeshRenderer>().material.color = Color.red;

            BallHandler.SetLastPaintedArea(_collider);

            GameManager.LoseHeart();
            Destroy(this.gameObject, 1f);
        }
    }
}
