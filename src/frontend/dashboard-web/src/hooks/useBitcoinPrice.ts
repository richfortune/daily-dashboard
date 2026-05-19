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

    const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;

    console.log("API BASE URL:", apiBaseUrl);



    useEffect(() => {
        async function fetchBitcoin() {
            try {
                setLoading(true);

                //const response = await fetch(`${import.meta.env.VITE_API_BASE_URL}/api/Bitcoin`)

                const response = await fetch(`${apiBaseUrl}/api/Bitcoin`);

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