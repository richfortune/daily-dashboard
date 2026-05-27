import { useState, useMemo } from "react";
import { useBitcoinHistory } from "../../hooks/useBitcoinHistory";
import { useCreateBitcoinSnapshot } from "../../hooks/useCreateBitcoinSnapshot";
import {
    LineChart,
    Line,
    XAxis,
    YAxis,
    CartesianGrid,
    Tooltip,
    ResponsiveContainer
} from "recharts";

export function BitcoinHistoryChart() {
    const { history, loading: historyLoading, error: historyError, refetch } = useBitcoinHistory();
    const { createSnapshot, loading: snapshotLoading } = useCreateBitcoinSnapshot();
    const [snapshotError, setSnapshotError] = useState<string | null>(null);

    // Gestione salvataggio snapshot
    const handleSaveSnapshot = async () => {
        setSnapshotError(null);
        const result = await createSnapshot();
        if (result) {
            await refetch();
        } else {
            setSnapshotError("Impossibile salvare lo snapshot. Controlla la connessione.");
        }
    };

    // Prepara e ordina i dati cronologicamente (dal più vecchio al più recente)
    const chartData = useMemo(() => {
        if (!history || history.length === 0) return [];
        return [...history]
            .sort((a, b) => new Date(a.timestamp).getTime() - new Date(b.timestamp).getTime());
    }, [history]);

    // Controlla se tutte le rilevazioni sono dello stesso giorno
    const isSameDay = useMemo(() => {
        if (chartData.length < 2) return true;
        const firstDay = new Date(chartData[0].timestamp).toDateString();
        return chartData.every(item => new Date(item.timestamp).toDateString() === firstDay);
    }, [chartData]);

    // Formatta l'asse X dinamicamente
    const formatXAxis = (tickItem: string) => {
        const d = new Date(tickItem);
        if (isNaN(d.getTime())) return "";
        if (isSameDay) {
            return d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        } else {
            return `${d.getDate().toString().padStart(2, '0')}/${(d.getMonth() + 1).toString().padStart(2, '0')} ${d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
        }
    };

    const formatDate = (dateStr: string | undefined) => {
        if (!dateStr) return "";
        const d = new Date(dateStr);
        return d.toLocaleDateString([], { day: '2-digit', month: '2-digit', year: 'numeric' }) + " " + d.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
    };

    // Calcolo angolo ed altezza asse X
    const tickAngle = chartData.length > 8 ? -30 : 0;
    const xAxisHeight = chartData.length > 8 ? 45 : 30;

    // Componente Tooltip Personalizzato Premium
    const CustomTooltip = ({ active, payload }: any) => {
        if (active && payload && payload.length) {
            const data = payload[0].payload;
            const dateObj = new Date(data.timestamp);
            const fullDate = dateObj.toLocaleDateString([], { day: '2-digit', month: '2-digit', year: 'numeric' });
            const fullTime = dateObj.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' });
            return (
                <div style={{
                    backgroundColor: "var(--surface)",
                    border: "1px solid rgba(0, 0, 0, 0.1)",
                    borderRadius: "12px",
                    padding: "12px",
                    boxShadow: "var(--shadow)",
                    color: "var(--text)"
                }}>
                    <p style={{ margin: 0, fontWeight: 700, fontSize: "0.8rem", color: "var(--text-soft)" }}>
                        {fullDate} {fullTime}
                     </p>
                     <p style={{ margin: "6px 0 0 0", fontSize: "1.1rem", fontWeight: 700, color: "#f7931a" }}>
                         ${data.price.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })} {data.currency}
                     </p>
                     <p style={{ margin: "4px 0 0 0", fontSize: "0.75rem", color: "var(--text-soft)" }}>
                         Sorgente: <span style={{ color: "var(--text)", fontWeight: 600 }}>{data.source}</span>
                     </p>
                </div>
            );
        }
        return null;
    };

    if (historyLoading && chartData.length === 0) {
        return (
            <div className="widget-card" style={{ display: "flex", alignItems: "center", justifyItems: "center", height: 280, textAlign: "center", gridColumn: "span 2" }}>
                <p className="widget-subtitle loading" style={{ margin: "auto" }}>Loading chart history...</p>
            </div>
        );
    }

    if (historyError && chartData.length === 0) {
        return (
            <div className="widget-card" style={{ display: "flex", alignItems: "center", justifyItems: "center", height: 280, textAlign: "center", gridColumn: "span 2" }}>
                <p className="widget-subtitle error" style={{ margin: "auto" }}>Error: {historyError}</p>
            </div>
        );
    }

    const totalCount = chartData.length;
    const firstTimestamp = chartData[0]?.timestamp;
    const lastTimestamp = chartData[chartData.length - 1]?.timestamp;

    return (
        <article className="widget-card" style={{ background: "var(--surface)", gridColumn: "span 2", display: "flex", flexDirection: "column", gap: "1rem" }}>
            {/* Header del widget */}
            <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-start", flexWrap: "wrap", gap: "16px" }}>
                <div>
                    <h3 className="widget-title" style={{ fontSize: "1rem", color: "var(--text)" }}>Storico Bitcoin</h3>
                    {totalCount > 0 ? (
                        <p className="widget-subtitle" style={{ fontSize: "0.85rem", marginTop: "4px" }}>
                            Dal {formatDate(firstTimestamp)} al {formatDate(lastTimestamp)}
                        </p>
                    ) : (
                        <p className="widget-subtitle" style={{ fontSize: "0.85rem", marginTop: "4px" }}>
                            Nessun dato storico disponibile
                        </p>
                    )}
                </div>

                <div style={{ display: "flex", flexDirection: "column", alignItems: "flex-end", gap: "8px" }}>
                    <button
                        onClick={handleSaveSnapshot}
                        disabled={snapshotLoading}
                        style={{
                            display: "flex",
                            alignItems: "center",
                            gap: "8px",
                            background: snapshotLoading ? "var(--text-soft)" : "linear-gradient(135deg, #f7931a 0%, #d87a0c 100%)",
                            color: "#fff",
                            border: "none",
                            padding: "8px 16px",
                            borderRadius: "20px",
                            fontWeight: 600,
                            cursor: snapshotLoading ? "not-allowed" : "pointer",
                            fontSize: "0.85rem",
                            transition: "all 0.2s ease",
                            boxShadow: "0 4px 12px rgba(247, 147, 26, 0.2)",
                        }}
                    >
                        {snapshotLoading ? "Salvataggio..." : "Salva rilevazione Bitcoin"}
                    </button>
                    {totalCount > 0 && (
                        <span style={{
                            backgroundColor: "var(--surface-soft)",
                            padding: "4px 10px",
                            borderRadius: "20px",
                            fontSize: "0.8rem",
                            fontWeight: 600,
                            color: "var(--primary-dark)"
                        }}>
                            {totalCount} {totalCount === 1 ? "rilevazione" : "rilevazioni"}
                        </span>
                    )}
                </div>
            </div>

            {/* Eventuale errore sul salvataggio dello snapshot */}
            {snapshotError && (
                <div style={{ color: "#ef4444", fontSize: "0.85rem", margin: "0 0 4px 0" }}>
                    {snapshotError}
                </div>
            )}

            {/* Grafico */}
            <div style={{ width: "100%", height: 220, marginTop: "0.5rem" }}>
                {totalCount > 0 ? (
                    <ResponsiveContainer width="100%" height="100%">
                        <LineChart data={chartData} margin={{ top: 5, right: 10, left: 10, bottom: 5 }}>
                            <CartesianGrid strokeDasharray="3 3" stroke="rgba(0, 0, 0, 0.05)" />
                            <XAxis 
                                dataKey="timestamp" 
                                stroke="var(--text-soft)"
                                fontSize={10}
                                tickLine={false}
                                tickFormatter={formatXAxis}
                                angle={tickAngle}
                                textAnchor={tickAngle !== 0 ? "end" : "middle"}
                                height={xAxisHeight}
                            />
                            <YAxis 
                                stroke="var(--text-soft)"
                                fontSize={10}
                                domain={['auto', 'auto']}
                                tickLine={false}
                                tickFormatter={(value) => `$${value.toLocaleString()}`}
                            />
                            <Tooltip content={<CustomTooltip />} />
                            <Line 
                                type="monotone" 
                                dataKey="price" 
                                stroke="#f7931a" 
                                strokeWidth={2}
                                dot={{ r: 2 }}
                                activeDot={{ r: 5 }} 
                            />
                        </LineChart>
                    </ResponsiveContainer>
                ) : (
                    <div style={{ display: "flex", height: "100%", alignItems: "center", justifyContent: "center" }}>
                        <p className="widget-subtitle">Nessun dato storico disponibile. Clicca su "Salva rilevazione Bitcoin" per aggiungere il primo punto.</p>
                    </div>
                )}
            </div>
        </article>
    );
}

export default BitcoinHistoryChart;
