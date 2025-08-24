import React, { memo } from 'react';
import {
  TextField,
  InputAdornment,
  IconButton,
  CircularProgress,
  Box,
} from '@mui/material';
import { Search, Clear } from '@mui/icons-material';

interface SearchInputProps {
  value: string;
  onChange: (value: string) => void;
  onClear: () => void;
  isLoading?: boolean;
  placeholder?: string;
}

const SearchInput: React.FC<SearchInputProps> = memo(({
  value,
  onChange,
  onClear,
  isLoading = false,
  placeholder = "Search products by name, description, category, brand, SKU...",
}) => {
  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    onChange(event.target.value);
  };

  const handleClearClick = () => {
    onClear();
  };

  const handleKeyPress = (event: React.KeyboardEvent) => {
    if (event.key === 'Escape') {
      onClear();
    }
  };

  return (
    <Box sx={{ mb: 3 }}>
      <TextField
        fullWidth
        variant="outlined"
        placeholder={placeholder}
        value={value}
        onChange={handleInputChange}
        onKeyDown={handleKeyPress}
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <Search color="action" />
            </InputAdornment>
          ),
          endAdornment: (
            <InputAdornment position="end">
              {isLoading && (
                <CircularProgress size={20} sx={{ mr: 1 }} />
              )}
              {value && (
                <IconButton
                  aria-label="clear search"
                  onClick={handleClearClick}
                  edge="end"
                  size="small"
                >
                  <Clear />
                </IconButton>
              )}
            </InputAdornment>
          ),
        }}
        sx={{
          '& .MuiOutlinedInput-root': {
            '&:hover fieldset': {
              borderColor: 'primary.main',
            },
          },
        }}
      />
    </Box>
  );
});

SearchInput.displayName = 'SearchInput';

export default SearchInput;
