import AppShell from "../components/layout/AppShell";
import WidgetGrid from "../components/dashboard/WidgetGrid";

function DashboardPage() {
    return (
        <AppShell activePage="Dashboard" onPageChange={() => {}}>
            <WidgetGrid />
        </AppShell>
    );
}

export default DashboardPage;