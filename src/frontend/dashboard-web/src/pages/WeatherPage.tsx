import React, { useState } from "react";
import { useWeather } from "../hooks/useWeather";
import { useWeatherFavorites } from "../hooks/useWeatherFavorites";

export function WeatherPage() {
    const [searchInput, setSearchInput] = useState("Rome");
    const { current, tomorrow, loading: weatherLoading, error: weatherError, search } = useWeather("Rome");
    const { favorites, loading: favoritesLoading, addFavorite, removeFavorite } = useWeatherFavorites();
    const [activeTab, setActiveTab] = useState<"oggi" | "domani">("oggi");

    const handleSearchSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        search(searchInput);
    };

    const handleSaveFavorite = async () => {
        if (!current) return;
        await addFavorite({
            cityName: current.city,
            country: current.country,
            latitude: current.latitude,
            longitude: current.longitude
        });
    };

    const handleSelectFavorite = (cityName: string) => {
        setSearchInput(cityName);
        search(cityName);
    };

    const isAlreadyFavorite = current && favorites.some(
        f => f.cityName.toLowerCase() === current.city.toLowerCase()
    );

    return (
        <div style={{ display: "flex", flexWrap: "wrap", gap: "24px", alignItems: "flex-start" }}>
            
            {/* Colonna Sinistra: Cerca & Preferiti */}
            <div style={{ flex: "1 1 300px", display: "flex", flexDirection: "column", gap: "24px" }}>
                
                {/* Modulo di ricerca */}
                <div className="widget-card" style={{ background: "var(--surface)", minHeight: "auto", padding: "20px" }}>
                    <form onSubmit={handleSearchSubmit} style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
                        <div>
                            <label htmlFor="city-input" style={{ display: "block", fontSize: "0.8rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase", marginBottom: "6px" }}>
                                Località
                            </label>
                            <input
                                id="city-input"
                                type="text"
                                value={searchInput}
                                onChange={(e) => setSearchInput(e.target.value)}
                                placeholder="Cerca città..."
                                style={{
                                    width: "100%",
                                    padding: "10px 14px",
                                    borderRadius: "10px",
                                    border: "1px solid rgba(0, 0, 0, 0.1)",
                                    fontSize: "1rem",
                                    outline: "none",
                                    background: "var(--bg)",
                                    color: "var(--text)",
                                    transition: "border 0.2s"
                                }}
                            />
                        </div>
                        <button
                            type="submit"
                            disabled={weatherLoading}
                            style={{
                                width: "100%",
                                height: "42px",
                                borderRadius: "10px",
                                border: "none",
                                background: "linear-gradient(135deg, var(--primary) 0%, var(--primary-dark) 100%)",
                                color: "#fff",
                                fontWeight: 600,
                                cursor: weatherLoading ? "not-allowed" : "pointer",
                                transition: "opacity 0.2s"
                            }}
                        >
                            {weatherLoading ? "Ricerca..." : "Cerca"}
                        </button>
                    </form>
                </div>

                {/* Lista Preferiti */}
                <div className="widget-card" style={{ background: "var(--surface)", minHeight: "200px", padding: "20px" }}>
                    <h3 className="widget-title" style={{ fontSize: "0.85rem", marginBottom: "16px", color: "var(--text)" }}>Località Preferite</h3>
                    
                    {favoritesLoading && favorites.length === 0 ? (
                        <p style={{ fontSize: "0.9rem", color: "var(--text-soft)", margin: 0 }}>Caricamento preferiti...</p>
                    ) : favorites.length === 0 ? (
                        <p style={{ fontSize: "0.9rem", color: "var(--text-soft)", margin: 0 }}>Nessuna città nei preferiti.</p>
                    ) : (
                        <ul style={{ listStyle: "none", padding: 0, margin: 0, display: "flex", flexDirection: "column", gap: "10px" }}>
                            {favorites.map((fav) => (
                                <li 
                                    key={fav.id} 
                                    style={{ 
                                        display: "flex", 
                                        justifyContent: "space-between", 
                                        alignItems: "center", 
                                        background: "var(--bg)", 
                                        padding: "8px 12px", 
                                        borderRadius: "10px"
                                    }}
                                >
                                    <button
                                        type="button"
                                        onClick={() => handleSelectFavorite(fav.cityName)}
                                        style={{
                                            border: "none",
                                            background: "transparent",
                                            textAlign: "left",
                                            fontWeight: 600,
                                            fontSize: "0.95rem",
                                            color: "var(--text)",
                                            cursor: "pointer",
                                            padding: 0,
                                            flex: 1
                                        }}
                                    >
                                        {fav.cityName} <span style={{ fontSize: "0.75rem", fontWeight: 400, color: "var(--text-soft)" }}>({fav.country})</span>
                                    </button>
                                    
                                    <button
                                        type="button"
                                        onClick={() => removeFavorite(fav.id)}
                                        aria-label={`Elimina ${fav.cityName} dai preferiti`}
                                        style={{
                                            border: "none",
                                            background: "transparent",
                                            color: "#ef4444",
                                            fontSize: "1.1rem",
                                            cursor: "pointer",
                                            padding: "2px 6px"
                                        }}
                                    >
                                        🗑️
                                    </button>
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
            </div>

            {/* Colonna Destra: Visualizzazione Dettagli Meteo */}
            <div style={{ flex: "2 1 400px", display: "flex", flexDirection: "column", gap: "24px", minWidth: "280px" }}>
                
                {weatherLoading && (
                    <div className="widget-card" style={{ display: "flex", alignItems: "center", justifyContent: "center", minHeight: "350px", width: "100%", background: "var(--surface)" }}>
                        <p className="widget-subtitle loading" style={{ margin: "auto", fontSize: "1.1rem" }}>Caricamento dati meteo...</p>
                    </div>
                )}

                {!weatherLoading && weatherError && (
                    <div className="widget-card" style={{ display: "flex", alignItems: "center", justifyContent: "center", minHeight: "350px", width: "100%", background: "var(--surface)" }}>
                        <div style={{ textAlign: "center", margin: "auto" }}>
                            <p style={{ fontSize: "1.2rem", fontWeight: 600, color: "#ef4444" }}>Errore</p>
                            <p className="widget-subtitle" style={{ color: "var(--text-soft)", marginTop: "8px" }}>{weatherError}</p>
                        </div>
                    </div>
                )}

                {!weatherLoading && !weatherError && current && tomorrow && (
                    <div className="widget-card" style={{ background: "var(--surface)", minHeight: "350px", display: "flex", flexDirection: "column", justifyContent: "flex-start", gap: "20px", padding: "24px" }}>
                        
                        {/* Intestazione città meteo e Pulsante Preferito */}
                        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", borderBottom: "1px solid rgba(0, 0, 0, 0.05)", paddingBottom: "16px", flexWrap: "wrap", gap: "16px" }}>
                            <div>
                                <div style={{ display: "flex", alignItems: "center", gap: "12px", flexWrap: "wrap" }}>
                                    <h2 style={{ fontSize: "1.8rem", fontWeight: 700, margin: 0, color: "var(--text)" }}>
                                        {current.city}
                                    </h2>
                                    
                                    {isAlreadyFavorite ? (
                                        <span style={{ color: "#f59e0b", fontSize: "1.1rem", display: "inline-flex", alignItems: "center", gap: "4px" }} title="Città nei preferiti">
                                            ★ <span style={{ fontSize: "0.8rem", fontWeight: 600 }}>Preferito</span>
                                        </span>
                                    ) : (
                                        <button
                                            type="button"
                                            onClick={handleSaveFavorite}
                                            style={{
                                                background: "transparent",
                                                color: "var(--primary-dark)",
                                                border: "1px solid var(--primary-dark)",
                                                padding: "4px 10px",
                                                borderRadius: "20px",
                                                fontWeight: 600,
                                                cursor: "pointer",
                                                fontSize: "0.75rem",
                                                transition: "all 0.2s"
                                            }}
                                        >
                                            ☆ Salva località
                                        </button>
                                    )}
                                </div>
                                <p style={{ fontSize: "0.95rem", color: "var(--text-soft)", margin: "4px 0 0 0" }}>
                                    {current.country || "Paese non specificato"} (Lat: {current.latitude.toFixed(2)}, Lon: {current.longitude.toFixed(2)})
                                </p>
                            </div>

                            {/* Selettore Tab */}
                            <div style={{ display: "flex", background: "var(--bg)", padding: "4px", borderRadius: "10px" }}>
                                <button
                                    type="button"
                                    onClick={() => setActiveTab("oggi")}
                                    style={{
                                        border: "none",
                                        padding: "8px 16px",
                                        borderRadius: "8px",
                                        fontWeight: 600,
                                        fontSize: "0.9rem",
                                        cursor: "pointer",
                                        background: activeTab === "oggi" ? "var(--surface)" : "transparent",
                                        color: activeTab === "oggi" ? "var(--text)" : "var(--text-soft)",
                                        boxShadow: activeTab === "oggi" ? "0 2px 8px rgba(0,0,0,0.05)" : "none",
                                        transition: "all 0.2s"
                                    }}
                                >
                                    Oggi
                                </button>
                                <button
                                    type="button"
                                    onClick={() => setActiveTab("domani")}
                                    style={{
                                        border: "none",
                                        padding: "8px 16px",
                                        borderRadius: "8px",
                                        fontWeight: 600,
                                        fontSize: "0.9rem",
                                        cursor: "pointer",
                                        background: activeTab === "domani" ? "var(--surface)" : "transparent",
                                        color: activeTab === "domani" ? "var(--text)" : "var(--text-soft)",
                                        boxShadow: activeTab === "domani" ? "0 2px 8px rgba(0,0,0,0.05)" : "none",
                                        transition: "all 0.2s"
                                    }}
                                >
                                    Domani
                                </button>
                            </div>
                        </div>

                        {/* Contenuto dei Tab */}
                        {activeTab === "oggi" ? (
                            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: "24px" }}>
                                {/* Temperatura */}
                                <div style={{ display: "flex", flexDirection: "column", justifyContent: "center" }}>
                                    <p style={{ fontSize: "0.85rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase" }}>Temperatura Attuale</p>
                                    <p style={{ fontSize: "4.5rem", fontWeight: 700, lineHeight: 1.1, color: "var(--text)", margin: "8px 0" }}>
                                        {Math.round(current.temperature)}°C
                                    </p>
                                    <p style={{ fontSize: "1.1rem", fontWeight: 600, color: "var(--primary-dark)" }}>
                                        {current.weatherDescription}
                                    </p>
                                </div>

                                {/* Dettagli addizionali */}
                                <div style={{ display: "flex", flexDirection: "column", gap: "16px", justifyContent: "center" }}>
                                    <div style={{ background: "var(--bg)", padding: "14px", borderRadius: "12px" }}>
                                        <p style={{ fontSize: "0.75rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase" }}>Vento</p>
                                        <p style={{ fontSize: "1.2rem", fontWeight: 700, margin: "4px 0 0 0", color: "var(--text)" }}>
                                            {current.windSpeed} km/h
                                        </p>
                                    </div>
                                    <div style={{ background: "var(--bg)", padding: "14px", borderRadius: "12px" }}>
                                        <p style={{ fontSize: "0.75rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase" }}>Ultimo Aggiornamento</p>
                                        <p style={{ fontSize: "1.1rem", fontWeight: 600, margin: "4px 0 0 0", color: "var(--text)" }}>
                                            {current.timestamp}
                                        </p>
                                    </div>
                                </div>
                            </div>
                        ) : (
                            <div style={{ display: "grid", gridTemplateColumns: "repeat(auto-fit, minmax(180px, 1fr))", gap: "24px" }}>
                                {/* Temperatura Min/Max */}
                                <div style={{ display: "flex", flexDirection: "column", justifyContent: "center" }}>
                                    <p style={{ fontSize: "0.85rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase" }}>Previsione Domani ({tomorrow.date})</p>
                                    <div style={{ display: "flex", alignItems: "baseline", gap: "12px", margin: "8px 0" }}>
                                        <span style={{ fontSize: "3.5rem", fontWeight: 700, color: "#ef4444" }}>
                                            {Math.round(tomorrow.temperatureMax)}°
                                        </span>
                                        <span style={{ fontSize: "2.2rem", fontWeight: 600, color: "#3b82f6" }}>
                                            {Math.round(tomorrow.temperatureMin)}°
                                        </span>
                                    </div>
                                    <p style={{ fontSize: "1.1rem", fontWeight: 600, color: "var(--primary-dark)" }}>
                                        {tomorrow.weatherDescription}
                                    </p>
                                </div>

                                {/* Dettagli addizionali */}
                                <div style={{ display: "flex", flexDirection: "column", gap: "16px", justifyContent: "center" }}>
                                    <div style={{ background: "var(--bg)", padding: "14px", borderRadius: "12px" }}>
                                        <p style={{ fontSize: "0.75rem", fontWeight: 700, color: "var(--text-soft)", textTransform: "uppercase" }}>Probabilità Precipitazioni</p>
                                        <p style={{ fontSize: "1.2rem", fontWeight: 700, margin: "4px 0 0 0", color: "var(--text)" }}>
                                            {tomorrow.precipitationProbability}%
                                        </p>
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                )}
            </div>

        </div>
    );
}

export default WeatherPage;
