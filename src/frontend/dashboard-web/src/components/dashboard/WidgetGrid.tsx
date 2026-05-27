import DashboardCard from "./DashboardCard";
import BitcoinHistoryChart from "./BitcoinHistoryChart";
import { widgets } from "../../data/mockWidgets";
import { useBitcoinPrice } from "../../hooks/useBitcoinPrice";
import { useRomeWeather } from "../../hooks/useRomeWeather";

function WidgetGrid() {
    const { price, loading: bitcoinLoading, error: bitcoinError, lastUpdated: bitcoinUpdated } = useBitcoinPrice();

    const {
        temperature,
        description,
        loading: weatherLoading,
        error: weatherError,
        lastUpdated: weatherUpdated
    } = useRomeWeather();

    return (
        <section>
            <div className="dashboard-header">
                <div>
                    <h2 className="dashboard-title">Overview</h2>
                    <p className="dashboard-subtitle">
                        Your most relevant daily widgets in one place.
                    </p>
                </div>
            </div>

            <div className="dashboard-grid">
                {widgets.map((widget) => {
                    if (widget.title === "Bitcoin") {
                        return (
                            <div key={widget.id} style={{ display: "contents" }}>
                                <DashboardCard
                                    title="Bitcoin"
                                    value={price}
                                    isLoading={bitcoinLoading}
                                    isError={!!bitcoinError}
                                    lastUpdated={bitcoinUpdated}
                                    backgroundColor={widget.backgroundColor}
                                />
                                <BitcoinHistoryChart />
                            </div>
                        );
                    }

                    if (widget.title === "Weather") {
                        return (
                            <DashboardCard
                                key={widget.id}
                                title="Weather"
                                value={temperature}
                                subtitle={
                                    weatherLoading
                                        ? "Fetching Rome weather..."
                                        : weatherError
                                            ? "Unable to load weather"
                                            : description
                                }
                                isLoading={weatherLoading}
                                isError={!!weatherError}
                                lastUpdated={weatherUpdated}
                                backgroundColor={widget.backgroundColor}
                            />
                        );
                    }

                    return (
                        <DashboardCard
                            key={widget.id}
                            title={widget.title}
                            value={widget.value}
                            backgroundColor={widget.backgroundColor}
                        />
                    );
                })}
            </div>
        </section>
    );
}

export default WidgetGrid;