import React, { useState, useCallback, useMemo } from 'react';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import {
  ThemeProvider,
  createTheme,
  CssBaseline,
  Container,
  AppBar,
  Toolbar,
  Typography,
  Box,
  Alert,
  Fade,
} from '@mui/material';
import { StorefrontOutlined } from '@mui/icons-material';

import ProductTable from './components/ProductTable';
import SearchInput from './components/SearchInput';
import { useProducts } from './hooks/useProducts';
import { useProductSearch } from './hooks/useProductSearch';
import { useDebounce } from './hooks/useDebounce';

// Create a client
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false,
      retry: 2,
      staleTime: 5 * 60 * 1000, // 5 minutes
    },
  },
});

// Create theme
const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2',
    },
    secondary: {
      main: '#dc004e',
    },
    background: {
      default: '#f5f5f5',
    },
  },
  typography: {
    h4: {
      fontWeight: 600,
    },
    h6: {
      fontWeight: 500,
    },
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        root: {
          backgroundImage: 'none',
          boxShadow: '0px 2px 4px -1px rgba(0,0,0,0.2)',
        },
      },
    },
  },
});

const AppContent: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  const debouncedSearchTerm = useDebounce(searchTerm);
  
  // Query for all products (initial load)
  const {
    data: allProducts,
    isLoading: isLoadingAll,
    error: allProductsError,
  } = useProducts();

  // Query for search results (when searching)
  const {
    data: searchResults,
    isLoading: isSearching,
    error: searchError,
  } = useProductSearch(debouncedSearchTerm, debouncedSearchTerm.trim().length > 0);

  // Determine which data to show
  const isSearchMode = debouncedSearchTerm.trim().length > 0;
  const displayedProducts = (isSearchMode ? searchResults : allProducts) ?? [];
  const isLoading = isSearchMode ? isSearching : isLoadingAll;
  const error = isSearchMode ? searchError : allProductsError;

  // Memoized handlers for performance
  const handleSearchChange = useCallback((value: string) => {
    setSearchTerm(value);
  }, []);

  const handleSearchClear = useCallback(() => {
    setSearchTerm('');
  }, []);

  // Memoized search loading state
  const isSearchLoading = useMemo(() => {
    return searchTerm.trim().length > 0 && searchTerm !== debouncedSearchTerm;
  }, [searchTerm, debouncedSearchTerm]);

  // Memoized result summary
  const resultSummary = useMemo(() => {
    if (isLoading) return 'Loading...';
    if (error) return 'Error occurred';
    if (isSearchMode) {
      return `Found ${displayedProducts.length} product${displayedProducts.length !== 1 ? 's' : ''} matching "${debouncedSearchTerm}"`;
    }
    return `Showing ${displayedProducts.length} product${displayedProducts.length !== 1 ? 's' : ''}`;
  }, [isLoading, error, isSearchMode, displayedProducts.length, debouncedSearchTerm]);

  return (
    <Box sx={{ flexGrow: 1, minHeight: '100vh', backgroundColor: 'background.default' }}>
      <AppBar position="static" elevation={1}>
        <Toolbar>
          <StorefrontOutlined sx={{ mr: 2 }} />
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Product Management System
          </Typography>
        </Toolbar>
      </AppBar>

      <Container maxWidth="xl" sx={{ mt: 4, mb: 4 }}>
        <Box sx={{ mb: 3 }}>
          <Typography variant="h4" component="h1" gutterBottom>
            Products
          </Typography>
          <Typography variant="body1" color="text.secondary" sx={{ mb: 2 }}>
            Search and browse our product catalog
          </Typography>
        </Box>

        <SearchInput
          value={searchTerm}
          onChange={handleSearchChange}
          onClear={handleSearchClear}
          isLoading={isSearchLoading}
        />

        {error && (
          <Fade in={!!error}>
            <Alert severity="error" sx={{ mb: 3 }}>
              {error instanceof Error ? error.message : 'An error occurred while loading products'}
            </Alert>
          </Fade>
        )}

        <Box sx={{ mb: 2 }}>
          <Typography variant="body2" color="text.secondary">
            {resultSummary}
          </Typography>
        </Box>

        <ProductTable
          products={displayedProducts}
          loading={isLoading}
          error={error instanceof Error ? error.message : null}
        />
      </Container>
    </Box>
  );
};

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <AppContent />
        <ReactQueryDevtools initialIsOpen={false} />
      </ThemeProvider>
    </QueryClientProvider>
  );
};

export default App;
