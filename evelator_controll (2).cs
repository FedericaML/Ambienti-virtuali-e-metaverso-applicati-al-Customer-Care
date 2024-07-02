using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evelator_controll : MonoBehaviour
{
    public List<int> sequenceElevator = new List<int>();
    public bool Elevator_in_run;
    public float[] FloorHighs;
    public GameObject ElevatorCabin;
    public GameObject[] FloorNumbers;
    public int CurrentFloorNumber;
    public float DoorOpenTime;
    public GameObject[] Door_outside_left;
    public float[] Door_outside_left_close_value;
    public float[] Door_outside_left_open_value;
    public GameObject[] Door_outside_right;
    public float[] Door_outside_right_close_value;
    public float[] Door_outside_right_open_value;
    public GameObject Door_inside_right;
    public float Door_inside_right_close_value;
    public float Door_inside_right_open_value;
    public GameObject Door_inside_left;
    public float Door_inside_left_close_value;
    public float Door_inside_left_open_value;

    private Coroutine ElevatorTaskCoroutine;
    private Coroutine DoorOpeningCoroutine;
    private Coroutine DoorClosingCoroutine;
    private bool Doors_finished;

    private Transform playerTransform;

    void Start()
    {
        ChangeFloorNumbers();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            playerTransform.SetParent(ElevatorCabin.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform.SetParent(null);
            playerTransform = null;
        }
    }

    public void OnFloorButtonPressed(string name)
    {
        Debug.Log("Button pressed: " + name);
        AddTaskEve(name);
    }

    public void AddTaskEve(string name)
    {
        int floorIndex = -1;

        if (name.Contains("Button floor 1"))
            floorIndex = 0;
        else if (name.Contains("Button floor 2"))
            floorIndex = 1;
        else if (name.Contains("Button floor 3"))
            floorIndex = 2;
        else if (name.Contains("Button floor 4"))
            floorIndex = 3;
        else if (name.Contains("Button floor 5"))
            floorIndex = 4;
        else if (name.Contains("Button floor 6"))
            floorIndex = 5;

        if (floorIndex >= 0)
        {
            sequenceElevator.Add(floorIndex);

            if (!Elevator_in_run)
            {
                Elevator_in_run = true;
                ElevatorTaskCoroutine = StartCoroutine(ExecuteTask());
            }
        }

        Debug.Log("Added floor to sequence: " + (floorIndex + 1));
        Debug.Log("Current sequenceElevator: " + string.Join(", ", sequenceElevator));
        Debug.Log("Started Elevator Task");
    }

    public IEnumerator ExecuteTask()
    {
        while (sequenceElevator.Count > 0)
        {
            int toReach = sequenceElevator[0];
            Vector3 targetPosition = new Vector3(ElevatorCabin.transform.localPosition.x, FloorHighs[toReach], ElevatorCabin.transform.localPosition.z);

            Debug.Log("Executing task. Target Position: " + targetPosition);

            while (ElevatorCabin.transform.localPosition != targetPosition)
            {
                ElevatorCabin.transform.localPosition = Vector3.MoveTowards(ElevatorCabin.transform.localPosition, targetPosition, Time.deltaTime);
                ChangeFloorNumbers();
                
                // Aggiorna la posizione del giocatore rispetto alla cabina
                if (playerTransform != null && playerTransform.parent == ElevatorCabin.transform)
                {
                    playerTransform.localPosition = Vector3.zero; // Assicurati che il giocatore sia centrato nella cabina
                }

                yield return null;
            }

            DoorOpeningCoroutine = StartCoroutine(HandleDoorOpen(toReach));
            Doors_finished = false;
            sequenceElevator.RemoveAt(0);
            yield return new WaitWhile(() => !Doors_finished);
        }

        Elevator_in_run = false;
        StopCoroutine(ElevatorTaskCoroutine);
    }

    public void ChangeFloorNumbers()
    {
        for (int i = 0; i < FloorHighs.Length; i++)
        {
            if (Mathf.Approximately(ElevatorCabin.transform.localPosition.y, FloorHighs[i]))
            {
                CurrentFloorNumber = i + 1;
                UpdateFloorDisplay();
                break;
            }
        }
    }

    public IEnumerator HandleDoorOpen(int WhichFloor)
    {
        while (true)
        {
            Door_inside_left.transform.localPosition = Vector3.Lerp(Door_inside_left.transform.localPosition, new Vector3(Door_inside_left_open_value, Door_inside_left.transform.localPosition.y, Door_inside_left.transform.localPosition.z), DoorOpenTime * Time.deltaTime);
            Door_inside_right.transform.localPosition = Vector3.Lerp(Door_inside_right.transform.localPosition, new Vector3(Door_inside_right_open_value, Door_inside_right.transform.localPosition.y, Door_inside_right.transform.localPosition.z), DoorOpenTime * Time.deltaTime);

            Door_outside_left[WhichFloor].transform.localPosition = Vector3.Lerp(Door_outside_left[WhichFloor].transform.localPosition, new Vector3(Door_outside_left_open_value[WhichFloor], Door_outside_left[WhichFloor].transform.localPosition.y, Door_outside_left[WhichFloor].transform.localPosition.z), DoorOpenTime * Time.deltaTime);
            Door_outside_right[WhichFloor].transform.localPosition = Vector3.Lerp(Door_outside_right[WhichFloor].transform.localPosition, new Vector3(Door_outside_right_open_value[WhichFloor], Door_outside_right[WhichFloor].transform.localPosition.y, Door_outside_right[WhichFloor].transform.localPosition.z), DoorOpenTime * Time.deltaTime);

            if (Mathf.Abs(Door_inside_left.transform.localPosition.x - Door_inside_left_open_value) <= 0.001f)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(5);

        DoorClosingCoroutine = StartCoroutine(HandleDoorClose(WhichFloor));
        StopCoroutine(DoorOpeningCoroutine);
    }

    public IEnumerator HandleDoorClose(int WhichFloor)
    {
        while (true)
        {
            Door_inside_left.transform.localPosition = Vector3.Lerp(Door_inside_left.transform.localPosition, new Vector3(Door_inside_left_close_value, Door_inside_left.transform.localPosition.y, Door_inside_left.transform.localPosition.z), DoorOpenTime * Time.deltaTime);
            Door_inside_right.transform.localPosition = Vector3.Lerp(Door_inside_right.transform.localPosition, new Vector3(Door_inside_right_close_value, Door_inside_right.transform.localPosition.y, Door_inside_right.transform.localPosition.z), DoorOpenTime * Time.deltaTime);

            Door_outside_left[WhichFloor].transform.localPosition = Vector3.Lerp(Door_outside_left[WhichFloor].transform.localPosition, new Vector3(Door_outside_left_close_value[WhichFloor], Door_outside_left[WhichFloor].transform.localPosition.y, Door_outside_left[WhichFloor].transform.localPosition.z), DoorOpenTime * Time.deltaTime);
            Door_outside_right[WhichFloor].transform.localPosition = Vector3.Lerp(Door_outside_right[WhichFloor].transform.localPosition, new Vector3(Door_outside_right_close_value[WhichFloor], Door_outside_right[WhichFloor].transform.localPosition.y, Door_outside_right[WhichFloor].transform.localPosition.z), DoorOpenTime * Time.deltaTime);

            if (Mathf.Abs(Door_inside_left.transform.localPosition.x - Door_inside_left_close_value) <= 0.001f)
                break;

            yield return null;
        }

        Doors_finished = true;
        StopCoroutine(DoorClosingCoroutine);
    }

    void UpdateFloorDisplay()
    {
        foreach (GameObject Numberassemble in FloorNumbers)
        {
            for (int i = 0; i < Numberassemble.transform.childCount; i++)
            {
                GameObject numberA = Numberassemble.transform.GetChild(i).gameObject;
                numberA.SetActive(false);

                if (i == CurrentFloorNumber - 1)
                    numberA.SetActive(true);
            }
        }
    }
}
