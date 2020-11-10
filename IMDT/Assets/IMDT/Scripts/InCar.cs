using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InCar : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    GameObject[] car;
    GameObject Hint;
    GameObject Time;
    GameObject drivecar;
    bool incar = false;
    public bool collision = false;
    Vector3 prepos;
    Quaternion prerotation;
    public float f = 5.0f;
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        car = GameObject.FindGameObjectsWithTag("Car");
        Hint = GameObject.Find("Hint");
        Time = GameObject.Find("TimeCount");
        for (int i=0;i<car.Length;i++)
        {
            car[i].GetComponentInChildren<LPPV_CarController>().enabled = false;
        }
        Hint.SetActive(false);
        Time.SetActive(false);
        InvokeRepeating("refresh",0.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Hint.SetActive(false);
        Vector3 position = player.transform.position;
        for (int i = 0; i < car.Length; i++)
        {
            if (!incar&&Vector3.Distance(position, car[i].transform.position) < 4)
            {
                Hint.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    drivecar = car[i];
                    prerotation = car[i].transform.rotation;
                    for (int j = 0; j < car.Length; j++)
                    {
                        if(car[j]!=drivecar)
                        {
                            car[j].SetActive(false);
                        }
                    }
                    car[i].GetComponentInChildren<LPPV_CarController>().enabled = true;
                    car[i].GetComponentInChildren<Camera>().enabled = true;
                    car[i].transform.position = new Vector3(-9, 0, -20);
                    if(car[i].name=="Bus")
                    {
                        drivecar = GameObject.Find("BusVehicle");
                    }
                    else if(car[i].name == "Sedan")
                    {
                        drivecar = GameObject.Find("SedanCar");
                    }
                    else if (car[i].name == "Utility")
                    {
                        drivecar = GameObject.Find("UtilityVehicle");
                    }
                    else
                    {
                        drivecar = GameObject.Find("SportsCar");
                    }
                    player.SetActive(false);
                    Hint.SetActive(false);
                    Time.SetActive(true);
                    incar = true;
                }
            }
        }
        if(incar)
        {
            if(drivecar.transform.position.x> -10.313224&& drivecar.transform.position.x < -7.313224&& drivecar.transform.position.z <320&& drivecar.transform.position.z >-15)
            {
                Vector3 prev = drivecar.GetComponent<LPPV_CarController>()._rgbd.velocity;

                
                    Vector3 v = prev - f * prev.normalized * UnityEngine.Time.deltaTime;
                    if (prev.x > 0 && v.x < 0)
                    {
                        v.x = 0;
                    }
                    if (prev.z > 0 && v.z < 0)
                    {
                        v.z = 0;
                    }
                    drivecar.GetComponent<LPPV_CarController>()._rgbd.velocity = v;
                

            }
            if(collision)
            {
                
                drivecar.transform.position = new Vector3(-8.813224f,prepos.y,prepos.z);
                drivecar.transform.rotation = prerotation;
                drivecar.GetComponent<LPPV_CarController>()._rgbd.velocity = new Vector3(0,0,0);
                collision = false;
            }
            if(drivecar.transform.position.z>320)
            {
                Time.GetComponent<TimeCount>().success = true;
            }
        }
    }
    void refresh()
    {
        if(incar)
        {
            prepos = drivecar.transform.position;
        }
    }
}
