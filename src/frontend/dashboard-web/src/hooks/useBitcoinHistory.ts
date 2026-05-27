import { useEffect, useState, useCallback } from "react";
import type { BitcoinHistoryState, BitcoinPriceHistory } from "../types/bitcoinHistory.types";

export function useBitcoinHistory(): BitcoinHistoryState {
    const [history, setHistory] = useState<BitcoinPriceHistory[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const fetchHistory = useCallback(async () => {
        try {
            setLoading(true);
            const response = await fetch(`${apiBaseUrl}/api/Bitcoin/history`);
            
            if (!response.ok) {
                throw new Error("Errore durante il caricamento dello storico");
            }

            const data = await response.json();
            setHistory(data);
            setError(null);
        } catch (err: any) {
            setError(err.message || "Errore nel recupero storico");
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl]);

    useEffect(() => {
        fetchHistory();
    }, [fetchHistory]);

    return { history, loading, error, refetch: fetchHistory };
}
