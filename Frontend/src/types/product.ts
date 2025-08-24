export interface ProductDto {
  id: string;
  name: string | null;
  description: string | null;
  category: string | null;
  brand: string | null;
  price: number;
  stockQuantity: number;
  sku: string | null;
  releaseDate: string;
  availabilityStatus: string | null;
  customerRating?: number | null;
  availableColors?: string | null;
  availableSizes?: string | null;
}

export interface ApiResponse<T> {
  data: T;
  status: number;
  message?: string;
}

export interface ApiError {
  type?: string;
  title?: string;
  status?: number;
  detail?: string;
  instance?: string;
}
