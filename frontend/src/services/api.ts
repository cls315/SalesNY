import axios from 'axios';

const API = axios.create({
  baseURL: '/api',
});

export const getKPIs = () => API.get('/Sales/dashboard/kpis');
export const getVentasPorTienda = () => API.get('/Sales/dashboard/ventas-por-tienda');

// Prueba sin parámetros (o con limit):
export const getTopProductos = (limit = 5) => API.get(`/Sales/dashboard/top-productos?limit=${limit}`);

export const getSales = (params: Record<string, any> = {}) => API.get('/Sales', { params });

export default API;