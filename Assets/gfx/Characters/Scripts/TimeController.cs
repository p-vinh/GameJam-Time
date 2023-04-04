using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    // Should rename script to Bar Manager instead of HealthBar. Bad Naming convention
    public HealthBar timebar;
    [SerializeField] private float currentTime;
    public float maxTime = 100;
    public float fillSpeed = 0.5f; // Speed at which the bar fills


    public struct RecordedData {
        public Vector2 pos;
        public Vector2 vel;
    }

    RecordedData[,] recordedData;
    int recordMax = 80000;
    int recordCount;
    int recordIndex;
    bool wasSteppingBack = false;
    [SerializeField] int numOfObjects = 25;
    List<TimeControlled> timeObjects;

    public void Start() {
        currentTime = maxTime;
        timebar.SetMaxHealth(maxTime);
    }

    private void Awake() {
        timeObjects  = new List<TimeControlled>(numOfObjects);
        foreach(TimeControlled obj in GameObject.FindObjectsOfType<TimeControlled>()) {
            if (timeObjects.Count < numOfObjects)
                timeObjects.Add(obj);
        }
        recordedData = new RecordedData[numOfObjects, recordMax];
    }

    public void ObjectToTime(GameObject obj) {
        if (timeObjects.Count < numOfObjects)
            timeObjects.Add(obj.GetComponent<TimeControlled>());
    }

    public void RemoveObject(GameObject obj) {
        timeObjects.Remove(obj.GetComponent<TimeControlled>());
    }

    private void Update() {
        bool stepBack = Input.GetKey(KeyCode.Q);
        bool stepForward = Input.GetKey(KeyCode.E);


        if (stepBack && currentTime > 0) {
            wasSteppingBack = true;
            
            // Decreasing time energy
            currentTime -= Time.deltaTime;
            timebar.SetHealth(currentTime);

            if (recordIndex > 0) {
                recordIndex--;

                for (int objectIndex = 0; objectIndex < timeObjects.Count; objectIndex++) {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    if (timeObject != null) {
                        // Disables collisions during rewind, so player/enemies dont take damage
                        if (timeObject.tag == "Player") {
                            timeObject.GetComponentInParent<Collider2D>().enabled = false;
                        }
                        RecordedData data = recordedData[objectIndex, recordIndex];
                        timeObject.transform.position = data.pos;
                        timeObject.velocity = data.vel;
                    }
                }
            }
        } else if (stepForward && currentTime > 0) {
            wasSteppingBack = true;

            // Decreasing time energy
            currentTime -= Time.deltaTime;
            timebar.SetHealth(currentTime);

            if (recordIndex < recordCount-1) {
                recordIndex++;

                for (int objectIndex = 0; objectIndex < timeObjects.Count; objectIndex++) {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    if (timeObject != null) {
                        // Disables collisions during rewind, so player/enemies dont take damage
                        if (timeObject.tag == "Player") {
                            timeObject.GetComponentInParent<Collider2D>().enabled = false;
                        }
                        RecordedData data = recordedData[objectIndex, recordIndex];
                        timeObject.transform.position = data.pos;
                        timeObject.velocity = data.vel;
                    }
                }
            }
        } else if (!stepBack) {
            // Recovering Time Energy
            currentTime += Time.deltaTime * fillSpeed;
            currentTime = Mathf.Clamp(currentTime, 0f, maxTime);
            timebar.SetHealth(currentTime);

            if (wasSteppingBack) {
                recordCount = recordIndex;
                wasSteppingBack = false;
            }

            if (recordMax <= recordCount) {
                recordIndex = 0; 
                recordCount = 0;
            } 
            else {
                for (int objectIndex = 0; objectIndex < timeObjects.Count; objectIndex++) {
                        TimeControlled timeObject = timeObjects[objectIndex];
                        if (timeObject != null) {
                            // Enables collisions during recording
                            if (timeObject.tag == "Player") {
                                timeObject.GetComponentInParent<Collider2D>().enabled = true;
                            }
                            RecordedData data = new RecordedData();
                            data.pos = timeObject.transform.position;
                            data.vel = timeObject.velocity;
                            recordedData[objectIndex, recordCount] = data;
                        }
                }
                recordCount++;
                recordIndex = recordCount;
            }

            foreach(TimeControlled timeObject in timeObjects) {
                if (timeObject != null) {
                    timeObject.TimeUpdate();
                }
                    
            }
        }
    }
}