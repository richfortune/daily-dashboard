type SidebarProps = {
    activePage: string;
    onPageChange: (page: string) => void;
};

function Sidebar({ activePage, onPageChange }: SidebarProps) {
    const menuItems = ["Dashboard", "Markets", "Weather", "Notes"];

    return (
        <aside className="sidebar">
            <div className="sidebar-brand">
                <p>Yama</p>
                <h2>Dashboard</h2>
            </div>

            <nav>
                <ul className="sidebar-nav">
                    {menuItems.map((item) => {
                        const isActive = item === activePage;

                        return (
                            <li key={item}>
                                <button
                                    type="button"
                                    onClick={() => onPageChange(item)}
                                    style={{
                                        background: isActive ? "#dff3ee" : "transparent",
                                        color: isActive ? "#1f2937" : "#64748b",
                                        fontWeight: isActive ? 700 : 500,
                                        width: "100%",
                                        border: "none",
                                        borderRadius: "14px",
                                        padding: "12px 14px",
                                        textAlign: "left",
                                        fontSize: "0.98rem",
                                        cursor: "pointer",
                                        whiteSpace: "nowrap",
                                        transition: "all 0.2s ease",
                                    }}
                                >
                                    {item}
                                </button>
                            </li>
                        );
                    })}
                </ul>
            </nav>

            <div className="sidebar-focus-card">
                <p
                    style={{
                        fontSize: "0.8rem",
                        fontWeight: 700,
                        textTransform: "uppercase",
                        marginBottom: "8px",
                        opacity: 0.8,
                    }}
                >
                    Focus
                </p>

                <p
                    style={{
                        fontSize: "0.95rem",
                        lineHeight: 1.4,
                    }}
                >
                    Keep your daily essentials visible and simple.
                </p>
            </div>
        </aside>
    );
}

export default Sidebar;