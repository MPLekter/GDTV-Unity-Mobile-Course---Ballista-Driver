using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    [SerializeField] float speedValue = 10f;
    [SerializeField] float speedGain = 1.1f;
    [SerializeField] float turningSpeed = 100f;

    private int turningValue; // negative LEFT positive RIGHT

    private void Start()
    {
        InvokeRepeating("IncreaseSpeed", 1f, 1f);
    }
    void Update()
    {
        transform.Translate(Vector3.forward * speedValue * Time.deltaTime);

        transform.Rotate(0f, turningValue * turningSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter" + other.name);

        if(other.CompareTag("OBSTACLE"))
        {
            SceneManager.LoadScene(0); //TODO: get rid of hardcoded main menu as '0'
        }
    }
    public void Turning(int value)
    {
        turningValue = value;
    }
    public void IncreaseSpeed()
    {
        speedValue += speedGain;
    }    
}
