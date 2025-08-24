import apiClient from './api';
import type { ProductDto } from '../types/product';
import { API_ENDPOINTS } from '../utils/constants';

export const productService = {
  // Get all products
  getAllProducts: async (): Promise<ProductDto[]> => {
    const response = await apiClient.get<ProductDto[]>(API_ENDPOINTS.PRODUCTS);
    return response.data;
  },

  // Search products by term
  searchProducts: async (searchTerm: string): Promise<ProductDto[]> => {
    if (!searchTerm.trim()) {
      return [];
    }
    const response = await apiClient.get<ProductDto[]>(
      `${API_ENDPOINTS.PRODUCT_SEARCH}?searchTerm=${encodeURIComponent(searchTerm)}`
    );
    return response.data;
  },

  // Get product by ID
  getProductById: async (id: string): Promise<ProductDto> => {
    const response = await apiClient.get<ProductDto>(`${API_ENDPOINTS.PRODUCTS}/${id}`);
    return response.data;
  },

  // Get product by SKU
  getProductBySku: async (sku: string): Promise<ProductDto> => {
    const response = await apiClient.get<ProductDto>(`${API_ENDPOINTS.PRODUCTS}/by-sku/${sku}`);
    return response.data;
  },
};
