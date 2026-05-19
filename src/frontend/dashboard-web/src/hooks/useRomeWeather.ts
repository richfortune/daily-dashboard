import { useEffect, useState } from "react";

type RomeWeatherState = {
    temperature: string;
    description: string;
    loading: boolean;
    error: string | null;
    lastUpdated: string | null;
};

// function getWeatherDescription(code: number): string {
//     switch (code) {
//         case 0:
//             return "Clear";
//         case 1:
//         case 2:
//         case 3:
//             return "Cloudy";
//         case 45:
//         case 48:
//             return "Fog";
//         case 51:
//         case 53:
//         case 55:
//             return "Drizzle";
//         case 61:
//         case 63:
//         case 65:
//             return "Rain";
//         case 71:
//         case 73:
//         case 75:
//             return "Snow";
//         case 80:
//         case 81:
//         case 82:
//             return "Showers";
//         case 95:
//             return "Thunderstorm";
//         default:
//             return "Weather unavailable";
//     }
// }

export function useRomeWeather(): RomeWeatherState {
    const [temperature, setTemperature] = useState<string>("--");
    const [description, setDescription] = useState<string>("--");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [lastUpdated, setLastUpdated] = useState<string | null>(null);

    useEffect(() => {
        async function fetchWeather() {
            try {
                setLoading(true);

                const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/api/weather/rome`);

                if (!response.ok) {
                    throw new Error("Weather request failed");
                }

                const data = await response.json();

                setTemperature(data.temperature);
                setDescription(data.description);
                setLastUpdated(new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));
                setError(null);
            } catch (err) {
                setError("Errore nel recupero meteo");
            } finally {
                setLoading(false);
            }
        }

        fetchWeather();
    }, []);

    return { temperature, description, loading, error, lastUpdated };
}