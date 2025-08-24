# Product Management System

A modern React application built with TypeScript, Material-UI, and React Query for managing and searching products with optimal performance.

## Features

- 🚀 **Modern Tech Stack**: React 18 + TypeScript + Vite
- 🎨 **Material-UI Design**: Beautiful, responsive UI components
- ⚡ **Performance Optimized**: Debounced search, memoization, and React Query caching
- 🔍 **Smart Search**: Real-time search across multiple product fields
- 📊 **Data Grid**: Sortable, paginated table with 1000+ items support
- 📱 **Responsive**: Works seamlessly on desktop, tablet, and mobile
- 🔄 **Real-time Updates**: Background refetch and cache management

## Architecture

### Frontend Stack
- **React 18** with TypeScript for type safety
- **Vite** for fast development and building
- **Material-UI (MUI)** for UI components and theming
- **TanStack Query** for API state management and caching
- **Axios** for HTTP requests

### Performance Features
- **Debounced Search**: 300ms delay prevents excessive API calls
- **Memoization**: `useMemo`, `useCallback`, `React.memo` for preventing unnecessary re-renders
- **Smart Caching**: React Query handles background updates and stale data
- **Virtual Scrolling**: DataGrid handles large datasets efficiently
- **Code Splitting**: Optimized bundle size

## API Integration

### Endpoints
- `GET /api/Product` - Load all products (initial load)
- `GET /api/Product/search?searchTerm={query}` - Search products

### Backend Requirements
- Backend API running on `http://localhost:5000`
- See API documentation at: `http://localhost:5000/swagger/v1/swagger.json`

## Getting Started

### Prerequisites
- Node.js 18+ 
- npm or yarn
- Backend API running on localhost:5000

### Installation

1. **Install dependencies**
   ```bash
   npm install
   ```

2. **Start development server**
   ```bash
   npm run dev
   ```

3. **Build for production**
   ```bash
   npm run build
   ```

4. **Preview production build**
   ```bash
   npm run preview
   ```

## Project Structure

```
src/
├── components/           # React components
│   ├── ProductTable.tsx  # Main data grid component
│   ├── SearchInput.tsx   # Debounced search input
│   └── LoadingSkeleton.tsx # Loading states
├── hooks/               # Custom React hooks
│   ├── useDebounce.ts   # Debouncing utility
│   ├── useProducts.ts   # Products query hook
│   └── useProductSearch.ts # Search query hook
├── services/            # API layer
│   ├── api.ts           # Axios configuration
│   └── productService.ts # Product API methods
├── types/               # TypeScript types
│   └── product.ts       # Product interfaces
├── utils/               # Utilities and constants
│   └── constants.ts     # API URLs and constants
├── App.tsx             # Main application component
└── main.tsx            # Application entry point
```

## Key Components

### ProductTable
- Material-UI DataGrid with advanced features
- Sortable columns, pagination, and filtering
- Responsive design with mobile-optimized columns
- Performance optimized for 1000+ items

### SearchInput
- Debounced input (300ms delay)
- Loading indicators and clear functionality
- Keyboard shortcuts (Escape to clear)
- Accessible design

### Custom Hooks
- `useProducts`: Manages initial product loading
- `useProductSearch`: Handles search functionality
- `useDebounce`: Prevents excessive API calls

## Performance Optimizations

### React Query Configuration
- **Stale Time**: 5 minutes for products, 2 minutes for search
- **Cache Time**: 10 minutes for products, 5 minutes for search
- **Background Refetch**: Disabled for better performance
- **Retry Logic**: 2 retries for products, 1 for search

### Component Optimization
- `React.memo` on expensive components
- `useMemo` for heavy computations
- `useCallback` for event handlers
- Debounced search input

### Bundle Optimization
- Tree shaking enabled
- Code splitting with dynamic imports
- Optimized Material-UI imports

## API Schema

### ProductDto Interface
```typescript
interface ProductDto {
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
```

## Browser Support

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

## Development

### Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build
- `npm run lint` - Run ESLint

### Environment Variables
- `VITE_API_BASE_URL` - Backend API URL (defaults to http://localhost:5000)

## Troubleshooting

### Common Issues

1. **API Connection Errors**
   - Ensure backend is running on port 5000
   - Check CORS configuration on backend
   - Verify API endpoints are accessible

2. **Performance Issues**
   - Check React Query DevTools for cache status
   - Monitor network requests in browser DevTools
   - Adjust debounce delay if needed

3. **Build Errors**
   - Clear node_modules and reinstall dependencies
   - Check TypeScript configuration
   - Verify all imports are correct

## Contributing

1. Follow TypeScript strict mode
2. Use Material-UI components consistently
3. Add proper error handling
4. Write performance-optimized code
5. Test on mobile devices

## License

MIT License - see LICENSE file for details
