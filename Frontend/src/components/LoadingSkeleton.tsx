import React from 'react';
import { Skeleton, TableCell, TableRow } from '@mui/material';

interface LoadingSkeletonProps {
  rows?: number;
  columns?: number;
}

const LoadingSkeleton: React.FC<LoadingSkeletonProps> = ({ 
  rows = 10, 
  columns = 8 
}) => {
  return (
    <>
      {Array.from({ length: rows }).map((_, rowIndex) => (
        <TableRow key={rowIndex}>
          {Array.from({ length: columns }).map((_, colIndex) => (
            <TableCell key={colIndex}>
              <Skeleton 
                variant="text" 
                height={24}
                animation="wave"
              />
            </TableCell>
          ))}
        </TableRow>
      ))}
    </>
  );
};

export default LoadingSkeleton;
