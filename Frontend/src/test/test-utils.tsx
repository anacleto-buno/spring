import React from 'react'
import { render } from '@testing-library/react'
import type { RenderOptions } from '@testing-library/react'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ThemeProvider, createTheme } from '@mui/material'
import type { ProductDto } from '../types/product'

// Create a test theme
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
  },
})

// Create a test query client
const createTestQueryClient = () =>
  new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  })

// Custom render function with providers
interface AllTheProvidersProps {
  children: React.ReactNode
}

const AllTheProviders = ({ children }: AllTheProvidersProps) => {
  const testQueryClient = createTestQueryClient()
  
  return (
    <QueryClientProvider client={testQueryClient}>
      <ThemeProvider theme={theme}>
        {children}
      </ThemeProvider>
    </QueryClientProvider>
  )
}

const customRender = (
  ui: React.ReactElement,
  options?: Omit<RenderOptions, 'wrapper'>,
) => render(ui, { wrapper: AllTheProviders, ...options })

// Mock product data for tests
export const mockProducts: ProductDto[] = [
  {
    id: '1',
    name: 'Test Product 1',
    description: 'Test description 1',
    category: 'Electronics',
    brand: 'Test Brand',
    price: 99.99,
    stockQuantity: 10,
    sku: 'TEST-001',
    releaseDate: '2023-01-01T00:00:00Z',
    availabilityStatus: 'In Stock',
    customerRating: 4.5,
    availableColors: 'Red, Blue, Green',
    availableSizes: 'S, M, L, XL',
  },
  {
    id: '2',
    name: 'Test Product 2',
    description: 'Test description 2',
    category: 'Clothing',
    brand: 'Test Brand 2',
    price: 49.99,
    stockQuantity: 0,
    sku: 'TEST-002',
    releaseDate: '2023-02-01T00:00:00Z',
    availabilityStatus: 'Out of Stock',
    customerRating: 3.8,
    availableColors: 'Black, White',
    availableSizes: 'XS, S, M',
  },
  {
    id: '3',
    name: 'Test Product 3',
    description: 'Test description 3',
    category: 'Home & Garden',
    brand: 'Test Brand 3',
    price: 199.99,
    stockQuantity: 5,
    sku: 'TEST-003',
    releaseDate: '2023-03-01T00:00:00Z',
    availabilityStatus: 'Limited Stock',
    customerRating: null,
    availableColors: null,
    availableSizes: null,
  },
]

// re-export everything
export * from '@testing-library/react'

// override render method
export { customRender as render }
