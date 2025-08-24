import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '../test/test-utils'
import userEvent from '@testing-library/user-event'
import SearchInput from '../components/SearchInput'

describe('SearchInput', () => {
  const mockOnChange = vi.fn()
  const mockOnClear = vi.fn()

  beforeEach(() => {
    mockOnChange.mockClear()
    mockOnClear.mockClear()
  })

  it('renders with placeholder text', () => {
    render(
      <SearchInput
        value=""
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    expect(screen.getByPlaceholderText(/search products/i)).toBeInTheDocument()
  })

  it('displays the current value', () => {
    const testValue = 'test search'
    render(
      <SearchInput
        value={testValue}
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    expect(screen.getByDisplayValue(testValue)).toBeInTheDocument()
  })

  it('calls onChange when user types', async () => {
    const user = userEvent.setup()
    render(
      <SearchInput
        value=""
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    const input = screen.getByPlaceholderText(/search products/i)
    await user.type(input, 'test')

    expect(mockOnChange).toHaveBeenCalledTimes(4) // once for each character
    expect(mockOnChange).toHaveBeenLastCalledWith('test')
  })

  it('shows clear button when there is a value', () => {
    render(
      <SearchInput
        value="test"
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    expect(screen.getByLabelText(/clear search/i)).toBeInTheDocument()
  })

  it('does not show clear button when value is empty', () => {
    render(
      <SearchInput
        value=""
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    expect(screen.queryByLabelText(/clear search/i)).not.toBeInTheDocument()
  })

  it('calls onClear when clear button is clicked', async () => {
    const user = userEvent.setup()
    render(
      <SearchInput
        value="test"
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    const clearButton = screen.getByLabelText(/clear search/i)
    await user.click(clearButton)

    expect(mockOnClear).toHaveBeenCalledTimes(1)
  })

  it('calls onClear when Escape key is pressed', () => {
    render(
      <SearchInput
        value="test"
        onChange={mockOnChange}
        onClear={mockOnClear}
      />
    )

    const input = screen.getByPlaceholderText(/search products/i)
    fireEvent.keyDown(input, { key: 'Escape' })

    expect(mockOnClear).toHaveBeenCalledTimes(1)
  })

  it('shows loading indicator when isLoading is true', () => {
    render(
      <SearchInput
        value="test"
        onChange={mockOnChange}
        onClear={mockOnClear}
        isLoading={true}
      />
    )

    expect(screen.getByRole('progressbar')).toBeInTheDocument()
  })

  it('does not show loading indicator when isLoading is false', () => {
    render(
      <SearchInput
        value="test"
        onChange={mockOnChange}
        onClear={mockOnClear}
        isLoading={false}
      />
    )

    expect(screen.queryByRole('progressbar')).not.toBeInTheDocument()
  })

  it('uses custom placeholder when provided', () => {
    const customPlaceholder = 'Custom search placeholder'
    render(
      <SearchInput
        value=""
        onChange={mockOnChange}
        onClear={mockOnClear}
        placeholder={customPlaceholder}
      />
    )

    expect(screen.getByPlaceholderText(customPlaceholder)).toBeInTheDocument()
  })
})
