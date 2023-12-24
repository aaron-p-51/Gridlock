using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static Action<Vehicle> OnVehicleSpawned;
    public static void RaiseOnVehicleSpawned(Vehicle vehicle) => OnVehicleSpawned?.Invoke(vehicle);

    public static Action<Vehicle> OnVehicleDestroyed;
    public static void RaiseOnVehicleDestroyed(Vehicle vehicle) => OnVehicleDestroyed?.Invoke(vehicle);

    public static Action<Vehicle> OnVehicleEnterScoreTrigger;
    public static void RaiseOnVehicleEnterScoreTrigger(Vehicle vehicle) => OnVehicleEnterScoreTrigger?.Invoke(vehicle);

    public static Action<float> OnScoreChange;
    public static void RaiseOnScoreChange(float score) => OnScoreChange?.Invoke(score);
}
