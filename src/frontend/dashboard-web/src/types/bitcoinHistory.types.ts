export interface BitcoinPriceHistory {
    id: number;
    price: number;
    currency: string;
    source: string;
    timestamp: string;
}

export interface BitcoinHistoryState {
    history: BitcoinPriceHistory[];
    loading: boolean;
    error: string | null;
    refetch: () => Promise<void>;
}
