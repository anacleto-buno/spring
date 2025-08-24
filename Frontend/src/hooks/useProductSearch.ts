import { useQuery } from '@tanstack/react-query';
import { productService } from '../services/productService';
import type { ProductDto } from '../types/product';

export const useProductSearch = (searchTerm: string, enabled: boolean = true) => {
  return useQuery<ProductDto[], Error>({
    queryKey: ['products', 'search', searchTerm],
    queryFn: () => productService.searchProducts(searchTerm),
    enabled: enabled && searchTerm.trim().length > 0,
    staleTime: 2 * 60 * 1000, // 2 minutes
    gcTime: 5 * 60 * 1000, // 5 minutes (was cacheTime)
    retry: 1,
    refetchOnWindowFocus: false,
  });
};
