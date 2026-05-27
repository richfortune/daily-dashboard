import { useState, useCallback } from "react";
import type { BitcoinPriceHistory } from "../types/bitcoinHistory.types";

export function useCreateBitcoinSnapshot() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    const createSnapshot = useCallback(async (): Promise<BitcoinPriceHistory | null> => {
        setLoading(true);
        setError(null);
        try {
            const response = await fetch(`${apiBaseUrl}/api/Bitcoin/history/snapshot`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                let errText = "Errore durante il salvataggio della rilevazione";
                try {
                    const text = await response.text();
                    if (text) errText = text;
                } catch {
                    // Ignore body reading error
                }
                throw new Error(errText);
            }

            const data = await response.json();
            return data as BitcoinPriceHistory;
        } catch (err: any) {
            const errMsg = err.message || "Errore durante il salvataggio";
            setError(errMsg);
            return null;
        } finally {
            setLoading(false);
        }
    }, [apiBaseUrl]);

    return { createSnapshot, loading, error };
}
