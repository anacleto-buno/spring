import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '../test/test-utils'
import ProductTable from '../components/ProductTable'
import { mockProducts } from '../test/test-utils'

// Mock the DataGrid component to avoid complex MUI testing issues
vi.mock('@mui/x-data-grid', () => ({
  DataGrid: ({ rows, columns, loading }: { rows?: unknown[]; columns?: unknown[]; loading?: boolean }) => (
    <div data-testid="data-grid">
      {loading && <div data-testid="loading">Loading...</div>}
      <div data-testid="rows-count">{rows?.length || 0}</div>
      <div data-testid="columns-count">{columns?.length || 0}</div>
      {rows?.map((row: unknown, index: number) => {
        const product = row as { id: string; name: string }
        return (
          <div key={product.id || index} data-testid={`product-${product.id || index}`}>
            {product.name || 'Unknown Product'}
          </div>
        )
      })}
    </div>
  ),
  GridToolbar: () => <div data-testid="grid-toolbar">Toolbar</div>,
}))

describe('ProductTable', () => {
  it('renders without crashing', () => {
    render(<ProductTable products={[]} />)
    expect(screen.getByTestId('data-grid')).toBeInTheDocument()
  })

  it('displays loading state', () => {
    render(<ProductTable products={[]} loading={true} />)
    expect(screen.getByTestId('loading')).toBeInTheDocument()
    expect(screen.getByText('Loading...')).toBeInTheDocument()
  })

  it('displays error state', () => {
    const errorMessage = 'Failed to load products'
    render(<ProductTable products={[]} error={errorMessage} />)
    
    expect(screen.getByText('Error Loading Products')).toBeInTheDocument()
    expect(screen.getByText(errorMessage)).toBeInTheDocument()
  })

  it('renders products when provided', () => {
    render(<ProductTable products={mockProducts} />)
    
    expect(screen.getByTestId('rows-count')).toHaveTextContent('3')
    expect(screen.getByTestId('product-1')).toHaveTextContent('Test Product 1')
    expect(screen.getByTestId('product-2')).toHaveTextContent('Test Product 2')
    expect(screen.getByTestId('product-3')).toHaveTextContent('Test Product 3')
  })

  it('renders empty state when no products', () => {
    render(<ProductTable products={[]} />)
    expect(screen.getByTestId('rows-count')).toHaveTextContent('0')
  })

  it('shows correct number of columns', () => {
    render(<ProductTable products={mockProducts} />)
    // The component should have columns, exact number depends on screen size
    const columnsCount = screen.getByTestId('columns-count')
    expect(columnsCount).toBeInTheDocument()
    // Should have at least the core columns
    expect(parseInt(columnsCount.textContent || '0')).toBeGreaterThan(0)
  })

  it('handles null/undefined products gracefully', () => {
    // @ts-expect-error Testing edge case
    render(<ProductTable products={null} />)
    expect(screen.getByTestId('data-grid')).toBeInTheDocument()
  })

  it('displays grid toolbar', () => {
    render(<ProductTable products={mockProducts} />)
    expect(screen.getByTestId('grid-toolbar')).toBeInTheDocument()
  })
})
