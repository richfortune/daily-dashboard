import { useEffect, useState } from "react";

type BitcoinState = {
    price: string;
    loading: boolean;
    error: string | null;
    lastUpdated: string | null;
};

export function useBitcoinPrice(): BitcoinState {
    const [price, setPrice] = useState<string>("--");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [lastUpdated, setLastUpdated] = useState<string | null>(null);

    useEffect(() => {
        async function fetchBitcoin() {
            try {
                setLoading(true);

                const response = await fetch(
                    "http://localhost:5000/api/bitcoin"
                );

                const data = await response.json();

                setPrice(data.price);
                setLastUpdated(data.lastUpdated);
                setError(null);
            } catch (err) {
                setError("Errore nel recupero dati");
            } finally {
                setLoading(false);
            }
        }

        fetchBitcoin();
    }, []);

    return { price, loading, error, lastUpdated };
}