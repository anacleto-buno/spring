import { useQuery } from '@tanstack/react-query';
import { productService } from '../services/productService';
import type { ProductDto } from '../types/product';

export const useProducts = () => {
  return useQuery<ProductDto[], Error>({
    queryKey: ['products'],
    queryFn: productService.getAllProducts,
    staleTime: 5 * 60 * 1000, // 5 minutes
    gcTime: 10 * 60 * 1000, // 10 minutes (was cacheTime)
    retry: 2,
    refetchOnWindowFocus: false,
  });
};
