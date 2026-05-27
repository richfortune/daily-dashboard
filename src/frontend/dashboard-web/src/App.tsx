import { useState } from "react";
import AppShell from "./components/layout/AppShell";
import WidgetGrid from "./components/dashboard/WidgetGrid";
import WeatherPage from "./pages/WeatherPage";

function App() {
  const [currentPage, setCurrentPage] = useState<string>("Dashboard");

  return (
    <AppShell activePage={currentPage} onPageChange={setCurrentPage}>
      {currentPage === "Dashboard" && <WidgetGrid />}
      {currentPage === "Weather" && <WeatherPage />}
      {currentPage !== "Dashboard" && currentPage !== "Weather" && (
        <div className="widget-card" style={{ background: "var(--surface)" }}>
          <div>
            <p className="widget-title">{currentPage}</p>
            <p className="widget-subtitle">Questa sezione è attualmente in fase di sviluppo.</p>
          </div>
        </div>
      )}
    </AppShell>
  );
}

export default App;