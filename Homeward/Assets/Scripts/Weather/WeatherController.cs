using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour
{
    DayNightController dayNight;

    enum WEATHER_STATE { mild, moderate, severe, default_ }
    string currentWeatherState; // THIS STRING VALUE IS EITHER = "MILD" / "MODERATE" / "SEVERE"
    int currentDay;
    int currentDayIncrementor;
    bool isDaySame;
    bool isDayChanging;

    string CurrentWeatherState
    {
        get { return currentWeatherState; }
        set { currentWeatherState = value; }
    }

    void Initialization()
    {
        currentDayIncrementor = 1;
        isDaySame = false;
        isDayChanging = false;
    }

    void Start()
    {
        dayNight = FindObjectOfType(typeof(DayNightController)) as DayNightController;
        Initialization();
    }

    void Update()
    {
        currentDay = dayNight.dayCount;

        Debug.Log(currentWeatherState);

        if (currentDay != currentDayIncrementor)
        {
            if (!isDaySame)
            {
                isDaySame = true;
                isDayChanging = false;
                float random_value = Random.value;
                int random_val_simplified = SimplifyRange(random_value);
                switch (random_val_simplified)
                {
                    case (0): ReturnWeatherStateInString("Mild"); break;
                    case (1): ReturnWeatherStateInString("Moderate"); break;
                    case (2): ReturnWeatherStateInString("Severe"); break;
                    default: ReturnWeatherStateInString("Error"); break;
                }
            }
        }

        if (currentDay == currentDayIncrementor)
        {
            if (!isDayChanging)
            {
                isDayChanging = true;
                currentDayIncrementor++;
                isDaySame = false;
            }
        }
    }

    WEATHER_STATE ReturnWeatherStateInString(string state)
    {
        if (state == "Mild") { currentWeatherState = "Mild"; return WEATHER_STATE.mild; }
        if (state == "Moderate") { currentWeatherState = "Moderate"; return WEATHER_STATE.moderate; }
        if (state == "Severe") { currentWeatherState = "Severe"; return WEATHER_STATE.severe; }
        else return WEATHER_STATE.default_;
    }

    int SimplifyRange(float value)
    {
        if (value <= 0.33F) return 0;
        if (value > 0.33F && value <= 0.66F) return 1;
        if (value > 0.66F) return 2;
        else return -1;
    }
}
