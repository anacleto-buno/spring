# React Product Management App - Implementation Instructions

## Overview
Create a modern React application using the latest project standards to display and search products with optimal performance and user experience.

## Project Structure and Implementation Plan

### Phase 1: Project Setup
1. **Initialize React App with Vite**
   - Use Vite for faster development and better performance
   - TypeScript support for type safety
   - ESLint and Prettier configuration

2. **Install Core Dependencies**
   - Material-UI (MUI) v5 for UI components
   - React Query (TanStack Query) for API state management
   - Axios for HTTP requests
   - React hooks for performance optimization

### Phase 2: API Integration Setup
1. **API Service Layer**
   - Base API configuration pointing to `http://localhost:5000`
   - Product service with endpoints:
     - `GET /api/Product` - Initial product load
     - `GET /api/Product/search?searchTerm={term}` - Search functionality

2. **TypeScript Interfaces**
   - ProductDto interface based on API schema
   - API response types
   - Component prop types

### Phase 3: Core Components Implementation
1. **App Component**
   - Main layout setup
   - React Query client provider
   - Material-UI theme provider

2. **ProductTable Component**
   - Material-UI DataGrid/Table implementation
   - Columns: Name, Description, Category, Brand, Price, Stock, SKU, Availability Status
   - Responsive design for mobile devices
   - Loading states and error handling

3. **SearchInput Component**
   - Debounced search input (300ms delay)
   - Clear search functionality
   - Loading indicator during search

### Phase 4: Performance Optimizations
1. **React Hooks Implementation**
   - `useMemo` for expensive calculations
   - `useCallback` for event handlers
   - `useDebounce` custom hook for search
   - `React.memo` for component memoization

2. **API Optimization**
   - React Query caching strategies
   - Background refetch configuration
   - Stale data management
   - Error boundary implementation

### Phase 5: User Experience Enhancements
1. **Loading States**
   - Skeleton loading for table rows
   - Search input loading indicator
   - Smooth transitions between states

2. **Error Handling**
   - API error messages
   - Retry mechanisms
   - Fallback UI components

3. **Responsive Design**
   - Mobile-first approach
   - Collapsible columns on small screens
   - Touch-friendly interface

## Technical Requirements Implementation

### API Integration
- **Initial Load**: Call `GET /api/Product` on component mount
- **Search**: Call `GET /api/Product/search?searchTerm={query}` with debouncing
- **Response Handling**: Handle 200, 400, 404, and 500 status codes

### Performance Features
- **Debounced Search**: 300ms delay to prevent excessive API calls
- **Memoization**: Prevent unnecessary re-renders
- **Virtual Scrolling**: For handling 1000+ items (if needed)
- **Optimistic Updates**: Immediate UI feedback

### Material-UI Table Features
- **Sortable Columns**: Click to sort by any column
- **Pagination**: Handle large datasets efficiently
- **Responsive**: Adapt to different screen sizes
- **Accessible**: ARIA labels and keyboard navigation

## File Structure
```
Frontend/
├── src/
│   ├── components/
│   │   ├── ProductTable.tsx
│   │   ├── SearchInput.tsx
│   │   └── LoadingSkeleton.tsx
│   ├── services/
│   │   ├── api.ts
│   │   └── productService.ts
│   ├── hooks/
│   │   ├── useDebounce.ts
│   │   ├── useProducts.ts
│   │   └── useProductSearch.ts
│   ├── types/
│   │   └── product.ts
│   ├── utils/
│   │   └── constants.ts
│   ├── App.tsx
│   └── main.tsx
├── package.json
├── vite.config.ts
├── tsconfig.json
└── README.md
```

## Implementation Steps

### Step 1: Setup Project
```bash
npm create vite@latest product-management-app -- --template react-ts
cd product-management-app
npm install
```

### Step 2: Install Dependencies
```bash
npm install @mui/material @emotion/react @emotion/styled
npm install @mui/x-data-grid
npm install @tanstack/react-query
npm install axios
npm install @mui/icons-material
```

### Step 3: Implement Core Functionality
1. Create API service layer
2. Implement Product table with Material-UI
3. Add search functionality with debouncing
4. Integrate React Query for state management

### Step 4: Add Performance Optimizations
1. Implement custom hooks
2. Add memoization where needed
3. Optimize re-renders
4. Add loading states

### Step 5: Testing and Refinement
1. Test with 1000+ products
2. Test search performance
3. Verify responsive design
4. Check accessibility compliance

## Success Criteria
- ✅ Fast initial load of products
- ✅ Smooth search with debouncing (< 300ms delay)
- ✅ Responsive Material-UI table
- ✅ Handle 1000+ products efficiently
- ✅ Clean, maintainable code structure
- ✅ TypeScript type safety
- ✅ Error handling and loading states
- ✅ Modern React best practices

## API Schema Reference

### ProductDto Interface
```typescript
interface ProductDto {
  id: string;
  name: string;
  description: string;
  category: string;
  brand: string;
  price: number;
  stockQuantity: number;
  sku: string;
  releaseDate: string;
  availabilityStatus: string;
  customerRating?: number;
  availableColors?: string;
  availableSizes?: string;
}
```

### API Endpoints
- `GET /api/Product` - Returns ProductDto[]
- `GET /api/Product/search?searchTerm={term}` - Returns ProductDto[]
- Base URL: `http://localhost:5000`

## Next Steps
1. Execute project setup commands
2. Implement each component following the structure above
3. Test functionality with the running backend API
4. Optimize performance based on real-world usage
