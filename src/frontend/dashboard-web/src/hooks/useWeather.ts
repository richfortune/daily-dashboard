import { useState, useEffect, useCallback } from "react";
import type { WeatherCurrent, WeatherTomorrow } from "../types/weather.types";

export function useWeather(initialCity: string = "Rome") {
    const [city, setCity] = useState(initialCity);
    const [current, setCurrent] = useState<WeatherCurrent | null>(null);
    const [tomorrow, setTomorrow] = useState<WeatherTomorrow | null>(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const fetchWeather = useCallback(async (searchCity: string) => {
        const trimmed = searchCity.trim();
        if (!trimmed) return;
        
        setLoading(true);
        setError(null);
        try {
            const [resCurrent, resTomorrow] = await Promise.all([
                fetch(`${apiBaseUrl}/api/Weather/current?city=${encodeURIComponent(trimmed)}`),
                fetch(`${apiBaseUrl}/api/Weather/tomorrow?city=${encodeURIComponent(trimmed)}`)
            ]);

            if (resCurrent.status === 404 || resTomorrow.status === 404) {
                throw new Error("Località non trovata");
            }

            if (!resCurrent.ok) {
                throw new Error("Errore nel recupero del meteo corrente");
            }

            if (!resTomorrow.ok) {
                throw new Error("Errore nel recupero delle previsioni per domani");
            }

            const currentData = await resCurrent.json();
            const tomorrowData = await resTomorrow.json();

            setCurrent(currentData);
            setTomorrow(tomorrowData);
            setCity(trimmed);
        } catch (err: any) {
            setError(err.message || "Errore durante il recupero dei dati meteo");
            setCurrent(null);
            setTomorrow(null);
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl]);

    useEffect(() => {
        fetchWeather(initialCity);
    }, [fetchWeather, initialCity]);

    return {
        city,
        current,
        tomorrow,
        loading,
        error,
        search: fetchWeather
    };
}
