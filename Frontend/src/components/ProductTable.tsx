import React, { memo, useMemo } from 'react';
import {
  DataGrid,
  GridToolbar,
} from '@mui/x-data-grid';
import type {
  GridColDef,
  GridRenderCellParams,
} from '@mui/x-data-grid';
import {
  Box,
  Paper,
  Typography,
  Chip,
  Rating,
  Tooltip,
  useTheme,
  useMediaQuery,
} from '@mui/material';
import type { ProductDto } from '../types/product';

interface ProductTableProps {
  products: ProductDto[];
  loading?: boolean;
  error?: string | null;
}

const ProductTable: React.FC<ProductTableProps> = memo(({ 
  products, 
  loading = false, 
  error = null 
}) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const formatCurrency = (amount: number): string => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(amount);
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    });
  };

  const getAvailabilityColor = (status: string | null): "default" | "primary" | "secondary" | "error" | "info" | "success" | "warning" => {
    if (!status) return 'default';
    const lowerStatus = status.toLowerCase();
    if (lowerStatus.includes('in stock') || lowerStatus.includes('available')) return 'success';
    if (lowerStatus.includes('out of stock') || lowerStatus.includes('unavailable')) return 'error';
    if (lowerStatus.includes('low stock') || lowerStatus.includes('limited')) return 'warning';
    if (lowerStatus.includes('discontinued')) return 'error';
    return 'info';
  };

  const columns: GridColDef[] = useMemo(() => {
    // Core columns that appear on all screen sizes
    const coreColumns: GridColDef[] = [
      {
        field: 'name',
        headerName: 'Product Name',
        flex: 1,
        minWidth: 180,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Tooltip title={params.row.description || 'No description'} placement="top">
            <Typography
              variant="body2"
              sx={{
                fontWeight: 'medium',
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
              }}
            >
              {params.value || 'N/A'}
            </Typography>
          </Tooltip>
        ),
      },
      {
        field: 'category',
        headerName: 'Category',
        width: 120,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Chip
            label={params.value || 'N/A'}
            size="small"
            variant="outlined"
            color="primary"
          />
        ),
      },
      {
        field: 'brand',
        headerName: 'Brand',
        width: 120,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Typography variant="body2">
            {params.value || 'N/A'}
          </Typography>
        ),
      },
      {
        field: 'price',
        headerName: 'Price',
        width: 100,
        type: 'number',
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Typography variant="body2" fontWeight="medium" color="primary">
            {formatCurrency(params.value)}
          </Typography>
        ),
      },
      {
        field: 'stockQuantity',
        headerName: 'Stock Qty',
        width: 90,
        type: 'number',
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Typography
            variant="body2"
            color={params.value > 10 ? 'success.main' : params.value > 0 ? 'warning.main' : 'error.main'}
            fontWeight="medium"
          >
            {params.value}
          </Typography>
        ),
      },
      {
        field: 'availabilityStatus',
        headerName: 'Availability',
        width: 120,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Chip
            label={params.value || 'Unknown'}
            size="small"
            color={getAvailabilityColor(params.value)}
            variant="filled"
          />
        ),
      },
    ];

    // Additional columns for larger screens
    const desktopColumns: GridColDef[] = [
      {
        field: 'description',
        headerName: 'Description',
        width: 200,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Tooltip title={params.value || 'No description'} placement="top">
            <Typography
              variant="body2"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                maxWidth: '100%',
              }}
            >
              {params.value || 'N/A'}
            </Typography>
          </Tooltip>
        ),
      },
      {
        field: 'sku',
        headerName: 'SKU',
        width: 120,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Typography variant="body2" fontFamily="monospace">
            {params.value || 'N/A'}
          </Typography>
        ),
      },
      {
        field: 'releaseDate',
        headerName: 'Release Date',
        width: 120,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Typography variant="body2">
            {formatDate(params.value)}
          </Typography>
        ),
      },
      {
        field: 'customerRating',
        headerName: 'Rating',
        width: 130,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          params.value ? (
            <Box display="flex" alignItems="center">
              <Rating
                value={params.value}
                readOnly
                size="small"
                precision={0.1}
              />
              <Typography variant="caption" sx={{ ml: 1 }}>
                ({params.value.toFixed(1)})
              </Typography>
            </Box>
          ) : (
            <Typography variant="caption" color="text.secondary">
              No rating
            </Typography>
          )
        ),
      },
      {
        field: 'availableColors',
        headerName: 'Colors',
        width: 150,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Tooltip title={params.value || 'No colors specified'} placement="top">
            <Typography
              variant="body2"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                maxWidth: '100%',
              }}
            >
              {params.value || 'N/A'}
            </Typography>
          </Tooltip>
        ),
      },
      {
        field: 'availableSizes',
        headerName: 'Sizes',
        width: 150,
        renderCell: (params: GridRenderCellParams<ProductDto>) => (
          <Tooltip title={params.value || 'No sizes specified'} placement="top">
            <Typography
              variant="body2"
              sx={{
                overflow: 'hidden',
                textOverflow: 'ellipsis',
                whiteSpace: 'nowrap',
                maxWidth: '100%',
              }}
            >
              {params.value || 'N/A'}
            </Typography>
          </Tooltip>
        ),
      },
    ];

    // Return different column configurations based on screen size
    return isMobile ? coreColumns : [...coreColumns, ...desktopColumns];
  }, [isMobile]);

  if (error) {
    return (
      <Paper elevation={2} sx={{ p: 3, textAlign: 'center' }}>
        <Typography variant="h6" color="error" gutterBottom>
          Error Loading Products
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {error}
        </Typography>
      </Paper>
    );
  }

  return (
    <Paper elevation={2} sx={{ height: '75vh', width: '100%', overflow: 'hidden' }}>
      <DataGrid
        rows={products}
        columns={columns}
        loading={loading}
        pageSizeOptions={[10, 25, 50, 100]}
        initialState={{
          pagination: {
            paginationModel: {
              pageSize: 25,
            },
          },
          sorting: {
            sortModel: [
              {
                field: 'name',
                sort: 'asc',
              },
            ],
          },
        }}
        slots={{
          toolbar: GridToolbar,
        }}
        slotProps={{
          toolbar: {
            showQuickFilter: false,
            quickFilterProps: { debounceMs: 500 },
          },
        }}
        sx={{
          '& .MuiDataGrid-cell': {
            borderColor: 'divider',
          },
          '& .MuiDataGrid-columnHeaders': {
            backgroundColor: 'background.paper',
            borderColor: 'divider',
            fontWeight: 600,
          },
          '& .MuiDataGrid-row:hover': {
            backgroundColor: 'action.hover',
          },
          '& .MuiDataGrid-virtualScroller': {
            overflowX: 'auto',
          },
        }}
        disableRowSelectionOnClick
        density={isMobile ? 'compact' : 'standard'}
        autoHeight={false}
      />
    </Paper>
  );
});

ProductTable.displayName = 'ProductTable';

export default ProductTable;
