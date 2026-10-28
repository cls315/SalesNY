import React, { useEffect, useState } from "react";
import {
  getKPIs,
  getVentasPorTienda,
  getTopProductos,
  getSales,
} from "./services/api";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  CartesianGrid,
  PieChart,
  Pie,
  Cell,
} from "recharts";
import { DollarSign, ShoppingBag, Users, Store, Filter } from "lucide-react";

const COLORS = ["#0088FE", "#00C49F", "#FFBB28", "#FF8042", "#8884d8"];

export default function App() {
  const [kpis, setKpis] = useState(null);
  const [ventasTienda, setVentasTienda] = useState([]);
  const [topProductos, setTopProductos] = useState([]);
  const [sales, setSales] = useState([]);
  const [loading, setLoading] = useState(true);

  // Filtros
  const [fechaInicio, setFechaInicio] = useState("");
  const [fechaFin, setFechaFin] = useState("");

  const loadDashboardData = async () => {
  try {
    setLoading(true);

    try {
      const kpiRes = await getKPIs();
      setKpis(kpiRes.data);
    } catch (e) {
      console.error("Error cargando KPIs:", e);
    }

    try {
      const tiendaRes = await getVentasPorTienda();
      setVentasTienda(tiendaRes.data);
    } catch (e) {
      console.error("Error cargando Ventas por Tienda:", e);
    }

    try {
      const prodRes = await getTopProductos(5);
      setTopProductos(prodRes.data);
    } catch (e) {
      console.error("Error cargando Top Productos:", e);
    }

  } finally {
    setLoading(false);
  }
};

  const loadSalesTable = async () => {
    try {
      const params = {};
      if (fechaInicio) params.fechaInicio = fechaInicio;
      if (fechaFin) params.fechaFin = fechaFin;

      const res = await getSales(params);
      setSales(res.data.data || []);
    } catch (error) {
      console.error("Error al cargar la tabla de ventas:", error);
    }
  };

  useEffect(() => {
    loadDashboardData();
    loadSalesTable();
  }, []);

  const handleFilter = (e) => {
    e.preventDefault();
    loadSalesTable();
  };

  if (loading) {
    return (
      <div
        style={{
          padding: "2rem",
          textAlign: "center",
          fontFamily: "sans-serif",
        }}
      >
        Cargando datos del Dashboard...
      </div>
    );
  }

  return (
    <div
      style={{
        padding: "2rem",
        fontFamily: "Arial, sans-serif",
        backgroundColor: "#f4f6f8",
        minHeight: "100vh",
      }}
    >
      <h1 style={{ marginBottom: "1.5rem", color: "#1a202c" }}>
        Sales NY — Dashboard de Ventas
      </h1>

      {/* TARJETAS KPI */}
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(200px, 1fr))",
          gap: "1rem",
          marginBottom: "2rem",
        }}
      >
        <div style={cardStyle}>
          <div>
            <span style={labelStyle}>Total Facturado</span>
            <h2 style={valueStyle}>
              ${kpis?.totalFacturado?.toLocaleString() ?? 0}
            </h2>
          </div>
          <DollarSign color="#2563eb" size={32} />
        </div>
        <div style={cardStyle}>
          <div>
            <span style={labelStyle}>Transacciones</span>
            <h2 style={valueStyle}>{kpis?.totalTransacciones ?? 0}</h2>
          </div>
          <ShoppingBag color="#16a34a" size={32} />
        </div>
        <div style={cardStyle}>
          <div>
            <span style={labelStyle}>Clientes</span>
            <h2 style={valueStyle}>{kpis?.totalClientes ?? 0}</h2>
          </div>
          <Users color="#d97706" size={32} />
        </div>
        <div style={cardStyle}>
          <div>
            <span style={labelStyle}>Tiendas</span>
            <h2 style={valueStyle}>{kpis?.totalTiendas ?? 0}</h2>
          </div>
          <Store color="#9333ea" size={32} />
        </div>
      </div>

      {/* GRÁFICAS */}
      <div
        style={{
          display: "grid",
          gridTemplateColumns: "repeat(auto-fit, minmax(400px, 1fr))",
          gap: "2rem",
          marginBottom: "2rem",
        }}
      >
        {/* Gráfica de Barras - Ventas por Tienda */}
        <div style={chartCardStyle}>
          <h3 style={{ marginBottom: "1rem" }}>Recaudación por Tienda</h3>
          <ResponsiveContainer width="100%" height={300}>
            <BarChart data={ventasTienda}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="tienda" />
              <YAxis />
              <Tooltip />
              <Bar dataKey="totalVentas" fill="#2563eb" name="Total ($)" />
            </BarChart>
          </ResponsiveContainer>
        </div>

        {/* Gráfica Torta - Top Productos */}
        <div style={chartCardStyle}>
          <h3 style={{ marginBottom: "1rem" }}>Top 5 Productos Vendidos</h3>
          <ResponsiveContainer width="100%" height={300}>
            <PieChart>
              <Pie
                data={topProductos}
                dataKey="totalRecaudado"
                nameKey="producto"
                cx="50%"
                cy="50%"
                outerRadius={100}
                label
              >
                {topProductos.map((entry, index) => (
                  <Cell
                    key={`cell-${index}`}
                    fill={COLORS[index % COLORS.length]}
                  />
                ))}
              </Pie>

              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </div>

      {/* FILTROS Y TABLA */}

      <div style={chartCardStyle}>
        <div
          style={{
            display: "flex",
            justifyContent: "space-between",
            alignItems: "center",
            flexWrap: "wrap",
            marginBottom: "1rem",
          }}
        >
          <h3>Registro de Transacciones</h3>

          <form
            onSubmit={handleFilter}
            style={{ display: "flex", gap: "1rem", alignItems: "center" }}
          >
            <label style={{ fontSize: "0.9rem" }}>
              Desde:{" "}
              <input
                type="date"
                value={fechaInicio}
                onChange={(e) => setFechaInicio(e.target.value)}
              />
            </label>

            <label style={{ fontSize: "0.9rem" }}>
              Hasta:{" "}
              <input
                type="date"
                value={fechaFin}
                onChange={(e) => setFechaFin(e.target.value)}
              />
            </label>

            <button type="submit" style={btnStyle}>
              <Filter size={16} /> Filtrar
            </button>
          </form>
        </div>

        <table
          style={{
            width: "100%",
            borderCollapse: "collapse",
            marginTop: "1rem",
          }}
        >
          <thead>
            <tr style={{ backgroundColor: "#f1f5f9", textAlign: "left" }}>
              <th style={thStyle}>ID</th>

              <th style={thStyle}>Fecha</th>

              <th style={thStyle}>Producto</th>

              <th style={thStyle}>Cliente</th>

              <th style={thStyle}>Tienda</th>

              <th style={thStyle}>Cantidad</th>

              <th style={thStyle}>Precio U.</th>

              <th style={thStyle}>Total</th>
            </tr>
          </thead>

          <tbody>
            {sales.map((item) => (
              <tr
                key={item.transactionId}
                style={{ borderBottom: "1px solid #e2e8f0" }}
              >
                <td style={tdStyle}>{item.transactionId}</td>
                <td style={tdStyle}>{item.transactionDate}</td>
                <td style={tdStyle}>{item.productGroup}</td>
                <td style={tdStyle}>{item.customerName}</td>
                <td style={tdStyle}>{item.storeCity}</td>
                <td style={tdStyle}>{item.quantitySold}</td>
                <td style={tdStyle}>${item.unitPrice}</td>
                <td style={tdStyle}>${item.total}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

// Estilos básicos en línea para acelerar la presentación

const cardStyle = {
  backgroundColor: "#fff",
  padding: "1.2rem",
  borderRadius: "8px",
  display: "flex",
  justifyContent: "space-between",
  alignItems: "center",
  boxShadow: "0 1px 3px rgba(0,0,0,0.1)",
};

const chartCardStyle = {
  backgroundColor: "#fff",
  padding: "1.5rem",
  borderRadius: "8px",
  boxShadow: "0 1px 3px rgba(0,0,0,0.1)",
};

const labelStyle = { color: "#64748b", fontSize: "0.85rem" };

const valueStyle = { margin: "0.3rem 0 0 0", color: "#0f172a" };

const thStyle = { padding: "0.75rem", fontSize: "0.85rem", color: "#475569" };

const tdStyle = { padding: "0.75rem", fontSize: "0.85rem" };

const btnStyle = {
  display: "flex",
  alignItems: "center",
  gap: "0.5rem",  backgroundColor: "#2563eb",
  color: "#fff",
  border: "none",
  padding: "0.5rem 1rem",
  borderRadius: "4px",
  cursor: "pointer",
};
