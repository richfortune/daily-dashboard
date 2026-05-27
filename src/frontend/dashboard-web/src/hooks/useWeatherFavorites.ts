import { useState, useEffect, useCallback } from "react";
import type { WeatherFavoriteLocation, CreateWeatherFavoriteLocationRequest } from "../types/weather.types";

export function useWeatherFavorites() {
    const [favorites, setFavorites] = useState<WeatherFavoriteLocation[]>([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const fetchFavorites = useCallback(async () => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch(`${apiBaseUrl}/api/Weather/favorites`);
            if (!response.ok) {
                throw new Error("Errore durante il recupero dei preferiti");
            }
            const data = await response.json();
            setFavorites(data);
        } catch (err: any) {
            setError(err.message || "Errore nel caricamento dei preferiti");
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl]);

    const addFavorite = useCallback(async (request: CreateWeatherFavoriteLocationRequest) => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch(`${apiBaseUrl}/api/Weather/favorites`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(request),
            });
            if (!response.ok) {
                throw new Error("Errore durante il salvataggio dei preferiti");
            }
            await fetchFavorites();
        } catch (err: any) {
            setError(err.message || "Errore nel salvataggio");
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl, fetchFavorites]);

    const removeFavorite = useCallback(async (id: number) => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch(`${apiBaseUrl}/api/Weather/favorites/${id}`, {
                method: "DELETE",
            });
            if (!response.ok) {
                throw new Error("Errore durante l'eliminazione del preferito");
            }
            await fetchFavorites();
        } catch (err: any) {
            setError(err.message || "Errore nell'eliminazione");
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl, fetchFavorites]);

    useEffect(() => {
        fetchFavorites();
    }, [fetchFavorites]);

    return { favorites, loading, error, addFavorite, removeFavorite, refetch: fetchFavorites };
}
